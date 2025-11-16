using UnityEngine;

public class InputHipotetico : MonoBehaviour
{
    public IslandSoundManager gestor_sonido;
    public MenuSoundManager gestor_menu_sonido;
    public UISoundManager gestorSonidosUI;
    private bool estadoPausa = false;
    private bool estadoAjustes = false;
    private bool maximizado = false;
    private bool confirmado=false;


    public void GestionarPausa()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.estadoPausa =!estadoPausa;
           this.gestor_sonido.CambiarEstadoPausa(estadoPausa);
        }
    }
    public void GestionarMusicaMenus()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            
            this.gestor_menu_sonido.CambiarEstadoAjustes();
        }
    }

    public void GestionarEfectosUI()
    {
        if (Input.GetKeyDown(KeyCode.F))    //caso maximizar/minimizar
        {
            this.maximizado = !maximizado;
            if (maximizado) gestorSonidosUI.ActivarSonidoMaximizar();
            else gestorSonidosUI.ActivarSonidoMinimizar();
          
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            this.confirmado = !confirmado;
            if (confirmado) gestorSonidosUI.ActivarSonidoConfirmar();
            else gestorSonidosUI.ActivarSonidoCancelar();
        }
        if (Input.GetMouseButtonDown(0)) gestorSonidosUI.ActivarSonidoBoton();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GestionarPausa();
        GestionarMusicaMenus();
        GestionarEfectosUI();
    }
}
