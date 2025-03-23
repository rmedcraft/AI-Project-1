using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using System.Threading.Tasks;

public class UIManager : MonoBehaviour {
    public GameObject startButton;
    public TMP_Dropdown dropdown;
    public GameController gameController;
    private bool isSearching;
    // Use this for initialization
    void Start() {
        isSearching = false;
    }

    // Update is called once per frame
    void Update() {
        if (isSearching) {
            startButton.SetActive(false);
            dropdown.enabled = false;
        } else {
            startButton.SetActive(true);
            dropdown.enabled = true;
        }

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

    public void SetSearching(bool isSearching) {
        this.isSearching = isSearching;
    }

}
public enum SearchType {
    None = 0,
    BFS = 1,
    DFS = 2,
    Dijkstra = 3,
    GreedyBest = 4,
    AManhattan = 5,
    AEuclidean = 6,

}
