using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Ok, let's get this one freeking thing straight.
//Top and bottom is the physical top and bottom.  As in transforms.
//Beginning and end reffers to the position int he card list.
//Going up = going up.  If a deck is FACE UP, then 0 is on the bottom and n-1 is on top.

public class CardDeck : MonoBehaviour {

    public float deckThicknessMod = 1;

    [SerializeField] GameObject cardSetManager;

    public GameObject parentHand;

    public List<int> cards;

    public bool inverted { get{return GetComponent<HoverHandle>().inverted;}}
    

	// Use this for initialization
	void Start () {
        cardSetManager = GameObject.Find("CardSetManager");
    }
	
    public void setCards(List<int> newCardIndexes)
    {
        cardSetManager = GameObject.Find("CardSetManager");
        cards = newCardIndexes;
        transform.localScale = cardSetManager.GetComponent<CardSetManager>().cardPrefab.transform.localScale;
        rescaleDeck();refreshCardFace();
    }

    public void shuffleDeck()
    {
        List<int> newList = new List<int>();

        //http://www.vcskicks.com/randomize_array.php
        Random r = new Random();
        int randomIndex = 0;
        while (cards.Count > 0)
        {
            randomIndex = (int)Random.Range(0, cards.Count - 1); //Choose a random object in the list
            newList.Add(cards[randomIndex]); //add it to the new, random list
            cards.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        cards = newList;
    }

    public GameObject draw(bool fromBottom = false)
    {
        CardSetManager cardSetManagerScript = cardSetManager.GetComponent<CardSetManager>();

        //Calculate how many cards will remain
        int newCardCount = cards.Count - 1;

        //Calculate card height
        float cardHeight = cardSetManagerScript.cardPrefab.transform.localScale.y;

        //Calculate new deck height
        float newDeckHeight = newCardCount * cardHeight;

        //Calculate offspring height
        float offspringHeight = cardHeight;

        //For dumb maths
        int negativeIfBottom = 1;
        if (fromBottom != inverted)
        {
            negativeIfBottom = -1;
        }

        //Calculate point where the card/deck will spawn.
        Vector3 offspringPosition = transform.position +
            ( - transform.up * (transform.localScale.y / 2) //Gets us to the edge of the current deck
            + transform.up * (offspringHeight / 2)      //Now, half of the height of a card.
            + transform.up * 0.05f) //New pos :D
             * negativeIfBottom;     //Reverse if we're drawing from the bottom.                

        //Calculate new deck position
        transform.position = transform.position
            + transform.up * (cardHeight * 0.5f * deckThicknessMod) * negativeIfBottom;

        int indexToRemove;
        if (fromBottom == inverted)
        {
            indexToRemove = cards.Count - 1;
        } else {
            indexToRemove = 0;
        }

        int newCardIndex = cards[indexToRemove];
        cards.RemoveAt(indexToRemove);

        //Rescale our size
        rescaleDeck();

        GameObject newCard = (GameObject)Instantiate(cardSetManagerScript.cardPrefab, offspringPosition, transform.rotation);
        newCard.GetComponent<CardTextureHandler>().setCardTextureByIndex(newCardIndex);
        newCard.GetComponent<CardDeck>().setCards(new List<int> { newCardIndex });

        //Invert the new deck if we are inverted too.
        if (inverted)
        {
            newCard.GetComponent<CardDeck>().flip();
        }

        //If new count == 0, destroy myself.
        if(newCardCount == 0)
        {
            Destroy(gameObject);
        }
        else {
            refreshCardFace();
        }
        return newCard;
    }

    public void refreshCardFace()
    {
        GetComponent<CardTextureHandler>().setCardTextureByIndex(cards[cards.Count - 1]);
    }

    public void addCards(List<int> addCards, bool toBottom = false)
    {
        //TODO:  Should depend on which side is top.  For now, add to the end.

        Debug.Log(toBottom);
        if (inverted != toBottom)
        {
            addCards.AddRange(cards);
            cards = addCards;
        }else{
            cards.AddRange(addCards);
        }

        rescaleDeck();
        refreshCardFace();
    }

    private void rescaleDeck()
    {
        CardSetManager cardSetManagerScript = cardSetManager.GetComponent<CardSetManager>();
        float cardHeight = cardSetManagerScript.cardPrefab.transform.localScale.y;

        transform.localScale = new Vector3(
            transform.localScale.x,
            Mathf.Sqrt(cards.Count) * cardHeight,
            transform.localScale.z);
    }

    public void sendToDeck(GameObject targetDeck, bool toBottom = false)
    {
        if(targetDeck.GetComponent<CardDeck>() == null)
        {
            throw new System.Exception("Cannot send a gameobject to a deck if it doesn't have a CardDeck script attatched.");
        }

        //If the target deck is inverted and this is also inverted, it doubles up on the inverts.  And we don't want that.
        //I'm actually not really sure how it works, but oh well.  I think it's because setHoverPosition doesn't undo inverts.
        if (inverted)
        {
            flip();
        }
        StartCoroutine(sendToDeckTick(targetDeck, toBottom));

    }

    IEnumerator sendToDeckTick(GameObject targetDeck, bool toBottom = false)
    {
        float startTime = Time.time;
        while (Vector3.Distance(transform.position, targetDeck.transform.position) > 0.05f
            && Time.time - startTime < 0.5f)
        {
            
            Debug.Log(targetDeck.transform.rotation.eulerAngles);
            GetComponent<HoverHandle>().setHoverPosition(targetDeck.transform.position, targetDeck.transform.rotation);
            yield return new WaitForFixedUpdate();
        }
        targetDeck.GetComponent<CardDeck>().addCards(cards, toBottom);
        Destroy(gameObject);
    }

    public void flip()
    {
        GetComponent<HoverHandle>().inverted = !GetComponent<HoverHandle>().inverted;
    }

    void removeFromHand()
    {
        parentHand.GetComponent<CardHand>().removeCardWithReference(gameObject);
        transform.parent = null;
    }

    void OnDestroy()
    {
        if(parentHand != null)
        {
            removeFromHand();
        }

    }
}
