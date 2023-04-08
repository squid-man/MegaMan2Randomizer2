using System;
using System.Collections.Generic;
using System.Text;

namespace MM2Randomizer.Enums
{
    /// <summary>
    /// http://6502.org/tutorials/6502opcodes.html
    /// </summary>
    public static class Opcode6502
    {
        /// <summary>
        /// Jump to subroutine.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#JSR"/>
        public const Byte JSR = 0x20;

        /// <summary>
        /// Bitwise AND with accumulator.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#AND"/>
        public const Byte AND = 0x29;

        /// <summary>
        /// Jump; MODE = Absolute.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#JMP"/>
        public const Byte JMP_Absolute = 0x4C;

        /// <summary>
        /// Return from subroutine.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#RTS"/>
        public const Byte RTS = 0x60;

        /// <summary>
        /// Store accumulator; MODE = Zero Page.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#STA"/>
        public const Byte STA_ZeroPage = 0x85;

        /// <summary>
        /// Load accumulator; MODE = Zero Page.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#LDA"/>
        public const Byte LDA_ZeroPage = 0xA5;

        /// <summary>
        /// Load accumulator; MODE = Immediate.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#LDA"/>
        public const Byte LDA_Immediate = 0xA9;

        /// <summary>
        /// Branch on carry set.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#BCS"/>
        public const Byte BCS = 0xB0;

        /// <summary>
        /// Compare accumulator.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#CMP"/>
        public const Byte CMP = 0xC5;

        /// <summary>
        /// Increment memory.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#INC"/>
        public const Byte INC = 0xE6;

        /// <summary>
        /// No operation.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#NOP"/>
        public const Byte NOP = 0xEA;

        /// <summary>
        /// Branch on Equal.
        /// </summary>
        /// <see cref="http://6502.org/tutorials/6502opcodes.html#BEQ"/>
        public const Byte BEQ = 0xF0;
    }
}
