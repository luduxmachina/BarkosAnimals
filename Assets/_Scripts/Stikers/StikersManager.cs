using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StikersManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Camera camera;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    GameObject fondo;

    [SerializeField]
    StickersGeneralesSO generalesDB;

    StikersGenerales stickerActual = StikersGenerales.None;
    public StikersGenerales StickerActual { get { return stickerActual; } }

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(camera.transform.position);
    }

    public void SetImage(StikersGenerales stickerType)
    {
        fondo.SetActive(true);
        
        Sprite sprite = generalesDB.GetSticker(stickerType);
        spriteRenderer.sprite = sprite;
        stickerActual = stickerType;
    }

    public void HideSprites()
    {
        fondo.SetActive(false);
        spriteRenderer.sprite = null;
        stickerActual = StikersGenerales.None;
    }
    
}
