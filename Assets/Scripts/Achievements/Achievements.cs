using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    public GameObject achNote;
    public static int i = 0;
    public int[] scoreArray = { 50, 100, 250, 500, 1000 };
    public int[] jumpArray = { 1, 25, 50, 100, 250 };
    public int[] crouchArray = { 1, 25, 50, 100, 250 };
    public int[] powerArray = { 5, 10, 20, 50, 100 };
    public int numOfElem = 5;
    public Text achTitleText; // Text displaying achievement title
    public Text achDescText; // Text displaying achievement description
    private int maxScoreAchLevel = -1; // Maximum achievement level reached for score
    private int lastCrouchCount = 0;
    private int lastJumpCount = 0;
    private int lastItemCount = 0;

    private void Awake() {
        LoadAchievements();
    }

    private void LoadAchievements() {
        maxScoreAchLevel = PlayerPrefs.GetInt("Score Level", -1);
        CharacterControl.crouchCount = PlayerPrefs.GetInt("Crouch Count", 0);
        lastCrouchCount = CharacterControl.crouchCount;
        CharacterControl.jumpCount = PlayerPrefs.GetInt("Jump Count", 0);
        lastJumpCount = CharacterControl.jumpCount;
        Item.itemCount = PlayerPrefs.GetInt("Item Count", 0);
        lastItemCount = Item.itemCount;
    }

    public void SaveAchievements() {
        PlayerPrefs.SetInt("Score Level", maxScoreAchLevel);
        PlayerPrefs.SetInt("Crouch Count", CharacterControl.crouchCount);
        PlayerPrefs.SetInt("Jump Count", CharacterControl.jumpCount);
        PlayerPrefs.SetInt("Item Count", Item.itemCount);
        PlayerPrefs.Save();
    }

    void Update() {
        for (i = 0; i < numOfElem; i++) {
            if (ScoreMenu.score == scoreArray[i]) {
                if (i > maxScoreAchLevel) {
                    maxScoreAchLevel = i;
                    StartCoroutine(TriggerAch(scoreArray[i], "score"));
                }
            }
            if (CharacterControl.crouchCount == crouchArray[i] && lastCrouchCount != CharacterControl.crouchCount) {
                lastCrouchCount = CharacterControl.crouchCount;
                StartCoroutine(TriggerAch(crouchArray[i], "crouch"));
            }
            if (CharacterControl.jumpCount == jumpArray[i] && lastJumpCount != CharacterControl.jumpCount) {
                lastJumpCount = CharacterControl.jumpCount;
                StartCoroutine(TriggerAch(jumpArray[i], "jump"));
            }
            if (Item.itemCount == powerArray[i] && lastItemCount != Item.itemCount) {
                lastItemCount = Item.itemCount;
                StartCoroutine(TriggerAch(powerArray[i], "powerup"));
            }
        }

        IEnumerator TriggerAch(int code, string achType) {
            achNote.SetActive(true);
            if (achType == "score") {
                achTitleText.text = "Run " + (i + 1);
                achDescText.text = "Obtain a score of " + code;
            }
            else if (achType == "crouch") {
                achTitleText.text = "Slider " + (i + 1);
                achDescText.text = "Crouch " + code + " times";
            }
            else if (achType == "jump") {
                achTitleText.text = "Jumper " + (i + 1);
                achDescText.text = "Jump " + code + " times";
            }
            else {
                achTitleText.text = "Collector " + (i + 1);
                achDescText.text = "Obtain " + code + " powerups";
            }
            yield return new WaitForSecondsRealtime(4);

            //Resetting UI
            achNote.SetActive(false);
        }
    }
}
