using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Handles the movement of the shield
public class ShieldToss : NetworkBehaviour {

    Rigidbody2D rb2D;
    OnDamage dEvent;
    Stun stun;
    bool apex;
    bool hit;
    public int damage;
    public float stunTime;
    public int speed;
    public string owner;


    // Use this for initialization
    void Start ()
    {
        apex = false;
        hit = false;
        speed = 8;
        rb2D = GetComponent<Rigidbody2D>();
        dEvent = GetComponent<OnDamage>();
        stun = GetComponent<Stun>();
        if(isServer)
            RpcRecieveThrow();
    }

    void LateUpdate()
    {
        if (hit)
            CmdDestroyShield();
    }

    [ClientRpc]
    void RpcRecieveThrow()
    {
        StartCoroutine("Throw");
    }

    [Command]
    void CmdDestroyShield()
    {
        NetworkServer.Destroy(this.gameObject);
    }

    IEnumerator Throw()
    {
        
        rb2D.velocity = transform.up * speed;
        yield return new WaitForSeconds(.2f);
        apex = true;
        yield return new WaitForSeconds(.03f);
        apex = false;
        yield return new WaitForSeconds(.20f);
        CmdDestroyShield();
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer)
            return;
        GameObject enemy = collision.gameObject;
        if (enemy == null)
            return;
        string uIdentity = enemy.transform.name;
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        collision.GetContacts(contacts);
        Vector2 colPos = enemy.transform.position;

        if (enemy.transform.tag == "Player" && uIdentity != owner)
        {
            if (apex)
                stun.CmdSendStunCoroutine(uIdentity, stunTime);
            GameObject go = GameObject.Find(uIdentity);
            go.GetComponent<PlayerHealth>().TakeDamage(damage);
            dEvent.RpcDamageText(uIdentity, damage, colPos);
            hit = true;
        }
    }
}
