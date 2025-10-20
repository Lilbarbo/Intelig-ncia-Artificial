using System;
using TMPro;
using UnityEngine;

public class DepositoComida : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsComidaText;
    public int capacidadeMaxima = 10;
    public int comidasTotais = 0;

    private void Start()
    {
        ReceberOuRetirarComida(0);
    }

    public void ReceberOuRetirarComida(int comida)
    {
        if (comida > 0 && DepositoLotado()) return;
        if(comida < 0 && DepositoVazio()) return;
        comidasTotais +=  comida;
        statsComidaText.text = "Total de comida: " + comidasTotais +"/"+  capacidadeMaxima;
    }
    
    public bool DepositoLotado() => comidasTotais >= capacidadeMaxima;
    public bool DepositoVazio() => comidasTotais <= 0;
}
