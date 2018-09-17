using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {


    public int points = 0;
    public int playerCount = 1;
    public int fieldHeight = 10;
    public int fieldWidth = 10;

    private List<int> properties = new List<int> ();
    private List<KeyCode> playersInput = new List<KeyCode> ();
    private List<Controller> playerControllers = new List<Controller> ();


    void Awake () {
        DontDestroyOnLoad (gameObject);
    }


    public void StartGame () {
        GetInitData ();
        CreatePlayers ();
        print ("properties: " + properties);
        print ("properties: " + playersInput);
        SceneManager.LoadScene (1);
    }


    private void GetInitData () {
        GameObject [] go = GameObject.FindGameObjectsWithTag ("UI Data");

        foreach (GameObject g in go) {
            if (g.transform.parent.name == "Amount") {
                properties.Add (Int32.Parse (g.GetComponent<Text> ().text));
            } else {
                playersInput.Add (getPlayerKeyBindings (g));
            }
        }
    }


    private KeyCode getPlayerKeyBindings (GameObject g) {
        KeyCode pK;
        string pKS = g.GetComponent<Text> ().text;
        pK = (KeyCode) System.Enum.Parse (typeof (KeyCode), pKS);
        return pK;
    }


    private void CreatePlayers () {
        for (int i = 0; i < properties [0]; i++) {

            List<KeyCode> t = playersInput.GetRange (i, i + 3);

            playerControllers.Add (new Controller (t [0], t [1], t [2], t [3]));

        }
    }


    public void EndGame () {

    }
}
