//Author: Richard Crisp
//Organisation: N/A (Independent)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClicked : MonoBehaviour {

    //create a vector3 for holding the position a character needs to move to
    private Vector3 finalPosition;

    
    void OnMouseDown()
    {
        //create a pointer to the pathfinder script in memory
        var pathFind = FindObjectOfType<PathFinder>();
        //the user can only move a character if it is an ally
        if (pathFind.playerUnitSelected == true)
        {
            //if movement of a character is not currently in progress the run this code
            if (pathFind.movementInProgress == false)
            {
                //the finalposition is taken from the location of this tile (clicked by the player)
                finalPosition = gameObject.transform.position;
                //use this Vector3 position and feed it into the integer fields for final position 
                //inside the pathFinderscript
                pathFind.finalPositionX = (int)finalPosition.x;
                pathFind.finalPositionY = (int)finalPosition.y;

                //create a pointer to the character gameObject which is tagged as being selected by the player
                //(this is the character that they want to move)
                var mover = GameObject.FindGameObjectWithTag("Selected");
                //find the SelectAlly script within this gameObject and create a pointer to it
                var allyGo = mover.GetComponent<PlayerClicked>();

                //movement is about to commence, so set that movement is in progress to prevent multiple movement
                //inputs for the player before the current sequence is completed
                pathFind.movementInProgress = true;
                //load the characterAction method inside the SelectAlly script in the tagged gameObject
                allyGo.CharacterAction();

            }
        }
            
        
	}
	
}
