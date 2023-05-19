using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text bestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private int blockAmount;

    private CameraShake cameraShaker;

    public AudioClip gameoverSound;


    // Start is called before the first frame update
    void Start()
    {
        Score.Instance.lastScore = 0; // Directly accessing a public variable in a singleton - totally legit way of doing it.
        cameraShaker = Camera.main.GetComponent<CameraShake>();
        SetupBrickField();
        bestScoreText.text = "Best score: " + Score.Instance.bestScore + " Name: " + Score.Instance.bestPlayer;
    }

    private void SetupBrickField()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        blockAmount = perLine * LineCount;

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

        blockAmount--;
        cameraShaker.ShakeCamera(0.1f, 0.05f);

        if (blockAmount <= 0)
        {
            // Set up a new set of bricks when you run out
            SetupBrickField();
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        AudioManager.Instance.Play(gameoverSound);
        Score.Instance.lastScore = m_Points; // Store the score to the singleton, to be added to the highscore list
        SceneManager.LoadScene(0); // No more gameover screen, straight back to main menu
        //        GameOverText.SetActive(true);
    }
}
