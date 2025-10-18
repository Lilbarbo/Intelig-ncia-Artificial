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
        statsMateriaisText.text = "Total: 0/" +recursoMaximoDisponivel + "\nMadeira: "+ madeiraTotal +"\nPedra: "+pedraTotal;
    }

    public void AdicionarRecurso(BehaviourTreeManager.TipoDeRecuso tipo, int quantidade = 1)
    {
        if (tipo == BehaviourTreeManager.TipoDeRecuso.Madeira)
        {
            madeiraTotal += quantidade;
            Debug.Log($"+{quantidade} madeira adicionada. Total: {madeiraTotal}");
        }
        else if (tipo == BehaviourTreeManager.TipoDeRecuso.Pedra)
        {
            pedraTotal += quantidade;
            Debug.Log($" +{quantidade} pedra adicionada. Total: {pedraTotal}");
        }
        
        var totalDeMateriais = madeiraTotal + pedraTotal;
        statsMateriaisText.text = "Total: " + totalDeMateriais + "/"+recursoMaximoDisponivel + "\nMadeira: "+ madeiraTotal +"\nPedra: "+pedraTotal;
        
    }
    
    
    public bool DepositoLotado() => madeiraTotal + pedraTotal >= recursoMaximoDisponivel;
    public bool DepositoVazio() => madeiraTotal + pedraTotal <= 0;
    public bool TemMadeira() => madeiraTotal > 0;
    public bool TemPedra() => pedraTotal > 0;
}
