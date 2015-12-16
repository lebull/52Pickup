using UnityEngine;
using System.Collections;

public class Highlighter : MonoBehaviour {

    public GameObject highlightObject;

    private RaycastManager raycastManager;

	// Use this for initialization
	void Start () {
        raycastManager = FindObjectOfType<RaycastManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (raycastManager) {
            highlightObject.SetActive(
                raycastManager.raycastGeneral().collider &&
                raycastManager.raycastGeneral().collider.gameObject == gameObject
            );
        }
    }

    void OnMouseEnter()
    {
        //highlightObject.SetActive(true);
    }

    void OnMouseExit()
    {
        //highlightObject.SetActive(false);
    }
}
