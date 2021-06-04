using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IPSConverter.IPS
{
    public class IPS
    {
        public const String PATCH = "PATCH";
        public const String EOF = "EOF";
        public const Int32 OffsetWidth = 3;
        public const Int32 LengthWidth = 2;

        public readonly IList<Hunk> Hunks = new List<Hunk>();

        private static int CompareRangesByOffset(Range l, Range r)
        {
            return l.Start.Value.CompareTo(r.Start.Value);
        }

        private IList<Hunk> SelectByOffset(Int32 in_Offset)
        {
            IList<Hunk> copy = Hunks.ToList();
            return copy.Where(hunk => hunk.EditRange.Start.Value <= in_Offset && hunk.EditRange.End.Value < in_Offset).ToList();
        }

        public bool EditsAreDisjoint()
        {
            List<Range> ranges = new List<Range>();
            foreach (Hunk h in Hunks)
            {
                ranges.Add(h.EditRange);
            }
            ranges.Sort(CompareRangesByOffset);
            IList<Hunk> conflicts = new List<Hunk>();
            foreach (Range r in ranges)
            {
                IEnumerable<int> re = Enumerable.Range(r.Start.Value, r.End.Value - r.Start.Value + 1);
                foreach (Range l in ranges)
                {
                    IEnumerable<int> le = Enumerable.Range(l.Start.Value, l.End.Value - l.Start.Value + 1);
                    // TODO: not sure if this is the correct equality
                    if (r.Equals(l))
                    {
                        continue;
                    }
                    IEnumerable<int> intersection = re.ToArray().Intersect(le.ToArray());
                    //Console.WriteLine($"r = {r}, re = {String.Join(" ", re.ToArray())}");
                    //Console.WriteLine($"l = {l}, le = {String.Join(" ", le.ToArray())}");
                    //Console.WriteLine($"r = {r}, l = {l}, intersection.Any = {intersection.Any()}, intersection = {0}", String.Join(" ", intersection.ToArray()));
                    if (intersection.Any())
                    {
                        //Console.WriteLine($"r = {r}, l = {l}");
                    }
                    foreach (int i in intersection)
                    {
                        //Console.WriteLine($"Multiple writes to {i}, in {r} and {l}");
                        conflicts = conflicts.Concat(SelectByOffset(i)).ToList();
                        //Console.WriteLine($"Multiple writes to 0x{i:X}");
                    }
                }
            }
            conflicts = conflicts.Distinct().ToList();
            Console.WriteLine("The following hunks have conflicting edits:");
            foreach (Hunk c in conflicts)
            {
                Console.WriteLine("" + c);
            }
            return false;
        }

        public void Parse(Stream in_Stream)
        {
            Hunks.Clear();

            String header = TakeString(in_Stream, PATCH.Length);

            if (header != PATCH)
            {
                throw new InvalidHeaderException();
            }

            while(true)
            {
                Int32[] offsetBytes = Take(in_Stream, OffsetWidth);
                if (offsetBytes.Length < OffsetWidth)
                {
                    // TODO: this case should arguably be an exception
                    // but inorder to support that we'll have to differentiate
                    // between the case where the input is exhausted
                    // and the case where we had partial read.
                    break;
                }
                String offsetString = Encoding.ASCII.GetString(Array.ConvertAll<Int32, Byte>(offsetBytes, Convert.ToByte));
                if (offsetString == EOF)
                {
                    // all done
                    break;
                }
                // Offsets are stored big-endian
                Int32 offset = offsetBytes[0] << 16 | offsetBytes[1] << 8 | offsetBytes[2];
                Int32 payloadLength = ParseLength(in_Stream);

                if (payloadLength == 0)
                { // We have an RLE hunk
                    Hunks.Add(ParseRLEHunk(in_Stream, offset));
                } else
                { // We have a regular hunk
                    Hunks.Add(ParseRegularHunk(in_Stream, in_Offset: offset, in_PayloadLength: payloadLength));
                }
            };
        }

        /// <summary>
        /// Attempts to read in_Amount bytes, but it may read fewer than that
        /// if the stream ends.
        /// </summary>
        /// <param name="in_Stream"></param>
        /// <param name="in_Amount"></param>
        /// <returns></returns>
        private Int32[] Take(Stream in_Stream, Int32 in_Amount)
        {
            Int32[] taken = new Int32[in_Amount];
            for (UInt32 i = 0; i < in_Amount; i++)
            {
                Int32 current = in_Stream.ReadByte();
                if (current == -1)
                {
                    return taken;
                }
                taken[i] = current;
            }
            return taken;
        }

        private String TakeString(Stream in_Stream, Int32 in_Amount)
        {
            Int32[] bytes = Take(in_Stream, in_Amount);
            return Encoding.ASCII.GetString(Array.ConvertAll<Int32, Byte>(bytes, Convert.ToByte));
        }

        private Int32 ParseLength(Stream in_Stream)
        {
            Int32[] lengthBytes = Take(in_Stream, LengthWidth);
            // TODO: also need to check the case where the second value is -1
            if (lengthBytes.Length < LengthWidth)
            {
                throw new UnexpectedEndOfFile();
            }
            return (lengthBytes[0] << 8 | lengthBytes[1]);
        }

        private RegularHunk ParseRegularHunk(Stream in_Stream, Int32 in_Offset, Int32 in_PayloadLength)
        {
            Int32[] payload = Take(in_Stream, in_PayloadLength);
            if (payload.Length < in_PayloadLength)
            {
                throw new UnexpectedEndOfFile();
            }
            return new RegularHunk() { Offset = in_Offset, Payload = payload };
        }

        private RLEHunk ParseRLEHunk(Stream in_Stream, Int32 in_Offset)
        {
            Int32 length = ParseLength(in_Stream);
            Int32 value = in_Stream.ReadByte();
            if( value == -1)
            {
                throw new UnexpectedEndOfFile();
            }
            return new RLEHunk()
            {
                Offset = in_Offset,
                Run = length,
                Value = value
            };
        }

    }

    public class InvalidHeaderException : Exception { }

    public class UnexpectedEndOfFile : Exception { }

    public abstract record Hunk: IReadOnlyList<Int32>
    {
        public abstract Int32 this[Int32 index] { get; }

        public Int32 Offset { get; init; }
        public abstract Int32 Count { get; }

        public abstract IEnumerator<Int32> GetEnumerator();
        public abstract T Match<T>(Func<RegularHunk, T> Regular, Func<RLEHunk, T> RLE);

        public Range EditRange
        {
            get
            {
                return new(Offset, Offset + Count - 1);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // TODO: is this an issue in the base clase?
            throw new NotImplementedException();
        }

        protected virtual bool PrintMembers(StringBuilder in_Builder)
        {
            in_Builder.Append($"Offset = 0x{Offset:X}, Length = {Count},");
            return true;
        }
    }

    public record RegularHunk : Hunk, IReadOnlyList<Int32>
    {
        public override Int32 this[Int32 index] => Payload[index];

        public Int32[] Payload { get; init; }

        public override int Count => Payload.Length;

        public override IEnumerator<Int32> GetEnumerator()
        {
            return (IEnumerator<Int32>)Payload.GetEnumerator();
        }

        public override T Match<T>(Func<RegularHunk, T> Regular, Func<RLEHunk, T> RLE)
        {
            return Regular(this);
        }

        public void Match(Action<RegularHunk> Regular, Action<RLEHunk> RLE)
        {
            this.Match<Int32>(
                Regular: x => { Regular(x); return 0; },
                RLE: x => { RLE(x); return 0; }
                );
        }

        protected override bool PrintMembers(StringBuilder in_Builder)
        {
            base.PrintMembers(in_Builder);
            foreach (Int32 p in Payload) {
                in_Builder.Append($" 0x{p:X}");
            }
            return true;
        }
    }

    public record RLEHunk : Hunk, IReadOnlyList<Int32>
    {
        public override Int32 this[Int32 index]
        {
            get
            {
                if (0 <= index && index < Run)
                {
                    return Value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public Int32 Value { get; init; }
        public Int32 Run { get; init; }

        public Int32[] Payload
        {
            get
            {
                return Enumerable.Range(1, Run).Select(i => Value).ToArray();
            }
        }

        public override Int32 Count => Run;

        public override IEnumerator<Int32> GetEnumerator()
        {
            return (IEnumerator<Int32>)Payload.GetEnumerator();
        }

        public override T Match<T>(Func<RegularHunk, T> Regular, Func<RLEHunk, T> RLE)
        {
            return RLE(this);
        }
        protected override bool PrintMembers(StringBuilder in_Builder)
        {
            base.PrintMembers(in_Builder);
            in_Builder.Append($" Repeat 0x{Value:X}");
            return true;
        }
    }
}
