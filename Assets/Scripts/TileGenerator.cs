using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnTileCreated : UnityEvent<Tile> { }

public class TileGenerator {

    private static TileGenerator tileGenerator;

    public static TileGenerator GetTileGenerator {
        get
        {
            if(tileGenerator == null)
            {
                tileGenerator = new TileGenerator();
                tileGenerator.onTileCreated = new OnTileCreated();
            }
            return tileGenerator;
        }
    }

    public OnTileCreated onTileCreated;

    public static void CreateTile()
    {
        Tile tempTile = new Tile(3, 3, 4);
        GetTileGenerator.onTileCreated.Invoke(tempTile);
    }
}
