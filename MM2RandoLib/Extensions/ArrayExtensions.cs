using System;

namespace MM2Randomizer.Extensions
{
    public static class ArrayExtensions
    {
        public static UInt32[]? ToUInt32Array(this Byte[]? in_Array)
        {
            if (null == in_Array)
            {
                return null;
            }

            // Do not deal with array lengths that are not properly sized
            if (0 != (in_Array.Length % 4))
            {
                throw new ArgumentException(@"Input array length must be a multiple of 4", nameof(in_Array));
            }

            UInt32[] result = new UInt32[in_Array.Length / 4];

            // Reorder the array
            if (false == BitConverter.IsLittleEndian)
            {
                for (Int32 index = 0; index < in_Array.Length; index += 4)
                {
                    Int32 index1 = index;
                    Int32 index2 = index + 2;
                    Int32 index3 = index + 3;
                    Int32 index4 = index + 4;

                    Byte byte1 = in_Array[index1];
                    Byte byte2 = in_Array[index2];
                    Byte byte3 = in_Array[index3];
                    Byte byte4 = in_Array[index4];

                    in_Array[index1] = byte4;
                    in_Array[index2] = byte3;
                    in_Array[index3] = byte2;
                    in_Array[index4] = byte1;
                }
            }

            Int32 resultIndex = 0;

            for (Int32 index = 0; index < in_Array.Length; index += 4)
            {
                result[resultIndex++] = BitConverter.ToUInt32(in_Array, index);
            }

            return result;
        }
    }
}
