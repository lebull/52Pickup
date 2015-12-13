using UnityEngine;
using System.Collections;

public class HoverHandle : MonoBehaviour {


    public Vector3 targetPosition; //Offset from parent if it has one, global otherwise.
    public Quaternion rotationOffset;
    public Quaternion offsetForInversion;
    private bool hover;

    public bool inverted;

    public float speedMod = 20f;
    public float angSpeedMod = 20f;

	// Update is called once per frame
	void Update () {
        if (hover)
        {
            offsetForInversion = Quaternion.Euler(0, 0, 0);
            if (inverted) {
                offsetForInversion = Quaternion.Euler(0, 0, 180);
            }
            
            //Get a psudo origin and rotation.  Go ahead and adjust for inversion.
            Quaternion originRot = Quaternion.identity * offsetForInversion;

            if (transform.parent != null) //Parent exists
            {
                Vector3 origin = transform.parent.position;
                originRot = transform.parent.rotation * offsetForInversion;


                //Move the card towards the target pos.
                // I don't know why this works QQ 
                transform.position = Vector3.Lerp(
                    transform.position,
                    origin + (originRot * targetPosition), //Multiply, makes the offset relative to the target with respect to orientation.
                    speedMod * Time.deltaTime);

                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    originRot * Quaternion.Euler(-90, 0, 0) * rotationOffset * offsetForInversion,
                    angSpeedMod * Time.deltaTime);

            }
            else //No parent
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    targetPosition, //Multiply, makes the offset relative to the target with respect to orientation.
                    speedMod * Time.deltaTime);

                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.identity * rotationOffset * offsetForInversion,
                    angSpeedMod * Time.deltaTime);
            }
        }
	}

    /// <summary>
    /// DEPRICATE THIS SHIT
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rotation"></param>
    public void setHoverPosition(Vector3 pos, float rotation)
    {
        setHoverPosition(pos, Quaternion.Euler(0, rotation, 0));
    }

    public void setHoverPosition(Vector3 pos, Quaternion rotation)
    {
        targetPosition = pos;
        hover = true;
        GetComponent<Rigidbody>().isKinematic = true;
        rotationOffset = rotation;
    }

    public void releaseFromHover()
    {
        hover = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
