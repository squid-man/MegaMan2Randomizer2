using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM2Randomizer.Data
{
    using Buffer = IList<byte>;

    public class BinaryBuffer
    {
        private Buffer buffer;
        private int position = 0;

        private void CheckReadSize(int in_BufferLength, int in_Index, int in_Count)
        {
            if (in_Index < 0 || in_Count < 0)
                throw new ArgumentOutOfRangeException();
            else if (checked(in_Index + in_Count) > in_BufferLength)
                throw new EndOfStreamException();
        }

        private void CheckWriteSize(int in_ElementSize, int in_BufferLength, int in_Index, int in_Count)
        {
            if (in_Index < 0 || in_Count < 0)
                throw new ArgumentOutOfRangeException();
            else if (checked(in_Index + in_Count) > in_BufferLength)
                throw new ArgumentException();
            else if (checked(position + in_Count * in_ElementSize) > buffer.Count)
                throw new OverflowException();
        }

        private ushort InternalReadUInt16(ref int pos, bool in_BigEndian = false)
        {
            ushort value;
            if (in_BigEndian)
            {
                value = (ushort)(((ushort)buffer[pos++]) << 8);
                value |= (ushort)buffer[pos++];
            }
            else
            {
                value = (ushort)buffer[pos++];
                value |= (ushort)(((ushort)buffer[pos++]) << 8);
            }

            return value;
        }

        private void InternalWriteUInt16(ushort in_Value, ref int pos, bool in_BigEndian = false)
        {
            if (in_BigEndian)
            {
                buffer[pos++] = (byte)(in_Value >> 8);
                buffer[pos++] = (byte)in_Value;
            }
            else
            {
                buffer[pos++] = (byte)in_Value;
                buffer[pos++] = (byte)(in_Value >> 8);
            }
        }

        public BinaryBuffer(Buffer in_Buffer)
        {
            buffer = in_Buffer;
        }

        public int Position
        {
            get { return position; }
            set { Seek(value); }
        }

        public byte this[int index]
        {
            get { return buffer[index]; }
            set { buffer[index] = value; }
        }

        public int Seek(int in_Offset, SeekOrigin in_Origin = SeekOrigin.Begin)
        {
            int newPos = position;
            checked
            {
                if (in_Origin == SeekOrigin.Begin)
                    newPos = in_Offset;
                else if (in_Origin == SeekOrigin.Current)
                    newPos += in_Offset;
                else if (in_Origin == SeekOrigin.End)
                    newPos = buffer.Count + in_Offset;
                else
                    throw new ArgumentException();
            }

            if (newPos < 0)
                throw new OverflowException();

            position = newPos;

            return position;
        }

        public int Read(bool in_Advance = true)
        {
            int value = -1;
            if (position < buffer.Count)
            {
                value = buffer[position];

                if (in_Advance)
                    position++;
            }

            return value;
        }

        public int Read(IList<byte> out_Buffer, int in_Index, int in_Count, bool in_Advance = true)
        {
            CheckReadSize(out_Buffer.Count, in_Index, in_Count);

            int pos = position;
            for (int i = in_Index; i < in_Index + in_Count; i++)
                out_Buffer[i] = buffer[pos++];

            if (in_Advance)
                position = pos;

            return in_Count;
        }

        public int Read(IList<ushort> out_Buffer, int in_Index, int in_Count, bool in_BigEndian = false, bool in_Advance = true)
        {
            CheckReadSize(out_Buffer.Count, in_Index, in_Count);

            int pos = position;
            for (int i = in_Index; i < in_Index + in_Count; i++)
                out_Buffer[i] = InternalReadUInt16(ref pos, in_BigEndian);

            if (in_Advance)
                position = pos;

            return in_Count;
        }

        public int Read(IList<short> out_Buffer, int in_Index, int in_Count, bool in_BigEndian = false, bool in_Advance = true)
        {
            return Read((IList<ushort>)out_Buffer, in_Index, in_Count, in_BigEndian, in_Advance);
        }

        public byte ReadByte(bool in_Advance = true)
        {
            byte value = buffer[position];

            if (in_Advance)
                position++;

            return value;
        }

        public sbyte ReadSByte(bool in_Advance = true)
        {
            return (sbyte)ReadByte(in_Advance);
        }

        public ushort ReadUInt16(bool in_BigEndian = false, bool in_Advance = true)
        {
            int pos = position;
            ushort value = InternalReadUInt16(ref pos, in_BigEndian);
            if (in_Advance)
                position = pos;

            return value;
        }

        public short ReadInt16(bool in_BigEndian = false, bool in_Advance = true)
        {
            return (short)ReadUInt16(in_Advance, in_BigEndian);
        }

        public void Write(byte in_Value, bool in_Advance = true)
        {
            buffer[position] = in_Value;

            if (in_Advance)
                position++;
        }

        public void Write(IReadOnlyList<byte> in_Buffer, int in_Index, int in_Count, bool in_Advance = true)
        {
            CheckWriteSize(1, in_Buffer.Count, in_Index, in_Count);

            int pos = position;
            for (int inPos = in_Index; inPos < in_Index + in_Count; inPos++)
                buffer[pos++] = in_Buffer[inPos];

            if (in_Advance)
                position = pos;
        }

        public void Write(sbyte in_Value, bool in_Advance = true)
        {
            Write((byte)in_Value, in_Advance);
        }

        public void Write(IReadOnlyList<sbyte> in_Buffer, int in_Index, int in_Count, bool in_Advance = true)
        {
            Write((IReadOnlyList<byte>)in_Buffer, in_Index, in_Count, in_Advance);
        }

        public void Write(ushort in_Value, bool in_BigEndian = false, bool in_Advance = true)
        {
            if (checked(position + 2) > buffer.Count)
                throw new OverflowException();

            int pos = position;
            InternalWriteUInt16(in_Value, ref pos, in_BigEndian);

            if (in_Advance)
                position = pos;
        }

        public void Write(IReadOnlyList<ushort> in_Buffer, int in_Index, int in_Count, bool in_BigEndian = false, bool in_Advance = true)
        {
            CheckWriteSize(2, in_Buffer.Count, in_Index, in_Count);

            int pos = position;
            for (int inPos = in_Index; inPos < in_Index + in_Count; inPos++)
                InternalWriteUInt16(in_Buffer[inPos], ref pos, in_BigEndian);

            if (in_Advance)
                position = pos;
        }

        public void Write(short in_Value, bool in_BigEndian = false, bool in_Advance = true)
        {
            Write((ushort)in_Value, in_BigEndian, in_Advance);
        }

        public void Write(IReadOnlyList<short> in_Buffer, int in_Index, int in_Count, bool in_BigEndian = false, bool in_Advance = true)
        {
            Write((IReadOnlyList<ushort>)in_Buffer, in_Index, in_Count, in_BigEndian, in_Advance);
        }
    }
}
