using UnityEngine;
using System.Collections;

public class Highlighter : MonoBehaviour {

    public GameObject highlightObject;

    private RaycastManager raycastManager;
    private MeshRenderer highlightObjectRenderer;

    private bool highlighted;

	// Use this for initialization
	void Start () {
        raycastManager = FindObjectOfType<RaycastManager>();
        highlightObjectRenderer = highlightObject.GetComponent<MeshRenderer>();
        highlighted = highlightObjectRenderer.enabled;
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
