using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Main : MonoBehaviour {

    PaletteBase m_pattle;
	// Use this for initialization
	void Start () {
        InitEasyTouch();
        if (Application.loadedLevelName == "TestPaletteTouch")
        {
            m_pattle = PaletteTouch.Instance();
        }
        else
        {
            m_pattle = PaletteTouchUGUI.Instance();
        }
        
    }

    // Update is called once per frame
    void Update () {
        m_pattle.Update();
    }


    public void InitEasyTouch()
    {
        GameObject.DontDestroyOnLoad(EasyTouch.instance.gameObject);

        //         EasyTouch.On_SimpleTap += this.OnTap;
        //         EasyTouch.On_Drag += this.OnDrag;
        //        EasyTouch.On_DragStart += this.OnDragStart;
        //         EasyTouch.On_DragEnd += this.OnDragEnd;
        EasyTouch.On_TouchStart += this.OnTouchStart;
        EasyTouch.On_TouchDown += this.OnTouchDown;
        EasyTouch.On_TouchUp += this.OnTouchUp;
        //         EasyTouch.On_Cancel += this.OnCancel;
        //         EasyTouch.On_TouchStart2Fingers += this.OnTouchStart2Fingers;
        //         EasyTouch.On_TouchDown2Fingers += this.OnTouchDown2Fingers;
        //         EasyTouch.On_TouchUp2Fingers += this.OnTouchUp2Fingers;
        //         EasyTouch.On_Cancel2Fingers += this.OnCancel2Fingers;
        //         EasyTouch.On_PinchIn += this.OnPinchIn;
        //         EasyTouch.On_PinchOut += this.OnPinchOut;
        //         EasyTouch.On_PinchEnd += this.OnPinchEnd;
    }

    private void OnTouchUp(Gesture gesture)
    {
        m_pattle.OnTouchUp(gesture);
    }

    private void OnTouchDown(Gesture gesture)
    {
        m_pattle.OnTouchDown(gesture);
    }

    private void OnTouchStart(Gesture gesture)
    {
        m_pattle.OnTouchStart(gesture);
    }
}
