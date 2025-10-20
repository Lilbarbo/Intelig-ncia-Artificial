using UnityEngine;

public class GetRefineResourceSelector : SelectorNode
{
    public override string name { get; protected set; } = "GetRefineResourceSelector";

    public override void Setup()
    {
        AddChild(new GetRefineResourceSequence());
        AddChild(new WaitToDepositLeaf());
    }
}
