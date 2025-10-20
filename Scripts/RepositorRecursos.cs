using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class RepositorRecursos : MonoBehaviour
{
    public enum Recursos { Madeira, Pedra, Comida }

    [Header("Estoque de nós para colocar no mapa")]
    public int numeroDeRecursosDisponiveis = 10;
    public int estoqueMaximo = 999;
    public TextMeshProUGUI totalRecursosText;

    [Header("Raycast / NavMesh")]
    public LayerMask layerChao;
    [SerializeField] private float maxProjDist = 0.5f;
    [SerializeField] private string areaName = "Walkable";
    [SerializeField] private bool exigirPontoExatoNoNavMesh = false;

    [Header("Prefabs")]
    public Recursos recursoSelecionado = Recursos.Madeira;
    [SerializeField] private GameObject[] prefabArvore;
    [SerializeField] private GameObject[] prefabPedra;
    [SerializeField] private GameObject[] prefabsComidas;

    [Header("Geração por tempo")]
    public TextMeshProUGUI recursosPorMinutoText;
    public bool gerarAutomaticamente = true;
    public float basePorMinuto = 2f;
    public float porCasaPorMinuto = 1f;
    public CabanasManager cabanasManager;

    private float acumuladorFracionario = 0f;
    private int areaMask;

    void Awake()
    {
        if (cabanasManager == null) cabanasManager = FindObjectOfType<CabanasManager>();
        int areaIndex = NavMesh.GetAreaFromName(areaName);
        areaMask = (areaIndex < 0) ? NavMesh.AllAreas : (1 << areaIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && numeroDeRecursosDisponiveis > 0)
            ColocarNovoRecurso();

        if (gerarAutomaticamente && numeroDeRecursosDisponiveis < estoqueMaximo)
            GerarRecursosAoLongoDoTempo();
    }

    private void GerarRecursosAoLongoDoTempo()
    {
        int casas = (cabanasManager != null) ? cabanasManager.totalCasasConstruidas : 0;

        float taxaPorSegundo = (basePorMinuto + porCasaPorMinuto * casas) / 60f;
        recursosPorMinutoText.text = "Recursos para repor por minuto: " + taxaPorSegundo * 60;
        totalRecursosText.text = "Total de recursos para repor: " + numeroDeRecursosDisponiveis;
        
        if (taxaPorSegundo <= 0f) return;

        acumuladorFracionario += taxaPorSegundo * Time.deltaTime;

        if (acumuladorFracionario >= 1f)
        {
            int gerar = Mathf.FloorToInt(acumuladorFracionario);
            acumuladorFracionario -= gerar;
            numeroDeRecursosDisponiveis = Mathf.Min(numeroDeRecursosDisponiveis + gerar, estoqueMaximo);
        }
    }
    

    private void ColocarNovoRecurso()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerChao))
        {
            if (!NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, maxProjDist, areaMask))
                return;

            if (exigirPontoExatoNoNavMesh && Vector3.Distance(hit.point, navHit.position) > 0.01f)
                return;

            Vector3 pos = navHit.position;

            switch (recursoSelecionado)
            {
                case Recursos.Madeira:
                    if (prefabArvore != null && prefabArvore.Length > 0)
                    {
                        int a = Random.Range(0, prefabArvore.Length);
                        Instantiate(prefabArvore[a], pos, prefabArvore[a].transform.rotation);
                    }
                    break;
                case Recursos.Pedra:
                    if (prefabPedra != null && prefabPedra.Length > 0)
                    {
                        int p = Random.Range(0, prefabPedra.Length);
                        Instantiate(prefabPedra[p], pos, prefabPedra[p].transform.rotation);
                    }
                    break;
                case Recursos.Comida:
                    if (prefabsComidas != null && prefabsComidas.Length > 0)
                    {
                        int i = Random.Range(0, prefabsComidas.Length);
                        Instantiate(prefabsComidas[i], pos, prefabsComidas[i].transform.rotation);
                    }
                    break;
            }

            numeroDeRecursosDisponiveis = Mathf.Max(0, numeroDeRecursosDisponiveis - 1);
        }
    }

    public void SelecionarRecurso(string recurso = "Generico")
    {
        if (recurso == "Madeira") recursoSelecionado = Recursos.Madeira;
        if (recurso == "Pedra")   recursoSelecionado = Recursos.Pedra;
        if (recurso == "Comida")  recursoSelecionado = Recursos.Comida;
    }
}
