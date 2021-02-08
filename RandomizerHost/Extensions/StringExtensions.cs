using System;
using System.Data.HashFunction;
using System.Data.HashFunction.FNV;
using System.Text;

namespace RandomizerHost.Extensions
{
    public static class StringExtensions
    {
        public static Int32 ToHash32(this String in_String)
        {
            // Convert the String to an array for hashing
            Byte[] seedStringBytes = Encoding.ASCII.GetBytes(in_String);

            // Create a new config in order to hash to a 32-bit number
            FNVConfig c = new FNVConfig()
            {
                HashSizeInBits = 32,
                Prime = new System.Numerics.BigInteger(1099511628211),
                Offset = new System.Numerics.BigInteger(14695981039346656037),
            };

            // Compute the hash
            IHashValue hashValue = FNV1aFactory.Instance.Create(c).ComputeHash(seedStringBytes);

            // Copy the hash to the Int32 seed
            Int32[] array = new Int32[1];
            hashValue.AsBitArray().CopyTo(array, 0);
            return array[0];
        }
    }
}
