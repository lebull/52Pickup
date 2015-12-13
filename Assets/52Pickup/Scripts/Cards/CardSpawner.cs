using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardSpawner : MonoBehaviour {

    public GameObject spawnPrefab;

    private List<GameObject> cardList;

	// Use this for initialization
	void Start () {
        cardList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            cardList.Add((GameObject)Instantiate(spawnPrefab, transform.position, transform.rotation));
            Debug.Log(cardList.Count);
        }

	}
}
