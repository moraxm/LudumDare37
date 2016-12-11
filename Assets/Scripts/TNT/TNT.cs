using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TNT : MonoBehaviour
{
    [Header("Explosion Levels")]
    [SerializeField]
    ExplosionData m_explosion;
    float m_acumTime;
    [SerializeField]
    float m_explosionTime;
    private Explosions m_explosions;
    private MeshRenderer m_meshRenderer;
    MouseActionsController m_mouseActions;
    private Text m_textComponent;

    public MultipleParticlesSystem explosionObject;

    static int s_totalTNTs = 0;
    public static int totalTNTs
    { get { return s_totalTNTs; } }

    // Use this for initialization
    void Awake()
    {
        m_explosions = FindObjectOfType<Explosions>();
        if (!m_explosions) Debug.LogError("No Explisions object in the scene!");
        ++s_totalTNTs;
        enabled = false;
        m_acumTime = 0;
        m_explosion = m_explosions.lowLevelExplosion;
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshRenderer.material.SetColor("_Color", m_explosion.color);

        m_mouseActions = GetComponent<MouseActionsController>();
        m_mouseActions.onFinishAction += OnFinishMouseAction;

        m_textComponent = GetComponentInChildren<Text>();
        m_textComponent.text = m_explosionTime.ToString("0.0") + "s";
        m_textComponent.color = m_explosion.color;

        GameManager.instance.onStartPlaying.AddListener(OnStartPlaying);
        GameManager.instance.onStartPlaceBombs.AddListener(OnStartPlaceBombs);
    }

    public static void ResetTNTStatic()
    {
        s_totalTNTs = 0;
    }

    void OnStartPlaying()
    {
        enabled = true;
    }

    void DisableComponents(bool disable)
    {
        m_textComponent.text = disable ? "" : m_explosionTime.ToString("0.0") + "s"; ;
        m_meshRenderer.enabled = !disable;
        if (disable)
            --s_totalTNTs;
        else if(!enabled)
            ++s_totalTNTs;

        Debug.Log("Total Tnts: " + totalTNTs);
    }

    void OnStartPlaceBombs()
    {
        m_acumTime = 0;
        DisableComponents(false);
		enabled = false;
        explosionObject.Stop();
    }


    void OnFinishMouseAction(MouseActionsController.MouseAction action)
    {

        switch (action)
        {
            case MouseActionsController.MouseAction.HOLD:
                Destroy(gameObject);
                break;
            case MouseActionsController.MouseAction.CLICK:
                increaseExplosionLevel();
                break;
            case MouseActionsController.MouseAction.DRAG_UP:
                increaseTime();
                break;
            case MouseActionsController.MouseAction.DRAG_DOWN:
                decreaseTime();
                break;
            default:
                break;
        }
    }

    private void decreaseTime()
    {
        if (m_explosionTime > 0)
        {
            m_explosionTime -= 0.5f;
            m_textComponent.text = m_explosionTime.ToString("0.0") + "s";
        }
        
    }

    private void increaseTime()
    {
        m_explosionTime += 0.5f;
        m_textComponent.text = m_explosionTime.ToString("0.0") + "s";
    }

    // Update is called once per frame
    void Update()
    {
        m_acumTime += Time.deltaTime;
        m_textComponent.text = (m_explosionTime - m_acumTime).ToString("0.0") + "s";
        if (m_acumTime >= m_explosionTime)
        {
            Explosion();
            enabled = false;
            DisableComponents(true);
        }
    }

    public void OnDisable()
    {
        
    }

    public void OnDestroy()
    {
        DisableComponents(true);
        m_mouseActions.onFinishAction -= OnFinishMouseAction;
        GameManager.instance.onStartPlaying.RemoveListener(OnStartPlaying);
        GameManager.instance.onStartPlaceBombs.RemoveListener(OnStartPlaceBombs);
    }

    private void Explosion()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, m_explosion.radius);
        foreach(Collider c in others)
        {
            Rigidbody rb = c.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(m_explosion.physicForce, transform.position, m_explosion.radius);
            }
        }

		explosionObject.gameObject.SetActive(true);
		explosionObject.Play();

		// TNT Sound
		string clip = "boom";
		clip += (Random.Range(1, 4)).ToString();
		UtilSound.instance.PlaySound(clip);
    }

    public void increaseExplosionLevel()
    {
        switch (m_explosion.level)
        {
            case ExplosionData.ExplosionsLevel.LOW:
                m_explosion = m_explosions.midLevelExplosion;
                break;
            case ExplosionData.ExplosionsLevel.MID:
                m_explosion = m_explosions.highLevelExplosion;
                break;
            case ExplosionData.ExplosionsLevel.HIGH:
                m_explosion = m_explosions.lowLevelExplosion;
                break;
            default:
                break;
        }
        m_meshRenderer.material.SetColor("_Color", m_explosion.color);
        m_textComponent.color = m_explosion.color;
    }

    public void OnEnable()
    {
        m_acumTime = 0;
    }




}
