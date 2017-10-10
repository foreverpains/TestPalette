using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorPaint
{
    public struct HSVColor
    {
        public float h;
        public float s;
        public float v;

        public static HSVColor cyan = new HSVColor(180.0f, 1.0f, 1.0f);
        public static HSVColor magenta = new HSVColor(300.0f, 1.0f, 1.0f);
        public static HSVColor yellow = new HSVColor(60.0f, 1.0f, 1.0f);
        public static HSVColor red = new HSVColor(0.0f, 1.0f, 1.0f);
        public static HSVColor green = new HSVColor(120.0f, 1.0f, 1.0f);
        public static HSVColor blue = new HSVColor(240.0f, 1.0f, 1.0f);
        public static HSVColor white = new HSVColor(0.0f, 0.0f, 1.0f);
        public static HSVColor black = new HSVColor(0.0f, 0.0f, 0.0f);

        public HSVColor(float _h, float _s, float _v)
        {
            h = _h;
            s = _s;
            v = _v;
        }
        
        public HSVColor(Vector3 _v)
        {
            h = _v.x;
            s = _v.y;
            v = _v.z;
        }

        public static implicit operator HSVColor(Color c)
        {
            float _h, _s, _v;
            Color.RGBToHSV(c, out _h, out _s, out _v);
            return new HSVColor(_h * 360, _s, _v);
        }
        public static implicit operator Color(HSVColor c)
        {
            return Color.HSVToRGB(c.h / 360.0f, c.s, c.v);
        }

        public static bool operator ==(HSVColor lhs, HSVColor rhs)
        {
            return lhs.h == rhs.h && lhs.s == rhs.s && lhs.v == rhs.v;
        }
        public static bool operator !=(HSVColor lhs, HSVColor rhs)
        {
            return lhs.h != rhs.h || lhs.s != rhs.s || lhs.v != rhs.v;
        }
    }
}
