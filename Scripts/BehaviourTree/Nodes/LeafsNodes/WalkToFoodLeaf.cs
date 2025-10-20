using UnityEngine;

public class WalkToFoodLeaf : LeafNode
{
    public override string name { get; protected set; } =  "WalkToFoodLeaf";

    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        if (!MyManager.EstouComFome() || MyManager.fomeAtual <= 0f) return Status.Falha;
        
        Debug.Log("AI QUE FOME");
        
        var distance = Vector3.Distance(MyManager.transform.position, MyManager.posicaoDepositoComida.position);
        if (distance <= 4f)
        {
            Debug.Log("Cheguei no rango");
            return Status.Sucesso;
        }
        
        MyManager.navMeshAgent.SetDestination(MyManager.posicaoDepositoComida.position);
        return Status.EmAndamento;
    }
}
