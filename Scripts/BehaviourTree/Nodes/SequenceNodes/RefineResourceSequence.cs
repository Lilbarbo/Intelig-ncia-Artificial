using UnityEngine;

public class RefineResourceSequence : SequenceNode
{
    public override string name { get; protected set; } = "GetRawResourceSelector";

    public override void Setup()
    {
        AddChild(new WalkToRefineLeaf());
        AddChild(new RefineResourceLeaf());
        AddChild(new WalkToRefineDepositLeaf());
        AddChild(new DepositRefineResourceSequence());
    }
}
