using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class HiScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hichScoreTextBlock;
    [SerializeField] TextMeshProUGUI topHiScoreText;
    [SerializeField] TextMeshProUGUI topHiScoreNameText;
    private string json;
    private HighScores highScores;
    private const int maxHighscoreEntries = 10;

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
                Debug.Log("Removing entry#" + i + ":" + highScores.highScoreList[i].playerName + highScores.highScoreList[i].score);
                highScores.highScoreList.RemoveAt(i);
            }
        }
    }

    private void SortList()
    {
        highScores.highScoreList.Sort((x, y) => x.score.CompareTo(y.score)); // Sorts in ascending order
        highScores.highScoreList.Reverse(); // Flips the list to descending order
    }

    private void ReadSingleton()
    {
        // TODO: Get last score from the singleton, add to the list
        // Temp: add single entry
        highScores.highScoreList.Add(new HighScoreEntry { score = UnityEngine.Random.Range(0, 200), playerName = "Mel" });
    }

    private void LoadScores(string filePath)
    {
        // Load scores from file
        if (File.Exists(filePath))
        {
            json = File.ReadAllText(filePath);
            highScores = JsonUtility.FromJson<HighScores>(json);
        }

        json = JsonUtility.ToJson(highScores);
    }

    private void PrintScores()
    {
        foreach (var x in highScores.highScoreList)
        {
            Debug.Log("Entry " + x + ":" + x.playerName.ToString() + " - " + x.score.ToString());
        }
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