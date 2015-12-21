using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighlightDriver : MonoBehaviour {

    RaycastManager raycastManager;

    Highlighter currentHighlight;
    Highlighter formerHighlight;

    // Use this for initialization
    void Start () {
        raycastManager = FindObjectOfType<RaycastManager>();
    }
	
	// Update is called once per frame
	void Update () {


        //Highlight new objects

        RaycastHit hit = raycastManager.raycastGeneral();
        if (hit.collider)
        {
            GameObject aimObject = hit.collider.gameObject;
            Highlighter hitHighlighter = aimObject.GetComponent<Highlighter>();
            if (hitHighlighter)
            {
                if(hitHighlighter != currentHighlight)
                {
                    setNewHighlight(hitHighlighter);
                }
            }
            else
            {
                setNewHighlight(null);
            }
        }
    }

    void setNewHighlight(Highlighter newHighlight)
    {
        formerHighlight = currentHighlight;
        currentHighlight = newHighlight;

        if (currentHighlight != null)
        {
            currentHighlight.setHighlight(true);
        }

        if (formerHighlight != null)
        {
            formerHighlight.setHighlight(false);
        }
    }
}
