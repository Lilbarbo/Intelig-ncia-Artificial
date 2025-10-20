using UnityEngine;

public class DepositRefineResourcesSelector : SelectorNode
{
    public override string name { get; protected set; } =  "DepositRefineResourcesSelector";

    public override void Setup()
    {
        AddChild(new DepositResourceLeaf());
        AddChild(new WaitToDepositLeaf());
    }
}
