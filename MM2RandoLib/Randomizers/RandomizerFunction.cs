using MM2Randomizer.Patcher;
using System;
using System.Collections.Generic;
using System.Text;

namespace MM2Randomizer.Randomizers;

public class RandomizerFunction : Randomizer
{
    public override string Name { get; }

    public RandomizerFunction(
        string name,
        Action<Patch, RandomizationContext> randomize,
        Action<RandomizationContext>? produceWithoutRandomization = null,
        IEnumerable<string>? dependencies = null, 
        IEnumerable<string>? products = null) 
        : base(dependencies, products)
    {
        Name = name;

        RandomizeFunction = randomize;
        ProduceWithoutRandomizationFunction = produceWithoutRandomization;
    }

    public override void Randomize(Patch patch, RandomizationContext context)
        => RandomizeFunction(patch, context);

    public override void ProduceWithoutRandomization(RandomizationContext context)
    {
        if (ProduceWithoutRandomizationFunction != null)
            ProduceWithoutRandomizationFunction(context);
    }

    Action<Patch, RandomizationContext> RandomizeFunction;
    Action<RandomizationContext>? ProduceWithoutRandomizationFunction;
}
