using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {


    public int points = 0;
    public int playerCount = 1;
    public int fieldHeight = 10;
    public int fieldWidth = 10;

    private List<string> properties;
    private List<string> playersInput;


    void Awake () {
        DontDestroyOnLoad (gameObject);
    }


    public void StartGame () {
        GetInitData ();
        print ("properties: " + properties.Count);
        print ("properties: " + playersInput.Count);
        SceneManager.LoadScene (1);
    }


    private void GetInitData () {
        GameObject [] go = GameObject.FindGameObjectsWithTag ("UI Data");

        foreach (GameObject g in go) {
            if (g.transform.parent.name == "Amount") {
                properties.Add (g.GetComponent<Text> ().text);
            } else {
                playersInput.Add (g.GetComponent<Text> ().text);
            }
        }
    }


    public void EndGame () {

    }
}
