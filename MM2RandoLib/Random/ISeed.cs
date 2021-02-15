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
        Boolean GetNextBoolean();


        // UInt8 Methods
        Byte GetNextUInt8();
        Byte GetNextUInt8(Int32 in_MaxValue);
        Byte GetNextUInt8(Int32 in_MinValue, Int32 in_MaxValue);

        // Int32 Methods
        Int32 GetNextInt32();
        Int32 GetNextInt32(Int32 in_MaxValue);
        Int32 GetNextInt32(Int32 in_MinValue, Int32 in_MaxValue);

        // Double Methods
        Double GetNextDouble();

        // IEnumerable Methods
        T GetNextElement<T>(IEnumerable<T> in_Elements);
        IList<T> Shuffle<T>(IEnumerable<T> in_List);
    }
}
