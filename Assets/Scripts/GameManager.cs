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
    private List<GameObject> playerControllers = new List<GameObject> ();

    private List<TileManager> tileManagers = new List<TileManager>();


    void Awake () {
        DontDestroyOnLoad (gameObject);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.G)) {
            TestStartGame();
        }
    }

    void FixedUpdate() {
        foreach(TileManager t in tileManagers) {
            t.FixedUpdate();
        }
    }


    public void StartGame () {
        GetInitData ();
        SceneManager.LoadScene (1);
        //CreatePlayers ();
        for (int i = 0; i < playerControllers.Count; i++)
        {
            //TileManager t = new TileManager(playerControllers[i].GetComponent<Controller>(), i * 25);
        }
    }

    public void TestStartGame() {
        Debug.Log("Start game");

        Controller c = new Controller(KeyCode.A, KeyCode.D, KeyCode.S, KeyCode.W);
        Controller c2 = new Controller(KeyCode.J, KeyCode.L, KeyCode.K, KeyCode.I);
        Controller c3 = new Controller(KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.UpArrow);

        TileManager t = new TileManager(c, 0, 10, 20);
        tileManagers.Add(t);
        TileManager t2 = new TileManager(c2, 15, 10, 20);
        tileManagers.Add(t2);
        TileManager t3 = new TileManager(c3, 30, 10, 20);
        tileManagers.Add(t3);
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


    /*private void CreatePlayers () {
        for (int i = 0; i < properties [0] * 4; i += 4) {
            List<KeyCode> t = playersInput.GetRange (i, i + 4);
            GameObject c = new GameObject ("Controller");
            c.AddComponent<Controller> ();
            c.GetComponent<Controller> ().SetKeys (t.ToArray ());
            playerControllers.Add (c);
        }
    } */


    public void EndGame () {

    }
}
