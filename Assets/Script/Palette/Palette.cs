using HedgehogTeam.EasyTouch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Palette : MonoBehaviour
{
    private Vector2 dragStart;
    private Vector2 dragEnd;
    private Vector2 dragPrev;

    //palette
    private bool isTouchPalette = false;
    private Vector2 m_startTouchPos;
    private bool isMoveH = false;
    private bool isMoveV = false;
    private const float MOVE_STEP_V = 120;
    private const float MOVE_STEP_H = 3;
    private float m_moveOffsetRoate = START_RTATE;
    private const float START_RTATE = 35;
    private float m_moveOffsetRadius = MOVE_STEP_V / 2 + 1;

// 
//     public delegate void PaletteColorChangeH(float h);
// 
//     public event PaletteColorChangeH OnPaletteColorChangeH;
// 
//     public delegate void PaletteColorChangeV(float h);
// 
//     public event PaletteColorChangeV OnPaletteColorChangeV;

    private bool isSelectPalette;
    public bool IsSelectPalette
    {
        set
        {
            isSelectPalette = value;
        }

        get
        {
            return isSelectPalette;
        }
    }

    // Use this for initialization
    void Start()
    {
        InitEasyTouch();
        m_paletteMaterial = (gameObject.GetComponent<Renderer>()).material;
        m_selectImage =GameObject.Find("Image").GetComponent<Image>();
        InitColorArr();

        OnPaletteColorChangeH((m_moveOffsetRoate / MOVE_STEP_H) % 360);
        OnPaletteColorChangeV(Mathf.Clamp(m_moveOffsetRadius / MOVE_STEP_V, -0.5f, 5.5f));
    }

    // Update is called once per frame
    void Update()
    {
        m_currColor = GetPaletteColor(new Vector2(0.5f, 0.9f));
        m_selectImage.color = m_currColor;
    }

    public void OnTouchStart(Gesture gesture)
    {
        if (gesture.position.y < Screen.height / 4)
        {
            m_startTouchPos = gesture.position;
            isTouchPalette = true;
            return;
        }
    }

    public void OnTouchDown(Gesture gesture)
    {
        if (isTouchPalette)
        {
            dragEnd = gesture.position;
            Vector2 moveDelta = dragEnd - dragPrev;
            dragPrev = dragEnd;
            if (isMoveH == false && isMoveV == false)
            {
                float x = Math.Abs(dragEnd.x - m_startTouchPos.x);
                float y = Math.Abs(dragEnd.y - m_startTouchPos.y);
                if (x > 5 || y > 5)
                {
                    if (x > y)
                    {
                        isMoveH = true;
                    }
                    else
                    {
                        isMoveV = true;
                    }
                }
            }

            if (isMoveH)
            {
                m_moveOffsetRoate += moveDelta.x;
                OnPaletteColorChangeH((m_moveOffsetRoate / MOVE_STEP_H) % 360);
            }
            else if (isMoveV)
            {
                m_moveOffsetRadius += moveDelta.y;
                float temp = m_moveOffsetRadius / MOVE_STEP_V;
                if (temp < -0.5f)
                {
                    temp = -0.5f;
                    m_moveOffsetRadius = temp * MOVE_STEP_V;
                }
                else if (temp > 5.5f)
                {
                    temp = 5.5f;
                    m_moveOffsetRadius = temp * MOVE_STEP_V;
                }
                OnPaletteColorChangeV(temp);
            }
            IsSelectPalette = false;
            return;
        }

    }

    public void OnTouchUp(Gesture gesture)
    {

        if (isTouchPalette)
        {

            m_moveOffsetRoate = START_RTATE + Mathf.Round((m_moveOffsetRoate - START_RTATE) / MOVE_STEP_H / 22.5f) * 22.5f * MOVE_STEP_H;
            OnPaletteColorChangeH((m_moveOffsetRoate / MOVE_STEP_H) % 360);

            float temp = -0.5f + Mathf.Round(m_moveOffsetRadius / MOVE_STEP_V);
            OnPaletteColorChangeV(temp);

            isTouchPalette = false;
            isMoveH = false;
            isMoveV = false;
            return;
        }
    }



#region UI

    private Color m_currColor;
    private Image m_selectImage;
    private Material m_paletteMaterial;

    const int COLOR_NUM = 16;
    const int CELL_SIZE = 16;
    const int width = CELL_SIZE * COLOR_NUM;
    const int height = CELL_SIZE * 7;
    Color[] pixels = new Color[width * height];
    float _RotateOffset = 0;
    float _RadiusOffset = 0;
    public void OnPaletteColorChangeH(float h)
    {
        _RotateOffset = h;
        m_paletteMaterial.SetFloat("_RotateOffset", h);
    }
    public void OnPaletteColorChangeV(float v)
    {
        _RadiusOffset = v;
        m_paletteMaterial.SetFloat("_RadiusOffset", v);
    }


    void InitColorArr()
    {

        int colorStep = 360 / (COLOR_NUM - 1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = y * width + x;
                float h, s, v;
                int i = x / CELL_SIZE;
                int j = y / CELL_SIZE;
                if (i == COLOR_NUM - 1)
                {
                    h = 0.0f;
                    s = 0.0f;
                    v = 1.0f - j / 6.0f;
                }
                else
                {
                    h = (i * colorStep) / 360.0f;
                    s = (j < 3) ? ((j + 1) * 0.25f) : 1.0f;
                    v = (j <= 3) ? 1.0f : (7 - j) * 0.25f;
                }

                pixels[index] = Color.HSVToRGB(h, s, v);
            }
        }
    }

    public Color GetPaletteColor(Vector2 _uv)
    {
        float _MinRadius = 0.3f;
        float _MaxRadius = 0.5f;
        Vector2 center = new Vector2(0.5f, 0.5f);
        float r = Vector2.Distance(_uv, center);

        Vector2 vec = _uv - center;
        float rotate = Mathf.Rad2Deg * (Mathf.Atan(vec.y / vec.x)) + _RotateOffset;
        float ra = ((r - _MinRadius) * 2.0f / (_MaxRadius - _MinRadius) + _RadiusOffset) / 7.0f;

        Vector2 uv = new Vector2(rotate / 360.0f, 1.0f - ra);

        int x = (int)(uv.x * width);
        int y = (int)(uv.y * height);
        int index = (int)(y * width + x);
        return pixels[index];

    }

    #endregion

    public void InitEasyTouch()
    {
        GameObject.DontDestroyOnLoad(EasyTouch.instance.gameObject);

//         EasyTouch.On_SimpleTap += this.OnTap;
//         EasyTouch.On_Drag += this.OnDrag;
//         EasyTouch.On_DragStart += this.OnDragStart;
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
}
