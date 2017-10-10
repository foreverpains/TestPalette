using HedgehogTeam.EasyTouch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteTouch : PaletteBase
{
    #region UI GameObject
    private Image m_bgImage;
    private Image m_selectImage;
    private RectTransform bgRect;
    private Canvas canvas;
    private RectTransform canvasRect;
    #endregion

    private Texture2D m_texture2D;

    private static PaletteTouch m_instance;
    public static PaletteTouch Instance()
    {
        if (m_instance == null)
        {
            m_instance = new PaletteTouch();
            m_instance.Init();
        }
        return m_instance;
    }

    public override void Init()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasRect = canvas.transform as RectTransform;
        m_bgImage = GameObject.Find("Bg").GetComponent<Image>();
        bgRect = m_bgImage.GetComponent<RectTransform>();
        m_selectImage = GameObject.Find("Image").GetComponent<Image>();
        InitColorArr();
    }

    public override void Update()
    {

    }

    public override bool OnTouchStart(Gesture gesture)
    {
        return ProccessGesture(gesture);
    }

    public override bool OnTouchDown(Gesture gesture)
    {
        return ProccessGesture(gesture);
    }

    public override bool OnTouchUp(Gesture gesture)
    {
        return ProccessGesture(gesture);
    }

    private bool ProccessGesture(Gesture gesture)
    {
        Vector2 mousePos = gesture.position;
        if (!gesture.IsOverRectTransform(bgRect))
        {
            return false;
        }

        Vector2 uiPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePos, canvas.GetComponent<Camera>(), out uiPos))
        {
            Vector2 leftTop = new Vector2(bgRect.localPosition.x - bgRect.rect.width / 2, bgRect.localPosition.y + bgRect.rect.height / 2);

            float uvx = (uiPos.x - leftTop.x) / bgRect.rect.width;
            float uvy = (leftTop.y - uiPos.y) / bgRect.rect.height;

            CurrColor = GetPaletteColor(uvx, 1 - uvy);
            m_selectImage.color = CurrColor;
        }
        return true;
    }


    private Color m_currColor;
    public Color CurrColor
    {
        get
        {
            return m_currColor;
        }

        set
        {
            m_currColor = value;
        }
    }

    void InitColorArr()
    {
        m_texture2D = m_bgImage.mainTexture as Texture2D;
    }

    public Color GetPaletteColor(float uvx, float uvy)
    {        
        return m_texture2D.GetPixel((int)(uvx * m_texture2D.width), (int)(uvy * m_texture2D.height));
    }
}
