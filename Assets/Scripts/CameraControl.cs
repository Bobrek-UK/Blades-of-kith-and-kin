//Author: Richard Crisp
//Organisation: N/A (Independent)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    //creates field to store Vector2 direction information for the camera.
    protected Vector3 direction;

    //creates field to store the speed at which camera moves around the game screen.
    [SerializeField]
    private float speed;

    

    // Use this for initialization
    void Start () {

               
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
        Move();
	}

    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        /* AnimatePeeps(direction);*/
    }

    private void GetInput()
    {
        //resets the direction on each update to ensure that if no key is held down character does not move
        direction = Vector2.zero;

        //based on player input set direction of player and set animation state
        if (Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
            
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = Vector2.down;
          
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = Vector2.left;
          
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = Vector2.right;
            
        }
    }
}
