using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[Serializable]
struct ComidaModelo
{
    public ItemNames tipoComida;
    public GameObject modelo;
}

public class CuencoManager : MonoBehaviour
{
    [SerializeField]List<ComidaModelo> comidasYModelo = new();

    ItemNames tipoComida = ItemNames.None;

    public void SetFood(ItemNames tipoComida)
    {
        this.tipoComida = tipoComida;
        foreach (var com in comidasYModelo)
        {
            if(tipoComida == com.tipoComida)
            {
                com.modelo.gameObject.SetActive(true);
            }
            else
            {
                com.modelo.gameObject.SetActive(false);
            }
        }
    }


    public ItemNames ComidaEnCuenco()
    {
        return tipoComida;
    }
}
