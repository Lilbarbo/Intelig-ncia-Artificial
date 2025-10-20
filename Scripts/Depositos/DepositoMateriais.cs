using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DepositoMateriais : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI statsMateriaisText;
    public int recursoMaximoDisponivel = 10;
    
    public int madeiraTotal = 0;
    public int pedraTotal = 0;

    private void Start()
    {
        AdicionarRecurso(BehaviourTreeManager.TipoDeRecuso.Nenhum, 0); //apenas para inicializar a UI
    }

    public void AdicionarRecurso(BehaviourTreeManager.TipoDeRecuso tipo, int quantidade = 1)
    {
        if (tipo == BehaviourTreeManager.TipoDeRecuso.Madeira)
        {
            madeiraTotal += quantidade;
        }
        else if (tipo == BehaviourTreeManager.TipoDeRecuso.Pedra)
        {
            pedraTotal += quantidade;
        }
        
        var totalDeMateriais = madeiraTotal + pedraTotal;
        statsMateriaisText.text = "Materiais Brutos" + "\nTotal: " + totalDeMateriais + "/"+recursoMaximoDisponivel + "\nMadeira: "+ madeiraTotal +"\nPedra: "+pedraTotal;
        
    }
    
    
    public bool DepositoLotado() => madeiraTotal + pedraTotal >= recursoMaximoDisponivel;
    public bool DepositoVazio() => madeiraTotal + pedraTotal <= 0;
    public bool TemMadeira() => madeiraTotal > 0;
    public bool TemPedra() => pedraTotal > 0;
}
