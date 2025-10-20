using UnityEngine;

public class DepositRefineResourceSequence : SequenceNode
{
    public override string name { get; protected set; } =   "DepositRefineResourceSequence";
    public override void Setup()
    {
        AddChild(new WaitToDepositLeaf());
        AddChild(new DepositResourceLeaf());
    }
}
