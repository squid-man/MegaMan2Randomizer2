using System;
using System.Collections.Generic;
using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers
{
    public class RItemGet : IRandomizer
    {
        public RItemGet() { }

        /// <summary>
        /// Shuffle which Robot Master awards Items 1, 2, and 3.
        /// </summary>
        public void Randomize(Patch in_Patch, RandomizationContext in_Context)
        {
            // 0x03C291 - Item # from Heat Man
            // 0x03C292 - Item # from Air Man
            // 0x03C293 - Item # from Wood Man
            // 0x03C294 - Item # from Bubble Man
            // 0x03C295 - Item # from Quick Man
            // 0x03C296 - Item # from Flash Man
            // 0x03C297 - Item # from Metal Man
            // 0x03C298 - Item # from Crash Man

            List<EItemNumber> itemGetList = new List<EItemNumber>()
            {
                EItemNumber.None,
                EItemNumber.None,
                EItemNumber.None,
                EItemNumber.None,
                EItemNumber.None,
                EItemNumber.One,
                EItemNumber.Two,
                EItemNumber.Three,
            };

            IList<EItemNumber> itemGetOrder = in_Context.Seed.Shuffle(itemGetList);

            for (Int32 index = 0; index < itemGetOrder.Count; ++index)
            {
                in_Patch.Add(
                    (Int32)EItemStageAddress.HeatMan + index,
                    (Byte)itemGetOrder[index],
                    String.Format("{0}man Item Get", ((EDmgVsBoss.Offset)(EBossIndex)index).ToString()));
            }
        }
    }
}
