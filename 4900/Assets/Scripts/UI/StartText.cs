using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Shows the countdown when game starts
public class StartText : MonoBehaviour {

    Text startText;

    // Use this for initialization
    void Start ()
    {
        startText = GetComponent<Text>();
        StartCoroutine("ShowText");
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator ShowText()
    {
        startText.text = "3";
        yield return new WaitForSeconds(1);
        startText.text = "2";
        yield return new WaitForSeconds(1);
        startText.text = "1";
        yield return new WaitForSeconds(1);
        startText.text = "FIGHT!";
        yield return new WaitForSeconds(1);
        startText.text = null;
    }
    
}
