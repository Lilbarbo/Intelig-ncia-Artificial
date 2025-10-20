using UnityEngine;

public class PatrolLeaf : LeafNode
{
    public override string name { get; protected set; } = "Patrol Leaf";


    public override void Setup()
    {
        
    }

    public override Status Process() 
    {
        if (MyManager.TemRecursoProximo()) return Status.Sucesso;
        
        MyManager.Patrulhar();
        return Status.EmAndamento;
    }
}
