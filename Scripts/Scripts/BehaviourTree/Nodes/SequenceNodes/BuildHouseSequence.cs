using Unity.VisualScripting;
using UnityEngine;

public class BuildHouseSequence : SequenceNode
{
    public override string name { get; protected set; } =  "BuildHouseSequence";

    public override void Setup()
    {
        AddChild(new WalkToConstructionSiteLeaf());
        AddChild(new DepositResourceLeaf());
    }
}
