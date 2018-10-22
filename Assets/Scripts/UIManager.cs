using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour {


    private List<GameObject> pC = new List<GameObject> ();
    private int _min = 1;
    private int _max = 3;
    public Controller c = new Controller (KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.UpArrow);


    //This is the functionality for adding/removing 1 to a counter
    //#####################################################################################################################################
    public void AddOne (int max = 99) {
        int a;
        Int32.TryParse (GetComponent<Text> ().text, out a);
        if (a < max)
            a += 1;
        GetComponent<Text> ().text = a.ToString ();
    }


    public void RemoveOne (int min = 1) {
        int r;
        Int32.TryParse (GetComponent<Text> ().text, out r);
        if (r > min)
            r -= 1;
        GetComponent<Text> ().text = r.ToString ();
    }


    //This is the functionality for adding/removing players
    //#####################################################################################################################################
    public void AddPlayerControl (GameObject playerControl) {
        if (pC.Count + 1 < _max) {
            GameObject c = Instantiate (playerControl);
            c.name = "Controls" + (pC.Count + 1).ToString ();
            c.transform.SetParent (playerControl.transform.parent);
            c.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
            pC.Add (c);
        }
    }


    public void RemovePlayerControl () {
        if (pC.Count + 1 > _min) {
            Destroy (pC [pC.Count - 1]);
            pC.Remove (pC [pC.Count - 1]);
        }
    }


    //This is the functionality for changing the input of the players in the UI
    //#####################################################################################################################################
    public void waitForKey (Text btnText) {
        StartCoroutine ("getKey", btnText);
    }


    IEnumerator getKey (Text btnText) {
        string k = btnText.text;
        // btnText.text = "";

        yield return new WaitUntil (() => Input.anyKeyDown == true);

        foreach (KeyCode vKey in System.Enum.GetValues (typeof (KeyCode))) {
            if (Input.GetKey (vKey)) {

                switch (btnText.name) {
                    case "left":
                        c.keyLeft = vKey;
                        break;
                    case "down":
                        c.keyDown = vKey;
                        break;
                    case "right":
                        c.keyRight = vKey;
                        break;
                    case "up":
                        c.keyRotate = vKey;
                        break;
                    default:
                        break;
                }

                k = vKey.ToString ();
                btnText.text = k;
                break;
            }
        }

        yield return null;
    }
}
