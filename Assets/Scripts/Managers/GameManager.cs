using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Place bombs")]
    public UnityEvent onStartPlaceBombs;

    [Header("Playing")]
    public UnityEvent onStartPlaying;

    [Header("ShowingAward")]
    public float m_showingAwardTime = 5.0f;

    [Header("GameOver")]
    public UnityEvent onGameOver;

    public enum GameState
    {
        PLACE_BOMBS,
        PLAYING,
        SHOWING_AWARD,
        GAME_OVER,
    }
    GameState m_gameState;
    public GameState state
    {
        get { return m_gameState; }
    }
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
        onStartPlaceBombs.Invoke();
        m_mainCamera = Camera.main;
        //m_mainCamera.enabled = false;
        m_gameState = GameState.PLACE_BOMBS;
        m_acumTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) StartGame();
        switch (m_gameState)
        {
            case GameState.PLACE_BOMBS:
                PlaceBombosUpdate();
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

    private void PlaceBombosUpdate()
    {

    }

    public void StartGame()
    {
        m_gameState = GameState.PLAYING;
        onStartPlaying.Invoke();
    }
}
