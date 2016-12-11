using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombsUI : MonoBehaviour {
    BuildPlane m_buildPlane;
    private Text m_textComponent;

	// Use this for initialization
	void Start () 
    {
        m_buildPlane = FindObjectOfType<BuildPlane>();
        if (!m_buildPlane) enabled = false;

        m_textComponent = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_textComponent.text = TNT.totalTNTs.ToString() + " / " + m_buildPlane.maxTNTs + " Bombs used";
	}
}
