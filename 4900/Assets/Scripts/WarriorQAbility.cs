using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Warrior class Q ability
public class WarriorQAbility : Ability {

    public GameObject shieldGO;
    public float stunTime = 1.5f;

    void Start ()
    {
        nAnim = GetComponent<NetworkAnimator>();
        nAnim.animator.SetBool("QPressed", false);
        this.slowDown = 50;
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
        }

    }

    IEnumerator CoolDown()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(coolDown);
        onCoolDown = false;
    }

    protected void WarriorQSlowDown()
    {
        this.SlowDown();
    }

    protected void WarriorQUndoSlow()
    {
        nAnim.animator.SetBool("QPressed", false);
        this.UndoSlow();
    }

    [Command]
    void CmdSendShield()
    {
        if (!isLocalPlayer)
            return;
        Vector3 position = this.transform.position + (transform.right * .6f);
        Quaternion rotationAmount = Quaternion.Euler(0, 0, -90);
        Quaternion rotation = transform.rotation * rotationAmount;
        GameObject shield = Instantiate(shieldGO, position, rotation);

        NetworkServer.SpawnWithClientAuthority(shield, this.gameObject);
        shield.GetComponent<ShieldToss>().damage = damage;
    }

   
}
