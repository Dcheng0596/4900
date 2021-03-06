﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Warrior class Q ability
public class WarriorQAbility : Ability {

    public GameObject shieldGO;
    public float stunTime = 2f;
    // the owner of the shield
    string myIdentity;

    void Start ()
    {
        anim = GetComponent<Animator>();
        this.slowDown = 50;
        this.coolDown = 4;
        this.damage = 12;
        this.onCoolDown = false;
        player = GetComponent<Player>();
        audio = GetComponent<AudioSync>();
    }
	
	void Update()
    {
        if (!isLocalPlayer)
            return;
        GetInput();
    }

    protected override void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !onCoolDown && !player.isStun && !player.usingAbility)
        {
            StartCoroutine("CoolDown");
            CmdSendAnimationParameter(true);
            player.usingAbility = true;
        }

    }

    IEnumerator CoolDown()
    {
        Text cooldownText = GameObject.Find("QText").GetComponent<Text>();

        for(int i = coolDown; i > 0; i--)
        {
            cooldownText.text = i.ToString();
            onCoolDown = true;
            yield return new WaitForSeconds(1);
        }
        onCoolDown = false;
        cooldownText.text = null;
    }

    protected void WarriorQSlowDown()
    {
        this.SlowDown();
        player.usingAbility = false;
    }

    protected void WarriorQUndoSlow()
    {
        this.UndoSlow();
    }

    void SendShield()
    {   if(isLocalPlayer)
            audio.PlaySound(3);
        CmdSendAnimationParameter(false);
        if (!isServer)
            return;
        Vector3 position = this.transform.position + (transform.right * .2f);
        Quaternion rotationAmount = Quaternion.Euler(0, 0, -90);
        Quaternion rotation = transform.rotation * rotationAmount;
        GameObject shield = Instantiate(shieldGO, position, rotation);

        NetworkServer.Spawn(shield);

        ShieldToss newShield = shield.GetComponent<ShieldToss>();
        myIdentity = this.gameObject.name;
        newShield.owner = myIdentity;
        Debug.Log(myIdentity);
        newShield.damage = damage;
        newShield.stunTime = stunTime;

    }


    [Command]
    void CmdSendAnimationParameter(bool state)
    {
        RpcRecieveAnimationParameter(state);
    }

    [ClientRpc]
    void RpcRecieveAnimationParameter(bool state)
    {
        if (state)
            anim.SetBool("QPressed", true);
        else
            anim.SetBool("QPressed", false);
    }
}
