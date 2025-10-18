using UnityEngine;

public class RefineResourceLeaf : LeafNode
{
    public override string name { get; protected set; } =  "RefineResourceLeaf";

    public float timeToRefine = 5f;

    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        if (MyManager.TemMadeiraRefinada() || MyManager.TemPedraRefinada()) return Status.Sucesso;
        
        timeToRefine -= Time.deltaTime;
        if (timeToRefine <= 0)
        {
            if (MyManager.TemMadeira())
            {
                MyManager.RemoverRecurso();
                MyManager.ColetarRecurso(BehaviourTreeManager.TipoDeRecuso.MadeiraRefinada);
                Debug.Log("Refinei Madeira");
                timeToRefine = 5f;
                return Status.Sucesso;
            }
            else if (MyManager.TemPedra())
            {
                MyManager.RemoverRecurso();
                MyManager.ColetarRecurso(BehaviourTreeManager.TipoDeRecuso.PedraRefinada);
                Debug.Log("Refinei Pedra");
                timeToRefine = 5f;
                return Status.Sucesso;
            }
        }

        Debug.Log("Estou refinando");
        return Status.EmAndamento;
    }
}
