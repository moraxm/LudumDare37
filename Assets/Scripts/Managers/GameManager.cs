using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    private BallManager m_ballManager;
    public Camera m_camera;

    [Header("Place bombs")]
    public UnityEvent onStartPlaceBombs;

    [Header("Playing")]
    public UnityEvent onStartPlaying;

    [Header("Win game")]
    public float m_showingWinGame = 5.0f;
    public UnityEvent OnWinGame;

    [Header("Lose game")]
    public UnityEvent onLoseGame;

    public enum GameState
    {
        PLACE_BOMBS,
        PLAYING,
        WAITING_BALL,
        WIN_GAME,
        LOSE_GAME,
    }
    GameState m_gameState;
    public GameState state
    {
        get { return m_gameState; }
    }
    float m_acumTime = 0;
    public float acumTime
    {
        get { return m_acumTime; }
    }

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
        }
    }


    // Use this for initialization
    void Start()
    {
        onStartPlaceBombs.Invoke();
        m_gameState = GameState.PLACE_BOMBS;
        m_acumTime = 0;
        GameObject go = Instantiate(ballPrefab);
        go.transform.position = new Vector3(0, 0, 0);
        m_ballManager = go.GetComponent<BallManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) StartGame();
        switch (m_gameState)
        {
            case GameState.PLACE_BOMBS:
                PlaceBombsUpdate();
                break;
            case GameState.PLAYING:
                PlayingUpdate();
                break;
            case GameState.WAITING_BALL:
                WaitingBallUpdate();
                break;
            case GameState.LOSE_GAME:
                LoseGameUpdate();
                break;
            case GameState.WIN_GAME:
                WinGameUpdate();
                break;
            default:
                break;
        }
    }

    #region states
    private void PlayingUpdate()
    {
        m_acumTime += Time.deltaTime;
        if (TNT.totalTNTs <= 0)
        {
            m_gameState = GameState.WAITING_BALL;
            m_ballManager.WaitForStop = true;
        }
    }
    private void PlaceBombsUpdate()
    {
    }
    private void WaitingBallUpdate()
    {
    }
    private void LoseGameUpdate() { }
    private void WinGameUpdate() { }
    #endregion
    public void StartGame()
    {
        m_gameState = GameState.PLAYING;
        m_acumTime = 0;
        onStartPlaying.Invoke();
    }
    public void Retry()
    {
        if (m_gameState != GameState.PLACE_BOMBS)
        {
            m_gameState = GameState.PLACE_BOMBS;
            onStartPlaceBombs.Invoke();
            m_ballManager.transform.position = Vector3.zero;
            m_ballManager.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    public void LoseGame()
    {
        onLoseGame.Invoke();
        m_gameState = GameState.LOSE_GAME;
        m_acumTime = 0;
    }
    public void WinGame()
    {
        OnWinGame.Invoke();
        m_gameState = GameState.WIN_GAME;
        m_acumTime = 0;
    }
    public void Result(bool success)
    {
        if(m_gameState != GameState.WAITING_BALL)
        {
            return;
        }
        if (!success)
        {
            LoseGame();
        }
        else
        {
            WinGame();
        }
    }
    public void ColocateCamera(Vector3 size, Vector3 position)
    {
        m_camera.transform.position = new Vector3(position.x, m_camera.transform.position.y, position.z);
        m_camera.orthographicSize = 1;
        Ray rayBottom = Camera.main.ScreenPointToRay(Vector3.zero);
        Ray rayTop = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0));

        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        RaycastHit infoBottom;
        if (Physics.Raycast(rayBottom, out infoBottom, 1000))
        {
            bottomLeft = infoBottom.point;
        }
        RaycastHit infoTop;
        if (Physics.Raycast(rayTop, out infoTop, 1000))
        {
            topRight = infoTop.point;
        }

        //widht
        float diffKnownX = topRight.x - bottomLeft.x;
        float diffNewX = size.x + 1;
        float newAspectRatioX = diffNewX * m_camera.orthographicSize / diffKnownX;

        //height
        float diffKnownY = topRight.z - bottomLeft.z;
        float diffNewY = size.z + 1;
        float newAspectRatioY = diffNewY * m_camera.orthographicSize / diffKnownY;

        m_camera.orthographicSize = Mathf.Max(newAspectRatioX, newAspectRatioY);
    }
}
