using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Handles the movement of the shield
public class ShieldToss : NetworkBehaviour {

    Rigidbody2D rb2D;
    public int speed;
	// Use this for initialization
	void Start ()
    {
        speed = 5;
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine("Throw");
    }
	
	// Update is called once per frame
	void Update ()
    {

	}
    
    IEnumerator Throw()
    {

        rb2D.velocity = transform.up * speed;
        yield return new WaitForSeconds(1);
        
        Destroy(this.gameObject);
    }
}
