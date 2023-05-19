using UnityEngine;

public class Score : MonoBehaviour
{
    // Singleton instance.
    public static Score Instance = null;
    public int lastScore;
    public string bestPlayer;
    public int bestScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        lastScore = 0;
    }
}
