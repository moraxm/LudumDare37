using UnityEngine;
using System.Collections;


public class MusicManager : MonoBehaviour {

    static MusicManager m_instance;   
    AudioSource m_audioSource;

    public void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else
            Destroy(this);
    }

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        m_audioSource = GetComponent<AudioSource>();
        if (!m_audioSource)
            m_audioSource = gameObject.AddComponent<AudioSource>();

        PlayMusic();

	}

    void PlayMusic()
    {
        m_audioSource.loop = true;
        m_audioSource.Play();
    }
}
