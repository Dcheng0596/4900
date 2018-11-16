using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Base class for all abilities
public abstract class Ability : NetworkBehaviour {

    protected Player player;
    public int slowDown;
    public int damage;
    protected NetworkAnimator nAnim;
    public GameObject dText;
    public Font OL;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Send damage over network
    [Command]
    protected void CmdDealDamage(string uniqueID, int damage)
    {
        GameObject go = GameObject.Find(uniqueID);
        go.GetComponent<PlayerHealth>().TakeDamage(damage);
    }

    // Functions to be called as an animation events
    protected virtual void CreateCollider()
    {

    }
    protected virtual void DestroyCollider()
    {

    }

    protected void SlowDown()
    {
        player.isAttacking = true;
        player.currentSpeed = player.speed - slowDown;
    }

    protected void UndoSlow()
    {
        player.isAttacking = false;
        player.currentSpeed = player.speed;
    }
    //////////////////////////////////////////////

    protected virtual void GetInput()
    {

    }

    [Command]
    protected void CmdSendDamageText(string uniqueID, int damage, Vector3 position)
    {
        RpcDamageText(uniqueID, damage, position);
    }

    // Creates damage text above damaged player 
    // Text size and color depends on amount of damage
    [ClientRpc]
    protected void RpcDamageText(string uniqueID, int damage, Vector3 position)
    {
        GameObject go = GameObject.Find(uniqueID);
        GameObject dmgTxt = Instantiate(dText, position, Quaternion.identity);
        dmgTxt.transform.position = position + new Vector3(0, .01f, 0);

        Text text = dmgTxt.GetComponentInChildren<Text>();
        text.text = damage.ToString();

        RectTransform rect = dmgTxt.transform.GetChild(0).gameObject.GetComponentInChildren<RectTransform>();

        if (damage < 10)
            rect.localScale = new Vector3(.04f, .04f, 0);
        else if (damage < 20)
        {
            text.font = OL;
            rect.localScale = new Vector3(.045f, .045f, 0);
        }
        else
        {
            rect.localScale = new Vector3(.05f, .05f, 0);
            text.font = OL;
            text.color = new Color(102, 0, 0);
        }


    }
}
