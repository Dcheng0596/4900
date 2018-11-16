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


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Send damage over network


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

   
}
