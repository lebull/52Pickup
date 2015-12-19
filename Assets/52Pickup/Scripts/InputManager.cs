using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public bool debug;

    public bool shortClick { get; private set; }
    public bool longClick { get; private set; }
    public bool swipeDown { get; private set; }
    public bool swipeUp { get; private set; }
    public bool swipeRight { get; private set; }


    private float longPressTimeThreshhold = 0.5f;
    private float swipeDeltaThreshhold = 50f;

    private Vector3 mousePosStart;
    private float mousePressStartTime;
    // Use this for initialization

    Vector3 getMousePos()
    {
        return Input.mousePosition;


        /*
        return new Vector3(
            Input.GetAxis("HSwipe"),
            Input.GetAxis("VSwipe")    
        );*/
    }

    void newUpdate()
    {
        shortClick = false;
        longClick = false;
        swipeDown = false;
        swipeUp = false;
        swipeRight = false;

        //Move this with the rest, future me.
        //swipeRight = Input.GetMouseButtonDown(1);

        if (Input.GetMouseButtonDown(0) )
        {
            //Start tracking our touches
            mousePosStart = getMousePos();
            mousePressStartTime = Time.time;
        }

        if (Input.GetMouseButton(0))
        {
            //Track our touches however we need to here.
            //Debug.Log(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Figure out what kind of input it was
            float mousePressTime = Time.time - mousePressStartTime;
            Vector3 mouseMovementDelta = mousePosStart - getMousePos();


            //Presses
            if(mouseMovementDelta.magnitude < swipeDeltaThreshhold)
            {
                //Short press
                if (mousePressTime > longPressTimeThreshhold)
                {
                    longClick = true;
                }

                //Long Press
                else
                {
                    shortClick = true;
                }
            }
            //Swipes
            else
            {
                //Horizontal swipe
                if(Mathf.Abs(mouseMovementDelta.x) > Mathf.Abs(mouseMovementDelta.y))
                {
                    swipeRight = true;
                }
                else//Vertical swipe
                {
                    if(mouseMovementDelta.y < 0) //Yes, it's backwards.  Mouse vert is inverted.
                    {
                        swipeUp = true;
                    }
                    else
                    {
                        swipeDown = true;
                    }
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {

        if (debug)
        {
            shortClick = Input.GetKeyDown(KeyCode.LeftControl);
            longClick = Input.GetKeyDown(KeyCode.Space);
            swipeDown = Input.GetKeyDown(KeyCode.S);
            swipeUp = Input.GetKeyDown(KeyCode.W);
            swipeRight = Input.GetKeyDown(KeyCode.D);
        }
        else
        {
            newUpdate();
        }
    }
}
