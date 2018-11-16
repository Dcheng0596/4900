using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Handles player movement and rotation
public class Player : NetworkBehaviour
{
    Animator animator;

    private Rigidbody2D rb2D;

    public int speed;
    public int currentSpeed;

    public float smoothSpeed;

    public bool isAttacking;

    void Start()
    {
        isAttacking = false;
        currentSpeed = speed;

        animator = gameObject.GetComponent<Animator>();
        animator.SetInteger("LowerState", 1);

        rb2D = GetComponent<Rigidbody2D>();

        if (!isLocalPlayer)
            rb2D.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;
        playerMovement();

        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        if (screenRect.Contains(Input.mousePosition))
            playerRotation();
    
        SetLowerState();
    }
  

    public override void OnStartLocalPlayer()
    {
        setCamera();
    }

    // Assign a camera to the local player then disable the rest
    void setCamera()
    {
        Camera[] cams = Camera.allCameras;
        CameraController controller;

        if (isLocalPlayer == true)
        {
            cams[0].enabled = true;
            controller = cams[0].GetComponent<CameraController>();
            controller.player = this.gameObject;
            for (int i = 1; i < cams.Length; i++)
            {
                cams[i].enabled = false;
                controller = cams[i].GetComponent<CameraController>();
                controller.enabled = false;
            }
        }
    }
   
    void playerMovement()
    {

        Vector2 dir = new Vector2(Mathf.Lerp(0, Input.GetAxisRaw("Horizontal"), .8f), Mathf.Lerp(0, Input.GetAxisRaw("Vertical"), .8f));
        dir.Normalize();
        rb2D.velocity = dir * currentSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false)
        {
            animator.SetInteger("LowerState", 1);
        }
    }

    // Sets the players rotation based on the mouse's position
    void playerRotation()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, smoothSpeed);
    }

    // Sets animator parameters based on the rotation of the player relative to it's direction
    void SetLowerState()
    {
        if (Input.GetKey(KeyCode.W))
        {
            angleThreshold(2, 5, 3, 4);
        }
    }

    // Sets animator paramaters based on the angle the player is facing 
    void angleThreshold(int a, int b, int c, int d)
    {
        if (this.transform.rotation.eulerAngles.z >= 40 && this.transform.rotation.eulerAngles.z <= 140)
            animator.SetInteger("LowerState", a);
        else if (this.transform.rotation.eulerAngles.z > 140 && this.transform.rotation.eulerAngles.z < 220)
            animator.SetInteger("LowerState", b);
        else if (this.transform.rotation.eulerAngles.z >= 220 && this.transform.rotation.eulerAngles.z <= 320)
            animator.SetInteger("LowerState", c);
        else if (this.transform.rotation.eulerAngles.z > 320 && this.transform.rotation.eulerAngles.z <= 360 ||
            this.transform.rotation.eulerAngles.z >= 0 && this.transform.rotation.eulerAngles.z < 40)
            animator.SetInteger("LowerState", d);
    }
}


