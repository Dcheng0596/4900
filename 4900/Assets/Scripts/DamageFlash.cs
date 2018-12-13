using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Player flashes a color when hit
public class DamageFlash : NetworkBehaviour {

    Player player;
    public float flashTime = .1f;

	void Start ()
    {
        player = GetComponent<Player>();
	}

    [Command]
    public void CmdSendDamageFlash(Color color)
    {
        RpcRecieveDamageFlash(color);
    }

    [ClientRpc]
    void RpcRecieveDamageFlash(Color color)
    {
        StartCoroutine("Flash", color);
    }

    IEnumerator Flash(Color color)
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        foreach(SpriteRenderer i in sprites)
            i.color = color;
   
        yield return new WaitForSeconds(flashTime);

        foreach (SpriteRenderer i in sprites)
            i.color = Color.white;

    }
}
