using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMenu : MonoBehaviour
{
    public GameObject menu;

    private void Awake() {
        menu.SetActive(false);
    }

    public void ShowMenu() {
        menu.SetActive(true);
    }
}
