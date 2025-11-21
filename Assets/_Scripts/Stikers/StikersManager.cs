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
    [SerializeField]
    bool fadeStickers = false;
    [SerializeField, HideIf("fadeStickers", false)]
    float timeToFade = 2.0f;
    StikersGenerales stickerActual = StikersGenerales.None;
    public StikersGenerales StickerActual { get { return stickerActual; } }
    bool isShowingSticker = false;
    float fadeTimer = 0.0f;

    void Start()
    {
        camera = Camera.main;
        HideSprites();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(camera.transform.position);
        if (fadeStickers && isShowingSticker)
        {
            fadeTimer += Time.deltaTime;
            if (fadeTimer >= timeToFade)
            {
                HideSprites();
                fadeTimer = 0.0f;
            }
        }
    }

    public void SetImage(StikersGenerales stickerType)
    {
        isShowingSticker = true;
        

        fondo.SetActive(true);
        Sprite sprite = generalesDB.GetSticker(stickerType);
        spriteRenderer.sprite = sprite;
        stickerActual = stickerType;
    }

    public void HideSprites()
    {
        isShowingSticker = false;

        fondo.SetActive(false);
        spriteRenderer.sprite = null;
        stickerActual = StikersGenerales.None;
    }
    
}
