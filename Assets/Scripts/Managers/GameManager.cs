using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Intro")]
    public float m_showingStructureTime = 3.0f;
    public UnityEvent onStartShowingStructure;

    [Header("Playing")]
    public UnityEvent onStartPlaying;

    [Header("ShowingAward")]
    public float m_showingAwardTime = 5.0f;

    [Header("GameOver")]
    public UnityEvent onGameOver;

    public enum GameState
    {
        INTRO,
        PLAYING,
        SHOWING_AWARD,
        GAME_OVER,
    }
    GameState m_gameState;
    float m_acumTime = 0;
    Camera m_mainCamera;

    static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            return m_instance;
        }
    }

    public void Awake()
    {
        if (m_instance != null)
        {
            Destroy(this);
        }
        else
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    // Use this for initialization
    void Start()
    {
        onStartShowingStructure.Invoke();
        m_mainCamera = Camera.main;
        m_mainCamera.enabled = false;
        m_gameState = GameState.INTRO;
        m_acumTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_gameState)
        {
            case GameState.INTRO:
                ShowingStructureUpdate();
                break;
            case GameState.PLAYING:
                PlayingUpdate();
                break;
            case GameState.SHOWING_AWARD:
                ShowingAwardUpdate();
                break;
            case GameState.GAME_OVER:
                break;
            default:
                break;
        }
    }

    private void ShowingAwardUpdate()
    {
        m_acumTime += Time.deltaTime;
        if (m_acumTime > m_showingAwardTime)
        {
            m_gameState = GameState.GAME_OVER;
        }
    }

    public void GameOver()
    {
        if (m_gameState == GameState.PLAYING)
        {
            onGameOver.Invoke();
            m_gameState = GameState.SHOWING_AWARD;
            m_mainCamera.enabled = false;
            m_acumTime = 0;
        }


    }

    private void PlayingUpdate()
    {

    }

    private void ShowingStructureUpdate()
    {
        m_acumTime += Time.deltaTime;
        if (m_acumTime > m_showingStructureTime)
        {
            m_gameState = GameState.PLAYING;
            onStartPlaying.Invoke();
        }
    }

    public void StartGame()
    {

    }
}
