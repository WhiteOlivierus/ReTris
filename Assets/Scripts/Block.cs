using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block {


    private int tileID;

    private GameObject go;


    public int TileID {
        get { return tileID; }
        set { tileID = value; }
    }


    public GameObject GO {
        get { return go; }
        set { go = value; }
    }


    public Block (int tileID) {
        TileID = tileID;
    }


    public void SetBlock (int tileID, GameObject prefab, Vector3 position, Quaternion quaternion, SpecialEffect se) {
        TileID = tileID;
        go = GameObject.Instantiate (prefab, position, quaternion);
        go.name = TileID.ToString ();

        switch (se) {
            case SpecialEffect.DROPFASTER:
                go.GetComponent<Renderer> ().material = PrefabManager.cubeDropFaster;
                break;
            case SpecialEffect.BLOCKROTATION:
                go.GetComponent<Renderer> ().material = PrefabManager.cubeBlockRotation;
                break;
            case SpecialEffect.SWITCHMOVEMENT:
                go.GetComponent<Renderer> ().material = PrefabManager.cubeSwitchMovement;
                break;
        }
    }


    public void SelfDestroy () {
        Debug.Log ("Destroy self");
        GameObject.Destroy (go);
    }
}
