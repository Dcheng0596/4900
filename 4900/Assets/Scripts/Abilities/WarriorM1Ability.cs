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
        anim = GetComponent<Animator>();

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
        Vector2[] points = { new Vector2(0.25f, 0.6f), new Vector2(0.6f, 0.3f),
        new Vector2(0.45f, -0.35f), new Vector2(-0.0f, -0.6f)};
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
            CmdSendAnimationParameter(true);
        }
        else
            CmdSendAnimationParameter(false);
    }

    [Command]
    void CmdSendAnimationParameter(bool state)
    {
        RpcRecieveAnimationParameter(state);
    }

    [ClientRpc]
    void RpcRecieveAnimationParameter(bool state)
    {
        if(state)
            anim.SetBool("M1Held", true);
        else
            anim.SetBool("M1Held", false);
    }
}
