using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour
{
    public GameObject menu; // The game over menu objecto
    public GameObject hud; // Object containing HUD elements
    public static int score = 0; // Current score value
    public Text scoreHudText; // Text displaying the score in the HUD
    public Text scoreMenuText; // Text displaying the score in the game over menu
    public Text highScoreMenuText; // Text displaying the high score in the game over menu

    private void Awake() {
        menu.SetActive(false); // Disable menu at start of scene
    }

    private void Update() {
        scoreHudText.text = score.ToString(); // Update score text
    }

    public void ShowMenu() {
        // Show the menu and hide the HUD
        hud.gameObject.SetActive(false);
        menu.SetActive(true);

        // Update high score if necessary
        int highScore = PlayerPrefs.GetInt("Score", 0);
        if (score > highScore) {
            highScore = score;
            PlayerPrefs.SetInt("Score", highScore);
            PlayerPrefs.Save();
        }

        // Display score text
        scoreMenuText.text = "Score: " + score.ToString();
        highScoreMenuText.text = "High Score: " + highScore.ToString();
    }
}
