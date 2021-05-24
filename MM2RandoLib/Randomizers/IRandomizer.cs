using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Randomizers
{
    public interface IRandomizer
    {
        void Randomize(Patch in_Patch, Settings in_Settings, ISeed in_Seed);
    }
}
