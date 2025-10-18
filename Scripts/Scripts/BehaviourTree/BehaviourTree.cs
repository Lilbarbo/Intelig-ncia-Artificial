using UnityEngine;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public enum Status
{
    Sucesso,
    Falha,
    EmAndamento,
    Desconhecido,
}

public abstract class Node
{
    public abstract string name {get; protected set;}

    public Node parentNode;
    public List<Node> childrenNodes = new();
    protected BehaviourTreeManager MyManager;
    protected int CurrentChildIndex = 0;

    public abstract void Setup();

    public virtual void AddChild(Node child, [CanBeNull] string n = null)
    {
        if (n != null) child.name = n;
        child.parentNode = this;
        childrenNodes.Add(child);
        
        child.Setup();
    }

    public virtual void Reset()
    {
        CurrentChildIndex = 0;
        foreach (var child in childrenNodes)
        {
            child.Reset();
        }
    }

    public virtual Status Process()
    {
        return Status.Desconhecido;
    }

    public void SetManager(BehaviourTreeManager manager)
    {
        MyManager = manager;
        foreach (var child in childrenNodes)
        {
            child.SetManager(manager);
        }
    }
}

public abstract class LeafNode : Node
{
    public override void AddChild(Node child, [CanBeNull] string n = null)
    {
        Debug.LogError("Folha não tem filho");
    }

    public override void Reset()
    {
        CurrentChildIndex = 0;
    }
}

public abstract class SequenceNode : Node
{
    public override Status Process()
    {
        for (; CurrentChildIndex < childrenNodes.Count; CurrentChildIndex++)
        {
            switch (childrenNodes[CurrentChildIndex].Process())
            {
                case Status.Sucesso:
                    continue;
                case Status.EmAndamento:
                    return Status.EmAndamento;
                case Status.Falha:
                    Reset();
                    return Status.Falha;
                case Status.Desconhecido:
                    return Status.Desconhecido;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        Reset();
        return Status.Sucesso;
    }
}

public abstract class SelectorNode : Node
{
    public override Status Process()
    {
        for (; CurrentChildIndex < childrenNodes.Count; CurrentChildIndex++)
        {
            switch (childrenNodes[CurrentChildIndex].Process())
            {
                case Status.Sucesso:
                    Reset();
                    return Status.Sucesso;
                case Status.EmAndamento:
                    return Status.EmAndamento;
                case Status.Falha:
                    continue;
                case Status.Desconhecido:
                    return Status.Desconhecido;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        Reset();
        return Status.Falha;
    }
}
