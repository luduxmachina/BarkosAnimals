
using System.Collections.Generic;
using UnityEngine;

public class PlayerInSceneEffects : MonoBehaviour
{
    [SerializeField]
    List<PlayerEffect> effects = new List<PlayerEffect>();
    [SerializeField]
    PlayerMovement playerMovement;
    [SerializeField]
    PlayerCurrentStats playerCurrentStats;
    [SerializeField]
    PlayerBaseStats playerBaseStats;

    public void AddEffect(PlayerEffect effect)
    {
        effects.Add(effect);
        UpdatePlayerStats();
    }
    public void RemoveEffect(PlayerEffect effect) //no deberiamos usar esto casi nunca creo
    {
        effects.Remove(effect);
        effect.RemoveEffect(playerCurrentStats);
        UpdatePlayerStats();
    }

    public void AddOnDashEffects()
    {
        AddEffect(new DashCooldownEffect(playerCurrentStats.currentStats.dashCooldown));
        AddEffect(new AntigravityEffect(playerCurrentStats.currentStats.dashNoGravityDuration, playerMovement.rb));
    }
    public void AddOnJumpEffects()
    {
        AddEffect(new JumpCooldownEffect(playerCurrentStats.currentStats.jumpCooldown));
    }
    public void AddStunt(float time)
    {
        AddEffect(new StuntEffect(time));
    }
    private void Awake()
    {
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }
        if (playerCurrentStats == null)
        {
            playerCurrentStats = GetComponent<PlayerCurrentStats>();
        }
        if (playerBaseStats == null)
        {
            playerBaseStats = GetComponent<PlayerBaseStats>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        effects.AddRange(UpgradeEffects.effects);
        UpdatePlayerStats();
    }
    // Update is called once per frame
    void Update()
    {
        List<PlayerEffect> effectsToRemove = new List<PlayerEffect>();
        foreach (PlayerEffect effect in effects)
        {
            if (!effect.isPermanent)
            {
                effect.duration -= Time.deltaTime;
                if (effect.duration <= 0)
                {
                    effectsToRemove.Add(effect);
                }
            }
        }
        foreach (PlayerEffect effect in effectsToRemove)
        {
            RemoveEffect(effect);
        }
        if (effectsToRemove.Count > 0)
        {
            UpdatePlayerStats();
        }

    }
    void UpdatePlayerStats()
    {
        playerCurrentStats.ResetStats(); //recalculo en base a stats base

        foreach (PlayerEffect effect in effects)
        {
            effect.ApplyEffect(playerCurrentStats);
        }
    }


    class DashCooldownEffect : PlayerEffect
    {
        public DashCooldownEffect(float duration)
        {
            this.duration = duration;
            isPermanent = false;
        }
        public override void ApplyEffect(PlayerCurrentStats playerStats)
        {
            playerStats.canDash = false;
        }
    }
    class JumpCooldownEffect : PlayerEffect
    {
        public JumpCooldownEffect(float duration)
        {
            this.duration = duration;
            isPermanent = false;
        }
        public override void ApplyEffect(PlayerCurrentStats playerStats)
        {
            playerStats.canJump = false;
        }
    }
    class AntigravityEffect : PlayerEffect //este es especialito lol
    {
        Rigidbody rb;
        public AntigravityEffect(float duration, Rigidbody rb)
        {
            this.duration = duration;
            isPermanent = false;
            this.rb = rb;
        }
        public override void ApplyEffect(PlayerCurrentStats playerStats)
        {
            rb.useGravity = false;
        }
        public override void RemoveEffect(PlayerCurrentStats playerStats)
        {
            rb.useGravity = true;
        }
    }
    class StuntEffect : PlayerEffect
    {
        public StuntEffect(float duration)
        {
            this.duration = duration;
            isPermanent = false;
        }
        public override void ApplyEffect(PlayerCurrentStats playerStats)
        {
            playerStats.canMove = false;
            playerStats.canDash = false;
            playerStats.canJump = false;
        }
        public override void RemoveEffect(PlayerCurrentStats playerStats)
        {
            playerStats.canMove = true;
            playerStats.canDash = true;
            playerStats.canJump = true;
        }
    }
}



