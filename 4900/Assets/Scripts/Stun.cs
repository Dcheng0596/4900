using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
        anim.SetInteger("LowerState", 1);
        anim.SetBool("Stunned", true);
        rb2D.velocity = Vector3.zero;
        player.enabled = false;

        yield return new WaitForSeconds(stunTime);
        this.GetComponent<Player>().enabled = true;
        anim.SetBool("Stunned", false);
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
}
