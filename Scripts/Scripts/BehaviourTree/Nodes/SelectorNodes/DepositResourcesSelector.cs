using UnityEngine;

public class DepositResourcesSelector : SelectorNode
{
    public override string name { get; protected set; } =  "DepositResourcesSelector";

    public override void Setup()
    {
        AddChild(new DepositResourcesSequence());
        AddChild(new WaitToDepositLeaf());
    }
}
