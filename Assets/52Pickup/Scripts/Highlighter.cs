using UnityEngine;
using System.Collections;

public class Highlighter : MonoBehaviour {

    public GameObject highlightObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        highlightObject.SetActive(true);
    }

    void OnMouseExit()
    {
        highlightObject.SetActive(false);
    }
}
