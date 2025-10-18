using UnityEngine;

public class WaitToRefineLeaf : LeafNode
{
    public override string name { get; protected set; } =  "WaitToRefineLeaf";

    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        if (MyManager.InventarioVazio()) return Status.Falha;
        
        Debug.Log("Estou esperando na refinaria");
        return Status.EmAndamento;
    }
}
