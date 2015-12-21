using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardHand : MonoBehaviour {


    [SerializeField]
    private GameObject _aimObject;
    public GameObject aimObject { get { return _aimObject; } }
    public bool condensed { get { return heldCards.Count == 1;  } }
    public List<GameObject> heldCards;

    private int maxCards = 52;
    private float maxFanAngle = (270f);
    private float fanRadius = 0.04f;
    private float depthPadding = 0.01f;
    private float handCenterOffset = 0.15f;

	// Update is called once per frame
	void Update () {
        purgeNullSpots();
    }


    void purgeNullSpots()
    {

        bool updated = false;
        int i = 0;
        while (i < heldCards.Count)
        {
            if (heldCards[i] == null)
            {
                heldCards.RemoveAt(i);
                updated = true;
            }
            else
            {
                i++;
            }

            if (updated)
            {
                updateCardPositions();
            }
        }
    }

    //Tell the cards where they are supposed to go.
    void updateCardPositions()
    {
        int count = heldCards.Count;

        //Get real fan angle
        float realFanAng = Mathf.Min(
            maxFanAngle * Mathf.Sqrt((float)count / maxCards),
            maxFanAngle
        );

        //If we dynamically make circle radius, do so here.

        //For each card in our hand
        for(int i = 0; i < heldCards.Count; i++)
        {
            if(heldCards[i] == null)
            {
                heldCards.RemoveAt(i);
            }
            else
            {
                //Angle in the circle
                float chordAng = -realFanAng * (((float)(i + 0.5) / heldCards.Count) - 0.5f);
                float upOffset = Mathf.Sin(chordAng * Mathf.Deg2Rad);
                float rightOffset = Mathf.Cos(chordAng * Mathf.Deg2Rad);
                float depthOffset = depthPadding * i;

                Vector3 positionOffset = Vector3.up * handCenterOffset + new Vector3(upOffset, rightOffset, depthOffset) * fanRadius
                    * Mathf.Sqrt(heldCards.Count); //Expand the radius based on size.

                Quaternion angleOffset = transform.rotation * Quaternion.Euler(90, 0, 0) * Quaternion.Euler(0, 180 - chordAng, 0);

                heldCards[i].GetComponent<HoverHandle>().setHoverPosition(positionOffset, angleOffset);
            }
        }
    }


    public void addCard(GameObject addCardObject, GameObject aheadOfExistingObject = null)
    {
        //If no existing object (probably adding via the marker), add to the back
        if(aheadOfExistingObject == null)
        {
            heldCards.Insert(0, addCardObject);
        }
        else // Add it ahead of the existing card.
        {
            int insertIndex = heldCards.IndexOf(aheadOfExistingObject) + 1;
            heldCards.Insert(insertIndex, addCardObject);
        }

        //We wana let the chillens spread out.
        while (addCardObject.GetComponent<CardDeck>().cards.Count > 1)
        {
            //Add children in front of me.
            addCard(addCardObject.GetComponent<CardDeck>().draw(), addCardObject);
        }

        if (addCardObject.GetComponent<CardDeck>().inverted)
        {
            addCardObject.GetComponent<CardDeck>().flip();
        }

        addCardObject.transform.parent = gameObject.transform;
        addCardObject.GetComponent<CardDeck>().parentHand = gameObject;
        updateCardPositions();
    }


    public void removeCardWithReference(GameObject cardDeck)
    {
        heldCards.Remove(cardDeck);
        updateCardPositions();
    }

    public void releaseCard(GameObject cardDeck)
    {
        removeCardWithReference(cardDeck);
        cardDeck.transform.parent = null;
        cardDeck.GetComponent<CardDeck>().parentHand = null;

        heldCards.Remove(cardDeck);
        updateCardPositions();
    }

    public void condenseAllCardsToDeck()
    {
        if (heldCards.Count > 0)
        {
            //GameObject newCard = (GameObject)Instantiate(FindObjectOfType<CardSetManager>().cardPrefab, transform.position, transform.rotation);
            GameObject newCard = heldCards[0];
            List<int> cardIndexes = new List<int>();

            while(heldCards.Count > 1)
            {
                GameObject card = heldCards[1];
                cardIndexes.AddRange(card.GetComponent<CardDeck>().cards);
                heldCards.Remove(card);
                Destroy(card);
                
            }

            newCard.GetComponent<CardDeck>().setCards(cardIndexes);

            //newCard.transform.parent = gameObject.transform;
            //newCard.GetComponent<CardDeck>().parentHand = gameObject;
            heldCards = new List<GameObject>() { newCard };

            updateCardPositions();
        }

    }
}
