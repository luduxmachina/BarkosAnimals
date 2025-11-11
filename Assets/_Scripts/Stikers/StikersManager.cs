using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StikersManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Camera camera;

    [SerializeField]
    List<Image> images;

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(camera.transform.position);
    }
}
