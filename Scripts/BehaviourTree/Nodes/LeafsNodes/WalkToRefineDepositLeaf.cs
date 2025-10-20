using UnityEngine;

public class WalkToRefineDepositLeaf : LeafNode
{
    public override string name { get; protected set; }  = "WalkToRefineDepositLeaf";

    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        if (MyManager.tipoDeTrabalhador == BehaviourTreeManager.TipoDeTrabalhador.Refinador)
        {
            if (!MyManager.TemMadeiraRefinada() && !MyManager.TemPedraRefinada())
                return Status.Falha;
        
            var distance = Vector3.Distance(MyManager.transform.position, MyManager.posicaoDepositoMateriaisRefinados.position);
            if (distance <= 6f)
            {
                return Status.Sucesso;
            }
        }

        if (MyManager.tipoDeTrabalhador == BehaviourTreeManager.TipoDeTrabalhador.Construtor)
        {
            if (MyManager.TemMadeiraRefinada() || MyManager.TemMadeiraRefinada()) return Status.Falha;
            
            
            var distance = Vector3.Distance(MyManager.transform.position, MyManager.posicaoDepositoMateriaisRefinados.position);
            if (distance <= 6f)
            {
                return Status.Sucesso;
            }
        }

        MyManager.navMeshAgent.SetDestination(MyManager.posicaoDepositoMateriaisRefinados.position);
        return Status.EmAndamento;
    }
}
