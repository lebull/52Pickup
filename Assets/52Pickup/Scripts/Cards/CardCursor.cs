using UnityEngine;
using System.Collections;

public class CardCursor : MonoBehaviour {

    InputManager inputManager;

    private float hitOffset = 0.02f;
    private float hoverOffset = 0.2f;

    private GameObject heldObject;
    private int heldObjectOrigionalLayer;

	// Use this for initialization
	void Start () {
        inputManager = FindObjectOfType<InputManager>();
	}

    // Update is called once per frame
    void Update() {

        transform.rotation = transform.parent.rotation;

        //Swipe Down
        updatePositionToGaze();

        //Pick up card
        if (inputManager.swipeDown)
        {
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
                RaycastHit cardHit = FindObjectOfType<RaycastManager>().raycastCard();
                if(cardHit.collider && cardHit.collider.gameObject.GetComponent<CardDeck>())
                {
                    pickUpObject(cardHit.collider.gameObject);
                }
            }else if(heldObject != null
                && heldObject.GetComponent<CardDeck>() != null)
            {
                RaycastHit handHit = FindObjectOfType<RaycastManager>().raycastHand();
                if (handHit.collider)
                {
                    handHit.collider.gameObject.GetComponent<CardHand>().addDeck(heldObject);
                }

                RaycastHit cardHit = FindObjectOfType<RaycastManager>().raycastCard();

                //If we're pointing at a deck, send to this deck
                if (cardHit.collider && cardHit.collider.gameObject != heldObject)
                {
                    heldObject.GetComponent<CardDeck>().sendToDeck(cardHit.collider.gameObject);
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
            heldObject.GetComponent<HoverHandle>().setHoverPosition(transform.position + transform.up * hoverOffset, transform.rotation);
        }
    }

    void pickUpObject(GameObject pickupObject)
    {
        if (pickupObject.transform && pickupObject.transform.parent)
        {
            pickupObject.transform.parent = null;
        }
        
        if(pickupObject.GetComponent<CardDeck>())
        {
            pickupObject.GetComponent<CardDeck>().parentHand = null;
        }


        heldObject = pickupObject;
        heldObjectOrigionalLayer = heldObject.layer;
        heldObject.layer = LayerMask.NameToLayer("IgnoreRaycast");
    }

    void releaseObject()
    {
        heldObject.layer = heldObjectOrigionalLayer;
        heldObject = null;
    }

    void updatePositionToGaze()
    {
        RaycastHit hit = FindObjectOfType<RaycastManager>().raycastTable();
        if (hit.collider)
        {
            transform.position = hit.point + hit.normal*hitOffset;
            transform.rotation = Quaternion.Euler(hit.normal);
        }
    }
}
