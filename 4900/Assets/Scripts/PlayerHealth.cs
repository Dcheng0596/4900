using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


// Handles player health, health slider, and status conditions
public class PlayerHealth : NetworkBehaviour
{
    public int startingHealth = 100;
    [SyncVar]                         
    public int currentHealth;                                   
    private Slider healthSlider;                              

    private Rigidbody2D rb2D;

    Animator anim;
                           
    Player player;                                                 
    bool isDead;                                               
    bool isDamaged;                                             


    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        healthSlider = GetComponentInChildren<Slider>();
        rb2D = GetComponent<Rigidbody2D>();
    
        currentHealth = startingHealth;
        healthSlider.value = startingHealth;

        if (isLocalPlayer)
        {
            Image fill = healthSlider.transform.GetChild(1).GetComponentInChildren<Image>();
            fill.color = new Color32(0, 229, 0, 255);
        }

    }

    void Update()
    {
        SetHealthSliderPosition();

        if (!isLocalPlayer)
            return;
        CmdSendSlider(currentHealth);    

        if (currentHealth <= 0)
        {
            Death();
        }

        isDamaged = false;
    }

    public void TakeDamage(int amount)
    {     
        isDamaged = true;
 
        if (!isServer)
            return;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
     
       
        if (currentHealth <= 0 && !isDead)
          Death();
    }

    [ClientRpc]
    void RpcSetSlider(int health)
    {
        healthSlider.value = health;
    }

    [Command]
    void CmdSendSlider(int health)
    {
        RpcSetSlider(health);
    }

    void Death()
    {
        isDead = true;

        player.enabled = false;
        rb2D.bodyType = RigidbodyType2D.Static;
        GetComponent<Animator>().enabled = false;
    }

    void SetHealthSliderPosition()
    {
        healthSlider.transform.position = this.transform.position + new Vector3(0, 0.55f, 0);
        healthSlider.transform.rotation = Quaternion.identity;
    }
    
   

}