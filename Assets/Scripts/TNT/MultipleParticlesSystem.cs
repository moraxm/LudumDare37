using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleParticlesSystem : MonoBehaviour {

    ParticleSystem[] particleSystems;
	// Use this for initialization
	void Awake () 
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
	}

    public void Play()
    {
        foreach (ParticleSystem p in particleSystems)
        {
            p.Play();
        }
    }

    public void Stop()
    {
        foreach (ParticleSystem p in particleSystems)
        {
            p.Stop();
        }
    }
}
