using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    Block [] blocks;
    [SerializeField]
    private int fieldWidth;
    [SerializeField]
    private int fieldHeight;

    Tile playerTile;

    public GameObject prefabBlock;

    public float timePerMove;

    private float timePassed;

    List<Tile> tiles;

    Controller player1;

    public int FieldWidth {
        get { return fieldWidth; }
        set { fieldWidth = value; }
    }

    public int FieldHeight {
        get { return fieldHeight; }
        set { fieldHeight = value; }
    }

    // Use this for initialization
    void Start () {
        tiles = new List<Tile> ();
        blocks = new Block [fieldWidth * fieldHeight];
        timePassed = 0f;
        player1 = new Controller (KeyCode.A, KeyCode.D, KeyCode.S, KeyCode.W);
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Input.GetKeyDown (KeyCode.G)) {
            AddTile ();
        }

        if (playerTile != null) {
            timePassed += Time.deltaTime;
            if (timePassed >= timePerMove) {
                if (!CheckDownPlayerTile (playerTile)) {
                    AddTile ();
                }
                timePassed = 0f;
            }
        }

        if (Input.GetKeyDown (player1.keyLeft)) {
            MoveTileLeft (playerTile);
        } else if (Input.GetKeyDown (player1.keyRight)) {
            MoveTileRight (playerTile);
        } else if (Input.GetKeyDown (player1.keyDown)) {
            CheckDownPlayerTile (playerTile);
        } else if (Input.GetKeyDown (player1.keyRotate)) {
            RotateTile (playerTile);
        }
    }

    private void AddTile () {
        Tile tempTile = new Tile (3, 3, 4);
        tiles.Add (tempTile);
        for (int i = 0; i < tempTile.TileWidth * tempTile.TileHeight; i++) {
            if (tempTile.Blocks [i] != null) {
                int tempTileSize = i;
                int tempPosition = 0;
                while (tempTileSize >= tempTile.TileWidth) {
                    tempTileSize -= tempTile.TileWidth;
                    tempPosition += fieldWidth;
                }
                tempPosition += tempTileSize;

                blocks [tempPosition] = tempTile.Blocks [i];

                int cubeIndexPos = tempPosition;
                int cubePosX = 0;
                int cubePosY = 0;

                while (cubeIndexPos >= fieldWidth) {
                    cubePosY++;
                    cubeIndexPos -= fieldWidth;
                }

                cubePosX = cubeIndexPos;


                GameObject cube = Instantiate (prefabBlock);
                cube.name = "Cube nr. " + i.ToString ();
                cube.transform.position = new Vector3 (cubePosX, -cubePosY, 0);

                tempTile.Blocks [i].GO = cube;
            }
        }

        playerTile = tempTile;
    }

    private void RotateTile (Tile t) {
        bool clearRotate = true;

        if (clearRotate) {
            Block [] tempBlocksArray = (Block []) blocks.Clone ();
            Block [] tempBlocksArraySmall = (Block []) t.Blocks.Clone ();

            for (int i = 0; i < tempBlocksArray.Length; i++) {
                if (blocks [i] != null) {
                    if (blocks [i].TileID == t.ID) {
                        tempBlocksArray [i] = null;
                    }
                }

            }

            for (int i = 0; i < tempBlocksArraySmall.Length; i++) {
                tempBlocksArraySmall [i] = null;
            }

            for (int i = 0; i < t.Blocks.Length; i++) {
                if (t.Blocks [i] != null) {

                    int index = System.Array.IndexOf (blocks, t.Blocks [i]);
                    Debug.Log ("Index Global:" + index.ToString ());
                    Debug.Log ("Index Local:" + i.ToString ());
                    int [] nrs = { index + 2, index + fieldWidth + 1, index + fieldWidth * 2, index - fieldWidth + 1, index, index + fieldWidth - 1, index - fieldWidth * 2, index - fieldWidth - 1, index - 2 };
                    int [] nrs2 = { i + 2, i + t.TileWidth + 1, i + t.TileWidth * 2, i - t.TileWidth + 1, i, i + t.TileWidth - 1, i - t.TileWidth * 2, i - t.TileWidth - 1, i - 2 };
                    Vector3 [] nrs3 = { new Vector3 (2, 0, 0), new Vector3 (1, -1, 0), new Vector3 (0, -2, 0), new Vector3 (1, 1, 0), new Vector3 (0, 0, 0), new Vector3 (-1, -1, 0), new Vector3 (0, 2, 0), new Vector3 (-1, 1, 0), new Vector3 (-2, 0, 0) };

                    Debug.Log ("Global :" + nrs [i].ToString ());
                    Debug.Log ("Local :" + nrs2 [i].ToString ());
                    tempBlocksArray [nrs [i]] = blocks [index];
                    tempBlocksArraySmall [nrs2 [i]] = t.Blocks [i];
                    t.Blocks [i].GO.transform.Translate (nrs3 [i]);
                }
            }

            for (int i = 0; i < blocks.Length; i++) {
                blocks [i] = tempBlocksArray [i];
            }

            for (int i = 0; i < t.Blocks.Length; i++) {
                t.Blocks [i] = tempBlocksArraySmall [i];
            }
        }
    }

    private void MoveTileLeft (Tile t) {
        bool clearLeft = true;

        foreach (Block b in t.Blocks) {
            if (b != null) {
                int index = System.Array.IndexOf (blocks, b);
                Debug.Log (index);
                if (index - 1 >= 0 && (index % fieldWidth) != 0) {
                    if (blocks [index - 1] != null) {
                        if (blocks [index - 1].TileID != t.ID) {
                            clearLeft = false;
                            break;
                        }
                    }
                } else {
                    Debug.Log ("First statement is false");
                    clearLeft = false;
                    break;
                }
            }
        }

        Debug.Log (clearLeft);

        if (clearLeft) {
            Block [] tempBlocksArray = (Block []) blocks.Clone ();

            foreach (Block b in t.Blocks) {
                if (b != null) {
                    int index = System.Array.IndexOf (blocks, b);
                    tempBlocksArray [index] = null;
                }
            }

            foreach (Block b in t.Blocks) {
                if (b != null) {
                    int index = System.Array.IndexOf (blocks, b);
                    b.GO.transform.Translate (-1, 0, 0);
                    tempBlocksArray [index - 1] = b;
                }
            }

            for (int i = 0; i < blocks.Length; i++) {
                blocks [i] = tempBlocksArray [i];
            }
        }
    }

    private void MoveTileRight (Tile t) {
        bool clearRight = true;

        foreach (Block b in t.Blocks) {
            if (b != null) {
                int index = System.Array.IndexOf (blocks, b);
                if (index + 1 < fieldWidth * fieldHeight && index % fieldWidth != fieldWidth - 1 % fieldWidth) {
                    if (blocks [index + 1] != null) {
                        if (blocks [index + 1].TileID != t.ID) {
                            clearRight = false;
                            break;
                        }
                    }
                } else {
                    clearRight = false;
                    break;
                }
            }
        }

        if (clearRight) {

            for (int i = fieldWidth * fieldHeight - 1; i >= 0; i--) {
                if (blocks [i] != null) {
                    if (blocks [i].TileID == t.ID) {
                        blocks [i + 1] = blocks [i];
                        blocks [i].GO.transform.Translate (1, 0, 0);
                        blocks [i] = null;
                    }
                }

            }
        }
    }

    private void CheckDown () {
        List<Tile> tempTiles = new List<Tile> ();

        foreach (Tile t in tiles) {
            bool clearDown = true;

            foreach (Block b in t.Blocks) {
                int index = System.Array.IndexOf (blocks, b);
                if (index + fieldWidth < fieldWidth * fieldHeight) {
                    if (blocks [index + fieldWidth] != null) {
                        if (blocks [index + fieldWidth].TileID != t.ID) {
                            clearDown = false;
                            break;
                        }
                    }
                } else {
                    clearDown = false;
                    break;
                }
            }

            if (clearDown) tempTiles.Add (t);
        }

        MoveDownInOrder (tempTiles);
    }

    private bool CheckDownPlayerTile (Tile t) {
        List<Tile> tileToMoveDown = new List<Tile> ();
        bool clearDown = true;

        foreach (Block b in t.Blocks) {
            if (b != null) {
                int index = System.Array.IndexOf (blocks, b);
                if (index + fieldWidth < fieldWidth * fieldHeight) {
                    if (blocks [index + fieldWidth] != null) {
                        if (blocks [index + fieldWidth].TileID != t.ID) {
                            clearDown = false;
                            break;
                        }
                    }
                } else {
                    clearDown = false;
                    break;
                }
            }
        }

        if (clearDown) {
            tileToMoveDown.Add (t);
            MoveDownInOrder (tileToMoveDown);
            return true;
        }
        return false;
    }

    private void MoveDownInOrder (List<Tile> tilesToMoveDown) {
        for (int i = fieldWidth * fieldHeight - 1; i >= 0; i--) {
            if (blocks [i] != null) {
                foreach (Tile t in tilesToMoveDown) {
                    if (blocks [i].TileID == t.ID) {
                        blocks [i + fieldWidth] = blocks [i];
                        blocks [i].GO.transform.Translate (0, -1, 0);
                        blocks [i] = null;
                    }

                }
            }

        }
    }
}
