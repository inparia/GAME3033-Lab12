using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadGameWidget : MenuWidget
{

    private GameDataList GameData;

    [Header("Scene to Load")]
    [SerializeField] private string SceneToLoad;

    [Header("Prefabs")]
    [SerializeField] private GameObject SaveSlotPrefab;
    [Header("References")]
    [SerializeField] private RectTransform LoadItemPanel;
    [SerializeField] private TMP_InputField NewGameInputField;


    [SerializeField] private bool Debug;

    private const string SaveFileKey = "FileSaveData";


    void Start()
    {
        if (Debug)
        {
            SaveDebugData();
        }
        WipeChildren();
        LoadGameData();
    }

    private void WipeChildren()
    {
        
        foreach (RectTransform saveSlot in LoadItemPanel)
        {
            Destroy(saveSlot.gameObject);
        }

        LoadItemPanel.DetachChildren();
    }

    public static void SaveDebugData()
    {
        GameDataList dataList = new GameDataList();
        dataList.SaveFileNames.AddRange(new List<string> {"Save1", "Save2", "Save3" });
        PlayerPrefs.SetString(SaveFileKey, JsonUtility.ToJson(dataList));
    }

    private void LoadGameData()
    {
        if (!PlayerPrefs.HasKey(SaveFileKey)) return;

        string jsonString = PlayerPrefs.GetString(SaveFileKey);
        GameData = JsonUtility.FromJson<GameDataList>(jsonString);

        if (GameData.SaveFileNames.Count <= 0) return;

        UnityEngine.Debug.Log(GameData.SaveFileNames);

        foreach (string saveName in GameData.SaveFileNames)
        {
            SaveSlotWidget widget = Instantiate(SaveSlotPrefab, LoadItemPanel).GetComponent<SaveSlotWidget>();
            widget.Initialize(this, saveName);

        }

    }


    public void LoadScene()
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    public void CreateNewGame()
    {
        if (string.IsNullOrEmpty(NewGameInputField.text)) return;
        GameManager.Instance.SetActiveSave(NewGameInputField.text);
        SceneManager.LoadScene(SceneToLoad);
    }


}

[Serializable]
class GameDataList
{
    public List<string> SaveFileNames = new List<string>();
}
