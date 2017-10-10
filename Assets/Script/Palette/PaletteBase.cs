using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PaletteBase
{

    public abstract void Init();

    public abstract void Update();

    public virtual bool OnTouchStart(Gesture gesture)
    {
        return false;
    }

    public virtual bool OnTouchDown(Gesture gesture)
    {
        return false;
    }

    public virtual bool OnTouchUp(Gesture gesture)
    {
        return false;
    }

    public virtual bool OnDragStart(Gesture gesture)
    {
        return false;
    }
}
