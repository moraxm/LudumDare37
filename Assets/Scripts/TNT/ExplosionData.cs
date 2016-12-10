using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ExplosionData 
{
    public float radius;
    public float physicForce;
    public Color color;
    public enum ExplosionsLevel
    {
        LOW,
        MID,
        HIGH,
    }
    private ExplosionsLevel m_level;
    public ExplosionsLevel level
    {
        get { return m_level; }
        set { m_level = value; }
    }
}
