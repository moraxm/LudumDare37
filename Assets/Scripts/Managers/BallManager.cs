using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public bool inTrigger;
    private Rigidbody m_rigidbody;
    public float minVelocity;
    private bool m_waitForStop;
    public bool WaitForStop
    {
        set { m_waitForStop = value; }
    }

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_waitForStop = false;
    }

    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }

    void Update()
    {
        if (!m_waitForStop)
        {
            return;
        }

		if (inTrigger || m_rigidbody.velocity.magnitude <= minVelocity) {
			EndGame();
			m_waitForStop = false;
		}
    }

    public void EndGame()
    {
        GameManager.instance.Result(inTrigger);
    }

}
