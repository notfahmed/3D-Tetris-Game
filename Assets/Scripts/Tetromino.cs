using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float fall = 0;
    public float fallSpeed = 1;
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
		}
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
            transform.position += new Vector3(-1, 0, 0);
		}
        else if(Input.GetKeyDown(KeyCode.UpArrow))
		{
            transform.Rotate(0, 0, 90);
		}
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
		{
            transform.position += new Vector3(0, -1, 0);

            fall = Time.time;
		}
	}
}
