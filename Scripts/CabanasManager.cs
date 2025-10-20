using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CabanasManager : MonoBehaviour
{
    [System.Serializable]
    public class CasaConfig
    {
        public Transform posicaoCasaEmConstrucao;
        public GameObject casaPronta;
        public int madeirasNecessarias = 0;
        public int pedrasNecessarias  = 0;
        public bool iniciarCasaProntaDesativada = true;
    }
    
    [Header("Configs Trabalhadores")]
    [SerializeField] private TextMeshProUGUI totalDeTrabalhadoresText;
    [SerializeField] private List<GameObject> trabalhadoresDesativados;
    [Range(0, 5)][SerializeField] private int trabalhadoresNovosPorCasa = 3;
    [HideInInspector] public int trabalhadoresAtivos;
    
    [Header("Fila de casas (ordem de construção)")]
    public List<CasaConfig> casas = new List<CasaConfig>(5);

    [Header("Estado atual")]
    public Transform posicaoCasaEmConstrucao; 
    public GameObject casaPronta;             

    public int madeirasNecessarias;           
    public int pedrasNecessarias;             

    public int quantidadeMadeirasAtual;
    public int quantidadePedrasAtual;      

    [Header("Leitura/Debug")]
    [SerializeField] private int indiceCasaAtual = 0;
    [SerializeField] private bool todasCasasConstruidas = false;
    public int totalCasasConstruidas = 0;
    
    [Header("UI Vitória/Derrota")]
    public GameObject telaDerrota;
    public GameObject telaVitoria;

    void Start()
    {
        Time.timeScale = 1;
        
        telaDerrota.SetActive(false);
        telaVitoria.SetActive(false);
        
        trabalhadoresAtivos = 3;
        foreach (var t in trabalhadoresDesativados)
        {
            t.SetActive(false);
        }
        CarregarCasa(0);
    }

    private void Update()
    {
        AtualizarUI();
        if (trabalhadoresAtivos <= 0)
        {
            Derrota();
        }
    }

    public bool PrecisaMadeira() => !todasCasasConstruidas && quantidadeMadeirasAtual < madeirasNecessarias;
    public bool PrecisaPedra()  => !todasCasasConstruidas && quantidadePedrasAtual  < pedrasNecessarias;
    public bool Completa()      => todasCasasConstruidas || (!PrecisaMadeira() && !PrecisaPedra());

    public void ReceberMaterial(BehaviourTreeManager.TipoDeRecuso tipo, int quantidade = 1)
    {
        if (todasCasasConstruidas) return;

        if (tipo == BehaviourTreeManager.TipoDeRecuso.MadeiraRefinada)
        {
            if (MadeiraLotada()) return;
            quantidadeMadeirasAtual = Mathf.Min(quantidadeMadeirasAtual + quantidade, madeirasNecessarias);
        }
        else if (tipo == BehaviourTreeManager.TipoDeRecuso.PedraRefinada)
        {
            if (PedraLotada()) return;
            quantidadePedrasAtual = Mathf.Min(quantidadePedrasAtual + quantidade, pedrasNecessarias);
        }

        if (!todasCasasConstruidas && quantidadeMadeirasAtual >= madeirasNecessarias && quantidadePedrasAtual >= pedrasNecessarias)
            ConstruirCasa();
    }

    public bool MadeiraLotada() => quantidadeMadeirasAtual >= madeirasNecessarias;
    public bool PedraLotada()   => quantidadePedrasAtual  >= pedrasNecessarias;
    
    private void CarregarCasa(int indice)
    {
        if (casas == null || casas.Count == 0)
        {
            todasCasasConstruidas = true;
            posicaoCasaEmConstrucao = null;
            casaPronta = null;
            madeirasNecessarias = pedrasNecessarias = 0;
            quantidadeMadeirasAtual = quantidadePedrasAtual = 0;
            return;
        }

        indiceCasaAtual = Mathf.Clamp(indice, 0, casas.Count - 1);
        var cfg = casas[indiceCasaAtual];

        posicaoCasaEmConstrucao = cfg.posicaoCasaEmConstrucao;
        casaPronta             = cfg.casaPronta;
        madeirasNecessarias    = Mathf.Max(0, cfg.madeirasNecessarias);
        pedrasNecessarias      = Mathf.Max(0, cfg.pedrasNecessarias);

        quantidadeMadeirasAtual = 0;
        quantidadePedrasAtual   = 0;

        if (casaPronta != null && cfg.iniciarCasaProntaDesativada)
            casaPronta.SetActive(false);

        todasCasasConstruidas = false;

    }

    private void ChamarNovosTrabalhadores()
    {
        for (int i = 0; i < trabalhadoresNovosPorCasa; i++)
        {
            var trabalhadorAleatorio = trabalhadoresDesativados[Random.Range(0, trabalhadoresDesativados.Count - 1)];
            trabalhadorAleatorio.SetActive(true);
            trabalhadoresDesativados.Remove(trabalhadorAleatorio);
            trabalhadoresAtivos++;
        }
    }

    private void ConstruirCasa()
    {
        if (casaPronta != null) casaPronta.SetActive(true);

        totalCasasConstruidas++;
        
        ChamarNovosTrabalhadores();

        AvancarParaProximaCasa();
    }

    private void AvancarParaProximaCasa()
    {
        int proxima = indiceCasaAtual + 1;
        if (proxima < casas.Count)
        {
            CarregarCasa(proxima);
        }
        else
        {
            // Terminou TODAS
            todasCasasConstruidas = true;
            Vitoria();
        }
    }

    private void AtualizarUI()
    {
        totalDeTrabalhadoresText.text = "Trabalhadores ativos: " + trabalhadoresAtivos;
    }

    private void Derrota()
    {
        Time.timeScale = 0;
        telaDerrota.SetActive(true);
    }

    private void Vitoria()
    {
        Time.timeScale = 0;
        telaVitoria.SetActive(true);
    }
}