using UnityEngine;
using System.Collections;

public class RaycastManager : MonoBehaviour {

    public bool debug;

    [SerializeField]
    private GameObject _aimObject;
    public GameObject aimObject { get { return _aimObject; } }



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public RaycastHit raycastHand()
    {
        return updateAimObject(LayerMask.GetMask("CardHand"));
    }

    public RaycastHit raycastTable()
    {
        return updateAimObject(LayerMask.GetMask("Table"));
    }

    public RaycastHit raycastCard()
    {
        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10, Color.green);

        // Get our aim object
        Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Card"));
        return hit;*/
        return updateAimObject(LayerMask.GetMask("Card"));

    }

    RaycastHit updateAimObject(LayerMask mask)
    {
        //http://answers.unity3d.com/questions/252847/check-if-raycast-hits-layer.html

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (debug)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10, Color.green);
        }

        Physics.Raycast(ray, out hit, Mathf.Infinity, mask);
        return hit;

    }
}
