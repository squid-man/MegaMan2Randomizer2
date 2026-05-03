using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Patcher
{
    public record IpsSegment(Int32 SrcOffs, Int32 Size, Int32 TgtOffs, Boolean IsRle);

    public class Patch
    {
        public Dictionary<Int32, ChangeByteRecord> Bytes { get; set; }

        public Patch()
        {
            Bytes = new Dictionary<Int32, ChangeByteRecord>();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="note"></param>
        public void Add(Int32 address, Byte value, String note = "")
        {
            ChangeByteRecord newByte = new ChangeByteRecord(address, value, note);

            // Either replace the Byte at the given address, or add it if it doesn't exist
            if (Bytes.ContainsKey(address))
            {
                Bytes[address] = newByte;
            }
            else
            {
                Bytes.Add(address, newByte);
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="note"></param>
        /// <param name="bigEndian"></param>
        public Int32 AddWord(Int32 address, Int32 value, String note = "", Boolean bigEndian = false)
        {
            if (bigEndian)
            {
                this.Add(address++, (byte)(value >> 8), $"{note}[0]");
                this.Add(address++, (byte)(value), $"{note}[1]");
            }
            else
            {
                this.Add(address++, (byte)(value), $"{note}[0]");
                this.Add(address++, (byte)(value >> 8), $"{note}[1]");
            }

            return address;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="in_StartAddress"></param>
        /// <param name="in_Value"></param>
        /// <param name="note"></param>
        public Int32 Add(Int32 in_StartAddress, IEnumerable<byte> in_Value, String note = "")
        {
            Int32 index = 0;

            foreach (Byte b in in_Value)
            {
                this.Add(in_StartAddress++, b, $"{note}[{index++}]");
            }

            return in_StartAddress;
        }


        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="rom"></param>
        public void ApplyRandoPatch(byte[] rom)
        {
            //GetStringSortedByAddress();

            foreach (var rec in Bytes.Values)
                rom[rec.Address] = rec.Value;
        }


        public String GetStringSortedByAddress()
        {
            IOrderedEnumerable<KeyValuePair<Int32, ChangeByteRecord>> sortDict =
                from kvp in Bytes orderby kvp.Key ascending select kvp;

            return ConvertDictToString(sortDict);
        }


        public String GetString()
        {
            return ConvertDictToString((IOrderedEnumerable<KeyValuePair<Int32, ChangeByteRecord>>)Bytes);
        }


        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private String ConvertDictToString(IOrderedEnumerable<KeyValuePair<Int32, ChangeByteRecord>> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<Int32, ChangeByteRecord> kvp in dict)
            {
                ChangeByteRecord b = kvp.Value;
                sb.Append($"0x{b.Address:X6}\t{b.Value:X2}\t{b.Note}");
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="in_FileName"></param>
        /// <param name="in_IpsPatch"></param>
        /// <param name="in_RebasePatch">If true or false, whether to relocate writes to the vanilla common bank (0x3c010+) to the expanded common bank (0x7c010+). If null, patches will be rebased unless they contain patches above the vanilla ROM size (0x40010+).</param>
        public void ApplyIPSPatch(String in_FileName, Byte[] in_IpsPatch, Boolean? in_RebasePatch = null)
        {
            using (FileStream romStream = new FileStream(
                in_FileName,
                FileMode.Open,
                FileAccess.ReadWrite,
                FileShare.ReadWrite))
            {
                ApplyIPSPatch(romStream, in_IpsPatch, in_RebasePatch);
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="in_Rom"></param>
        /// <param name="in_IpsPatch"></param>
        /// <param name="in_RebasePatch">If true or false, whether to relocate writes to the vanilla common bank (0x3c010+) to the expanded common bank (0x7c010+). If null, patches will be rebased unless they contain patches above the vanilla ROM size (0x40010+).</param>
        public void ApplyIPSPatch(Byte[] in_Rom, Byte[] in_IpsPatch, Boolean? in_RebasePatch = null)
        {
            using (MemoryStream stream = new(in_Rom))
                ApplyIPSPatch(stream, in_IpsPatch, in_RebasePatch);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="in_RomStream"></param>
        /// <param name="in_IpsPatch"></param>
        /// <param name="in_RebasePatch">If true or false, whether to relocate writes to the vanilla common bank (0x3c010+) to the expanded common bank (0x7c010+). If null, patches will be rebased unless they contain patches above the vanilla ROM size (0x40010+).</param>
        public void ApplyIPSPatch(Stream in_RomStream, Byte[] in_IpsPatch, Boolean? in_RebasePatch = null)
        {
            var ipsSegs = EnumIpsSegments(in_IpsPatch).ToList();
            Boolean rebasePatch = in_RebasePatch 
                ?? !ipsSegs.Any(s => s.TgtOffs + s.Size > 0x40010);
            Int64 romLength = in_RomStream.Length;
            foreach (var seg in ipsSegs)
            {
                Int32 offset = seg.TgtOffs,
                    recordSize = seg.Size;
                if (rebasePatch)
                {
                    if (offset + recordSize > 0x40010)
                        throw new ArgumentException(@"The IPS patch contains unrebasable changes");

                    if (offset >= 0x3c010)
                        offset += 0x40000;
                    else if (offset + recordSize > 0x3c010)
                        // If any such patches appear this will need to be implemented
                        throw new NotImplementedException(@"Patches with changes that cross the bank $e/f boundary are not supported");
                }

                in_RomStream.Seek(offset, SeekOrigin.Begin);

                // IPS Record
                if (!seg.IsRle)
                {
                    in_RomStream.Write(in_IpsPatch, seg.SrcOffs, recordSize);
                }
                // IPS RLE Record
                else
                {
                    // Initialize an array of bytes to the specified value
                    Byte[] repeatBuffer = new Byte[recordSize];
                    Array.Fill(repeatBuffer, in_IpsPatch[seg.SrcOffs]);

                    in_RomStream.Write(repeatBuffer);
                }
            }
        }

        public static IEnumerable<IpsSegment> EnumIpsSegments(Byte[] in_IpsPatch)
        {
            if (in_IpsPatch.Length < 5)
                throw new ArgumentException(@"The IPS patch data is not valid", nameof(in_IpsPatch));

            // Read the first 5 bytes of the patch. An IPS patch will always
            // begin with "PATCH"
            String ipsPatchHeader = Encoding.ASCII.GetString(in_IpsPatch, 0, 5);

            if (@"PATCH" != ipsPatchHeader)
                throw new ArgumentException(@"The IPS patch header is not valid", nameof(in_IpsPatch));

            Boolean badPatch = false;
            Int32 currentIndex = 5;
            while (currentIndex + 7 <= in_IpsPatch.Length)
            {
                // Get the next three bytes. These combine to make the next
                // 24-bit offset into the file
                Byte offsetHighByte = in_IpsPatch[currentIndex++];
                Byte offsetMiddleByte = in_IpsPatch[currentIndex++];
                Byte offsetLowByte = in_IpsPatch[currentIndex++];

                Int32 offset =
                    (0x10000 * offsetHighByte) +
                    (0x100 * offsetMiddleByte) +
                    offsetLowByte;

                Byte recordHighByte = in_IpsPatch[currentIndex++];
                Byte recordLowByte = in_IpsPatch[currentIndex++];

                Int32 recordSize = (0x100 * recordHighByte) + recordLowByte;
                Int32 srcSizeLeft = recordSize != 0 ? recordSize : 3;

                if (currentIndex + srcSizeLeft + 3 > in_IpsPatch.Length)
                {
                    badPatch = true;
                    break;
                }

                // IPS Record
                if (recordSize > 0)
                {
                    yield return new IpsSegment(currentIndex, recordSize, offset, false);

                    currentIndex = currentIndex + recordSize;
                }
                // IPS RLE Record
                else
                {
                    Byte repeatCountHighByte = in_IpsPatch[currentIndex++];
                    Byte repeatCountLowByte = in_IpsPatch[currentIndex++];

                    Int32 repeatCount =
                        (0x100 * repeatCountHighByte) +
                        repeatCountLowByte;

                    yield return new IpsSegment(currentIndex, repeatCount, offset, true);

                    currentIndex++;
                }
            }

            if (badPatch 
                || in_IpsPatch.Length < currentIndex + 3
                || Encoding.ASCII.GetString(in_IpsPatch, currentIndex, 3) != @"EOF")
                // The IPS patch was not properly terminated
                throw new ArgumentException(@"The IPS patch data is not properly terminated", nameof(in_IpsPatch));
        }
    }
}
