using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Warrior classe's Mouse 2 ability
public class WarriorM2Ability : Ability {

    BoxCollider2D m2;

    void Start ()
    {
        this.damage = 15;
        player = GetComponent<Player>();
        nAnim = GetComponentInParent<NetworkAnimator>();
    }
	
	void Update ()
    {

        if (!isLocalPlayer)
            return;
        GetInput();
    }


    protected override void GetInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            nAnim.animator.SetTrigger("M2Pressed");
            DisableMovement();
        }

    }

    // Deals damage at collision contact point and creates approriate damage text
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocalPlayer)
            return;
        Debug.Log("M2");
        GameObject enemy = collision.gameObject;

        string uIdentity = enemy.transform.name;
        string myIdentity = this.gameObject.transform.name;

        if (enemy == null)
            return;
        if (enemy.transform.tag == "Player" && uIdentity != myIdentity)
        {

            this.CmdDealDamage(uIdentity, damage);

            ContactPoint2D[] contacts = new ContactPoint2D[1];
            collision.GetContacts(contacts);

            Vector2 colPos = enemy.transform.position;
            CmdSendDamageText(uIdentity, damage, colPos);

        }
    }

    protected void WarriorM2CreateCollider()
    {
        this.CreateCollider();
    }

    protected void WarriorM2DestroyCollider()
    {
        this.DestroyCollider();
    }

    protected override void CreateCollider()
    {
        if (!isLocalPlayer)
            return;

        m2 = gameObject.AddComponent<BoxCollider2D>();
        m2.isTrigger = true;
        m2.transform.Translate(Vector3.up * .00001f);
        m2.size = new Vector2(.3f, 1);
        m2.offset = new Vector2(.3f, 0);
    }
    
    protected override void DestroyCollider()
    {
        if (!isLocalPlayer)
            return;
        Destroy(m2);
    }

    void DisableMovement()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.enabled = false;
        nAnim.animator.SetInteger("LowerState", 1);
    }
    public void EnableMovement()
    {
        player.enabled = true;
    }
}
