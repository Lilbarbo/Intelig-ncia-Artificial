using UnityEngine;

public class RefineResourceSelector : SelectorNode
{
    public override string name { get; protected set; } =  "RefineResourceSelector";

    public override void Setup()
    {
        AddChild(new RefineResourceSequence());
        AddChild(new WaitToRefineLeaf());
    }
}
