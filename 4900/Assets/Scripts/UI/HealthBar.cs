using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public PlayerHealth health;

    private Slider healthBar;
    private Text healthText;
	// Use this for initialization
	void Awake ()
    {
        healthBar = this.GetComponent<Slider>();
        healthText = this.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateHealth();
	}

    void UpdateHealth()
    {
        if(health != null)
        {
            healthBar.maxValue = health.startingHealth;
            healthBar.value = health.currentHealth;
            healthText.text = health.currentHealth.ToString() + "/" + health.startingHealth.ToString();
        }
    }
}
