using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tester : MonoBehaviour {

    public GameObject cardPrefab;

	// Use this for initialization
	void Start () {

        spawnDeck();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void spawnDeck()
    {
        CardSetManager cardSetManager = FindObjectOfType<CardSetManager>();

        List<int> newCards = new List<int>();

        //Generate cards
        for (int i = 0; i < cardSetManager.cardCount; i++)
        {
            newCards.Add(i);
        }

        GameObject newCardDeck = (GameObject)Instantiate(cardPrefab, new Vector3(0, 1.5f, 0), Quaternion.identity);
        newCardDeck.GetComponent<CardDeck>().setCards(newCards);
        newCardDeck.GetComponent<CardDeck>().shuffleDeck();
        newCardDeck.GetComponent<CardDeck>().flip();
        newCardDeck.transform.rotation *= Quaternion.Euler(0, 0, 180);
    }
}
