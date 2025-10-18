using UnityEngine;

public class DepositResourceLeaf : LeafNode
{
    public override string name { get; protected set; } =  "DepositResourceLeaf";

    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        if (MyManager.InventarioVazio()) return Status.Falha;
        
        if (MyManager.TemMadeira())
        {
            if (MyManager.scriptDepositoMateriaisBrutos.DepositoLotado()) return Status.Falha;
            
            Debug.Log("Depositei madeira");
            MyManager.scriptDepositoMateriaisBrutos.AdicionarRecurso(BehaviourTreeManager.TipoDeRecuso.Madeira);
        }
        else if (MyManager.TemPedra())
        {
            if (MyManager.scriptDepositoMateriaisBrutos.DepositoLotado()) return Status.Falha;
            
            Debug.Log("Depositei madeira");
            MyManager.scriptDepositoMateriaisBrutos.AdicionarRecurso(BehaviourTreeManager.TipoDeRecuso.Pedra);
        }
        else if (MyManager.TemComida())
        {
            if (MyManager.scriptDepositoComida.DepositoLotado()) return Status.Falha;
            
            Debug.Log("Depositei comida");
            MyManager.scriptDepositoComida.ReceberOuRetirarComida(1);
        }
        else if (MyManager.TemMadeiraRefinada())
        {
            if (MyManager.tipoDeTrabalhador == BehaviourTreeManager.TipoDeTrabalhador.Refinador)
            {
                if(MyManager.scriptDepositoRefinados.DepositoLotado()) return Status.Falha;
            
                MyManager.scriptDepositoRefinados.ReceberOuRetirarMaterial(BehaviourTreeManager.TipoDeRecuso.MadeiraRefinada, 1);
            }
            else if (MyManager.tipoDeTrabalhador == BehaviourTreeManager.TipoDeTrabalhador.Construtor)
            {
                if (!MyManager.scripsCabanas.PrecisaMadeira() || MyManager.scripsCabanas.Completa())
                {
                    return Status.Falha;
                }
                
                MyManager.scripsCabanas.ReceberMaterial(BehaviourTreeManager.TipoDeRecuso.MadeiraRefinada);
            }
        }
        else if (MyManager.TemPedraRefinada())
        {
            if (MyManager.tipoDeTrabalhador == BehaviourTreeManager.TipoDeTrabalhador.Refinador)
            {
                if(MyManager.scriptDepositoRefinados.DepositoLotado()) return Status.Falha;
            
                MyManager.scriptDepositoRefinados.ReceberOuRetirarMaterial(BehaviourTreeManager.TipoDeRecuso.PedraRefinada, 1);
            }
            else if (MyManager.tipoDeTrabalhador == BehaviourTreeManager.TipoDeTrabalhador.Construtor)
            {
                if (!MyManager.scripsCabanas.PrecisaPedra() || MyManager.scripsCabanas.Completa())
                {
                    return Status.Falha;
                }
                
                MyManager.scripsCabanas.ReceberMaterial(BehaviourTreeManager.TipoDeRecuso.PedraRefinada);
            }
        }
        

        bool remover = MyManager.RemoverRecurso();
        if (remover)
        {
            Debug.Log("Deu tudo certo");
            return Status.Sucesso;
        }

        return Status.Falha;
    }
}
