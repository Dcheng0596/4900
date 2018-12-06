using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Displays crosshair at mouse location
public class Crosshair : MonoBehaviour {

    public Texture2D crosshairImage;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot;

    void Start ()
    {
        setCrosshair();
    }

    void setCrosshair()
    {
        hotSpot = new Vector3(crosshairImage.width / 2, crosshairImage.height / 2);
        Cursor.SetCursor(crosshairImage, hotSpot, cursorMode);
    }
}
