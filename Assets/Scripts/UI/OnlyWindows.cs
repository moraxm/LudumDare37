using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyWindows : MonoBehaviour 
{
    public void Awake()
    {
#if !UNITY_STANDALONE
        gameObject.SetActive(false);
#endif
    }
}
