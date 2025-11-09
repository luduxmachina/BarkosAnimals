using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Shop System/Skin")]
public class SkinSO : ScriptableObject
{
   
    public int price;
    public string skinName;

    public GameObject skinModelo;
}
