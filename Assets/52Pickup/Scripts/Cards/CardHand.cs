using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardHand : MonoBehaviour {


    [SerializeField]
    private GameObject _aimObject;
    public GameObject aimObject { get { return _aimObject; } }

    public List<GameObject> heldCards;

    private int maxCards = 52;
    private float maxFanAngle = (270f);
    private float fanRadius = 0.04f;
    private float depthPadding = 0.01f;

	// Update is called once per frame
	void Update () {
        updateCardPositions();
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
            //Angle in the circle
            float chordAng = -realFanAng * (((float)(i + 0.5) / heldCards.Count) - 0.5f);
            float upOffset = Mathf.Sin(chordAng * Mathf.Deg2Rad);
            float rightOffset = Mathf.Cos(chordAng * Mathf.Deg2Rad);
            float depthOffset = depthPadding * i;

            Vector3 positionOffset = new Vector3(upOffset, rightOffset, depthOffset) * fanRadius 
                * Mathf.Sqrt(heldCards.Count); //Expand the radius based on size.

            Quaternion angleOffset = transform.rotation * Quaternion.Euler(90, 0, 0) * Quaternion.Euler(0, 180- chordAng , 0);

            heldCards[i].GetComponent<HoverHandle>().setHoverPosition(positionOffset, angleOffset);
        }
    }

    public void addDeck(GameObject cardObject)
    {
        while(cardObject.GetComponent<CardDeck>().cards.Count > 0)
        {
            addCard(cardObject.GetComponent<CardDeck>().draw());
        }
    }

    public void addCard(GameObject cardObject)
    {
        heldCards.Add(cardObject);
        updateCardPositions();
        cardObject.transform.parent = gameObject.transform;
        if (cardObject.GetComponent<CardDeck>().inverted)
        {
            cardObject.GetComponent<CardDeck>().flip();
        }

        //Set the parent to me
        cardObject.GetComponent<CardDeck>().parentHand = gameObject;
    }

    public void removeCardWithReference(GameObject cardDeck)
    {
        heldCards.Remove(cardDeck);
        updateCardPositions();
    }
}
