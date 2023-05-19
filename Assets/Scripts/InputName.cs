using UnityEngine;
using TMPro;
using System.IO;


public class InputName : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] string playerName;

    // Start is called before the first frame update
    void Start()
    {
        // Load player name
        LoadName();
        nameText.text = playerName;

        // TODO: Load highscore list
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableInputField()
    {
        nameInputField.gameObject.SetActive(true);
        nameInputField.Select();
    }
    public void DisableInputField()
    {
        nameInputField.gameObject.SetActive(false);
    }

    public void ChangeName(string newName)
    {
        playerName = newName;
        nameText.text = playerName;
        DisableInputField();

        // Don't bother saving the name in a singleton - save to file and read when scene loads
        SaveName();
    }

    [System.Serializable]
    struct SavedPlayerName
    {
        public string playerName;
    }

    public void SaveName()
    {
        SavedPlayerName player = new();
        player.playerName = playerName;

        string json = JsonUtility.ToJson(player);

        File.WriteAllText(Application.persistentDataPath + Path.AltDirectorySeparatorChar + "playername.json", json);
    }

    public void LoadName()
    {
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "playername.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavedPlayerName data = JsonUtility.FromJson<SavedPlayerName>(json);

            playerName = data.playerName;
        }
    }


}
