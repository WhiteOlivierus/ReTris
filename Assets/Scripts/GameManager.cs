using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {


    public int points = 0;
    public int offsetField = 10;

    public Text playerCount, fieldHeight, fieldWidth;
    private int playerCountValue, fieldWidthValue, fieldHeightValue;

    private List<Controller> playerControllers = new List<Controller> ();

    private List<TileManager> tileManagers = new List<TileManager> ();
    private int currentTileManager = 0;

    private bool gameStarted = false;

    private Vector3 newCamPosition;

    private int playersGameOver = 0;


    void Awake () {
        DontDestroyOnLoad (gameObject);
    }


    void Update () {
        Scene currentScene = SceneManager.GetActiveScene ();
        string sceneName = currentScene.name;

        if (sceneName == "scene_0" && !gameStarted) {
            gameStarted = true;
            LoadPlayField ();
            Camera.main.transform.position = newCamPosition;
        }

        foreach (TileManager t in tileManagers) {
            Debug.Log (t.id);
            t.FixedUpdate ();
        }
    }


    void TempUpdate () {
        Debug.Log ("Update " + currentTileManager.ToString ());
        tileManagers [currentTileManager].FixedUpdate ();

        if (currentTileManager >= tileManagers.Count - 1) {
            currentTileManager = 0;
            Debug.Log ("Set tilemanager to 0");
        } else {
            currentTileManager++;
            Debug.Log ("Set tilemanager to " + currentTileManager.ToString ());
        }
    }


    public void StartGame () {
        GetInitData ();
        SceneManager.LoadScene (1);
    }


    public void LoadPlayField () {
        Debug.Log ("Start game");
        TileManager.currentid = 0;
        int lastPos = 0;

        for (int i = 0; i < playerControllers.Count; i++) {
            Debug.Log ("create field");
            TileManager t = new TileManager (playerControllers [i], lastPos, fieldHeightValue, fieldWidthValue);
            tileManagers.Add (t);
            lastPos += fieldHeightValue + offsetField;
        }

        newCamPosition = new Vector3 (((lastPos - ((offsetField * playerCountValue) / 2f)) / 2f) - .5f, ((fieldWidthValue / 2f) * -1f) + .5f, fieldWidthValue * -1f);
    }


    private void GetInitData () {
        playerCountValue = Int32.Parse (playerCount.text);
        fieldWidthValue = Int32.Parse (fieldWidth.text);
        fieldHeightValue = Int32.Parse (fieldHeight.text);

        GameObject [] go = GameObject.FindGameObjectsWithTag ("Input");

        foreach (GameObject tempC in go) {
            playerControllers.Add (tempC.GetComponent<UIManager> ().c);
        }
    }


    public void GameOver () {
        playersGameOver++;

        if (playersGameOver >= playerCountValue) {
            Camera.main.transform.GetChild (0).GetComponent<MeshRenderer> ().enabled = true;
            StartCoroutine ("toMain");
        }
    }


    IEnumerator toMain () {
        yield return new WaitForSeconds (2f);
        SceneManager.LoadScene (0);
    }
}
