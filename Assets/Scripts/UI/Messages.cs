using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Messages : MonoBehaviour
{
    [System.Serializable]
    public class Message
    {
        public string message;
        public float time;
    }
    public Message[] messages;
    float m_acumTime;
    private int m_messageIdx;
    Text m_textComponent;

    // Use this for initialization
    void Start()
    {
        m_acumTime = 0;
        m_messageIdx = 0;
        m_textComponent = GetComponent<Text>();

        if (messages.Length == 0) enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_acumTime += Time.deltaTime;
        if (m_acumTime >= messages[m_messageIdx].time)
        {
            m_acumTime = 0;
            ++m_messageIdx;
            if (m_messageIdx >= messages.Length)
                m_messageIdx = 0;
            m_textComponent.text = messages[m_messageIdx].message;
        }
    }
}
