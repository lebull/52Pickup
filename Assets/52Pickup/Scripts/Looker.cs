using UnityEngine;
using System.Collections;

public class Looker : MonoBehaviour {

    public GameObject target;

	// Update is called once per frame
	void Start () {
	    if(target != null)
        {
            transform.LookAt(target.transform.position);
        }
	}
}
