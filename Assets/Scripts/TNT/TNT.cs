﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TNT : MonoBehaviour
{
    [Header("Explosion Levels")]
    [SerializeField]
    ExplosionData m_explosion;
    float m_acumTime;
    [SerializeField]
    int m_explosionTime;
    private Explosions m_explosions;
    private MeshRenderer m_meshRenderer;
    MouseActionsController m_mouseActions;
    private Text m_textComponent;

    public GameObject explosionObject;

    static int s_totalTNTs = 0;
    public static int totalTNTs
    { get { return s_totalTNTs; } }

    // Use this for initialization
    void Awake()
    {
        ++s_totalTNTs;
        m_explosions = FindObjectOfType<Explosions>();
        if (!m_explosions) Debug.LogError("No Explisions object in the scene!");
        enabled = false;
        m_acumTime = 0;
        m_explosion = m_explosions.lowLevelExplosion;
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshRenderer.material.SetColor("_Color", m_explosion.color);

        m_mouseActions = GetComponent<MouseActionsController>();
        m_mouseActions.onFinishAction += OnFinishMouseAction;

        m_textComponent = GetComponentInChildren<Text>();
        m_textComponent.text = m_explosionTime.ToString() + "s";

        GameManager.instance.onStartPlaying.AddListener(OnStartPlaying);
    }

    void OnStartPlaying()
    {
        enabled = true;
    }



    void OnFinishMouseAction(MouseActionsController.MouseAction action)
    {
        Debug.Log(action);
        switch (action)
        {
            case MouseActionsController.MouseAction.HOLD:
                Destroy(gameObject);
                break;
            case MouseActionsController.MouseAction.CLICK:
                increaseExplosionLevel();
                break;
            case MouseActionsController.MouseAction.DRAG_UP:
                increaseTime();
                break;
            case MouseActionsController.MouseAction.DRAG_DOWN:
                decreaseTime();
                break;
            default:
                break;
        }
    }

    private void decreaseTime()
    {
        if (m_explosionTime > 0)
        {
            --m_explosionTime;
            m_textComponent.text = m_explosionTime.ToString() + "s";
        }
        
    }

    private void increaseTime()
    {
        ++m_explosionTime;
        m_textComponent.text = m_explosionTime.ToString() + "s";
    }

    // Update is called once per frame
    void Update()
    {
        m_acumTime += Time.deltaTime;
        m_textComponent.text = (m_explosionTime - m_acumTime).ToString() + "s";
        if (m_acumTime >= m_explosionTime)
        {
            Explosion();
            enabled = false;
        }
    }

    public void OnDestroy()
    {
        --s_totalTNTs;
        m_mouseActions.onFinishAction -= OnFinishMouseAction;
        GameManager.instance.onStartPlaying.RemoveListener(OnStartPlaying);
    }

    private void Explosion()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, m_explosion.radius);
        foreach(Collider c in others)
        {
            Rigidbody rb = c.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(m_explosion.physicForce, transform.position, m_explosion.radius);
            }
        }
        explosionObject.transform.parent = null;
        explosionObject.SetActive(true);
        Destroy(this.gameObject);
        Destroy(explosionObject.gameObject, 5);
    }

    public void increaseExplosionLevel()
    {
        switch (m_explosion.level)
        {
            case ExplosionData.ExplosionsLevel.LOW:
                m_explosion = m_explosions.midLevelExplosion;
                break;
            case ExplosionData.ExplosionsLevel.MID:
                m_explosion = m_explosions.highLevelExplosion;
                break;
            case ExplosionData.ExplosionsLevel.HIGH:
                m_explosion = m_explosions.lowLevelExplosion;
                break;
            default:
                break;
        }
        m_meshRenderer.material.SetColor("_Color", m_explosion.color);
    }

    public void OnEnable()
    {
        m_acumTime = 0;
    }




}