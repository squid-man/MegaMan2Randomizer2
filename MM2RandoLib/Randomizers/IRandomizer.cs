using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers
{
    public interface IRandomizer
    {
        void Randomize(Patch in_Patch, RandomizationContext in_Context);
    }
}
