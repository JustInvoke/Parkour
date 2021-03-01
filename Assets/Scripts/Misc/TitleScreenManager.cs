using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public void StartOnClick() {
        SceneManager.LoadScene(StringBank.LevelSceneName);
    }

    public void OptionsOnClick() {
        Debug.Log("Show options menu");
    }

    public void AchievementsOnClick() {
        SceneManager.LoadScene(StringBank.AchievementScene);
    }
    public void QuitOnClick() {
        Application.Quit();
    }
}
