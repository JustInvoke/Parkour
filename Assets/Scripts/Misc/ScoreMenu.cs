using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ScoreMenu : MonoBehaviour
{
    public GameObject menu; // The game over menu objecto
    public GameObject hud; // Object containing HUD elements
    public static int score = 0; // Current score value
    public Text scoreHudText; // Text displaying the score in the HUD
    public Text scoreMenuText; // Text displaying the score in the game over menu
    public Text highScoreMenuText; // Text displaying the high score in the game over menu

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundDeath;
    [SerializeField] private AudioClip soundBeep;
    [SerializeField] private AudioClip soundFanfare;

    private void Awake() {
        menu.SetActive(false); // Disable menu at start of scene
    }

    private void Update() {
        scoreHudText.text = score.ToString(); // Update score text
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && menu.activeSelf) {
            audioSource.PlayOneShot(soundBeep);
        }
    }

    public void ShowMenu() {
        audioSource.PlayOneShot(soundDeath);
        // Show the menu and hide the HUD
        hud.gameObject.SetActive(false);
        menu.SetActive(true);

        // Update high score if necessary
        int highScore = PlayerPrefs.GetInt("Score", 0);
        if (score > highScore) {
            audioSource.PlayOneShot(soundFanfare);
            highScore = score;
            PlayerPrefs.SetInt("Score", highScore);
            PlayerPrefs.Save();
        }

        // Display score text
        scoreMenuText.text = "Score: " + score.ToString();
        highScoreMenuText.text = "High Score: " + highScore.ToString();
    }
}
