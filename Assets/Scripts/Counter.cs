using System;
using UnityEngine;
using UnityEngine.UI;


public class Counter : MonoBehaviour {


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


}
