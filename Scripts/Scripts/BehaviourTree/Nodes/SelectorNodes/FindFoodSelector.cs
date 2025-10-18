using UnityEngine;

public class FindFoodSelector : SelectorNode
{
    public override string name { get; protected set; }  = "Find Food Selector";

    public override void Setup()
    {
        AddChild(new FindFoodSequence());
        AddChild(new DieLeaf());
    }
}
