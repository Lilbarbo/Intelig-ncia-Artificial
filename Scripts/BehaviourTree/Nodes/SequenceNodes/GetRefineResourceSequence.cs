using UnityEngine;

public class GetRefineResourceSequence : SequenceNode
{
    public override string name { get; protected set; } =  "GetRefineResourceSequence";

    public override void Setup()
    {
        AddChild(new WalkToRefineDepositLeaf());
        AddChild(new CollectResourceLeaf());
    }
}
