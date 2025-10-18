using Unity.VisualScripting;
using UnityEngine;

public class ColetorRootNode : SelectorNode
{
    public override string name { get; protected set; } = "Coletor Root Node";
    public override void Setup()
    {
        AddChild(new FindFoodSelector());
        AddChild(new DepositResourcesSelector());
        AddChild(new FindResourceSelector());
    }
}
