using UnityEngine;

public class WaitToDepositLeaf : LeafNode
{
    public override string name { get; protected set; } = "WaitToDepositLeaf";
    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        var trabalhador = MyManager.tipoDeTrabalhador;

        if (trabalhador == BehaviourTreeManager.TipoDeTrabalhador.Coletor)
        {
            if (MyManager.InventarioVazio()) return Status.Falha;

            var recurso = MyManager.PegarTipoDeRecursoAtual();
            if (recurso == BehaviourTreeManager.TipoDeRecuso.Madeira || recurso == BehaviourTreeManager.TipoDeRecuso.Pedra)
            {
                if (!MyManager.scriptDepositoMateriaisBrutos.DepositoLotado()) return Status.Sucesso;
            }
            else if (recurso == BehaviourTreeManager.TipoDeRecuso.Comida)
            {
                if(!MyManager.scriptDepositoComida.DepositoLotado()) return Status.Sucesso;
            }
        
            Debug.Log("WaitToDepositLeaf Process");
        }

        if (trabalhador == BehaviourTreeManager.TipoDeTrabalhador.Refinador)
        {
            if (MyManager.TemMadeiraRefinada() || MyManager.TemPedraRefinada())
            {
                Debug.Log("Estou esperando para depositar um recurso refinado");
                if(!MyManager.scriptDepositoRefinados.DepositoLotado()) return  Status.Sucesso;
            }
            else
            {
                if (MyManager.InventarioCheio()) return Status.Falha;
            
                Debug.Log("Eu refinador estou esperando para pegar um recurso bruto");
            
                if(!MyManager.scriptDepositoMateriaisBrutos.DepositoVazio()) return Status.Sucesso;   
            }
        }

       if (trabalhador == BehaviourTreeManager.TipoDeTrabalhador.Construtor)
{
    if (MyManager.InventarioCheio()) 
        return Status.Falha;

    var cabana   = MyManager.scripsCabanas;   
    var deposito = MyManager.scriptDepositoRefinados;

    if (cabana == null) 
    {
        MyManager.materialAguardado = BehaviourTreeManager.TipoDeRecuso.Nenhum;
        MyManager.debugAguardando   = "sem referência da cabana";
        return Status.Falha;
    }

    if (cabana.Completa())
    {
        MyManager.materialAguardado = BehaviourTreeManager.TipoDeRecuso.Nenhum;
        MyManager.debugAguardando   = "cabana completa";
        return Status.Falha; // nada a esperar
    }

    bool precisaM = cabana.PrecisaMadeira();
    bool precisaP = cabana.PrecisaPedra();

    bool temM = deposito.TemMadeiraRefinada();
    bool temP = deposito.TemPedraRefinada();

    if (precisaM && !precisaP)
    {
        MyManager.materialAguardado = BehaviourTreeManager.TipoDeRecuso.MadeiraRefinada;
        MyManager.debugAguardando   = "esperando madeira refinada";
        return temM ? Status.Sucesso : Status.EmAndamento;
    }
    else if (precisaP && !precisaM)
    {
        MyManager.materialAguardado = BehaviourTreeManager.TipoDeRecuso.PedraRefinada;
        MyManager.debugAguardando   = "esperando pedra refinada";
        return temP ? Status.Sucesso : Status.EmAndamento;
    }
    else // precisaM && precisaP  -> faltam as duas
    {

        if (!temM && !temP)
        {
            MyManager.materialAguardado = BehaviourTreeManager.TipoDeRecuso.Nenhum; // sem preferência (qualquer um)
            MyManager.debugAguardando   = "esperando qualquer material (depósito vazio)";
            return Status.EmAndamento;
        }
        
        MyManager.materialAguardado = temM 
            ? BehaviourTreeManager.TipoDeRecuso.MadeiraRefinada 
            : BehaviourTreeManager.TipoDeRecuso.PedraRefinada;

        MyManager.debugAguardando = temM ? "madeira disponível (ok para coletar)"
                                         : "pedra disponível (ok para coletar)";
        return Status.Sucesso;
    }
}
      
        MyManager.navMeshAgent.SetDestination(MyManager.transform.position);
        return Status.EmAndamento;
    }
}
