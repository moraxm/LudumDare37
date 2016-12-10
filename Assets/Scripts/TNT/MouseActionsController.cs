using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseActionsController : MonoBehaviour 
{
    public enum MouseAction
    {
        HOLD,
        CLICK,
        DRAG_UP,
        DRAG_DOWN,
    }
    MouseAction m_action;
    public delegate void ActionDelegate(MouseAction actionType);
    public ActionDelegate onFinishAction;
    private float m_acumTime;
    private Vector3 m_prevMousePosition;
    public float m_minTimeToDrag = 0.5f;
    public float m_distanceToDetectDrag = 1;
    public int m_timeToDetectHold = 1;

    public void OnMouseDown()
    {
        m_action = MouseAction.CLICK;
        m_acumTime = 0;
        m_prevMousePosition = Input.mousePosition;
    }

    public void OnMouseUp()
    {
        if (m_action == MouseAction.CLICK)
            onFinishAction.Invoke(m_action);
    }

    public void OnMouseDrag()
    {
        m_acumTime += Time.deltaTime;
        if (CheckDrag())
        {
            onFinishAction(m_action);
        }
        else
        {
            if (m_acumTime > m_timeToDetectHold && m_action != MouseAction.DRAG_UP && m_action != MouseAction.DRAG_DOWN)
            {
                m_action = MouseAction.HOLD;
                onFinishAction(m_action);
            }
        }
    }

    private bool CheckDrag()
    {
        bool toReturn = false;
        if (m_acumTime < m_minTimeToDrag) return toReturn;


        if (Input.mousePosition.y - m_prevMousePosition.y > m_distanceToDetectDrag)
        {
            m_action = MouseAction.DRAG_UP;
            m_prevMousePosition = Input.mousePosition;
            toReturn = true;
        }
        else if (Input.mousePosition.y - m_prevMousePosition.y < -m_distanceToDetectDrag)
        {
            m_action = MouseAction.DRAG_DOWN;
            m_prevMousePosition = Input.mousePosition;
            toReturn = true;
        }
        return toReturn;
    }



}
