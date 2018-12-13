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
    private DamageFlash flash;

    private Rigidbody2D rb2D;
    private Text gameOverText;

    Animator anim;

    Player player;
    bool isDead;
    bool isDamaged;
    bool isGameOver;


    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        healthSlider = GetComponentInChildren<Slider>();
        flash = GetComponent<DamageFlash>();
        rb2D = GetComponent<Rigidbody2D>();

        currentHealth = startingHealth;
        healthSlider.value = startingHealth;

        isGameOver = false;

        SetUIHealthBar();

        if (isLocalPlayer)
        {
            Image fill = healthSlider.transform.GetChild(1).GetComponentInChildren<Image>();
            fill.color = new Color32(0, 230, 0, 255);
        }

    }

    void SetUIHealthBar()
    {
        if (isLocalPlayer)
        {
            GameObject UI = GameObject.Find("UI");

            HealthBar UIhealthBar = UI.GetComponentInChildren<HealthBar>();

            UIhealthBar.health = this.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        SetHealthSliderPosition();

        if (currentHealth <= 0 && !isGameOver)
        {
            Death();
            isGameOver = true;
        }

        if (!isLocalPlayer)
            return;
        CmdSendSlider(currentHealth);

        isDamaged = false;
    }

    public void TakeDamage(int amount)
    {
        isDamaged = true;
        if (!isServer)
            return;
        Color color = new Color(1f, .6f, .6f);
        flash.CmdSendDamageFlash(color);
        currentHealth -= amount;
        healthSlider.value = currentHealth;

    }

    [ClientRpc]
    void RpcSetSlider(int health)
    {
        if (healthSlider != null)
            healthSlider.value = health;
    }

    [Command]
    void CmdSendSlider(int health)
    {
        RpcSetSlider(health);
    }

    [Command]
    void CmdDisableCollider()
    {
        RpcDisableCollider();
    }
    [ClientRpc]
    void RpcDisableCollider()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        Destroy(collider);
    }
    void Death()
    {
        if (isLocalPlayer)
        {
            isDead = true;

            CmdDisableCollider();
            rb2D.velocity = Vector3.zero;
            player.enabled = false;

            CmdSendDeathAnimationParameter(true);
        }
        StartCoroutine("GameOverText");


    }

    IEnumerator GameOverText()
    {
        GameObject lobbyManager = GameObject.Find("LobbyManager");
        NetworkLobbyManager lobby = lobbyManager.GetComponent<NetworkLobbyManager>();

        GameObject ScreenText = GameObject.Find("ScreenText");
        Text gameOverText = ScreenText.GetComponent<Text>();

        if (isLocalPlayer)
        {
            Debug.Log("Death");
            yield return new WaitForSeconds(2);
            gameOverText.text = "YOU LOSE";
            yield return new WaitForSeconds(4);
            ScreenText.GetComponent<RectTransform>().localScale = new Vector2(5, 5);
            gameOverText.text = "Exiting Lobby ...";
            yield return new WaitForSeconds(2);
        }
        else
        {
            yield return new WaitForSeconds(2);
            gameOverText.text = "YOU WIN";
            yield return new WaitForSeconds(4);
            ScreenText.GetComponent<RectTransform>().localScale = new Vector2(5, 5);
            gameOverText.text = "Exiting Lobby ...";
            yield return new WaitForSeconds(2);
        }
    
        lobby.ServerReturnToLobby();
        
        
    }
    void SetHealthSliderPosition()
    {
        healthSlider.transform.position = transform.position + new Vector3(0, 0.65f, 0);
        healthSlider.transform.rotation = Quaternion.identity;
    }

    [Command]
    void CmdSendDeathAnimationParameter(bool state)
    {
        RpcRecieveDeathAnimationParameter(state);
    }

    [ClientRpc]
    void RpcRecieveDeathAnimationParameter(bool state)
    {
        if (state)
            anim.SetBool("Dead", true);
        else
            anim.SetBool("Death", false);

        anim.SetLayerWeight(anim.GetLayerIndex("Lower"), 0);
    }

}