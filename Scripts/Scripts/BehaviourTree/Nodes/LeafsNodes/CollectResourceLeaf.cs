using UnityEngine;

public class CollectResourceLeaf : LeafNode
{
    public override string name { get; protected set; } = "CollectResourceLeaf";

    private float _timeToCollect = 3f;

    public override void Setup() { }

    public override Status Process()
    {
        
        if (MyManager.tipoDeTrabalhador == BehaviourTreeManager.TipoDeTrabalhador.Coletor)
        {
            if (MyManager.InventarioCheio()) return Status.Falha;

            if (MyManager.posicaoArvoreProxima == null)
            {
                Debug.Log("WTF");
                return Status.Falha;
            }
            
            _timeToCollect -= Time.deltaTime;

            if (_timeToCollect <= 0)
            {
                var tipo = MyManager.PegarTipoDeRecursoAtual();

                Debug.Log($"Terminei de coletar {tipo}");

                bool coletei = MyManager.ColetarRecurso(tipo);

                if (coletei)
                {
                    Debug.Log($"Coletei {tipo}");
                    _timeToCollect = 3f;
                    //MyManager.recursoQueEstouCarregando = tipo;
                    MyManager.RemoverRecursoDoMapa();
                    return Status.Sucesso;
                }
                else
                {
                    Debug.LogError("Deu errado a coleta");
                    _timeToCollect = 3f;
                    return Status.Falha;
                }
            }
            
        }

        if (MyManager.tipoDeTrabalhador == BehaviourTreeManager.TipoDeTrabalhador.Refinador)
        {
            if(MyManager.InventarioCheio())  return Status.Falha;
            
            if (MyManager.scriptDepositoMateriaisBrutos.DepositoVazio()) return Status.Falha;
            
            if (MyManager.scriptDepositoMateriaisBrutos.TemMadeira() &&
                MyManager.scriptDepositoMateriaisBrutos.TemPedra())
            {
                int i = Random.Range(0, 2);
                if (i == 0)
                {
                    MyManager.ColetarRecurso(BehaviourTreeManager.TipoDeRecuso.Madeira);
                    MyManager.scriptDepositoMateriaisBrutos.AdicionarRecurso(BehaviourTreeManager.TipoDeRecuso.Madeira, -1);
                    return Status.Sucesso;
                }
                else
                {
                    MyManager.ColetarRecurso(BehaviourTreeManager.TipoDeRecuso.Pedra);
                    MyManager.scriptDepositoMateriaisBrutos.AdicionarRecurso(BehaviourTreeManager.TipoDeRecuso.Pedra, -1);
                    return Status.Sucesso;
                }
            }
            else if (MyManager.scriptDepositoMateriaisBrutos.TemMadeira())
            {
                MyManager.ColetarRecurso(BehaviourTreeManager.TipoDeRecuso.Madeira);
                MyManager.scriptDepositoMateriaisBrutos.AdicionarRecurso(BehaviourTreeManager.TipoDeRecuso.Madeira, -1);
                return Status.Sucesso;
            }
            else if (MyManager.scriptDepositoMateriaisBrutos.TemPedra())
            {
                MyManager.ColetarRecurso(BehaviourTreeManager.TipoDeRecuso.Pedra);
                MyManager.scriptDepositoMateriaisBrutos.AdicionarRecurso(BehaviourTreeManager.TipoDeRecuso.Pedra, -1);
                return Status.Sucesso;
            }
        }

        if (MyManager.tipoDeTrabalhador == BehaviourTreeManager.TipoDeTrabalhador.Construtor)
{
    if (MyManager.InventarioCheio()) return Status.Falha;

    var cabana   = MyManager.scripsCabanas;
    var deposito = MyManager.scriptDepositoRefinados;

    if (cabana == null || cabana.Completa()) return Status.Falha;

    bool precisaMadeira = cabana.PrecisaMadeira();
    bool precisaPedra   = cabana.PrecisaPedra();

    bool temMadeira = deposito.TemMadeiraRefinada();
    bool temPedra   = deposito.TemPedraRefinada();

    var alvo = BehaviourTreeManager.TipoDeRecuso.Nenhum;

    if (precisaMadeira && !precisaPedra)
    {
        alvo = temMadeira ? BehaviourTreeManager.TipoDeRecuso.MadeiraRefinada
                          : BehaviourTreeManager.TipoDeRecuso.Nenhum;
    }
    else if (precisaPedra && !precisaMadeira)
    {
        alvo = temPedra ? BehaviourTreeManager.TipoDeRecuso.PedraRefinada
                        : BehaviourTreeManager.TipoDeRecuso.Nenhum;
    }
    else 
    {
        if (temMadeira && temPedra)
        {
            alvo = (Random.Range(0, 2) == 0)
                ? BehaviourTreeManager.TipoDeRecuso.MadeiraRefinada
                : BehaviourTreeManager.TipoDeRecuso.PedraRefinada;
        }
        else if (temMadeira)
        {
            alvo = BehaviourTreeManager.TipoDeRecuso.MadeiraRefinada;
        }
        else if (temPedra)
        {
            alvo = BehaviourTreeManager.TipoDeRecuso.PedraRefinada;
        }
        else
        {
            alvo = BehaviourTreeManager.TipoDeRecuso.Nenhum;
        }
    }

    if (alvo == BehaviourTreeManager.TipoDeRecuso.Nenhum)
        return Status.Falha;

    MyManager.ColetarRecurso(alvo);
    deposito.ReceberOuRetirarMaterial(alvo, -1);
    return Status.Sucesso;
}
        
        return Status.EmAndamento;
    }
}