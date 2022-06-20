using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM2Randomizer.Enums;
using MM2Randomizer.Extensions;
using MM2Randomizer.Patcher;
using MM2Randomizer.Settings.Options;

namespace MM2Randomizer.Randomizers.Stages
{
    public class RStages : IRandomizer
    {
        private List<StageFromSelect>? StageSelect;
        private StringBuilder debug = new StringBuilder();

        public RStages() { }

        public override String ToString()
        {
            return debug.ToString();
        }

        public static List<StageFromSelect> VanillaStageSelect()
        {
            // StageSelect  Address    Value
            // -----------------------------
            // Bubble Man   0x034670   3
            // Air Man      0x034671   1
            // Quick Man    0x034672   4
            // Wood Man     0x034673   2
            // Crash Man    0x034674   7
            // Flash Man    0x034675   5
            // Metal Man    0x034676   6
            // Heat Man     0x034677   0

            List<StageFromSelect> StageSelect = new List<StageFromSelect>
            {
                new StageFromSelect(
                    in_PortraitName: "Bubble Man",
                    in_PortraitAddress: ERMPortraitAddress.BubbleMan,
                    in_InitialPortraitDestination: ERMPortraitDestination.BubbleMan,
                    in_TextAddress: ERMPortraitText.BubbleMan,
                    in_TextValues: "BUBBLE"
                ),

                new StageFromSelect(
                    in_PortraitName: "Air Man",
                    in_PortraitAddress: ERMPortraitAddress.AirMan,
                    in_InitialPortraitDestination: ERMPortraitDestination.AirMan,
                    in_TextAddress: ERMPortraitText.AirMan,
                    in_TextValues: "`AIR```"
                ),

                new StageFromSelect(
                    in_PortraitName: "Quick Man",
                    in_PortraitAddress: ERMPortraitAddress.QuickMan,
                    in_InitialPortraitDestination: ERMPortraitDestination.QuickMan,
                    in_TextAddress: ERMPortraitText.QuickMan,
                    in_TextValues: "QUICK`"
                ),

                new StageFromSelect(
                    in_PortraitName: "Wood Man",
                    in_PortraitAddress: ERMPortraitAddress.WoodMan,
                    in_InitialPortraitDestination: ERMPortraitDestination.WoodMan,
                    in_TextAddress: ERMPortraitText.WoodMan,
                    in_TextValues: "WOOD``"
                ),

                new StageFromSelect(
                    in_PortraitName: "Crash Man",
                    in_PortraitAddress: ERMPortraitAddress.CrashMan,
                    in_InitialPortraitDestination: ERMPortraitDestination.CrashMan,
                    in_TextAddress: ERMPortraitText.CrashMan,
                    in_TextValues: "CRASH`"
                ),

                new StageFromSelect(
                    in_PortraitName: "Flash Man",
                    in_PortraitAddress: ERMPortraitAddress.FlashMan,
                    in_InitialPortraitDestination: ERMPortraitDestination.FlashMan,
                    in_TextAddress: ERMPortraitText.FlashMan,
                    in_TextValues: "FLASH`"
                ),

                new StageFromSelect(
                    in_PortraitName: "Metal Man",
                    in_PortraitAddress: ERMPortraitAddress.MetalMan,
                    in_InitialPortraitDestination: ERMPortraitDestination.MetalMan,
                    in_TextAddress: ERMPortraitText.MetalMan,
                    in_TextValues: "METAL`"
                ),

                new StageFromSelect(
                    in_PortraitName: "Heat Man",
                    in_PortraitAddress: ERMPortraitAddress.HeatMan,
                    in_InitialPortraitDestination: ERMPortraitDestination.HeatMan,
                    in_TextAddress: ERMPortraitText.HeatMan,
                    in_TextValues: "HEAT``"
                ),
            };

            return StageSelect;
        }

        public void FixPortraits<T>(ref Dictionary<EBossIndex, T> portraitBG_x, ref Dictionary<EBossIndex, T> portraitBG_y)
        {
            if (null == this.StageSelect)
            {
                throw new NullReferenceException(@"Object has not been initialized. Call Randomize() first");
            }

            // Get the new stage order
            Dictionary<EBossIndex, EBossIndex> newOrder = new();

            foreach (StageFromSelect stage in StageSelect)
            {
                newOrder[StageFromSelect.GetBossIndex(stage.PortraitDestination.Old)] = StageFromSelect.GetBossIndex(stage.PortraitDestination.New);
            }

            // Permute portrait x/y values via the shuffled stage-order array
            Dictionary<EBossIndex, T> cpy = new();

            foreach (EBossIndex i in EBossIndex.RobotMasters)
            {
                cpy[newOrder[i]] = portraitBG_y[i];
            }

            portraitBG_y = new Dictionary<EBossIndex, T>(cpy);
            cpy.Clear();

            foreach (EBossIndex i in EBossIndex.RobotMasters)
            {
                cpy[newOrder[i]] = portraitBG_x[i];
            }

            portraitBG_x = new Dictionary<EBossIndex, T>(cpy);
        }

        /// <summary>
        /// Shuffle the Robot Master stages.  This shuffling will not be indicated by the Robot Master portraits.
        /// </summary>
        public void Randomize(Patch in_Patch, RandomizationContext in_Context)
        {

            StageSelect = VanillaStageSelect();

            List<Byte> newStageOrder = new();
            Int32 count = StageSelect.Count;

            for (Byte i = 0; i < count; i++)
            {
                newStageOrder.Add(i);
            }

            newStageOrder = in_Context.Seed.Shuffle(newStageOrder).ToList();

            debug.AppendLine("Stage Select:");
            for (Int32 i = 0; i < count; i++)
            {
                StageFromSelect stage = StageSelect[i];

                // Change portrait destination
                stage.PortraitDestination.New = StageSelect[newStageOrder[i]].PortraitDestination.Old;

                // Erase the portrait text if StageNameHidden flag is set
                if (BooleanOption.True == in_Context.ActualizedBehaviorSettings?.GameplayOption.HideStageNames)
                {
                    for (Int32 k = 0; k < 6; k++)
                    {
                        // Write in a blank space at each letter ('f' by my cipher)
                        in_Patch.Add((Int32)stage.TextAddress + k, '`'.AsCreditsCharacter(), $"Hide Stage Select Portrait Text");
                    }

                    for (Int32 k = 0; k < 3; k++)
                    {
                        // Write in a blank space over "MAN"; 32 8-pixel tiles until the next row, 3 tiles until "MAN" text
                        in_Patch.Add((Int32)stage.TextAddress + 32 + 3 + k, '`'.AsCreditsCharacter(), $"Hide Stage Select Portrait Text");
                    }
                }
                // Change portrait text to match new destination
                else
                {
                    String newlabel = StageSelect[newStageOrder[i]].TextValues;
                    for (Int32 j = 0; j < newlabel.Length; j++)
                    {
                        Char c = newlabel[j];
                        in_Patch.Add((Int32)stage.TextAddress + j, c.AsCreditsCharacter(), $"Stage Select Portrait Text");
                    }
                }

                debug.AppendLine($"{Enum.GetName(typeof(EStageID), stage.PortraitDestination.Old)}'s portrait -> {Enum.GetName(typeof(EStageID), StageSelect[i].PortraitDestination.New)} stage");
            }

            foreach (StageFromSelect stage in StageSelect)
            {
                in_Patch.Add((Int32)stage.PortraitAddress, (Byte)stage.PortraitDestination.New, $"Stage Select {stage.PortraitName} Destination");
            }
        }
    }

}
