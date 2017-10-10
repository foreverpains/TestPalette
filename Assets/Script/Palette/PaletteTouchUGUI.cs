using ColorPaint;
using Common;
using HedgehogTeam.EasyTouch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class PaletteTouchUGUI : PaletteBase
{
    #region UI GameObject
    private Dictionary<int,Image> m_itemDict = new Dictionary<int, Image>();
    private Image m_selectImage;
    private RectTransform m_selectRootRect;
    private Slider m_slider;
    private Transform rootTrans;
    #endregion

    private bool isShow;

    private Vector2 m_currScreenPos = new Vector2(566,232);


    private int m_currSV = 3;
    private Vector3[] m_colorStepArr = {
            new Vector3(0, 0.25f, 1),
            new Vector3(0, 0.5f, 1),
            new Vector3(0, 0.75f, 1) ,
            new Vector3(0, 1, 1),
            new Vector3(0, 1,0.75f),
            new Vector3(0, 1,0.5f),
            new Vector3(0, 1,0.25f),
    };

    private Vector3[] m_grayStepArr = {
            new Vector3(0, 0, 1f),
            new Vector3(0, 0, 0.82f),
            new Vector3(0, 0, 0.66f) ,
            new Vector3(0, 0, 0.5f),
            new Vector3(0, 0, 0.32f),
            new Vector3(0, 0, 0.16f),
            new Vector3(0, 0, 0),
     };

    private int m_currColorKey;

    private Color m_currColor;
    public Color CurrColor
    {
        get
        {
            return m_currColor;
        }
    }

    private void SetCurrColor()
    {
        if (m_currColorKey == 0 || m_currColorKey == m_itemDict.Count-1)
        {
            m_currColor = new HSVColor(0, 0, m_grayStepArr[m_currSV].z);
        }
        else
        {
            HSVColor c = m_itemDict[m_currColorKey].color;
            m_currColor = new HSVColor(c.h, m_colorStepArr[m_currSV].y, m_colorStepArr[m_currSV].z);
        }
      
        
        //test
        m_selectImage.color = CurrColor;
    }

    private static PaletteTouchUGUI m_instance;
    
    public static PaletteTouchUGUI Instance()
    {
        if (m_instance == null)
        {
            m_instance = new PaletteTouchUGUI();
            m_instance.Init();
        }
        return m_instance;
    }

    public override void Init()
    {
       
        GameObject canvas = GameObject.Find("Canvas");
        rootTrans = ObjectEX.FindComponentInChildren<Transform>(canvas, "Root");
        GameObject panel = ObjectEX.GetGameObjectByName(canvas, "Panel");
        m_selectRootRect = ObjectEX.FindComponentInChildren<RectTransform>(panel, "SelectItemRoot"); 
        m_selectImage = ObjectEX.FindComponentInChildren<Image>(panel, "SelectItem");
        Image[] m_items = panel.GetComponentsInChildren<Image>();
        m_slider = ObjectEX.FindComponentInChildren<Slider>(canvas, "Slider");
        m_slider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        foreach (var v in m_items)
        {
           
            if (v.name.Contains("Image"))
            {
                int start = v.name.IndexOf('(') + 1;
                int len = v.name.IndexOf(')') - start;
                string str = v.name.Substring(start, len);
                int index = 0;
                try
                {
                    index = int.Parse(str);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Int Parse Error: str = " + str);
                }

                try
                {
                    m_itemDict.Add(index, v);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("m_itemDict add Error: index = " + index);
                }
               
            }
        }

        int hNum = 11;
        float colorStepH = 360 / hNum;
        float colorStepV = colorStepH / 4;

        foreach (KeyValuePair<int, Image> kv in m_itemDict)
        {
            int i = kv.Key;
            int x = (i % hNum + 7)%hNum;
            int y = i / hNum;
            kv.Value.color = new HSVColor(colorStepH * x + colorStepV * y, 1, 1);

        }

        m_itemDict[0].color = Color.white;
        m_itemDict[m_itemDict.Count - 1].color = Color.black;

        CalcCurrSelectColor(m_currScreenPos);

        //test
        DoMovePalette();
    }

    private void OnValueChanged()
    {
        m_currSV = (int)m_slider.value;
        SetCurrColor();              
    }

    public override void Update()
    {
       
    }

    public override bool OnTouchStart(Gesture gesture)
    {        
        return CalcCurrSelectColor(gesture.position);
    }
    public override bool OnTouchDown(Gesture gesture)
    {        
        return CalcCurrSelectColor(gesture.position);
    }

    public override bool OnTouchUp(Gesture gesture)
    {        
        return CalcCurrSelectColor(gesture.position);
    }

    private bool CalcCurrSelectColor(Vector2 screenPosition)
    {
        //Debug.Log(screenPosition);
        foreach (var v in m_itemDict)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(v.Value.rectTransform, screenPosition, null))
            {               
                m_selectRootRect.localPosition = v.Value.rectTransform.localPosition;
                m_selectImage.color = v.Value.color;
                m_currColorKey = v.Key;
                SetCurrColor();
                return true;
            }
        }
        return false;
    }

    public void DoMovePalette()
    {
        if (isShow)
        {
            Tweener tweener = rootTrans.DOLocalMoveY(-600, 1f);
            tweener.onComplete = delegate ()
            {
                isShow = false;
            };
           
        }
        else
        {
            Tweener tweener = rootTrans.DOLocalMoveY(0, 1f);
            tweener.onComplete = delegate ()
            {
                isShow = true;
            };
        }
    }

    //private bool CalcCurrSelectColor(Vector2 screenPosition)
    //{
    //    Debug.Log(screenPosition);
    //    if (screenPosition == null)
    //    {
    //        return false;
    //    }
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

    //    eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

    //    List<RaycastResult> results = new List<RaycastResult>();

    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

    //    GameObject currItem = null;

    //    foreach (var v in results)
    //    {
    //        Debug.Log(v.gameObject.name);
    //        if (v.gameObject.name.Contains("Image"))
    //        {
    //            currItem = v.gameObject;
    //            break;
    //        }
    //    }
    //    //bug.Log("results.Count ==================" + results.Count);
    //    if (currItem != null)
    //    {
    //        CurrColor = (currItem.GetComponent<Image>()).color;
    //        //Bridge.SetCurrColor(CurrColor);
    //        m_selectImage.color = CurrColor;
    //        return true;
    //    }
    //    return false;
    //}

}
