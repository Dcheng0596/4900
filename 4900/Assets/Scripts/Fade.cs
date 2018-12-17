using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Applies a fade out effect to text
public class Fade : MonoBehaviour {

    Text text;
    public float fadeOutTime = .5f;
    public float waitTime = .5f;
    bool isWaiting;

	void Start ()
    {
        isWaiting = true;
        text = GetComponent<Text>();
        StartCoroutine("FadeOut");
	}
	
	void Update ()
    {
        // rising text effect
        this.transform.position += new Vector3(0, .002f, 0);
    }

    // Lets text display for 'waitTime' amount then fades out text
    IEnumerator FadeOut()
    {       
        if (isWaiting)
        {
            isWaiting = false;

            yield return new WaitForSeconds(waitTime);          
        }

        // reduce the alpha transparency of the text
        Color originalColor = text.color;
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
        Destroy(transform.parent.gameObject);
    }
}
