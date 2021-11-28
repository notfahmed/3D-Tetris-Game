using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float fall = 0;
    public float fallSpeed = 1;

    public bool allowRotation = true;
    public bool limitRotation = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckUserInput();
    }

    void CheckUserInput()
	{
        /*
         *If the user inputs the "Right Arrow" Key we transform the current moving tetromino by 1 in the x direction. 
         *Then we check if the current position is valid. If it is valid we update the grid to show the user the new position
         *If it is not valid we move back the tetromino by -1 in the x direction to
        */
        if (Input.GetKeyDown(KeyCode.RightArrow))
		{

            transform.position += new Vector3(1, 0, 0); 
			if (CheckIsValidPosition())
			{
				FindObjectOfType<Game>().UpdateGrid(this);
			}
			else
			{
                transform.position += new Vector3(-1, 0, 0);
            }
		}
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
            transform.position += new Vector3(-1, 0, 0);
            if (CheckIsValidPosition())
            {
		FindObjectOfType<Game>().UpdateGrid(this);
            }
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (allowRotation)
			{
				if (limitRotation)
				{
                    if(transform.rotation.eulerAngles.z >= 90)
					{
                        transform.Rotate(0, 0, -90);
					}
					else
					{
                        transform.Rotate(0, 0, 90);
					}
				}
				else
				{
                    transform.Rotate(0, 0, 90);
                }
                if (CheckIsValidPosition())
                {
			FindObjectOfType<Game>().UpdateGrid(this);
                }
                else
                {
					if (limitRotation)
					{
                        if (transform.rotation.eulerAngles.z >= 90)
                        {
                            transform.Rotate(0, 0, -90);
                        }
                        else
                        {
                            transform.Rotate(0, 0, 90);
                        }
                    }
					else
					{
                        transform.Rotate(0, 0, -90);
					}
                }
            }
           
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
		{
            transform.position += new Vector3(0, -1, 0);

            if (CheckIsValidPosition())
            {
		        FindObjectOfType<Game>().UpdateGrid(this);
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
		        FindObjectOfType<Game>().DeleteRow();

				if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
				{
                    FindObjectOfType<Game>().GameOver();
				}

		        enabled = false;
		        FindObjectOfType<Game>().SpawnNextTetromino();
            }

            fall = Time.time;
		}
	}

    bool CheckIsValidPosition()
	{
        foreach(Transform mino in transform)
		{
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);

            //Checks if a tetromino is within a grid
            if(FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false)
			{
                return false;
			}

		    //Checks if the tetromino colided with another tetromino
		    if(FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
		    {
			    return false;
		    }
		}
        return true;
	}
}
