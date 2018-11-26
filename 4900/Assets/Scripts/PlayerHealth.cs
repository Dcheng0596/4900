﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


// Handles player health, health slider, and status conditions
public class PlayerHealth : NetworkBehaviour
{
    public int startingHealth = 100;
    [SyncVar]
    public int currentHealth;
    private Slider healthSlider;

    private Rigidbody2D rb2D;

    Animator anim;

    Player player;
    bool isDead;
    bool isDamaged;


    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        healthSlider = GetComponentInChildren<Slider>();
        rb2D = GetComponent<Rigidbody2D>();

        currentHealth = startingHealth;
        healthSlider.value = startingHealth;

        if (isLocalPlayer)
        {
            Image fill = healthSlider.transform.GetChild(1).GetComponentInChildren<Image>();
            fill.color = new Color32(0, 229, 0, 255);
        }

    }

    void Update()
    {
        SetHealthSliderPosition();

        if (!isLocalPlayer)
            return;
        CmdSendSlider(currentHealth);

        if (currentHealth <= 0)
        {
            Death();
        }

        isDamaged = false;
    }

    public void TakeDamage(int amount)
    {
        isDamaged = true;

        if (!isServer)
            return;
        currentHealth -= amount;
        healthSlider.value = currentHealth;

    }

    [ClientRpc]
    void RpcSetSlider(int health)
    {
        healthSlider.value = health;
    }

    [Command]
    void CmdSendSlider(int health)
    {
        RpcSetSlider(health);
    }

    [Command]
    void CmdDisableCollider()
    {
        RpcDisableCollider();
    }
    [ClientRpc]
    void RpcDisableCollider()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        Destroy(collider);
    }
    void Death()
    {
        isDead = true;

        CmdDisableCollider();
        player.enabled = false;
        CmdSendDeathAnimationParameter(true);
        //Disable Lower layer 
        anim.SetLayerWeight(anim.GetLayerIndex("Lower"), 0);
    }

    void SetHealthSliderPosition()
    {
        healthSlider.transform.position = transform.position + new Vector3(0, 0.65f, 0);
        healthSlider.transform.rotation = Quaternion.identity;
    }

    [Command]
    void CmdSendDeathAnimationParameter(bool state)
    {
        RpcRecieveDeathAnimationParameter(state);
    }

    [ClientRpc]
    void RpcRecieveDeathAnimationParameter(bool state)
    {
        if (state)
            anim.SetBool("Dead", true);
        else
            anim.SetBool("Death", false);
    }

}