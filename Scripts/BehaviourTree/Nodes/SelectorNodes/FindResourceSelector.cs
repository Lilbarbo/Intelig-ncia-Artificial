using UnityEngine;

public class FindResourceSelector : SelectorNode
{
    public override string name { get; protected set; } = "Find Resource Selector";
    public override void Setup()
    {
        AddChild(new CollectResourceSequence());
        AddChild(new PatrolLeaf());
    }
}
