using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    Block[] blocks;
    [SerializeField]
    private int fieldWidth;
    [SerializeField]
    private int fieldHeight;

    List<Tile> tiles;
    
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
	}

    private void AddTile()
    {
        Tile tempTile = new Tile(3, 2, 4);
        tiles.Add(tempTile);
        for (int i = 0; i < tempTile.TileWidth * tempTile.TileHeight; i++)
        {
            if (tempTile.Blocks[i] != null)
            {
                int tempTileSize = i;
                int tempPosition = 0;
                while(tempTileSize >= tempTile.TileWidth)
                {
                    tempTileSize -= 3;
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
                cube.transform.position = new Vector3(cubePosX, cubePosY, 0);
                       
                tempTile.Blocks[i].GO = cube;
            }
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
                    Debug.Log(index);             
                    tempBlocksArray[index] = null;
                }
            }

            foreach (Block b in t.Blocks)
            {
                if (b != null)
                {
                    int index = System.Array.IndexOf(blocks, b);
                    Debug.Log(index + " + " + fieldWidth + " : " + b.TileID);
                    b.GO.transform.Translate(0, -1, 0);
                    tempBlocksArray[index + fieldWidth] = b;
                    int index2 = System.Array.IndexOf(tempBlocksArray, b);
                    Debug.Log(index2);
                }
            }
        }

        blocks = tempBlocksArray;
    }
}
