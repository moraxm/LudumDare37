using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour {

    Text m_textComponent;
	// Use this for initialization
	void Awake () 
    {
        m_textComponent = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (GameManager.instance.state == GameManager.GameState.PLAYING)
            m_textComponent.text = "Time: " + GameManager.instance.acumTime.ToString("0.00");
        else
            m_textComponent.text = "";
	}
}
