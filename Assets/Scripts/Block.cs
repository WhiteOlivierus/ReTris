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

    public void SetBlock(int tileID, GameObject prefab, Vector3 position, Quaternion quaternion)
    {
        TileID = tileID;
        go = GameObject.Instantiate(prefab, position, quaternion);
        go.name = TileID.ToString();
    }

    public GameObject GO
    {
        get { return go; }
        set { go = value; }
    }

    public void SelfDestroy()
    {
        Debug.Log("Destroy self");
        GameObject.Destroy(go);
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
