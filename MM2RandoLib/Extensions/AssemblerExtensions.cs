using js65;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MM2Randomizer.Extensions
{
    public static class AssemblerExtensions
    {
        public static async Task<byte[]> ApplyAsync(this Assembler asm, byte[] rom)
        {
            var res = await asm.Apply(rom);
            Debug.Assert(res is not null);

            // TODO: Error handling for this should be improved at some point
            if (!res.success)
            {
                StringBuilder sb = new();
                sb.AppendLine("Assembly compilation failed");
                foreach (var msg in res.messages)
                {
                    sb.AppendLine();
                    sb.AppendLine(msg.ToString());
                }

                throw new Exception(sb.ToString());
            }

            return res.romdata;
        }
    }
}
