using UnityEngine;
using System.Collections;

public class Highlighter : MonoBehaviour {

    public GameObject highlightObject;

    private RaycastManager raycastManager;
    private MeshRenderer highlightObjectRenderer;

    private bool highlighted;

	// Use this for initialization
	void Start () {
        setHighlight(false);
    }

    public void setHighlight(bool highlight)
    {
        highlightObject.GetComponent<MeshRenderer>().enabled = highlight;
    }

    // Update is called once per frame
    void Update () {
        /*
        if (raycastManager) {

            
            bool shouldBeHighlighted = raycastManager.raycastGeneral().collider &&
                raycastManager.raycastGeneral().collider.gameObject == gameObject;

            if(highlighted != shouldBeHighlighted)
            {
                //Show/Hide our renderer conditionally
                highlightObjectRenderer.enabled = shouldBeHighlighted;
                highlighted = shouldBeHighlighted;
            }
            

        }*/
    }
}
