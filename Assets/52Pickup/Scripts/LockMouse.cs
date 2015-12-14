using UnityEngine;
using System.Collections;

public class LockMouse : MonoBehaviour {

	// Use this for initialization
	void OnGUI () {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
