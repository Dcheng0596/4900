using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Warrior class Space ability
public class WarriorSpaceAbility : Ability
{

    PolygonCollider2D space;
    Rigidbody2D rb2D;
    public float forwardSpeed;

    void Start()
    {
        anim = GetComponent<Animator>();
        this.coolDown = 6;
        this.damage = 17;
        this.onCoolDown = false;
        forwardSpeed = 2.5f;
        player = GetComponent<Player>();
        rb2D = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;
        GetInput();
    }


    protected override void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !onCoolDown)
        {
            StartCoroutine("CoolDown");
            CmdSendAnimationParameter(true);
        }

    }

    protected void WarriorSpaceDisableMovement()
    {
        player.CmdSendAnimationParameter(1);
        rb2D.velocity = Vector3.zero;
        player.enabled = false;
        StartCoroutine("MoveForward");
    }

    protected void WarriorSpaceEnableMovement()
    {
        player.enabled = true;
    }
    // Deals damage at collision contact point and creates approriate damage text


    protected void WarriorSpaceCreateCollider()
    {
        GetComponent<OnTrigger>().ability = OnTrigger.Ability.Space;
        this.CreateCollider();
    }

    protected void WarriorSpaceDestroyCollider()
    {
        this.DestroyCollider();
    }

    IEnumerator MoveForward()
    {

        Debug.Log("Entered");
        Rigidbody2D rb2D = player.GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(.12f);
        rb2D.velocity = transform.right * forwardSpeed;
        yield return new WaitForSeconds(.52f);
        rb2D.velocity = Vector3.zero;
    }
    protected override void CreateCollider()
    {
        if (!isLocalPlayer)
            return;

        space = gameObject.AddComponent<PolygonCollider2D>();
        space.isTrigger = true;
        space.transform.Translate(Vector3.up * .00001f);
        Vector2[] points = { new Vector2(0.8f, 0.4f), new Vector2(0, 0.9f),
        new Vector2(0, -0.9f), new Vector2(0.8f, -0.4f)};
        space.points = points;
    }

    protected override void DestroyCollider()
    {
        if (!isLocalPlayer)
            return;
        Destroy(space);
        CmdSendAnimationParameter(false);
    }


    IEnumerator CoolDown()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(coolDown);
        onCoolDown = false;
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
            anim.SetBool("SpacePressed", true);
        else
            anim.SetBool("SpacePressed", false);
    }
}
