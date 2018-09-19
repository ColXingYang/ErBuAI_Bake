using System;
using UnityEngine;

[Serializable]
public class Good
{ 
    public float pos;

    public Good()
    {
        pos = 0;
    }

    public Good(float pos)
    {
        this.pos = pos;
    }
}
