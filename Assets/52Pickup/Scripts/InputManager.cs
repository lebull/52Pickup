using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public bool shortClick { get { return _shortClick; } }
    public bool longClick { get { return _longClick; } }
    public bool swipeDown { get { return _swipeDown; } }
    public bool swipeUp { get { return _swipeUp; } }

    private bool _shortClick;
    private bool _longClick;
    private bool _swipeDown;
    private bool _swipeUp;

    private bool _clickPressed;
    private float _clickPressedStartTime;

    private float _swipeLastMovedTime;

    private float longPressTimeThreshhold = 0.2f;
    private float swipeGestureRestTime = 0.2f;

    // Use this for initialization
    void Start () {
        _shortClick = false;
        _longClick = false;
        _swipeDown = false;
        _swipeUp = false;

        _clickPressed = false;
    }
	
	// Update is called once per frame
	void Update () {

        _shortClick = false;
        _longClick = false;
        _swipeDown = false;
        _swipeUp = false;

        if (Input.GetMouseButtonDown(0) && _clickPressed == false)
        {
            _clickPressedStartTime = Time.time;
            _clickPressed = true;
        }

        float pressedTime = Time.time - _clickPressedStartTime;

        if (_clickPressed == true && Input.GetMouseButtonUp(0))
        {
            _clickPressed = false;
            _shortClick = true;
        }

        if (_clickPressed == true && pressedTime > longPressTimeThreshhold)
        {
            _clickPressed = false;
            _longClick = true;
        }


        //Swipe gesture
        if(Time.time - _swipeLastMovedTime > swipeGestureRestTime)
        {
            //Swipe down
            if(Input.GetAxis("Swipe") < 0)
            {
                _swipeDown = true;
            }

            //Swipe Up
            if (Input.GetAxis("Swipe") > 0)
            {
                _swipeUp = true;
            }
        }
        if(Input.GetAxis("Swipe") != 0)
        {
            _swipeLastMovedTime = Time.time;
        }

        
    }
}
