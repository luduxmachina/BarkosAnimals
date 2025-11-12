using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Shop System/Skin")]
public class SkinSO : ScriptableObject
{
   
    public int price;
    public string skinName;
    public Sprite previewImage;
    public GameObject skinModelo;
}
