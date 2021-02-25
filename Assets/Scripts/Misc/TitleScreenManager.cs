using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundBeep;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) {
            audioSource.PlayOneShot(soundBeep);
        }
    }

    public void StartOnClick() {
        SceneManager.LoadScene(StringBank.LevelSceneName);
    }

    public void OptionsOnClick() {
        Debug.Log("Show options menu");
    }

    public void QuitOnClick() {
        Application.Quit();
    }


}
