using System;
using UnityEngine;

public class PlayerBaseStats : MonoBehaviour
{
    public Stats baseStats;


}
[Serializable]
public class Stats
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f; // Degrees per second
    [Header("Jump")]
    public float heightFromGround = 1f;
    public float jumpForce = 5f;
    public float jumpCooldown = 0.3f;
    [Header("Dash")]
    public float dashForce = 5f;
    public float dashCooldown = 0.3f;
    public float dashNoGravityDuration = 0.15f;

    public Stats Clone()
    {
        return (Stats)this.MemberwiseClone();
    }
}
