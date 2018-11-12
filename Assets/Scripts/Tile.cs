using System.Collections.Generic;
using UnityEngine;

//Made by Daniel Bergshoeff
public enum SpecialEffect {
    DROPFASTER,
    RANDOMTILE,
    BLOCKROTATION,
    SWITCHMOVEMENT,
    NONE
}


public class Tile {


    private static int currentid;

    private Block [] blocks;
    private int tileWidth, tileHeight, amtBlocks, id;

    public SpecialEffect specialEffect;


    public int TileWidth {
        get { return tileWidth; }
        set { tileWidth = value; }
    }


    public int TileHeight {
        get { return tileHeight; }
        set { tileHeight = value; }
    }


    public int AmtBlocks {
        get { return amtBlocks; }
        set { amtBlocks = value; }
    }


    public int ID {
        get { return id; }
        set { id = value; }
    }


    public Block [] Blocks {
        get { return blocks; }
        set { blocks = value; }
    }


    public Tile (int tileWidth, int tileHeight, int amtBlocks) {
        id = currentid;
        currentid++;

        TileWidth = tileWidth;
        TileHeight = tileHeight;
        AmtBlocks = amtBlocks;

        blocks = new Block [tileWidth * tileHeight];

        List<int> options = new List<int> ();
        List<int> allOptions = new List<int> ();

        for (int i = 0; i < tileWidth * tileHeight; i++) {
            allOptions.Add (i);
        }

        int rnd0 = Random.Range (0, tileWidth * tileHeight - 1);
        if (blocks [rnd0] == null) { blocks [rnd0] = new Block (id); }
        allOptions.Remove (rnd0);

        options = AddOptions (options, allOptions, rnd0);

        for (int i = 0; i < amtBlocks - 1; i++) {
            int rnd = options [Random.Range (0, options.Count)];
            if (blocks [rnd] == null) {
                blocks [rnd] = new Block (id);
                options.Remove (rnd);
                AddOptions (options, allOptions, rnd);
            }
        }

        int rnd1 = Random.Range (0, 40);
        if (rnd1 == 0) specialEffect = SpecialEffect.DROPFASTER;
        else if (rnd1 == 1) specialEffect = SpecialEffect.RANDOMTILE;
        else if (rnd1 == 2) specialEffect = SpecialEffect.BLOCKROTATION;
        else if (rnd1 == 3) specialEffect = SpecialEffect.SWITCHMOVEMENT;
        else specialEffect = SpecialEffect.NONE;
    }


    public Tile CloneTile () {
        Tile t = new Tile (TileWidth, TileHeight, AmtBlocks);

        for (int i = 0; i < Blocks.Length; i++) {
            t.Blocks [i] = null;

            if (Blocks [i] != null) {
                t.Blocks [i] = new Block (t.ID);
            }
        }

        return t;
    }


    private List<int> AddOptions (List<int> options, List<int> availableOptions, int lastAddedNr) {
        List<int> tempList = new List<int> ();

        if (lastAddedNr % tileWidth == 0) {
            tempList.Add (lastAddedNr + 1);
        } else if (lastAddedNr % tileWidth == tileWidth - 1) {
            tempList.Add (lastAddedNr - 1);
        } else {
            tempList.Add (lastAddedNr + 1);
            tempList.Add (lastAddedNr - 1);
        }

        if (lastAddedNr - tileWidth >= 0) tempList.Add (lastAddedNr - tileWidth);
        if (lastAddedNr + tileWidth < tileWidth * tileHeight) tempList.Add (lastAddedNr + tileWidth);

        foreach (int i in tempList) {

            if (availableOptions.Contains (i)) {
                options.Add (i);
                availableOptions.Remove (i);
            }
        }

        return options;
    }
}
