using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers
{
    public class RItemGet : IRandomizer
    {
        private readonly StringBuilder debug = new();
        public override String ToString()
        {
            return debug.ToString();
        }

        public RItemGet()
        {
            debug = new();
        }

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

            List<EItemNumber> itemGetList = new()
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

            // The dictionary is just to make the indexing in the
            // loop a bit nicer to express. The key ordering here
            // doesn't actually matter.
            IDictionary<EBossIndex, EItemNumber> itemGetOrder = EBossIndex.RobotMasters
                .Zip(in_Context.Seed.Shuffle(itemGetList))
                .ToDictionary(x => x.First, x => x.Second);

            foreach (EBossIndex index in EBossIndex.RobotMasters)
            {
                in_Patch.Add(
                    (Int32)EItemStageAddress.HeatMan + index.Offset,
                    (Byte)itemGetOrder[index],
                    String.Format("{0}man Item Get", ((EDmgVsBoss.Offset)index).ToString()));
            }

            // Dump the boss rewards to the log
            debug.AppendLine("ItemGet Table:");
            debug.AppendLine("-------------------------------------");
            Dictionary<EBossIndex, EItemNumber> stagesWithItems = itemGetOrder
                .Where(x => x.Value == EItemNumber.One
                         || x.Value == EItemNumber.Two
                         || x.Value == EItemNumber.Three)
                .ToDictionary(x => x.Key, x => x.Value );
            foreach (KeyValuePair<EBossIndex, EItemNumber> i in stagesWithItems)
            {
                String itemName = i.Value switch
                {
                    EItemNumber.One => "Item 1",
                    EItemNumber.Two => "Item 2",
                    EItemNumber.Three => "Item 3",
                    EItemNumber.None => "",
                };
                debug.AppendLine($"{i.Key.Name} stage\t -> {itemName}");
            }
        }
    }
}
