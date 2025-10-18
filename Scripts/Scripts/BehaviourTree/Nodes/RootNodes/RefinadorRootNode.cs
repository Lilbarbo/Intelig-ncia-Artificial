using Unity.VisualScripting;
using UnityEngine;

public class RefinadorRootNode : SelectorNode
{
    public override string name { get; protected set; } =  "RefinadorRootNode";

    public override void Setup()
    {
        AddChild(new FindFoodSelector());
        AddChild(new GetRawResourceSelector());
        AddChild(new RefineResourceSelector());
    }
}
