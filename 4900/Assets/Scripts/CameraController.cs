using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles camera Movement
public class CameraController : MonoBehaviour {

    public GameObject player;

    public Vector3 offset;
    Vector3 velocity;

    public float smoothSpeed;

    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate ()
    {
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        if (screenRect.Contains(Input.mousePosition))
            followMouse();

	}

    // Makes the camera follow the players mouse and move along with the player
    void followMouse()
    {
        Vector3 pTransform;

        if (player != null)
        {
            pTransform = new Vector3(player.transform.position.x, player.transform.position.y);

            float maxScreenPoint = 0.66f;

            Vector3 mousePos = Input.mousePosition * (maxScreenPoint) + new Vector3(Screen.width * ((1f - maxScreenPoint) * 0.5f),
                                                                                    Screen.height * ((1f - maxScreenPoint) * 0.5f), 0f);
            Vector3 position = (pTransform + Camera.main.ScreenToWorldPoint(mousePos)) / 2f;
            Vector3 destination = new Vector3(position.x, position.y, -10);

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothSpeed);

        }
    }
}
