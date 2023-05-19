using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class HiScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hichScoreTextBlock;
    [SerializeField] TextMeshProUGUI LastScoreText;
    private string json;
    private HighScores highScores;
    private const int maxHighscoreEntries = 10;
    private string playerName;

    void Start()
    {
        string filePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "highscores.json";
        highScores = new HighScores();

        LoadScores(filePath);
        ReadSingleton();
        SortList();
        RemoveExcess();
        PrintScores();
        SaveScores(filePath);
    }

    private void SaveScores(string filePath)
    {
        // Save 10 entries to the file
        json = JsonUtility.ToJson(highScores);
        File.WriteAllText(filePath, json);
    }

    private void RemoveExcess()
    {
        // Only keep the first 10 entries on the list
        if (highScores.highScoreList.Count > maxHighscoreEntries)
        {
            for (int i = maxHighscoreEntries; i < highScores.highScoreList.Count; i++)
            {
                highScores.highScoreList.RemoveAt(i);
            }
        }
    }

    private void SortList()
    {
        highScores.highScoreList.Sort((x, y) => x.score.CompareTo(y.score)); // Sorts in ascending order
        highScores.highScoreList.Reverse(); // Flips the list to descending order
        Score.Instance.bestScore = highScores.highScoreList[0].score;
        Score.Instance.bestPlayer = highScores.highScoreList[0].playerName;
    }

    private void ReadSingleton()
    {
        // Read name from a file
        playerName = GameObject.Find("Canvas").GetComponent<InputName>().LoadName();
        if (playerName == null)
        {
            playerName = "Player";
        }

        // Get score from singleton, if not 0 then add to the highscore list. To be sorted in a different method.
        int scoreLastRound = Score.Instance.lastScore;
        if (scoreLastRound != 0)
        {
            highScores.highScoreList.Add(new HighScoreEntry { score = scoreLastRound, playerName = playerName });
            LastScoreText.gameObject.SetActive(true);
            LastScoreText.text = "Score last round: " + scoreLastRound;
        }
    }

    private void LoadScores(string filePath)
    {
        // Load scores from file
        if (File.Exists(filePath))
        {
            json = File.ReadAllText(filePath);
            highScores = JsonUtility.FromJson<HighScores>(json);
        } else
        {
            highScores = JsonUtility.FromJson<HighScores>("{}");
        }

        json = JsonUtility.ToJson(highScores);
    }

    private void PrintScores()
    {
        string hstb = "Highscores:";
        foreach (var x in highScores.highScoreList)
        {
            hstb += "<br>" + x.score.ToString("0000") + "\t" + x.playerName.ToString();
        }
        hichScoreTextBlock.text = hstb;
    }

    [Serializable]
    class HighScoreEntry
    {
        public string playerName;
        public int score;
    }

    [Serializable]
    class HighScores
    {
        public List<HighScoreEntry> highScoreList;
    }
}