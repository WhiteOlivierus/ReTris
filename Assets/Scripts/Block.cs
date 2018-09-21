using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {

    private int tileID;

    private GameObject go;

    public int TileID
    {
        get { return tileID; }
        set { tileID = value; }
    }

    public Block(int tileID)
    {
        TileID = tileID;
    }

    public GameObject GO
    {
        get { return go; }
        set { go = value; }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
