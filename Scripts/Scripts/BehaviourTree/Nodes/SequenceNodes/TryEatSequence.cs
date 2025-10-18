using UnityEngine;

public class TryEatSequence : SequenceNode
{
    public override string name { get; protected set; } =  "TryEatSequence";

    public override void Setup()
    {
        AddChild(new WaitForFoodLeaf());
        AddChild(new EatFoodLeaf());
    }
}
