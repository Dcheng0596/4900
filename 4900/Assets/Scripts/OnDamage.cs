using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Applies the correct effects correspodning to the triger
public class OnDamage : NetworkBehaviour {

    public GameObject dText;
    public Font OL;
    public enum Ability { M1, M2, Q, E};
    public Ability ability;
    Player player;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocalPlayer)
            return;
        Debug.Log("M2");
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
            int damage;
            float stunTime;
            switch(ability)
            {
                case Ability.M1:
                    damage = GetComponent<WarriorM1Ability>().damage;
                    damageEvent(uIdentity, damage, colPos);
                    break;
                case Ability.M2:
                    damage = GetComponent<WarriorM2Ability>().damage;
                    stunTime = GetComponent<WarriorM2Ability>().stunTime;
                    damageEvent(uIdentity, damage, colPos);
                    CmdSendStunCoroutine(uIdentity, stunTime);
                    break;
                default:
                    Debug.LogError("No Ability Seleceted");
                    break;
            }
        }
    }
    void damageEvent(string uIdentity, int damage, Vector2 colPos)
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
    void RpcDamageText(string uniqueID, int damage, Vector3 position)
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
    [Command]
    void CmdSendStunCoroutine(string uniqueID, float stunTime)
    {
        GameObject go = GameObject.Find(uniqueID);
        go.GetComponent<PlayerHealth>().RpcRecieveStunCoroutine(stunTime);
    }
}
