using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CardSetManager : MonoBehaviour {

    public GameObject cardPrefab;
    public int cardCount;
    public Vector2 CardTextureDimensions;
    public List<GameObject> activeCards;


	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.D))
        {
            destroyDeck();
            spawnDeck();
        }
	}

    void destroyDeck()
    {
        foreach (GameObject card in activeCards)
        {
            Destroy(card);
        }
    }

    public void spawnDeck()
    {

        List<int> newCards = new List<int>();

        //Generate cards
        for (int i = 0; i < cardCount; i++)
        {
            newCards.Add(i);
        }

        GameObject newCardObject = (GameObject)Instantiate(cardPrefab, new Vector3(0, 1.5f, 0), Quaternion.identity);
        CardDeck newCardDeck = newCardObject.GetComponent<CardDeck>();
        newCardDeck.setCards(newCards);
        newCardDeck.shuffleDeck();
        newCardDeck.flip();
        newCardDeck.cardSetManager = gameObject;
        activeCards.Add(newCardObject);

        newCardDeck.transform.rotation *= Quaternion.Euler(0, 0, 180);
    }
}
