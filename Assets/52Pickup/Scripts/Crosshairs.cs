using UnityEngine;
using System.Collections;

public class Crosshairs : MonoBehaviour {
    public Texture2D crosshairTexture;
    public Rect position;

    public bool show;


    void Start()
    {
        position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height -
            crosshairTexture.height) / 2, crosshairTexture.width, crosshairTexture.height);
    }

    void OnGUI()
    {
        if (show == true)
        {
            GUI.DrawTexture(position, crosshairTexture);
        }
    }
}
