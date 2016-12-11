using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlane : MonoBehaviour {
    public GameObject prefab;
	[HideInInspector]
    public int maxTNTs = 2;
    public AnimationCurve curve;
	// Use this for initialization
	void Start () 
    {
        int size = LevelManager.instance.CurrentLevel / 3;
        //maxTNTs = size + 2 * (int)(Mathf.Pow(2, size));
        maxTNTs = (int)curve.Evaluate(size);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseDown()
    {
        if (GameManager.instance.state != GameManager.GameState.PLACE_BOMBS) return;
        Ray pos = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(pos.origin,pos.direction,Color.red,100);
        RaycastHit info;
        if (Physics.Raycast(pos,out info,1000))
        {
            if (TNT.totalTNTs < maxTNTs)
            {
				GameObject tnt = Instantiate<GameObject>(prefab,info.point,prefab.transform.rotation) as GameObject;
				Vector3 position = tnt.transform.localPosition;
				position.y = position.y + tnt.GetComponent<Renderer>().bounds.size.y / 2.0f;
				tnt.transform.localPosition = position;
            }

        }
    }


}
