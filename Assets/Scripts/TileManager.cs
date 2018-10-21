using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager {

    public static int currentid;

    private int id;

    Block [] blocks;
    [SerializeField]
    private int fieldWidth;
    [SerializeField]
    private int fieldHeight;

    private int startingPoint;
    private int startingPointField;

    Tile playerTile;

    public GameObject prefabBlock;
    public GameObject prefabBlockBorder;

    public float timePerMove = 0.25f;

    private float timePassed;

    private float timeBeforeDown = 0.05f;

    private TileGenerator tg;

    Queue<Tile> tilesToAdd;
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
    public TileManager(Controller c, int s, int fw, int fh) {
        tiles = new List<Tile> ();
        FieldWidth = fw;
        FieldHeight = fh;
        blocks = new Block [fieldWidth * fieldHeight];
        timePassed = 0f;
        timePerMove = 0.25f;
        player1 = c;

        id = currentid;
        currentid++;

        PrefabManager pm = GameObject.Find("PrefabManager").GetComponent<PrefabManager>();
        prefabBlock = pm.prefabBlock;
        prefabBlockBorder = pm.prefabBorderBlock;

        startingPoint = fieldWidth / 2;
        startingPointField = s;

        tilesToAdd = new Queue<Tile>();
        
        tg = TileGenerator.GetTileGenerator;
        tg.onTileCreated.AddListener(AddTileToQueue);

        tg.CreateTile();

        AddBorder();
        AddTile(tg.GetTile(id));
    }

    

    // Update is called once per frame
    public void FixedUpdate () {
        if (playerTile != null)
        {
            if (timeBeforeDown < 0.06f)
            {
                timeBeforeDown += Time.deltaTime;
            }

            if (Input.GetKeyDown(player1.keyLeft))
            {
                MoveTileLeft(playerTile);
            }
            else if (Input.GetKeyDown(player1.keyRight))
            {
                MoveTileRight(playerTile);
            }
            else if (Input.GetKey(player1.keyDown) && timeBeforeDown > 0.05f)
            {
                timeBeforeDown -= 0.05f;
                if (!CheckDownPlayerTile(playerTile))
                {
                    while (CheckForRows())
                    {
                        Debug.Log("Row has been cleared");
                    }

                    AddTile(tg.GetTile(id));
                }
                timePassed = 0f;
            }
            else if (Input.GetKeyDown(player1.keyRotate))
            {
                RotateTile(playerTile);
            }



            timePassed += Time.deltaTime;
            if (timePassed >= timePerMove)
            {
                if (!CheckDownPlayerTile(playerTile))
                {
                    while (CheckForRows())
                    {
                        Debug.Log("Row has been cleared");
                    }

                    AddTile(tg.GetTile(id));
                }
                timePassed = 0f;
            }
        }
    }

    private void AddBorder() {
        for (int i = 0; i < fieldWidth; i++)
        {
            Block b = new Block(-1);
            b.SetBlock(-1, prefabBlockBorder, new Vector3(i + startingPointField, 1, 0), Quaternion.identity);
            Block c = new Block(-1);
            c.SetBlock(-1, prefabBlockBorder, new Vector3(i + startingPointField, -fieldHeight, 0), Quaternion.identity);
        }

        for (int i = 0; i < fieldHeight; i++)
        {
            Block b = new Block(-1);
            b.SetBlock(-1, prefabBlockBorder, new Vector3(-1 + startingPointField, -i, 0), Quaternion.identity);
            Block c = new Block(-1);
            c.SetBlock(-1, prefabBlockBorder, new Vector3(fieldWidth + startingPointField, -i, 0), Quaternion.identity);
        }
    }

    private bool CheckForRows()
    {
        bool rowCleared = false;

        for (int i = 0; i < fieldHeight; i++)
        {
            bool fullRow = true;

            for (int j = 0; j < fieldWidth; j++)
            {
                if(blocks[fieldWidth * i + j] == null)
                {
                    fullRow = false;
                    break;
                }
            }

            if(fullRow)
            {
                ClearRow(i);
                rowCleared = true;
            }
        }

        return rowCleared;
    }

    private void ClearRow(int rowIndex)
    {
        Debug.Log(id + ": Clear row " + rowIndex);
        for (int i = 0; i < fieldWidth; i++)
        {
            foreach (Tile t in tiles)
            {
                if(blocks[rowIndex * fieldWidth + i].TileID == t.ID)
                {
                    int index = System.Array.IndexOf(t.Blocks, blocks[rowIndex * fieldWidth + i]);
                    t.Blocks[index] = null;
                }
            }

            blocks[rowIndex * fieldWidth + i].SelfDestroy();
            blocks[rowIndex * fieldWidth + i] = null;
        }

        for (int i = rowIndex * fieldWidth - 1; i >= 0; i--)
        {
            if(blocks[i] != null) {
                blocks[i].GO.transform.Translate(0, -1, 0);
                blocks[i + fieldWidth] = blocks[i];
                blocks[i] = null;
            }
        }
    }

    private bool CheckGameOver(int index) {
        if(blocks[index] != null) {
            Debug.Log(index.ToString() + " is not null");
            return true;
        }
        return false;
    }

    private void GameOver() {
        ResetGame();
    }

    private void ResetGame() {
        Debug.Log("Reset tilemanager " + id.ToString());
        for (int i = 0; i < blocks.Length; i++)
        {
            if(blocks[i] != null) {
                blocks[i].SelfDestroy();
                blocks[i] = null;
            }
        }

        tiles.Clear();
        tiles.TrimExcess();
        timePassed = 0f;
        playerTile = null;
    }

    private void AddTileToQueue(Tile t)
    {
        
    }

    private void AddTile (Tile c) {

        Tile t = c.CloneTile();

        bool gameOver = false;

        for (int i = 0; i < t.TileWidth * t.TileHeight; i++) {
            if (t.Blocks [i] != null) {
                int tempTileSize = i;
                int tempPosition = 0;
                while (tempTileSize >= t.TileWidth) {
                    tempTileSize -= t.TileWidth;
                    tempPosition += fieldWidth;
                }
                tempPosition += tempTileSize += startingPoint - t.TileWidth / 2;

                if(CheckGameOver(tempPosition)) {
                    GameOver();
                    gameOver = true;
                    break;
                }

                int cubeIndexPos = tempPosition;
                int cubePosX = 0;
                int cubePosY = 0;

                while (cubeIndexPos >= fieldWidth) {
                    cubePosY++;
                    cubeIndexPos -= fieldWidth;
                }

                cubePosX = cubeIndexPos;

                t.Blocks[i].SetBlock(t.ID, prefabBlock, new Vector3(cubePosX + startingPointField, -cubePosY, 0), Quaternion.identity);
                blocks [tempPosition] = t.Blocks [i];
            }
        }

        if(!gameOver) {
            tiles.Add (t);

            Debug.Log(id + ": New player tile");

            playerTile = t;
        }
    }

    private void RotateTile (Tile t) {
        bool clearRotate = true;

        for (int i = 0; i < t.Blocks.Length; i++)
        {
            if (t.Blocks[i] != null)
            {

                int index = System.Array.IndexOf(blocks, t.Blocks[i]);
                int[] nrs = { index + 2, index + fieldWidth + 1, index + fieldWidth * 2, index - fieldWidth + 1, index, index + fieldWidth - 1, index - fieldWidth * 2, index - fieldWidth - 1, index - 2 };
                if(nrs[i] < 0 || nrs[i] >= fieldWidth * fieldHeight)
                {
                    clearRotate = false;
                }
                else if(blocks[nrs[i]] != null)
                {
                    if (blocks[nrs[i]].TileID != t.ID)
                    {
                        clearRotate = false;
                    }
                }
                else if((nrs[i] > index && nrs[i] % fieldWidth == 0) || (nrs[i] < index && nrs[i] % fieldWidth == fieldWidth - 1 % fieldWidth))
                {
                    clearRotate = false;
                }
            }
        }

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

    private bool CheckDownPlayerTile (Tile t) {
        List<Tile> tileToMoveDown = new List<Tile> ();
        bool clearDown = true;

        foreach (Block b in t.Blocks) {
            if (b != null) {
                int index = System.Array.IndexOf (blocks, b);
                if (index + fieldWidth < fieldWidth * fieldHeight) {
                    if (blocks [index + fieldWidth] != null) {
                        if (blocks [index + fieldWidth].TileID != t.ID) {
                            Debug.Log(id + ": Tile id: " + t.ID);
                            Debug.Log(id + ": Tile is trying to move onto a different tile");
                            Debug.Log(id + ": Tile id trying to move onto is " + blocks[index + fieldWidth].TileID);
                            clearDown = false;
                            break;
                        }
                    }
                } else {
                    Debug.Log(id + ": Tile is trying to move out of range");
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

        Debug.Log(id + ": Can't move playertile");
        return false;
    }

    private void MoveDownInOrder (List<Tile> tilesToMoveDown) {
        Debug.Log(id + ": Move down player tile");
        for (int i = fieldWidth * fieldHeight - 1; i >= 0; i--) {
            if (blocks [i] != null) {
                foreach (Tile t in tilesToMoveDown) {
                    if (blocks [i].TileID == t.ID) {
                        blocks [i + fieldWidth] = blocks [i];
                        blocks [i + fieldWidth].GO.transform.Translate (0, -1, 0);
                        blocks [i] = null;
                        break;
                    }

                }
            }

        }
    }
}
