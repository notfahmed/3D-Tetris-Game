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
		//Part 6 Changes
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
            Vector2 pos = FindObjectOfType<Game>().round(mino.position);

            if(FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false)
			{
                return false;
			}

		}
        return true;
	}
}
