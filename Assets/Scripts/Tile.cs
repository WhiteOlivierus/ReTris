using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

    private static int currentid;

    private Block[] blocks;
    private int tileWidth;
    private int tileHeight;
    private int amtBlocks;
    private int id;


    public int TileWidth
    {
        get { return tileWidth; }
        set { tileWidth = value; }
    }


    public int TileHeight
    {
        get { return tileHeight; }
        set { tileHeight = value; }
    }

    
    public int AmtBlocks
    {
        get { return amtBlocks; }
        set { amtBlocks = value; }
    }


    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public Block[] Blocks
    {
        get { return blocks; }
        set { blocks = value; }
    }


    public Tile(int tileWidth, int tileHeight, int amtBlocks)
    {
        id = currentid;
        currentid++;

        TileWidth = tileWidth;
        TileHeight = tileHeight;
        AmtBlocks = amtBlocks;
        
        blocks = new Block[tileWidth * tileHeight];

        List<int> options = new List<int>();
        List<int> allOptions = new List<int>();

        for (int i = 0; i < tileWidth * tileHeight; i++)
        {
            allOptions.Add(i);
        }

        int rnd0 = Random.Range(0, tileWidth * tileHeight - 1);
        if (blocks[rnd0] == null) { blocks[rnd0] = new Block(id); }
        allOptions.Remove(rnd0);

        options = AddOptions(options, allOptions, rnd0);

        for (int i = 0; i < amtBlocks - 1; i++)
        {
            int rnd = options[Random.Range(0, options.Count)];
            if (blocks[rnd] == null)
            {
                blocks[rnd] = new Block(id);
                options.Remove(rnd);
                AddOptions(options, allOptions, rnd);
            }
        }
    }

    public Tile CloneTile() {
        Tile t = new Tile(TileWidth, TileHeight, AmtBlocks);
        for (int i = 0; i < Blocks.Length; i++)
        {
            t.Blocks[i] = null;
            if(Blocks[i] != null)
            {
                t.Blocks[i] = new Block(t.ID);
            }
        }
        return t;
    }


    // Use this for initialization
    void Start () {
        
	}
	

	// Update is called once per frame
	void Update () {
		
	}

    private List<int> AddOptions(List<int> options, List<int> availableOptions, int lastAddedNr) {
        List<int> tempList = new List<int>();

        if(lastAddedNr % tileWidth == 0)
            tempList.Add(lastAddedNr + 1);
        else if(lastAddedNr % tileWidth == tileWidth - 1)
            tempList.Add(lastAddedNr - 1);
        else
        {
            tempList.Add(lastAddedNr + 1);
            tempList.Add(lastAddedNr - 1);
        }

        if(lastAddedNr - tileWidth >= 0)
            tempList.Add(lastAddedNr - tileWidth);
        if(lastAddedNr + tileWidth < tileWidth * tileHeight)
            tempList.Add(lastAddedNr + tileWidth);

        foreach(int i in tempList)
        {
            if(availableOptions.Contains(i))
            {
                options.Add(i);
                availableOptions.Remove(i);
            }
        }

        return options;
    }
}
