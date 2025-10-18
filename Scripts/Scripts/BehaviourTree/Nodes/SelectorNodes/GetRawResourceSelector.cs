using UnityEngine;

public class GetRawResourceSelector : SelectorNode
{
    public override string name { get; protected set; } =  "GetRawResourceSelector";

    public override void Setup()
    {
        AddChild(new GetRawResourceSequence());
        AddChild(new WaitToDepositLeaf());
    }
}
