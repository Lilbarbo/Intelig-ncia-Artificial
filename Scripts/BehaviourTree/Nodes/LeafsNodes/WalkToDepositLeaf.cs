using UnityEngine;

public class WalkToDepositLeaf : LeafNode
{
    public override string name { get; protected set; } =  "WalkToDepositLeaf";

    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        if (MyManager.tipoDeTrabalhador == BehaviourTreeManager.TipoDeTrabalhador.Coletor)
        {
            if (MyManager.InventarioVazio()) return Status.Falha;
            
            if (MyManager.recursoQueEstouCarregando == BehaviourTreeManager.TipoDeRecuso.Comida)
            {
                var distancia1 = Vector3.Distance(MyManager.transform.position,MyManager.posicaoDepositoComida.position);
                if (distancia1 <= 5f)
                {
                    Debug.Log("Cheguei ao deposito de comida");
                    return Status.Sucesso;
                }
                Debug.Log("Estou indo para o deposito de comida");
                MyManager.navMeshAgent.SetDestination(MyManager.posicaoDepositoComida.position);
            }
            else
            {
                var distancia = Vector3.Distance(MyManager.transform.position,MyManager.posicaoDepositoMateriaisBrutos.position);
                if (distancia <= 5f)
                {
                    Debug.Log("Cheguei ao deposito de materiais");
                    return Status.Sucesso;
                }
                Debug.Log("Estou indo para o deposito de materiais");
                MyManager.navMeshAgent.SetDestination(MyManager.posicaoDepositoMateriaisBrutos.position);
            }
        }
        else if (MyManager.tipoDeTrabalhador == BehaviourTreeManager.TipoDeTrabalhador.Refinador)
        {
            if(MyManager.InventarioCheio()) return Status.Falha; 
            
            var distancia = Vector3.Distance(MyManager.transform.position,MyManager.posicaoDepositoMateriaisBrutos.position);
            if (distancia <= 5f)
            {
                Debug.Log("Cheguei ao deposito de materiais");
                return Status.Sucesso;
            }
            Debug.Log("Estou indo para o deposito de materiais");
            MyManager.navMeshAgent.SetDestination(MyManager.posicaoDepositoMateriaisBrutos.position);
        }
        
        return Status.EmAndamento;
    }
}
