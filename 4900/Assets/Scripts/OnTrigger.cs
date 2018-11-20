using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Applies the correct effects correspodning to the trigger
public class OnTrigger : NetworkBehaviour {

    public enum Ability { M1, M2, Q, E};
    public Ability ability;
    Stun stun;
    OnDamage dEvent;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocalPlayer)
            return;
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
            dEvent = GetComponent<OnDamage>();
            switch(ability)
            {
                case Ability.M1:
                    damage = GetComponent<WarriorM1Ability>().damage;
                    dEvent.DamageEvent(uIdentity, damage, colPos);
                    break;
                case Ability.M2:
                    damage = GetComponent<WarriorM2Ability>().damage;
                    dEvent.DamageEvent(uIdentity, damage, colPos);
                    stunTime = GetComponent<WarriorM2Ability>().stunTime;
                    stun = GetComponent<Stun>();
                    stun.CmdSendStunCoroutine(uIdentity, stunTime);              
                    break;
                default:
                    Debug.LogError("No Ability Seleceted");
                    break;
            }
        }

        // Destroy and colliders that could still remain 
        foreach (Collider c in GetComponents<Collider>())
        {
            if (c.isTrigger == true)
                Destroy(c);
        }

    }
    
    
}
