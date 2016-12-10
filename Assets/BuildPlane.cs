using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlane : MonoBehaviour {
    public GameObject prefab;
    public int maxTNTs = 3;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseDown()
    {
        Ray pos = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(pos.origin,pos.direction,Color.red,100);
        RaycastHit info;
        if (Physics.Raycast(pos,out info,1000))
        {
            if (TNT.totalTNTs < maxTNTs)
            {
                GameObject obj = Instantiate<GameObject>(prefab,info.point,Quaternion.identity);
            }

        }
    }


}
