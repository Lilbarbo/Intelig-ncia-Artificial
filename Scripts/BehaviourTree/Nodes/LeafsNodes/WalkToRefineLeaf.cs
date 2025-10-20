using UnityEngine;

public class WalkToRefineLeaf : LeafNode 
{
    public override string name { get; protected set; } = "WalkToRefineLeaf";

    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        if (MyManager.TemMadeiraRefinada() || MyManager.TemPedraRefinada()) return Status.Sucesso; 
        if (!MyManager.TemPedra() && !MyManager.TemMadeira()) return  Status.Falha;
        
        var distancia = Vector3.Distance(MyManager.transform.position, MyManager.posicaoRefinaria.position);
        if (distancia <= 6f)
        {
            return Status.Sucesso;
        }

        MyManager.navMeshAgent.SetDestination(MyManager.posicaoRefinaria.position);
        return Status.EmAndamento;
    }
}
