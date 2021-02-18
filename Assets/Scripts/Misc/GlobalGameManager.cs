using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    //Singleton Reference
    public static GlobalGameManager Instance { get { return _instance; } }
    private static GlobalGameManager _instance;
    private void Awake() {
        //Set this object not to destroy on loading new scenes
        DontDestroyOnLoad(gameObject);

        //Setup of Monobehaviour Singleton
        if (_instance != this && _instance != null) {
            Destroy(_instance.gameObject);
        }
        _instance = this;
    }

    // Pause the game
    public static void Pause() {
        Time.timeScale = 0.0f;
    }

    // Unpause the game
    public static void Unpause() {
        Time.timeScale = 1.0f;
    }
}
