using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Gives each player in the scene a unique identifier
public class PlayerIdentity : NetworkBehaviour {

    [SyncVar] private string playerUniqueIdentity;
    NetworkInstanceId playerNetId;
    private Transform myTransform;
    public override void OnStartLocalPlayer()
    {
        
        GetNetIdentity();
        SetIdentity();
    }

    void Awake ()
    {
        myTransform = transform;
	}
	
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
