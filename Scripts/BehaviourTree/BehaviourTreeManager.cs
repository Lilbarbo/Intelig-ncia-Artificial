using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BehaviourTreeManager : MonoBehaviour
{

    public enum TipoDeTrabalhador
    {
        Coletor,
        Refinador,
        Construtor,
    }

    public enum TipoDeRecuso
    {
        Nenhum,
        Madeira,
        Pedra,
        Comida,
        MadeiraRefinada,
        PedraRefinada,
    }
    
    [Header("Tipo de trabalhador")]
    public TipoDeTrabalhador tipoDeTrabalhador;

    [Header("Configs Fome")]
    public float fomeMaxima = 100f;
    public float fomeAtual;
    public Transform posicaoDepositoComida;
    public DepositoComida scriptDepositoComida;
    
    [Header("Configs Patrulha")]
    public List<Transform> patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Configs detecção e distâncias")]
    public float detectionRange = 5f;
    public float distanceToCollectResource = 2f;
    public LayerMask layerArvore;
    public LayerMask layerPedra;
    public LayerMask layerComida;
    [HideInInspector] public Transform posicaoArvoreProxima = null;
    
    [Header("Componentes")]
    public NavMeshAgent navMeshAgent;
    
    [Header("Configs Depositos")]
    public Transform posicaoDepositoMateriaisBrutos;
    public DepositoMateriais scriptDepositoMateriaisBrutos;
    public Transform posicaoRefinaria;
    public Transform posicaoDepositoMateriaisRefinados;
    public DepositoRefinados scriptDepositoRefinados;
    public BehaviourTreeManager.TipoDeRecuso materialAguardado = BehaviourTreeManager.TipoDeRecuso.Nenhum;
    public string debugAguardando = "";
    
    [Header("Configs Cabanas")]
    public CabanasManager scripsCabanas;
    
    [Header("Configs Inventário")]
    public int capacidadeAtual = 0;
    public TipoDeRecuso tipoRecursoAtual = TipoDeRecuso.Nenhum; //Armazena o tipo de recurso do recurso(arvore,pedra,comida) em que estou a caminho
    public TipoDeRecuso recursoQueEstouCarregando; //Armazena o tipo de recurso que eu já coletei
    
    private Node rootNode;
    
    
    private void Awake() //Método chamado antes do Awake
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start() //Método chamado no primeiro frame
    {
        fomeAtual = fomeMaxima;
        InitializeTree();
    }

    void Update() //A cada frame
    {
        Tick();
        
        fomeAtual -=  Time.deltaTime;
        if (fomeAtual <= 0)
        {
            Morrer();
        }
    }

    private void Tick() //Método que verifica o STATUS que a árvore retornou (a todo momento)
    {
        if (rootNode == null) return;

        var status = rootNode.Process();

        switch (status)
        {
            case Status.Sucesso:
            case Status.Falha:
                rootNode.Reset();   
                break;

            case Status.EmAndamento:
            case Status.Desconhecido:
                break;
        }
    }


    private void InitializeTree() //Inicializa o NÓ RAIZ do TRABALHADOR selecionado (Coletor, Construtor ou Refinador)
    {
        switch (tipoDeTrabalhador)
        {
            case TipoDeTrabalhador.Coletor:
                rootNode = new ColetorRootNode();
                break;
            case TipoDeTrabalhador.Refinador:
                rootNode = new RefinadorRootNode();
                break;
            case TipoDeTrabalhador.Construtor:
                rootNode = new ConstrutorRootNode();
                break;
        }
        
        rootNode.Setup();
        rootNode.SetManager(this);
    }

    public void Patrulhar() //Fica se movimentando em pontos específicos do mapa
    {
        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        if (navMeshAgent.remainingDistance <= 2f)
        {
            currentPatrolIndex = Random.Range(0, patrolPoints.Count);
        }
    }

    public bool ColetarRecurso(TipoDeRecuso tipo) //Coleta e armazena um determinado recurso
    {
        if (tipo == TipoDeRecuso.Nenhum) return false;

        if (tipoRecursoAtual == TipoDeRecuso.Nenhum)
        {
            tipoRecursoAtual = tipo;
            recursoQueEstouCarregando = tipo;
        }

        if (InventarioCheio())
        {
            return false;
        }
        
        capacidadeAtual++;
        return true;
    }

    public bool RemoverRecurso() //Remove o recurso do PRÓPRIO INVENTÁRIO
    {
        if (tipoRecursoAtual == TipoDeRecuso.Nenhum || InventarioVazio()) return false;
        
        capacidadeAtual--;
        tipoRecursoAtual = TipoDeRecuso.Nenhum;
        recursoQueEstouCarregando = TipoDeRecuso.Nenhum;
        
        return true;
    }

    public bool SeAlimentar() //Come
    {
        if(!EstouComFome()) return false;
        
        fomeAtual = fomeMaxima;
        return true;
    }
    
    public TipoDeRecuso PegarTipoDeRecursoAtual() //Pega a LAYER do RECURSO MAIS PRÓXIMO (posicaoArvoreProxima) e armazena
    {
        if (posicaoArvoreProxima == null)
            return TipoDeRecuso.Nenhum;

        int layer = posicaoArvoreProxima.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Arvore"))
            return TipoDeRecuso.Madeira;

        if (layer == LayerMask.NameToLayer("Pedra"))
            return TipoDeRecuso.Pedra;
        
        if(layer == LayerMask.NameToLayer("Comida"))
            return TipoDeRecuso.Comida;

        return TipoDeRecuso.Nenhum;
    }

    public void Morrer() //Se deleta
    {
        scripsCabanas.trabalhadoresAtivos--;
        Destroy(gameObject);
    }

    public bool TemRecursoProximo() //Verifica se há recurso próximos e armazena sua POSIÇÃO
    {
        var layerRecurso = layerArvore | layerPedra | layerComida;
        var colliderArvores = Physics.OverlapSphere(transform.position, detectionRange, layerRecurso);
        if (colliderArvores.Length == 0)
        {
            posicaoArvoreProxima = null;
            return false;
        }
        
        float menorDistancia = float.MaxValue;
        Transform arvoreMaisProxima = null;
        
        foreach (var colliderArvore in colliderArvores)
        {
            float distancia = Vector3.Distance(transform.position, colliderArvore.transform.position);
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                arvoreMaisProxima = colliderArvore.transform;
            }
        }
        
        posicaoArvoreProxima = arvoreMaisProxima;
        return posicaoArvoreProxima != null;
    }
    
    public void RemoverRecursoDoMapa() //Destroi o objeto do recurso coletado
    {
        if (posicaoArvoreProxima == null) return;
        Destroy(posicaoArvoreProxima.gameObject);
    }

    //Verificações em relação ao INVENTÁRIO ou FOME do trabalhador
    public bool EstouComFome() => fomeAtual <= fomeMaxima * 0.3;
    public bool InventarioCheio() => capacidadeAtual >= 1;
    public bool InventarioVazio() => capacidadeAtual == 0;
    public bool TemMadeira() => recursoQueEstouCarregando == TipoDeRecuso.Madeira; 
    public bool TemPedra() => recursoQueEstouCarregando == TipoDeRecuso.Pedra;
    public bool TemComida() =>  recursoQueEstouCarregando == TipoDeRecuso.Comida;
    public bool TemPedraRefinada() => recursoQueEstouCarregando == TipoDeRecuso.PedraRefinada;
    public bool TemMadeiraRefinada() => recursoQueEstouCarregando == TipoDeRecuso.MadeiraRefinada;
}

