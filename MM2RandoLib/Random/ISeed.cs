using System;
using System.Collections.Generic;
using System.Text;

namespace MM2Randomizer.Random
{
    public interface ISeed
    {
        //
        // Properties
        //

        String SeedString
        {
            get;
        }


        String Identifier
        {
            get;
        }


        //
        // Public Methods
        //

        void Next();

        // Boolean Methods
        Boolean NextBoolean();


        // UInt8 Methods
        Byte NextUInt8();
        Byte NextUInt8(Int32 in_MaxValue);
        Byte NextUInt8(Int32 in_MinValue, Int32 in_MaxValue);

        // Int32 Methods
        Int32 NextInt32();
        Int32 NextInt32(Int32 in_MaxValue);
        Int32 NextInt32(Int32 in_MinValue, Int32 in_MaxValue);

        // Double Methods
        Double NextDouble();

        // IEnumerable Methods
        T NextElement<T>(IEnumerable<T> in_Elements);
        IList<T> Shuffle<T>(IEnumerable<T> in_List);
    }
}
