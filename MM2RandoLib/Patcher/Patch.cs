using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Patcher
{
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
        public Int32 Add(Int32 in_StartAddress, Byte[] in_Value, String note = "")
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
        /// <param name="filename"></param>
        public void ApplyRandoPatch(String filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                //GetStringSortedByAddress();

                foreach (KeyValuePair<Int32, ChangeByteRecord> kvp in Bytes)
                {
                    stream.Position = kvp.Key;
                    stream.WriteByte(kvp.Value.Value);
                }
            }
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
        /// <param name="romname"></param>
        /// <param name="patchBytes"></param>
        public void ApplyIPSPatch(String in_FileName, Byte[] in_IpsPatch)
        {
            if (in_IpsPatch.Length < 5)
            {
                throw new ArgumentException(@"The IPS patch data is not valid", nameof(in_IpsPatch));
            }

            // Read the first 5 bytes of the patch. An IPS patch will always
            // begin with "PATCH"
            String ipsPatchHeader = Encoding.ASCII.GetString(in_IpsPatch, 0, 5);

            if (@"PATCH" != ipsPatchHeader)
            {
                throw new ArgumentException(@"The IPS patch header is not valid", nameof(in_IpsPatch));
            }

            FileStream romStream = new FileStream(
                in_FileName,
                FileMode.Open,
                FileAccess.ReadWrite,
                FileShare.ReadWrite);

            Boolean endOfFile = false;

            using (romStream)
            {
                Int64 romLength = romStream.Length;

                Int32 currentIndex = 5;

                while (currentIndex < in_IpsPatch.Length && false == endOfFile)
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

                    // IPS Record
                    if (recordSize > 0)
                    {
                        // This is the simple case. Seek into the file at the
                        // specified offset, write the data that comes after
                        // the record size, and increment the current patch
                        // index by the size of the record
                        romStream.Seek(offset, SeekOrigin.Begin);
                        romStream.Write(in_IpsPatch, currentIndex, recordSize);
                        currentIndex = currentIndex + recordSize;
                    }
                    // IPS RLE Record
                    else
                    {
                        // This is the repeat value case. Read the repeat count
                        // and the value to repeat. Seek into the file at the
                        // specified offset, and write the value the number of
                        // times specified by the repeat count.
                        Byte repeatCountHighByte = in_IpsPatch[currentIndex++];
                        Byte repeatCountLowByte = in_IpsPatch[currentIndex++];

                        Int32 repeatCount =
                            (0x100 * repeatCountHighByte) +
                            repeatCountLowByte;

                        Byte repeatByteValue = in_IpsPatch[currentIndex++];

                        // Initialize an array of bytes to the specified value
                        Byte[] repeatBuffer = new Byte[repeatCount];
                        Array.Fill(repeatBuffer, repeatByteValue);

                        romStream.Seek(offset, SeekOrigin.Begin);
                        romStream.Write(repeatBuffer);
                    }

                    // Check for the end-of-file
                    String ipsPatchEof = Encoding.ASCII.GetString(in_IpsPatch, currentIndex, 3);
                    endOfFile = (@"EOF" == ipsPatchEof);
                }

                romStream.Close();
            }

            // The IPS patch was not properly terminated
            if (false == endOfFile)
            {
                throw new ArgumentException(@"The IPS patch data is not properly terminated", nameof(in_IpsPatch));
            }
        }
    }
}
