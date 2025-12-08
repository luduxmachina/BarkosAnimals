using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class StikersManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Camera camera;

    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    SpriteRenderer spriteDoble1;
    [SerializeField]
    SpriteRenderer spriteDoble2;

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

        spriteDoble1.sprite = null;
        spriteDoble2.sprite = null;

        fondo.SetActive(true);
        Sprite sprite = generalesDB.GetSticker(stickerType);
        spriteRenderer.sprite = sprite;
        stickerActual = stickerType;
    }

    public void SetImage(StikersGenerales[] stickerType)
    {
        isShowingSticker = true;

        spriteRenderer.sprite = null;

        fondo.SetActive(true);
        Sprite sprite1 = generalesDB.GetSticker(stickerType[0]);
        Sprite sprite2 = generalesDB.GetSticker(stickerType[1]);
        spriteDoble1.sprite = sprite1;
        spriteDoble2.sprite = sprite2;
        stickerActual = stickerType[0];
    }

    public void HideSprites()
    {
        isShowingSticker = false;

        fondo.SetActive(false);
        spriteRenderer.sprite = null;
        spriteDoble1.sprite = null;
        spriteDoble2.sprite = null;
        stickerActual = StikersGenerales.None;
    }
    
}
