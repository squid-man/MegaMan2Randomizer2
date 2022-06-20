using System;
using System.Collections.Generic;
using System.Linq;

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

        void Reset();

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

        // Array Methods
        Object? NextArrayElement(Array in_Array);

        // IEnumerable Methods
        T NextElement<T>(IEnumerable<T> in_Elements);
        IList<T> Shuffle<T>(IEnumerable<T> in_List);

        /// <summary>
        /// Similar to NextElement, but it deletes
        /// the element from the dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="in_Dictionary"></param>
        /// <returns></returns>
        KeyValuePair<TKey, TValue> NextElementAndRemove<TKey, TValue>(IDictionary<TKey, TValue> in_Dictionary)
        {
            Int32 count = in_Dictionary.Count();
            Int32 index = this.NextInt32(count);
            KeyValuePair<TKey, TValue> element = in_Dictionary.ElementAt(index);
            in_Dictionary.Remove(element);
            return element;
        }

        public Dictionary<TKey, TValue> Shuffle<TKey, TValue>(IDictionary<TKey, TValue> in_Dict) where TKey : notnull
        {
            List<TValue> values =this.Shuffle(in_Dict.Values).ToList();

            return in_Dict.Keys
                .Zip(values, (key, value) => new KeyValuePair<TKey, TValue>(key, value))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        // Enum Methods
        T NextEnum<T>()
        {
            if (false == typeof(T).IsEnum)
            {
                throw new Exception("The generic type must be an enum");
            }

            Object? retval = this.NextElement(Enum.GetValues(typeof(T)).Cast<T>());

            return (T?)retval ?? throw new NullReferenceException();
        }
    }
}
