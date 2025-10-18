using UnityEngine;

public class DieLeaf : LeafNode
{
    public override string name { get; protected set; } = "Die Leaf";

    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        if (MyManager.fomeAtual > 0f) return Status.Falha;
        MyManager.Morrer();
        return Status.Sucesso;
    }
}
