using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    Block[] blocks;
    [SerializeField]
    private int fieldWidth;
    [SerializeField]
    private int fieldHeight;

    Tile playerTile;

    public float timePerMove;

    private float timePassed;

    List<Tile> tiles;

    Controller player1;
    
    public int FieldWidth
    {
        get { return fieldWidth; }
        set { fieldWidth = value; }
    }

    public int FieldHeight
    {
        get { return fieldHeight; }
        set { fieldHeight = value; }
    }

    // Use this for initialization
    void Start () {
        tiles = new List<Tile>();
        blocks = new Block[fieldWidth * fieldHeight];
        timePassed = 0f;
        player1 = new Controller(KeyCode.A, KeyCode.D, KeyCode.S, KeyCode.W);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKeyDown(KeyCode.G))
        {
            AddTile();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            CheckDown();
        }

        if (playerTile != null)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= timePerMove)
            {
                if (!CheckDownPlayerTile(playerTile))
                {
                    AddTile();
                }
                timePassed = 0f;
            }
        }

        if (Input.GetKeyDown(player1.keyLeft))
        {
            MoveTileLeft(playerTile);
        }
        else if (Input.GetKeyDown(player1.keyRight))
        {
            MoveTileRight(playerTile);
        }
        else if (Input.GetKeyDown(player1.keyDown))
        {
            CheckDownPlayerTile(playerTile);
        }
	}

    private void AddTile()
    {
        Tile tempTile = new Tile(3, 2, 4);
        tiles.Add(tempTile);
        for (int i = 0; i < tempTile.TileWidth * tempTile.TileHeight; i++)
        {
            if (tempTile.Blocks[i] != null)
            {
                Debug.Log(i);
                int tempTileSize = i;
                int tempPosition = 0;
                while(tempTileSize >= tempTile.TileWidth)
                {
                    tempTileSize -= tempTile.TileWidth;
                    tempPosition += fieldWidth; 
                }
                tempPosition += tempTileSize;

                blocks[tempPosition] = tempTile.Blocks[i];

                int cubeIndexPos = tempPosition;
                int cubePosX = 0;
                int cubePosY = 0;

                while(cubeIndexPos >=  fieldWidth)
                {
                    cubePosY++;
                    cubeIndexPos -= fieldWidth;
                }

                cubePosX = cubeIndexPos;



                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = "Cube nr. " + i.ToString();
                cube.transform.position = new Vector3(cubePosX, -cubePosY, 0);
                       
                tempTile.Blocks[i].GO = cube;
            }
        }

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] != null)
            {
                Debug.Log(i + ": " + blocks[i].TileID);
            }
        }

        playerTile = tempTile;
    }

    private void MoveTileLeft(Tile t)
    {
        bool clearLeft = true;

        foreach (Block b in t.Blocks)
        {
            int index = System.Array.IndexOf(blocks, b);
            if (index - 1 >= 0 && index - 1 % fieldWidth != fieldWidth - 1 % fieldWidth)
            {
                if (blocks[index - 1] != null)
                {
                    if (blocks[index - 1].TileID != t.ID)
                    {
                        clearLeft = false;
                        break;
                    }
                }
            }
            else
            {
                clearLeft = false;
                break;
            }
        }

        if(clearLeft)
        {
            Block[] tempBlocksArray = (Block[])blocks.Clone();

            foreach (Block b in t.Blocks)
            {
                if (b != null)
                {
                    int index = System.Array.IndexOf(blocks, b);
                    tempBlocksArray[index] = null;
                }
            }

            foreach (Block b in t.Blocks)
            {
                if (b != null)
                {
                    int index = System.Array.IndexOf(blocks, b);
                    b.GO.transform.Translate(-1, 0, 0);
                    tempBlocksArray[index - 1] = b;
                }
            }

            blocks = tempBlocksArray;
        }
    }

    private void MoveTileRight(Tile t)
    {
        bool clearRight = true;

        foreach (Block b in t.Blocks)
        {
            int index = System.Array.IndexOf(blocks, b);
            if (index + 1 < fieldWidth * fieldHeight && index + 1 % fieldWidth != fieldWidth + 1 % fieldWidth)
            {
                if (blocks[index + 1] != null)
                {
                    if (blocks[index + 1].TileID != t.ID)
                    {
                        clearRight = false;
                        break;
                    }
                }
            }
            else
            {
                clearRight = false;
                break;
            }
        }

        if (clearRight)
        {
            Block[] tempBlocksArray = (Block[])blocks.Clone();

            foreach (Block b in t.Blocks)
            {
                if (b != null)
                {
                    int index = System.Array.IndexOf(blocks, b);
                    tempBlocksArray[index] = null;
                }
            }

            foreach (Block b in t.Blocks)
            {
                if (b != null)
                {
                    int index = System.Array.IndexOf(blocks, b);
                    b.GO.transform.Translate(1, 0, 0);
                    tempBlocksArray[index + 1] = b;
                }
            }

            blocks = tempBlocksArray;
        }
    }

    private void CheckDown()
    {
        List<Tile> tempTiles = new List<Tile>();

        foreach(Tile t in tiles)
        {
            bool clearDown = true;

            foreach(Block b in t.Blocks)
            {
                int index = System.Array.IndexOf(blocks, b);
                if (index + fieldWidth < fieldWidth * fieldHeight)
                {
                    if (blocks[index + fieldWidth] != null)
                    {
                        if (blocks[index + fieldWidth].TileID != t.ID)
                        {
                            clearDown = false;
                            break;
                        }
                    }
                }
                else
                {
                    clearDown = false;
                    break;
                }
            }

            if (clearDown) tempTiles.Add(t);            
        }

        MoveDownInOrder(tempTiles);
    }

    private bool CheckDownPlayerTile(Tile t)
    {
        List<Tile> tileToMoveDown =  new List<Tile>();
        bool clearDown = true;

        foreach (Block b in t.Blocks)
        {
            int index = System.Array.IndexOf(blocks, b);
            if (index + fieldWidth < fieldWidth * fieldHeight)
            {
                if (blocks[index + fieldWidth] != null)
                {
                    if (blocks[index + fieldWidth].TileID != t.ID)
                    {
                        clearDown = false;
                        break;
                    }
                }
            }
            else
            {
                clearDown = false;
                break;
            }
        }

        if(clearDown)
        {
            tileToMoveDown.Add(t);
            MoveDownInOrder(tileToMoveDown);
            return true;
        }
        return false;
    }

    private void MoveDownInOrder(List<Tile> tilesToMoveDown)
    {
        Block[] tempBlocksArray = new Block[fieldWidth * fieldHeight];

        tempBlocksArray = (Block[])blocks.Clone();

        foreach (Tile t in tilesToMoveDown)
        {
            foreach(Block b in t.Blocks)
            {
                if (b != null)
                {
                    int index = System.Array.IndexOf(blocks, b);
                    tempBlocksArray[index] = null;
                }
            }

            foreach (Block b in t.Blocks)
            {
                if (b != null)
                {
                    int index = System.Array.IndexOf(blocks, b);
                    b.GO.transform.Translate(0, -1, 0);
                    tempBlocksArray[index + fieldWidth] = b;
                    int index2 = System.Array.IndexOf(tempBlocksArray, b);
                }
            }
        }

        blocks = tempBlocksArray;

        for (int i = 0; i < blocks.Length; i++)
        {
            if(blocks[i] != null)
            {
                Debug.Log(i + ": " + blocks[i].TileID);
            }
        }
    }
}
