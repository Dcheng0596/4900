using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerIdentity : NetworkBehaviour {

    [SyncVar] private string playerUniqueIdentity;
    NetworkInstanceId playerNetId;
    private Transform myTransform;
    public override void OnStartLocalPlayer()
    {
        
        GetNetIdentity();
        SetIdentity();
    }

    // Use this for initialization
    void Awake ()
    {
        myTransform = transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (myTransform.name == "" || myTransform.name == "Player GO(Clone)")
            SetIdentity();

	}
    [Client]
    void GetNetIdentity()
    {
        playerNetId = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyName(MakeUniqueIdentity());
    }

    void SetIdentity()
    {
        if (!isLocalPlayer)
            myTransform.name = playerUniqueIdentity;
        else
            myTransform.name = MakeUniqueIdentity();
    }


    string MakeUniqueIdentity()
    {
        string uniqueName = "Player " + playerNetId.ToString();
        return uniqueName;
    }

    [Command]
    void CmdTellServerMyName(string name)
    {
        playerUniqueIdentity = name;
    }
}
