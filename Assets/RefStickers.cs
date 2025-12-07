using UnityEngine;

public class RefStickers : MonoBehaviour
{
    [SerializeField]StickersGeneralesSO stickersGeneralesSO;

    private void Start()
    {
        int num = stickersGeneralesSO.stickerList.Count;
    }
}
