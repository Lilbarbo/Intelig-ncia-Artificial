using System;
using TMPro;
using UnityEngine;

public class DepositoRefinados : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsMaterialRefinadoText;
    public int maximoDeMateriais = 10;

    public int totalMadeiraRefinada = 0;
    public int totalPedraRefinada = 0;

    private void Start()
    {
        ReceberOuRetirarMaterial(BehaviourTreeManager.TipoDeRecuso.Nenhum, 0);//apenas para inicializar a UI
    }

    public void ReceberOuRetirarMaterial(BehaviourTreeManager.TipoDeRecuso tipo, int quantidade)
    {
        if (tipo == BehaviourTreeManager.TipoDeRecuso.MadeiraRefinada)
        {
            totalMadeiraRefinada += quantidade;
        }

        if (tipo == BehaviourTreeManager.TipoDeRecuso.PedraRefinada)
        {
            totalPedraRefinada += quantidade;
        }
        
        var total = totalMadeiraRefinada +  totalPedraRefinada;
        statsMaterialRefinadoText.text = "Materiais Refinados" +  "\nTotal: " + total + "/" + maximoDeMateriais + "\nMadeiraRefinada: " + totalMadeiraRefinada + "\nPedraRefinada: " + totalPedraRefinada; ;
    }
    
    public bool DepositoLotado() => totalMadeiraRefinada +  totalPedraRefinada >= maximoDeMateriais;
    public bool DepositoVazio() => totalMadeiraRefinada + totalPedraRefinada <= 0;
    public bool TemMadeiraRefinada() => totalMadeiraRefinada > 0;
    public bool TemPedraRefinada() => totalPedraRefinada > 0;
}
