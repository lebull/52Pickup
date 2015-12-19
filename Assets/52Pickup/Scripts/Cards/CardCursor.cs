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



        //Swipe Down
        
        if (inputManager.swipeDown)
        {
            //Pick up card
            RaycastHit cardHit = FindObjectOfType<RaycastManager>().raycastCard();
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

                //If we're pointing at a hand
                RaycastHit handHit = FindObjectOfType<RaycastManager>().raycastHand();
                if (handHit.collider)
                {
                    handHit.collider.gameObject.GetComponent<CardHand>().condenseAllCardsToDeck();
                    pickUpObject(handHit.collider.gameObject.GetComponent<CardHand>().heldCards[0]);

                }
                else
                {
                    //If we are pointing at a CardDeck
                    RaycastHit cardHit = FindObjectOfType<RaycastManager>().raycastCard();
                    if (cardHit.collider && cardHit.collider.gameObject.GetComponent<CardDeck>())
                    {
                        pickUpObject(cardHit.collider.gameObject);
                    }
                }


            }else if(heldObject != null
                && heldObject.GetComponent<CardDeck>() != null)
            {
                //If we're pointing at a hand
                RaycastHit handHit = FindObjectOfType<RaycastManager>().raycastHand();
                if (handHit.collider)
                {
                    handHit.collider.gameObject.GetComponent<CardHand>().addCard(heldObject);
                }

                //If we're pointing at a CardDeck
                RaycastHit cardHit = FindObjectOfType<RaycastManager>().raycastCard();
                if (cardHit.collider && cardHit.collider.gameObject != heldObject)
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
        pickupObject.GetComponent<CardDeck>().removeFromHand();

        heldObject = pickupObject;
        heldObjectOrigionalLayer = heldObject.layer;
        //heldObject.transform.parent = transform;
        heldObject.layer = LayerMask.NameToLayer("IgnoreRaycast");
    }

    void releaseObject()
    {
        //Reset the layer in case it needs to be raycasted
        heldObject.layer = heldObjectOrigionalLayer;

        //If we are still the parent, remove it from our transform hiearchy.
        if(heldObject.transform.parent == transform)
        {
            heldObject.transform.parent = null;
        }

        heldObject = null;
    }

    void updatePositionToGaze()
    {
        RaycastHit hit = FindObjectOfType<RaycastManager>().raycastGeneral();
        if (hit.collider)
        {
            //Hide cursor if we hit a card.
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Card"))
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = true;
            }
            
            transform.position = hit.point + hit.normal*hitOffset;
            transform.rotation = Quaternion.Euler(hit.normal);
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
            transform.position = defaultCursorLocation.transform.position;
        }
    }
}
