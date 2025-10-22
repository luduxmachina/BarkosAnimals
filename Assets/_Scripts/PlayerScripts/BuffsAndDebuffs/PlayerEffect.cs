public class PlayerEffect
{
    public bool isPermanent=true;
    public float duration;

    public virtual void ApplyEffect(PlayerCurrentStats playerMovement)
    {

    }
    /// <summary>
    /// Opcional
    /// </summary>
    /// <param name="playerMovement"></param>
    public virtual void RemoveEffect(PlayerCurrentStats playerMovement) 
    {

    }
}
