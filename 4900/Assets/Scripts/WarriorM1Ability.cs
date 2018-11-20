using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Warrior class Mouse 1 ability
public class WarriorM1Ability : Ability{

    PolygonCollider2D m1;

    void Start() {
        slowDown = 10;
        this.damage = 5;
        player = GetComponent<Player>();
        nAnim = GetComponent<NetworkAnimator>();

    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
        GetInput();
    }

    // Deals damage at collision contact point and creates approriate damage text
    

    // Wrapper functions for animation events
    protected void WarriorM1CreateCollider()
    {
        GetComponent<OnTrigger>().ability = OnTrigger.Ability.M1;
        this.CreateCollider();
    }

    protected void WarriorM1DestroyCollider()
    {
        this.DestroyCollider();
    }

    protected void WarriorM1SlowDown()
    {
        this.SlowDown();
    }

    protected void WarriorM1UndoSlow()
    {
        this.UndoSlow();
    }
    ///////////////////////////////////////////

    protected override void CreateCollider()
    {
        if (!isLocalPlayer)
            return;

        m1 = gameObject.AddComponent<PolygonCollider2D>();
        m1.isTrigger = true;
        m1.transform.Translate(Vector3.up * .00001f);
        Vector2[] points = { new Vector2(0.2464974f, 0.5841055f), new Vector2(0.6117219f, 0.2829182f),
        new Vector2(0.4555643f, -0.3497448f), new Vector2(-0.01756102f, -0.6067277f)};
        m1.points = points;
    }
    protected override void DestroyCollider()
    {
        if(!isLocalPlayer)
            return;
        Destroy(m1);
    }

    protected override void GetInput()
    {
        if (Input.GetMouseButton(0))
        {
            player.isAttacking = true;
            
            nAnim.animator.SetBool("M1Held", true);

        }
        else
            nAnim.animator.SetBool("M1Held", false);



    }

}
