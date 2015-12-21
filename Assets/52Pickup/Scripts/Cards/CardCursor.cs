using UnityEngine;
using System.Collections;

public class CardCursor : MonoBehaviour {

    InputManager inputManager;

    public GameObject defaultCursorLocation;

    private float hitOffset = 0.02f;
    private float hoverOffset = 0.1f;

    public GameObject heldObject;

    private int heldObjectOrigionalLayer;

	// Use this for initialization
	void Start () {
        inputManager = FindObjectOfType<InputManager>();
	}

    // Update is called once per frame
    void Update() {

        transform.rotation = transform.parent.rotation;
        updatePositionToGaze();

        RaycastHit cardHit = FindObjectOfType<RaycastManager>().raycastCard();
        RaycastHit handHit = FindObjectOfType<RaycastManager>().raycastHand();

        //Testing for Gaze
        if (cardHit.collider)
        {
            CardPreview cardPreview = FindObjectOfType<CardPreview>();
            cardPreview.previewCard(cardHit.collider.gameObject);
        }

        //Swipe Down
        if (inputManager.swipeDown)
        {
            //Pick up card
            if (cardHit.collider
                && cardHit.collider.gameObject.layer == LayerMask.NameToLayer("Card")
                && cardHit.collider.gameObject.GetComponent<HoverHandle>() != null)
            {
                if (heldObject == null)//If our hand is empty, just make it our heldobject.
                {
                    pickUpObject(cardHit.collider.gameObject.GetComponent<CardDeck>().draw());
                }
                else//Else, add it to our heldObject deck
                {
                    cardHit.collider.gameObject.GetComponent<CardDeck>().draw().GetComponent<CardDeck>().sendToDeck(heldObject, true);
                }
            }
        }

        //Swipe up
        if (inputManager.swipeUp)
        {
            if (heldObject != null)
            {
                //Draw from the bottom of the deck.
                heldObject.GetComponent<CardDeck>().draw(true);
            }
        }

        //Long Press
        if (inputManager.longClick)
        {
            //Place something just by dropping it.
            if(heldObject != null 
                && heldObject.GetComponent<HoverHandle>() != null)
            {
                heldObject.GetComponent<HoverHandle>().releaseFromHover();
                releaseObject();
            }
        }

        //Short press
        if (inputManager.shortClick)
        {
            //If we aren't holding a deck, try to pick up a deck.
            if (heldObject == null)
            {

                //If we're pointing at a hand marker
                if (handHit.collider)
                {
                    CardHand hand = handHit.collider.gameObject.GetComponent<CardHand>();
                    hand.condenseAllCardsToDeck();
                    
                    if(hand.heldCards.Count > 0)
                    {
                        pickUpObject(hand.heldCards[0]);
                    } 
                    

                }
                else
                {
                    //If we are pointing at a CardDeck
                    if (cardHit.collider && cardHit.collider.gameObject.GetComponent<CardDeck>())
                    {
                        pickUpObject(cardHit.collider.gameObject);
                    }
                }


            }else if(heldObject != null
                && heldObject.GetComponent<CardDeck>() != null)
            {
                //If we're pointing at a hand
                if (handHit.collider)
                {
                    GameObject toHandObject = releaseObject();
                    handHit.collider.gameObject.GetComponent<CardHand>().addCard(toHandObject);
                }
                //If we're pointing at a CardDeck
                else if (cardHit.collider && cardHit.collider.gameObject != heldObject)
                {
                    //If it's in a hand
                    if(cardHit.collider.gameObject.GetComponent<CardDeck>().parentHand != null)
                    {
                        //Add to the hand in front of the deck
                        CardHand hand = cardHit.collider.gameObject.GetComponent<CardDeck>().parentHand.GetComponent<CardHand>();
                        hand.addCard(heldObject, cardHit.collider.gameObject);
                    }
                    else
                    {
                        //Just add it to the deck
                        heldObject.GetComponent<CardDeck>().sendToDeck(cardHit.collider.gameObject);
                    }
                    
                    releaseObject();
                }
                else //If we're not really pointint at anything, just drop it.
                {
                    heldObject.GetComponent<HoverHandle>().releaseFromHover();
                    releaseObject();
                }
            }
        }

        //Right swipe
        if(inputManager.swipeRight)
        {
            if (heldObject)
            {
                heldObject.GetComponent<CardDeck>().flip();
            }
        }

        //Update heldObject position
        transform.rotation = transform.parent.rotation;
        if (heldObject != null && heldObject.GetComponent<HoverHandle>() != null)
        {
            heldObject.GetComponent<HoverHandle>().setHoverPosition(transform.position + Vector3.up * hoverOffset, transform.rotation);
        }
    }

    void pickUpObject(GameObject pickupObject)
    {
        //If it's in a hand, it needs to be removed
        CardDeck cardDeck = pickupObject.GetComponent<CardDeck>();
        if(cardDeck.parentHand)
        {
            cardDeck.parentHand.GetComponent<CardHand>().releaseCard(pickupObject);
        }
       

        heldObject = pickupObject;
        heldObjectOrigionalLayer = heldObject.layer;
        //heldObject.transform.parent = transform;
        heldObject.layer = LayerMask.NameToLayer("IgnoreRaycast");
    }

    GameObject releaseObject()
    {
        //Reset the layer in case it needs to be raycasted
        heldObject.layer = heldObjectOrigionalLayer;

        //If we are still the parent, remove it from our transform hiearchy.
        if(heldObject.transform.parent == transform)
        {
            heldObject.transform.parent = null;
        }

        GameObject returnObject = heldObject;

        heldObject = null;

        return returnObject;
        
    }

    void updatePositionToGaze()
    {
        RaycastHit tableHit = FindObjectOfType<RaycastManager>().raycastTable();
        RaycastHit cardHit = FindObjectOfType<RaycastManager>().raycastCard();
        RaycastHit handHit = FindObjectOfType<RaycastManager>().raycastHand();
        //If we're hitting something, move our position directly above it.

        /*
        if (cardHit.collider)
        {
            transform.position = cardHit.transform.position;
            GetComponent<MeshRenderer>().enabled = false; //Hide curser renderer
        }
        else*/ if (handHit.collider)
        {
            transform.position = handHit.transform.position;
            GetComponent<MeshRenderer>().enabled = false; //Hide curser renderer
        }
        else if (tableHit.collider)
        {
            transform.position = tableHit.point + tableHit.normal * hitOffset;
            transform.rotation = Quaternion.Euler(tableHit.normal);
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
            //transform.position = defaultCursorLocation.transform.position;
        }
    }
}
