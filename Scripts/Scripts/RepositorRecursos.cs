using UnityEngine;
using UnityEngine.AI; // <-- importante

public class RepositorRecursos : MonoBehaviour
{
    public enum Recursos { Madeira, Pedra, Comida }

    public int numeroDeRecursosDisponiveis = 10;
    public LayerMask layerChao;

    public Recursos recursoSelecionado = Recursos.Madeira;

    [SerializeField] private GameObject prefabArvore;
    [SerializeField] private GameObject prefabPedra;
    [SerializeField] private GameObject[] prefabsComidas;

    [SerializeField] private float maxProjDist = 0.5f;
    [SerializeField] private string areaName = "Walkable";
    [SerializeField] private bool exigirPontoExatoNoNavMesh = false;

    private int areaMask;

    void Awake()
    {
        int areaIndex = NavMesh.GetAreaFromName(areaName);
        areaMask = (areaIndex < 0) ? NavMesh.AllAreas : (1 << areaIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && numeroDeRecursosDisponiveis > 0)
        {
            ColocarNovoRecurso();
        }
    }

    private void ColocarNovoRecurso()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerChao))
        {
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, maxProjDist, areaMask))
            {
                if (exigirPontoExatoNoNavMesh && Vector3.Distance(hit.point, navHit.position) > 0.01f)
                {
                    return;
                }

                Vector3 pos = navHit.position;

                switch (recursoSelecionado)
                {
                    case Recursos.Madeira:
                        Instantiate(prefabArvore, pos, prefabArvore.transform.rotation);
                        break;
                    case Recursos.Pedra:
                        Instantiate(prefabPedra, pos, prefabPedra.transform.rotation);
                        break;
                    case Recursos.Comida:
                        int i = Random.Range(0, prefabsComidas.Length);
                        Instantiate(prefabsComidas[i], pos, prefabsComidas[i].transform.rotation);
                        break;
                }

                numeroDeRecursosDisponiveis--;
            }
        }
    }

    public void SelecionarRecurso(string recurso = "Generico")
    {
        if (recurso == "Madeira") recursoSelecionado = Recursos.Madeira;
        if (recurso == "Pedra") recursoSelecionado = Recursos.Pedra;
        if (recurso == "Comida") recursoSelecionado = Recursos.Comida;
    }
}
