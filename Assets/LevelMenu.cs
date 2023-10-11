using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] Levels;

    public void Awake() 
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for(int i = 0; i < Levels.Length; i++) {
            Levels[i].interactable = false;
        }
        for(int i = 0; i < unlockedLevel; i++) {
            Levels[i].interactable = true;
        }
    }

    public void OpenLevel(int LevelID)
    {
        string LevelName = "Level" + LevelID;
        SceneManager.LoadScene(LevelName);
    }

}
