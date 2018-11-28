using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AudioSync : NetworkBehaviour {

    AudioSource source;
    public AudioClip[] clips;

	void Start ()
    {
        source = this.GetComponent<AudioSource>();
	}
	
	public void PlaySound(int id)
    {
        if (id >= 0 && id < clips.Length)
            CmdSendServerSoundID(id);
    }

    [Command]
    void CmdSendServerSoundID(int id)
    {
        RpcRecieveSoundID(id);
    }

    [ClientRpc]
    void RpcRecieveSoundID(int id)
    {
        source.PlayOneShot(clips[id]);
    }
}
