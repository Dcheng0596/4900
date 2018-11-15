using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Warrior classe's Mouse 2 ability
public class WarriorM2Ability : Ability {

	void Start ()
    {
        player = GetComponent<Player>();
        nAnim = GetComponentInParent<NetworkAnimator>();
    }
	
	void Update ()
    {

        if (!isLocalPlayer)
            return;
        GetInput();
    }


    protected override void GetInput()
    {
        if (Input.GetMouseButtonDown(1))
        {

            nAnim.animator.SetTrigger("M2Pressed");
        }

    }

    protected override void CreateCollider()
    {

    }
}
