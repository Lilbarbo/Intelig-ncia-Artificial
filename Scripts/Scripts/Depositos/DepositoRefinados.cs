using UnityEngine;

public class DepositoRefinados : MonoBehaviour
{
    public int maximoDeMateriais = 10;

    public int totalMadeiraRefinada = 0;
    public int totalPedraRefinada = 0;


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
    }
    
    public bool DepositoLotado() => totalMadeiraRefinada +  totalPedraRefinada >= maximoDeMateriais;
    public bool DepositoVazio() => totalMadeiraRefinada + totalPedraRefinada <= 0;
    public bool TemMadeiraRefinada() => totalMadeiraRefinada > 0;
    public bool TemPedraRefinada() => totalPedraRefinada > 0;
}
