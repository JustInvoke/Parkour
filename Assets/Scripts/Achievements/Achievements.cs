using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    public GameObject achNote;
    public static int i = 0;
    public int[] scoreArray = {50, 100, 250, 500, 1000};
    public int[] jumpArray = {1, 25, 50, 100, 250};
    public int[] crouchArray = {1, 25, 50, 100, 250};
    public int[] powerArray = {5, 10, 20, 50, 100};
    public int numOfElem = 5;
    public Text achTitleText; // Text displaying achievement title
    public Text achDescText; // Text displaying achievement description

    void Update()
    {
        for (i = 0; i < numOfElem; i++)
        {
            if (ScoreMenu.score == scoreArray[i])
                StartCoroutine(TriggerAch(scoreArray[i], "score"));
            if (CharacterControl.crouchCount == crouchArray[i])
                StartCoroutine(TriggerAch(crouchArray[i], "crouch"));
            if (CharacterControl.jumpCount == jumpArray[i])
                StartCoroutine(TriggerAch(jumpArray[i], "jump"));
            if (Item.itemCount == powerArray[i])
                StartCoroutine(TriggerAch(powerArray[i], "powerup"));
        }

        IEnumerator TriggerAch(int code, string achType)
        {
            achNote.SetActive(true);
            if (achType == "score")
            {
                achTitleText.text = "Run " + (i + 1);
                achDescText.text = "Obtain a score of " + code;
            }
            else if (achType == "crouch")
            {
                achTitleText.text = "Slider " + (i + 1);
                achDescText.text = "Crouch " + code + " times";
            }
            else if (achType == "jump")
            {
                achTitleText.text = "Jumper " + (i + 1);
                achDescText.text = "Jump " + code + " times";
            }
            else
            {
                achTitleText.text = "Collector " + (i + 1);
                achDescText.text = "Obtain " + code + " powerups";
            }
            yield return new WaitForSecondsRealtime(4);

            //Resetting UI
            achNote.SetActive(false);
        }
    }
}
