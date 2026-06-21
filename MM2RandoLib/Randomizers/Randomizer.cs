using MM2Randomizer.Patcher;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MM2Randomizer.Randomizers;

public abstract class Randomizer
{
    // Dependencies are things that must be produced before this randomizer runs, and Products are things produced by this randomizer that may be dependencies of other randomizers. By convention both refer to fields of RandomizationContext that should be referenced using nameof, though legacy randomizers that aren't updated to pass info this way could use their own name.
    public IReadOnlyList<string> Dependencies { get; }
    public IReadOnlyList<string> Products { get; }

    public Randomizer(IEnumerable<string>? dependencies = null, IEnumerable<string>? products = null)
    {
        Dependencies = dependencies != null
            ? dependencies.ToArray()
            : Array.Empty<string>();
        Products = products != null
            ? products.ToArray() 
            : Array.Empty<string>();
    }

    /// <summary>
    /// Called to apply the randomizer.
    /// </summary>
    /// <param name="in_Patch"></param>
    /// <param name="in_Context"></param>
    public abstract void Randomize(Patch in_Patch, RandomizationContext in_Context);

    /// <summary>
    /// Called when the randomizer is not enabled. Must generate any products of the randomizer so that randomizers that depend on these products will work correctly.
    /// </summary>
    /// <param name="in_Context"></param>
    /// <exception cref="Exception"></exception>
    public virtual void ProduceWithoutRandomization(RandomizationContext in_Context)
    {
        if (Products.Count != 0)
            throw new Exception($"Randomizer.ProduceWithoutRandomization must be overridden in {this.GetType().Name}");
    }
}
