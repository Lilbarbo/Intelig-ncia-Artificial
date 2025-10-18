using UnityEngine;

public class EatFoodLeaf : LeafNode
{
    public override string name { get; protected set; } =  "EatFoodLeaf";

    private float tempoParaSeAlimentar = 1.5f;

    public override void Setup()
    {
        
    }

    public override Status Process()
    {
        if (!MyManager.EstouComFome()) return Status.Falha;
        if (MyManager.scriptDepositoComida.DepositoVazio()) return Status.Falha;

        tempoParaSeAlimentar -= Time.deltaTime;
        if (tempoParaSeAlimentar <= 0f)
        {
            MyManager.SeAlimentar();
            MyManager.scriptDepositoComida.ReceberOuRetirarComida(-1);
            return Status.Sucesso;
        }

        Debug.Log("Estou me alimentando");
        return Status.EmAndamento;
    }
}
