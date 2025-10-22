using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestTamañosSO", menuName = "Scriptable Objects/TestTamañosSO")]
public class TestTamañosSO : ScriptableObject
{
    public List<CustomBoolMatrix> ListOfMatrix = new();
}
