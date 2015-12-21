using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tester : MonoBehaviour {

    public GameObject cardPrefab;

	// Use this for initialization
	void Start () {

        CardSetManager cardSetManager = FindObjectOfType<CardSetManager>();
        cardSetManager.spawnDeck();

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void helloWorld()
    {
        Debug.Log("Hello World!");
    }
}
