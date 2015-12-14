using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public bool shortClick { get { return _shortClick; } }
    public bool longClick { get { return _longClick; } }
    public bool swipeDown { get { return _swipeDown; } }
    public bool swipeUp { get { return _swipeUp; } }
    public bool swipeRight {  get { return Input.GetMouseButtonDown(1); } }

    private bool _shortClick;
    private bool _longClick;
    private bool _swipeDown;
    private bool _swipeUp;

    private bool _clickPressed;
    private float _clickPressedStartTime;

    private float _swipeLastMovedTime;

    private float longPressTimeThreshhold = 0.2f;
    private float swipeGestureRestTime = 0.2f;

    private float swipeDeltaThreshhold = 0.1f;

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
        if (Time.time - _swipeLastMovedTime > swipeGestureRestTime)
        {
            Debug.Log(Input.GetAxis("VSwipe"));
            //Swipe down
            if (-Input.GetAxis("VSwipe") > swipeDeltaThreshhold)
            {
                _swipeDown = true;
            }

            //Swipe Up
            if (Input.GetAxis("VSwipe") > swipeDeltaThreshhold)
            {
                _swipeUp = true;
            }

            /*
            //Swipe Left
            if (Input.GetAxis("HSwipe") < swipeDeltaThreshhold)
            {
                _swipeDown = true;
            }*/

            //Swipe Right
            if (Input.GetAxis("HSwipe") > swipeDeltaThreshhold)
            {
                //_swipeUp = true;
            }
        }



        if(Mathf.Abs(Input.GetAxis("VSwipe")) > swipeDeltaThreshhold || Mathf.Abs(Input.GetAxis("HSwipe")) > swipeDeltaThreshhold)
        {
            _swipeLastMovedTime = Time.time;
        }

        
    }
}
