using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class UIManager : MonoBehaviour {
    public GameObject startButton;
    public TMP_Dropdown dropdown;
    public GameController gameController;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //Enable buttons for main menu only
        // if (currentScreen == UIScreen.MainMenu) {
        //     NewGameButton.SetActive(true);
        //     OptionsButton.SetActive(true);
        //     ReturnToMenuBtn.SetActive(false);
        // } else if (currentScreen == UIScreen.Options) {
        //     NewGameButton.SetActive(false);
        //     OptionsButton.SetActive(false);
        //     ReturnToMenuBtn.SetActive(true);
        // }

        // probably something like 
        // if(game is running) {
        //     startButton.SetActive(false);
        //     dropDown.SetActive(false);
        // } else {
        //     startButton.SetActive(true);
        //     dropDown.SetActive(true);
        // }
        // or if i'm being really fancy 
        // startButton.SetActive(game is running)

        // converts dropdown to enum

    }
    public void StartButtonClicked() {
        if (gameController) {
            if (dropdown) {
                SearchType s = (SearchType)dropdown.value;
                // pass s to the beginSearch function, which will pass it to the pathfinder class to determine which alg to use
                if (s != SearchType.None) {
                    gameController.BeginSearch(s);
                } else {
                    // dont do anything, maybe put a popup on screen
                    Debug.Log("UIManager Error: Please select a search algorithm");
                }
            }
        }
    }

}
public enum SearchType {
    None = 0,
    BFS = 1,
    DFS = 2,
    Dijkstra = 3,
    GreedyBest = 4,
    A = 5

}
