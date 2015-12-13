using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardHand : MonoBehaviour {


    [SerializeField]
    private GameObject _aimObject;
    public GameObject aimObject { get { return _aimObject; } }

    public List<GameObject> heldCards;

    private int maxCards = 52;
    private float maxFanAngle = (200f);
    private float fanRadius = 0.15f;
    private float depthPadding = 0.01f;

	// Update is called once per frame
	void Update () {
        //updateAimObject();

        //Check for inputs.
        /*
        if (Input.GetMouseButtonDown(0) && aimObject != null)
        {
            heldCards.Add(aimObject);
            updateCardPositions();
            aimObject.transform.parent = gameObject.transform;
        }*/
    }
    /*
    void updateAimObject()
    {
        //http://answers.unity3d.com/questions/252847/check-if-raycast-hits-layer.html

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10, Color.green);

        // Get our aim object
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Card")))
        {
            _aimObject = hit.transform.gameObject;
        }
        else
        {
            _aimObject = null;
        }
    }*/


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

            Vector3 positionOffset = new Vector3(upOffset, rightOffset, depthOffset) * fanRadius;

            heldCards[i].GetComponent<HoverHandle>().setHoverPosition(transform.rotation * positionOffset, chordAng);
        }
    }

    public void addCard(GameObject cardObject)
    {
        heldCards.Add(cardObject);
        updateCardPositions();
        cardObject.transform.parent = gameObject.transform;
    }
}
