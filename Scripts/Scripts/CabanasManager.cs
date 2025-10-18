using System.Collections.Generic;
using UnityEngine;

public class CabanasManager : MonoBehaviour
{
    [System.Serializable]
    public class CasaConfig
    {
        public Transform posicaoCasaEmConstrucao;
        public GameObject casaPronta;
        public int madeirasNecessarias = 0;
        public int pedrasNecessarias  = 0;
        [Tooltip("Se marcado, garante que a casa começa desativada (será ativada quando concluir).")]
        public bool iniciarCasaProntaDesativada = true;
    }

    [Header("Fila de casas (ordem de construção)")]
    [Tooltip("Preencha aqui as 5 casas na ordem. O script sempre trabalha em uma por vez.")]
    public List<CasaConfig> casas = new List<CasaConfig>(5);

    [Header("Estado atual (reflete SEMPRE a casa ativa)")]
    public Transform posicaoCasaEmConstrucao; // mantém o nome
    public GameObject casaPronta;             // mantém o nome

    public int madeirasNecessarias;           // mantém o nome
    public int pedrasNecessarias;             // mantém o nome

    public int quantidadeMadeirasAtual;       // mantém o nome
    public int quantidadePedrasAtual;         // mantém o nome

    [Header("Leitura/Debug")]
    [SerializeField] private int indiceCasaAtual = 0;
    [SerializeField] private bool todasCasasConstruidas = false;

    void Start()
    {
        // Garante um estado inicial consistente
        CarregarCasa(0);
    }

    // ==== API existente (mantida) =========================================================
    public bool PrecisaMadeira() => !todasCasasConstruidas && quantidadeMadeirasAtual < madeirasNecessarias;
    public bool PrecisaPedra()  => !todasCasasConstruidas && quantidadePedrasAtual  < pedrasNecessarias;
    public bool Completa()      => todasCasasConstruidas || (!PrecisaMadeira() && !PrecisaPedra());

    public void ReceberMaterial(BehaviourTreeManager.TipoDeRecuso tipo, int quantidade = 1)
    {
        if (todasCasasConstruidas) return; // nada a fazer

        if (tipo == BehaviourTreeManager.TipoDeRecuso.MadeiraRefinada)
        {
            if (MadeiraLotada()) return;
            Debug.Log($"[Cabanas] Recebi Madeira (Casa #{indiceCasaAtual+1})");
            quantidadeMadeirasAtual = Mathf.Min(quantidadeMadeirasAtual + quantidade, madeirasNecessarias);
        }
        else if (tipo == BehaviourTreeManager.TipoDeRecuso.PedraRefinada)
        {
            if (PedraLotada()) return;
            Debug.Log($"[Cabanas] Recebi Pedra (Casa #{indiceCasaAtual+1})");
            quantidadePedrasAtual = Mathf.Min(quantidadePedrasAtual + quantidade, pedrasNecessarias);
        }

        if (!todasCasasConstruidas && quantidadeMadeirasAtual >= madeirasNecessarias && quantidadePedrasAtual >= pedrasNecessarias)
            ConstruirCasa();
    }

    public bool MadeiraLotada() => quantidadeMadeirasAtual >= madeirasNecessarias;
    public bool PedraLotada()   => quantidadePedrasAtual  >= pedrasNecessarias;

    // ==== Internos ========================================================================
    private void CarregarCasa(int indice)
    {
        // Lista vazia: marca tudo concluído e evita NRE
        if (casas == null || casas.Count == 0)
        {
            todasCasasConstruidas = true;
            posicaoCasaEmConstrucao = null;
            casaPronta = null;
            madeirasNecessarias = pedrasNecessarias = 0;
            quantidadeMadeirasAtual = quantidadePedrasAtual = 0;
            Debug.LogWarning("[Cabanas] Nenhuma casa configurada.");
            return;
        }

        // Clampa índice
        indiceCasaAtual = Mathf.Clamp(indice, 0, casas.Count - 1);
        var cfg = casas[indiceCasaAtual];

        // Atualiza os CAMPOS EXISTENTES para apontarem para a casa ativa
        posicaoCasaEmConstrucao = cfg.posicaoCasaEmConstrucao;
        casaPronta             = cfg.casaPronta;
        madeirasNecessarias    = Mathf.Max(0, cfg.madeirasNecessarias);
        pedrasNecessarias      = Mathf.Max(0, cfg.pedrasNecessarias);

        // Zera progresso da casa ativa
        quantidadeMadeirasAtual = 0;
        quantidadePedrasAtual   = 0;

        // Garante estado inicial da casa (se configurado)
        if (casaPronta != null && cfg.iniciarCasaProntaDesativada)
            casaPronta.SetActive(false);

        todasCasasConstruidas = false;

        Debug.Log($"[Cabanas] Iniciando Casa #{indiceCasaAtual+1}/{casas.Count} | Necessário: {madeirasNecessarias} madeira, {pedrasNecessarias} pedra.");
    }

    private void ConstruirCasa()
    {
        if (casaPronta != null) casaPronta.SetActive(true);
        Debug.Log($"[Cabanas] Casa #{indiceCasaAtual+1} concluída!");

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

            // Mantém os campos com os valores da última casa concluída (compatibilidade),
            // mas as queries Precisa*/Completa() passam a refletir o término global.
            Debug.Log("[Cabanas] Todas as casas foram construídas!");
        }
    }
}
