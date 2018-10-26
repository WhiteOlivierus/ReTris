using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class OnTileCreated : UnityEvent<Tile> { }


public class TileGenerator {


    public OnTileCreated onTileCreated;

    private Dictionary<int, int> tileManagers;

    private List<Tile> tiles;

    private static TileGenerator tileGenerator;


    public static TileGenerator GetTileGenerator {
        get {
            if (tileGenerator == null) {
                tileGenerator = new TileGenerator ();
                tileGenerator.onTileCreated = new OnTileCreated ();
                tileGenerator.tileManagers = new Dictionary<int, int> ();
                tileGenerator.tiles = new List<Tile> ();
            }
            return tileGenerator;
        }
    }


    public Tile GetTile (int tileManagerId) {
        if (tileManagers.ContainsKey (tileManagerId)) {
            tileManagers [tileManagerId]++;
        } else {
            tileManagers.Add (tileManagerId, 1);
        }

        if (tileManagers [tileManagerId] - 1 >= tiles.Count) CreateTile ();

        return tiles [tileManagers [tileManagerId] - 1];
    }


    public void CreateTile () {
        Tile tempTile = new Tile (3, 3, 4);
        tiles.Add (tempTile);
    }
}
