using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Synchronizes the players rotation with the sever and clients
public class PlayerSyncRotation : NetworkBehaviour {

    [SyncVar] private Quaternion syncPlayerRotation;

    private Transform playerTransform;
    private float lerpRate = 15;

    private void Update()
    {
        transmitRotation();
        lerpRotation();
    }

    void lerpRotation()
    {
        if(isLocalPlayer == false && playerTransform != null)
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
    }

    [Command]
    void CmdRotationToServer(Quaternion playerRot)
    {
        syncPlayerRotation = playerRot; 
    }


    [Client]
    void transmitRotation()
    {
        if(isLocalPlayer == true && playerTransform != null)
        {
            CmdRotationToServer(playerTransform.rotation);
        }
            
    }
}
