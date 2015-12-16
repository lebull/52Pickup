using UnityEngine;
using System.Collections;

public class RaycastManager : MonoBehaviour {

    public bool debug;

    public AimMode aimMode;

    public enum AimMode
    {
        mouse,
        gaze
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public RaycastHit raycastHand()
    {
        return getDefaultRaycastFromMask(LayerMask.GetMask("CardHand"));
    }

    public RaycastHit raycastTable()
    {
        return getDefaultRaycastFromMask(LayerMask.GetMask("Table"));
    }

    public RaycastHit raycastCard()
    {
        return getDefaultRaycastFromMask(LayerMask.GetMask("Card"));

    }

    public RaycastHit raycastGeneral()
    {
        return getDefaultRaycastFromMask(~LayerMask.GetMask("IgnoreRaycast"));

    }

    RaycastHit getDefaultRaycastFromMask(LayerMask mask)
    {
        //http://answers.unity3d.com/questions/252847/check-if-raycast-hits-layer.html

        Ray ray;

        switch (aimMode){
            case AimMode.gaze:
                ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
                break;
            case AimMode.mouse:
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                break;
            default:
                ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
                break;
        }

        
        RaycastHit hit;

        if (debug)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10, Color.green);
        }

        Physics.Raycast(ray, out hit, Mathf.Infinity, mask);
        return hit;

    }
}
