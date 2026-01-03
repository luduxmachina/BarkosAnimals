using System.Collections.Generic;
using UnityEngine;
public enum TipoParticula
{
    None,
    Corazones,
    Chuleton,
    Zanahoria,
    Pan
}

public class ParticulasManager : MonoBehaviour
{
    public ParticleSystem corazones;
    public ParticleSystem chuleton;
    public ParticleSystem zanahoria;
    public ParticleSystem pan;

    Dictionary<TipoParticula, ParticleSystem> mapa;

    void Awake()
    {
        mapa = new Dictionary<TipoParticula, ParticleSystem>
        {
            { TipoParticula.Corazones, corazones },
            { TipoParticula.Chuleton, chuleton },
            { TipoParticula.Zanahoria, zanahoria },
            { TipoParticula.Pan, pan }
        };
    }

    public void Activar(TipoParticula tipo)
    {
        foreach (var ps in mapa.Values)
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (mapa.TryGetValue(tipo, out var psActiva))
            psActiva.Play();
    }
}

//Ejemplo de como se usa
/*
 
    [SerializeField] ParticulasManager particulas;
    
    particulas.Activar(TipoParticula.Zanahoria);
 
 */