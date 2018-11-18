using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WarriorQAbility : Ability {

    public float stunTime = 1.5f;

    void Start ()
    {
        nAnim = GetComponent<NetworkAnimator>();
        nAnim.animator.SetBool("QPressed", false);
        this.coolDown = 8;
        this.damage = 20;
        this.onCoolDown = false;
        player = GetComponent<Player>();
    }
	
	void Update()
    {
        if (!isLocalPlayer)
            return;
        GetInput();
    }

    protected override void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !onCoolDown)
        {
            StartCoroutine("CoolDown");
            nAnim.animator.SetBool("QPressed", true);
            DisableMovement();
        }
        else
            nAnim.animator.SetBool("QPressed", false);

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

    IEnumerator CoolDown()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(coolDown);
        onCoolDown = false;
    }
}
