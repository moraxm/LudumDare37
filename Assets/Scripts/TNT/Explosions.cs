using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosions : MonoBehaviour 
{
    [Header("Explosion Levels")]
    [SerializeField]
    ExplosionData m_explosionLowLevel;
    public ExplosionData lowLevelExplosion { get { return m_explosionLowLevel; } }
    [SerializeField]
    ExplosionData m_explosionMidLevel;
    public ExplosionData midLevelExplosion { get { return m_explosionMidLevel; } }
    [SerializeField]
    ExplosionData m_explosionHighLevel;
    public ExplosionData highLevelExplosion { get { return m_explosionHighLevel; } }

    public void Awake()
    {
        m_explosionLowLevel.level = ExplosionData.ExplosionsLevel.LOW;
        m_explosionMidLevel.level = ExplosionData.ExplosionsLevel.MID;
        m_explosionHighLevel.level = ExplosionData.ExplosionsLevel.HIGH;
    }

    

    
}
