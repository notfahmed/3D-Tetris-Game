using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 20;
 
    public static Transform[,] grid = new Transform[gridWidth,gridHeight];

    public int scoreOneLine = 40;
    public int scoreTwoLines = 100;
    public int scoreThreeLines = 300;
    public int scoreFourLines = 1200;

    public Text scoreHUD;

    private int numbersofRowsThisTurn = 0;

    private int currentScore = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnNextTetromino();
    }

    void Update()
	{
        UpdateScore();
        UpdateUI();
	}

    public void UpdateUI()
	{
        scoreHUD.text = currentScore.ToString();
	}

    public void UpdateScore()
	{
        if(numbersofRowsThisTurn > 0)
		{
			switch (numbersofRowsThisTurn)
			{
                case 1:
                    ClearedOneLine();
                    break;
                case 2:
                    ClearedTwoLines();
                    break;
                case 3:
                    ClearedThreeLines();
                    break;
                case 4:
                    ClearedFourLines();
                    break;
                default:
                    break;
			}
            numbersofRowsThisTurn = 0;
		}
	}

    public void ClearedOneLine()
	{
        currentScore += scoreOneLine;
	}

    public void ClearedTwoLines()
	{
        currentScore += scoreTwoLines;
	}

    public void ClearedThreeLines()
	{
        currentScore += scoreThreeLines;
	}

    public void ClearedFourLines()
	{
        currentScore += scoreFourLines;
	}

    public bool CheckIsAboveGrid(Tetromino tetromino)

        //Checks if a tetromino is above the grid by getting the current position and the grid height then comparing them.
	{
        for(int i = 0; i < gridWidth; i++)
		{
            foreach(Transform mino in tetromino.transform)
			{
                Vector2 pos = Round(mino.position);
                if(pos.y > gridHeight - 1)
				{
                    return true;
				}
			}
		}
        return false;
	}

    public bool IsFullRowAt(int y)
    {
    	for(int x = 0; x < gridWidth; ++x)
	    {  
		    if(grid[x, y] == null) //The position is null meaning there is no block in that position therefore the row is not full
		    {
			    return false;
		    }
	    }
        numbersofRowsThisTurn++;

        return true;
    }
    
    public void DeleteMinoAt (int y) 
    {
        //This works along IsFUllRowAt as we can essentially delete rows with this method with the result of the IsFullRowAt
        for (int x = 0; x < gridWidth; ++x)
	{
		Destroy(grid[x,y].gameObject);
		grid[x,y] = null;
	}
    }
    
    public void MoveRowDown (int y) 
    {
        //This will be used in conjunction with MoveAllRowsDown as we move each row using this method called by MoveAllRowsDown

        for (int x = 0; x < gridWidth; ++x)
	{
		if(grid[x,y] != null)
		{
			grid[x, y-1] = grid[x,y];
			grid[x,y] = null;
			grid[x,y-1].position += new Vector3(0, -1, 0);
		}
	}
    }
    
    public void MoveAllRowsDown (int y)
    {
        //As the name suggests, we move all the rows down by looping through the heigth and calling moverowdown for each level.

        for (int x = y; x < gridHeight; ++x)
	{
		MoveRowDown(x);
	}
    }
    
    public void DeleteRow ()
    {
        //The method that drives all of the other methods so that rows can be cleared with a single call.

        for (int y = 0; y < gridHeight; ++y) //Loop through the height
	{
		if(IsFullRowAt(y)) //Find any row that is full
		{
			DeleteMinoAt(y); //Delete the row that is full
			MoveAllRowsDown(y+1); //Anything above the cleared row should be moved down
			--y;
		}
	}
    }

    public void UpdateGrid (Tetromino tetromino)
    {
        //Updates grid everytime a tetromino lands on the grid or other tetrominos
    	for(int y = 0; y < gridHeight; ++y)
	    {
		    for(int x = 0; x < gridWidth; ++x)
		    {
			    if(grid[x, y] != null)
			    {
				    if(grid[x,y].parent == tetromino.transform)
				    {
					    grid[x,y] = null;
				    }
			    }
		    }
	    }
    	foreach (Transform mino in tetromino.transform)
	    {
		    Vector2 pos = Round(mino.position);
		    if(pos.y < gridHeight)
		    {
			    grid[(int)pos.x, (int)pos.y] = mino;
		    }
	    }
    }
    
    public Transform GetTransformAtGridPosition (Vector2 pos)
    {
        //Check if a piece interfers with another piece in the grid.
    	if(pos.y > gridHeight - 1)
	    {
		    return null;
	    }
	    else
	    {
		    return grid[(int)pos.x, (int)pos.y];
	    }
    }
    
    public void SpawnNextTetromino()
	{
        GameObject nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector2(5.0f, 20.0f), Quaternion.identity);
	}
    
    string GetRandomTetromino() 
	{
        //Randomly selects a Tetromino for the next piece
        int randomTetromino = Random.Range(1, 8);
        string randomTetrominoName = "Prefabs/Tetromino_T";

		switch (randomTetromino)
		{
            case 1:
                randomTetrominoName = "Prefabs/Tetromino_T";
                break;
            case 2:
                randomTetrominoName = "Prefabs/Tetromino_Long";
                break;
            case 3:
                randomTetrominoName = "Prefabs/Tetromino_Square";
                break;
            case 4:
                randomTetrominoName = "Prefabs/Tetromino_J";
                break;
            case 5:
                randomTetrominoName = "Prefabs/Tetromino_L";
                break;
            case 6:
                randomTetrominoName = "Prefabs/Tetromino_S";
                break;
            case 7:
                randomTetrominoName = "Prefabs/Tetromino_Z";
                break;
        }
        return randomTetrominoName;
	}

    public void GameOver()
	{
        //Loads Game Over scene

        SceneManager.LoadScene("GameOver");
    }

    public bool CheckIsInsideGrid(Vector2 pos) //Used to check if a Tetromino is inside of the grid
	{
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
	}

    public Vector2 Round(Vector2 pos) //Round function
	{
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
	}
}
