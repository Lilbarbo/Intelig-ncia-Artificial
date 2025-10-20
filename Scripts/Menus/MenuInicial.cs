using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public GameObject FundoMenu;
    public GameObject FundoOpcoes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IniciarJogo()
    {
        SceneManager.LoadScene("Daniel");
    }

    public void FecharJogo()
    {
        Application.Quit();
        Debug.Log("Voc� fechou o jogo");
    }

    public void Opcoes()
    {
        FundoMenu.SetActive(false);
        FundoOpcoes.SetActive(true);
    }

    public void Voltar()
    {
        FundoMenu.SetActive(true);
        FundoOpcoes.SetActive(false);
    }
}
