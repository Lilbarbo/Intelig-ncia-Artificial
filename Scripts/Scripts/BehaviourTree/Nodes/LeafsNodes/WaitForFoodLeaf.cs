using UnityEngine;

public class WaitForFoodLeaf : LeafNode
{
    public override string name { get; protected set; } =  "WaitForFoodLeaf";

    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        if (!MyManager.EstouComFome() || MyManager.fomeAtual <= 0f) return Status.Falha;

        if (!MyManager.scriptDepositoComida.DepositoVazio())
        {
            return Status.Sucesso;
        }
        
        Debug.Log("Estou esperando comida >:(");
        return Status.EmAndamento;
    }
}
