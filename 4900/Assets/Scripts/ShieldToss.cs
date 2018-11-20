using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Handles the movement of the shield
public class ShieldToss : NetworkBehaviour {

    Rigidbody2D rb2D;
    OnDamage dEvent;
    public int damage;
    public int speed;
    
	// Use this for initialization
	void Start ()
    {
        speed = 7;
        rb2D = GetComponent<Rigidbody2D>();
        dEvent = GetComponent<OnDamage>();
        RpcSendThrow();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}
    [ClientRpc]
    void RpcSendThrow()
    {
        StartCoroutine("Throw");
    }
    IEnumerator Throw()
    {

        rb2D.velocity = transform.up * speed;
        yield return new WaitForSeconds(1);
        
        Destroy(this.gameObject);
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject enemy = collision.gameObject;
        if (enemy == null)
            return;
        string uIdentity = enemy.transform.name;
        string myIdentity = this.gameObject.transform.name;
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        collision.GetContacts(contacts);
        Vector2 colPos = enemy.transform.position;

        if (enemy.transform.tag == "Player" && uIdentity != myIdentity)
        {
            dEvent.DamageEvent(uIdentity, damage, colPos);
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
