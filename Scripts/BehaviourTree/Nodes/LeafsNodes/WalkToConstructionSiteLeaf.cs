using UnityEngine;

public class WalkToConstructionSiteLeaf : LeafNode
{
    public override string name { get; protected set; } =  "WalkToConstructionSiteLeaf";

    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        if (MyManager.InventarioVazio()) return Status.Falha;

        var distance = Vector3.Distance(MyManager.transform.position,
            MyManager.scripsCabanas.posicaoCasaEmConstrucao.position);
        if (distance <= 5f)
        {
            return Status.Sucesso;
        }
        
        MyManager.navMeshAgent.SetDestination(MyManager.scripsCabanas.posicaoCasaEmConstrucao.position);
        return Status.EmAndamento;
    }
}
