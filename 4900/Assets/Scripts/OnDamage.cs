using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Applies the correct effects correspodning to the trigger
public class OnDamage: NetworkBehaviour {

    public GameObject dText;
    public Font OL;

    public void DamageEvent(string uIdentity, int damage, Vector2 colPos)
    {
        this.CmdDealDamage(uIdentity, damage); 
        CmdSendDamageText(uIdentity, damage, colPos);
    }
    [Command]
     void CmdDealDamage(string uniqueID, int damage)
    {
        GameObject go = GameObject.Find(uniqueID);
        go.GetComponent<PlayerHealth>().TakeDamage(damage);
    }

    [Command]
     void CmdSendDamageText(string uniqueID, int damage, Vector3 position)
    {
        RpcDamageText(uniqueID, damage, position);
    }

    // Creates damage text above damaged player 
    // Text size and color depends on amount of damage
    [ClientRpc]
    public void RpcDamageText(string uniqueID, int damage, Vector3 position)
    {
       // GameObject go = GameObject.Find(uniqueID);
        GameObject dmgTxt = Instantiate(dText, position, Quaternion.identity);
        dmgTxt.transform.position = position + new Vector3(0, .01f, 0);

        Text text = dmgTxt.GetComponentInChildren<Text>();
        text.text = damage.ToString();

        RectTransform rect = dmgTxt.transform.GetChild(0).gameObject.GetComponentInChildren<RectTransform>();

        if (damage < 10)
            rect.localScale = new Vector3(.035f, .035f, 0);
        else if (damage < 20)
        {
            text.font = OL;
            rect.localScale = new Vector3(.035f, .035f, 0);
        }
        else
        {
            rect.localScale = new Vector3(.037f, .038f, 0);
            text.font = OL;
            text.color = new Color(102, 0, 0);
        }

    }
    
}
