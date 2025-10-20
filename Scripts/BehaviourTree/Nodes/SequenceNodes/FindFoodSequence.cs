using UnityEngine;

public class FindFoodSequence : SequenceNode
{
    public override string name { get; protected set; } =  "Find Food Sequence";

    public override void Setup()
    {
        AddChild(new WalkToFoodLeaf());
        AddChild(new TryEatSequence());
    }
}
