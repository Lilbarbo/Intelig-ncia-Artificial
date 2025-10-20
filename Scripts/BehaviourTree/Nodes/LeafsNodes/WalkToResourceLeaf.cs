using UnityEngine;

public class WalkToResourceLeaf : LeafNode
{
    public override string name { get; protected set; } =  "WalkToResourceLeaf";

    public override void Setup()
    {
       
    }

    public override Status Process()
    {
        if (!MyManager.TemRecursoProximo() || MyManager.InventarioCheio()) return Status.Falha;
        if (MyManager.posicaoArvoreProxima == null) return Status.Falha;

        if (Vector3.Distance(MyManager.posicaoArvoreProxima.position, MyManager.transform.position) <=
            MyManager.distanceToCollectResource)
        {
            Debug.Log("Cheguei até o recurso");
            return Status.Sucesso;
        }

        if (MyManager.posicaoArvoreProxima != null)
        {
            MyManager.navMeshAgent.SetDestination(MyManager.posicaoArvoreProxima.position);
        }
        
        Debug.Log("Encontrei uma arvore e estou a caminho dela");
        return Status.EmAndamento;
    }
}
