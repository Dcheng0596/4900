using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Applies the stun status condition to a player
// While stunned a player can not perform any actions
public class Stun : NetworkBehaviour {

    Animator anim;
    Rigidbody2D rb2D;
    Player player;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }
    IEnumerator StunCoroutine(float stunTime)
    {
        CmdSendAnimationParameter(true);
        rb2D.velocity = Vector3.zero;
        player.canMove = false;
        player.isStun = true;
        yield return new WaitForSeconds(stunTime);
        rb2D.velocity = Vector3.zero;
        CmdSendAnimationParameter(false);
        player.canMove = true;
        player.isStun = false;
        player.usingAbility = false;
        player.currentSpeed = player.speed;
    }

    [ClientRpc]
    public void RpcRecieveStunCoroutine(float stunTime)
    {

        StartCoroutine("StunCoroutine", stunTime);
    }

    [Command]
    public void CmdSendStunCoroutine(string uniqueID, float stunTime)
    {
        GameObject go = GameObject.Find(uniqueID);
        go.GetComponent<Stun>().RpcRecieveStunCoroutine(stunTime);
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
        {
            anim.SetBool("Stunned", true);
            anim.SetInteger("LowerState", 1);
        }
        else
            anim.SetBool("Stunned", false);
    }
}
