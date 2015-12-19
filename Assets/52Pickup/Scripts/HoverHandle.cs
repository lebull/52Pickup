using UnityEngine;
using System.Collections;

public class HoverHandle : MonoBehaviour {


    public Vector3 targetPosition; //Offset from parent if it has one, global otherwise.
    public Quaternion rotationOffset;
    public Quaternion offsetForInversion;
    private bool hover;

    public bool inverted { get; private set; }

    public float speedMod = 20f;
    public float angSpeedMod = 20f;

    private bool locationNeedsUpdating;

    private float tolerance = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if ( hover && locationNeedsUpdating )

        {
                updatePositionAndRotation();
        }
        
    }

    private void updatePositionAndRotation()
    {
        //Set to false if we need to go through this again to get to our target pos.
        bool weMadeItToOurLocation = true;

        offsetForInversion = Quaternion.Euler(0, 0, 0);
        if (inverted)
        {
            offsetForInversion = Quaternion.Euler(0, 0, 180);
        }

        if (transform.parent != null) //Parent exists
        {
            Vector3 origin = transform.parent.position;
            Quaternion originRot = transform.parent.rotation;

            Vector3 localizedTargetPosition = origin + (originRot * targetPosition);

            //Move the card towards the target pos.
            // I don't know why this works QQ 
            transform.position = Vector3.Lerp(
                transform.position,
                localizedTargetPosition, //Multiply, makes the offset relative to the target with respect to orientation.
                speedMod * Time.deltaTime);

            if ((transform.position - localizedTargetPosition).magnitude > tolerance)
            {
                float debug = (transform.position - localizedTargetPosition).magnitude;
                weMadeItToOurLocation = false;
            }

        }
        else //No parent
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition, //Multiply, makes the offset relative to the target with respect to orientation.
                speedMod * Time.deltaTime);

            if ((transform.position - targetPosition).magnitude > tolerance)
            {
                weMadeItToOurLocation = false;
            }

        }

        Quaternion targetRotation = Quaternion.identity * rotationOffset * offsetForInversion;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            angSpeedMod * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) > tolerance)
        {
            weMadeItToOurLocation = false;
        }

        if (weMadeItToOurLocation)
        {
            locationNeedsUpdating = false;
        }
    }
    
    public void flip()
    {
        inverted = !inverted;
        offsetForInversion = Quaternion.Euler(0, 0, 0);
        if (inverted)
        {
            offsetForInversion = Quaternion.Euler(0, 0, 180);
        }

        locationNeedsUpdating = true;
    }

    public void setHoverPosition(Vector3 pos, Quaternion rotation)
    {
        targetPosition = pos;
        hover = true;
        GetComponent<Rigidbody>().isKinematic = true;
        rotationOffset = rotation;
        GetComponent<Collider>().isTrigger = true;

        locationNeedsUpdating = true;
    }

    public void releaseFromHover()
    {
        hover = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().isTrigger = false;
    }
}
