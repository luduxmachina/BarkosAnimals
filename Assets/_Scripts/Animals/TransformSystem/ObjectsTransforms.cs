using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public interface INode
{
    string Name { get; }

    public List<Transform> GetTransforms();
}

public class BranchNode: INode
{
    string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public INode childNode1;
    public INode childNode2;

    public BranchNode(INode childNode1, INode childNode2)
    {
        this.childNode2 = childNode2;
        this.childNode1 = childNode1;
    }

    public List<Transform> GetTransforms()
    {
        List<Transform> list =  childNode1.GetTransforms();
        List<Transform> list2 = childNode2.GetTransforms();
        list.AddRange(list2);
        return list;
    }
}

public class LeafNode: INode
{
    public string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public LeafNode() { }

    public LeafNode(List<Transform>transforms)
    {
        this.transforms = transforms;
    }

    public List<Transform> transforms = new List<Transform>();

    public List<Transform> GetTransforms()
    {
        return transforms;
    }

    public void AddNewTransform(Transform newTranstorm)
    {
        transforms.Add(newTranstorm);
    }
}

public class ObjectsTransforms
{
    public INode FirstNode;

    public ObjectsTransforms()
    {
        INode herbívoroPasivNode = new LeafNode();
        INode carnivorePasivNode = new LeafNode();
        INode herbívoroActivNode = new LeafNode();
        INode carnivoreActivNode = new LeafNode();
        INode pasiveNode = new BranchNode(herbívoroPasivNode, carnivorePasivNode);
        INode agresiveNode = new BranchNode(herbívoroActivNode, carnivoreActivNode);
        INode itemNode = new LeafNode();
        INode playerNode = new LeafNode();
        INode animals = new BranchNode(agresiveNode, pasiveNode);
        INode noPlayer = new BranchNode(itemNode, animals);
        INode allTransforms = new BranchNode(playerNode, noPlayer);

        this.FirstNode = allTransforms;
    }
    
}
