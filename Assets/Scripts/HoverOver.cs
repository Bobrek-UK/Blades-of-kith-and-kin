//Author: Richard Crisp
//Organisation: N/A (Independent)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOver : MonoBehaviour {

    //create a variable for holding a grid position
    private Vector3 HoverPosition;

    //create a bool for storing whether this tile has been moused over
    //this applies until the mouse has moved out of the tile. this prevents multiple
    //copies of the UI sprites from loading at the same time
    private bool isHovered = false;

    //create gameobects for holding the original tile and instantiating clones into 
    //the UI. Clones are created so the original prefab remains after sprite clean up,
    //allowing further clones to be created
    private GameObject SelectedTile;
    private GameObject CurrentSelectedTile;
    private GameObject AttackTile;
    private GameObject CurrentAttackTile;
    private GameObject AssistTile;
    private GameObject CurrentAssistTile;
    private GameObject MoveTile;
    private GameObject CurrentMoveTile;
    private GameObject DefendTile;
    private GameObject CurrentDefendTile;

    private void Awake()
    {
        //associate the correct sprite with each gameobject
        SelectedTile = GameObject.Find("SelectedTile");
        AttackTile = GameObject.Find("AttackTile");
        AssistTile = GameObject.Find("AssistTile");
        MoveTile = GameObject.Find("MoveTile");
        DefendTile = GameObject.Find("DefendTile");
    }

    //a method that can be called from other scripts/methods to clear the UI of instantiated action sprites
    //(move, attack,defend, assist etc) this does not clear the movement/pathfinder grid
    public void ClearObjects()
    {
        
        Destroy(CurrentAttackTile);
        Destroy(CurrentAssistTile);
        Destroy(CurrentMoveTile);
        Destroy(CurrentDefendTile);
    }

    // When the user hovers over one of the tiles (currently this script is attached to the blue pathfinder tiles
    //(for movement) and each of the character and enemy sprites (for action)).
    void OnMouseOver() {



        //create a pointer to the PathFinder script, this will be used in multiple conditions and branches
        var path = FindObjectOfType<PathFinder>();
        var tiles = FindObjectOfType<GetTiles>();



        //set hoverposition to the current position of this tile
        HoverPosition = transform.position;

        //create an integer for storing the value of entitycheck at the current grid position
        //int entityCheck = path.entityPresent[(int)HoverPosition.x, (int)HoverPosition.y];

        //removes any decimal places from x and y position to ensure a centred position on the tile
        HoverPosition.x = (int)HoverPosition.x;
        HoverPosition.y = (int)HoverPosition.y;

        //Debug.Log(tiles.tileInfo[(int)HoverPosition.x, (int)HoverPosition.y].MovesLeft);

        //ensures the object is the placed in front of the background
        HoverPosition.z = -10;
        //Debug.Log(tiles.tileInfo[(int)HoverPosition.x, (int)HoverPosition.y].AllyPresent == true);
        //if this is a blue tile the movement sprite may need to be loaded
        if (path.playerUnitSelected == true)
        {
            if (name == "BlueTile(Clone)")
            {
                //only load a new movement sprite for this tile if one has not already been created
                if (isHovered == false)
                {
                    //and only load a new movement sprite for this tile if movement is not currently in progress
                    if (path.movementInProgress == false)
                    {
                        //set isSelected to true so that another movement sprite will not be created here
                        isHovered = true;

                        //find the original MoveTile sprite and set it to a visible scale
                        GameObject.Find("MoveTile").transform.localScale = new Vector3(1, 1, 1);
                        //instantiate the close at the current position
                        CurrentMoveTile = Instantiate(MoveTile, HoverPosition, transform.rotation);
                        //tag the clone with the PathFinder tag for sprite clean up later
                        CurrentMoveTile.tag = ("PathFinder");
                        //find the original MoveTile and set it to an invisible scale
                        GameObject.Find("MoveTile").transform.localScale = new Vector3(0, 0, 0);
                    }
                }
            }

            //if the entity is set to 22 this has been picked up by the attack routine as an enemy that is in range of the
            //currently selected character. An attack sprite may need to be loaded        
            else if (tiles.tileInfo[(int)HoverPosition.x, (int)HoverPosition.y].MovesLeft == -400)
            {
                //only load an attack sprite at this location if one has not already been created
                if (isHovered == false)
                {
                    //show that the sprite has already been created for this tile on further checks
                    isHovered = true;
                    //find the original attack tile sprite and set it to a visible scale
                    GameObject.Find("AttackTile").transform.localScale = new Vector3(1, 1, 1);
                    //create a clone of the attack tile at the current location
                    CurrentAttackTile = Instantiate(AttackTile, HoverPosition, transform.rotation);
                    //tag the cloned tile with PathFinder for sprite cleanup later
                    CurrentAttackTile.tag = ("PathFinder");
                    //find the original attack tile sprite and set it to an invisible scale
                    GameObject.Find("AttackTile").transform.localScale = new Vector3(0, 0, 0);
                }
            }

            //if the entity is set to 11, this has been picked up by the attack routine as an ally that is in range of the 
            //currently selected character. An assist sprite may need to be loaded

            else if (tiles.tileInfo[(int)HoverPosition.x, (int)HoverPosition.y].AllyPresent == true)
            {
                //only load an assist sprite at this location if one has not already been created
                if (isHovered == false)
                {
                    //show that the sprite has already been created for this tile on further checks
                    isHovered = true;
                    //find the original assist tile sprite and set it to a visible scale
                    GameObject.Find("AssistTile").transform.localScale = new Vector3(1, 1, 1);
                    //create a clone of the assist tile at the current location
                    CurrentAssistTile = Instantiate(AssistTile, HoverPosition, transform.rotation);
                    //tag the cloned tile with PathFinder for sprite clean up later on
                    CurrentAssistTile.tag = ("PathFinder");
                    //find the original assist sprite and set it to an invisible scale
                    GameObject.Find("AssistTile").transform.localScale = new Vector3(0, 0, 0);
                }
            }

            //if the entity is set to 99, this has been picked up by the attack routine as the currently selected character
            //following the movement phase. A defend sprite may need to be loaded

            if ((int)HoverPosition.x == path.startPositionX && (int)HoverPosition.y == path.startPositionY)
            {
                //only load a defend sprite at this location if one has not already been created
                if (isHovered == false)
                {
                    //show that the sprite has already been created for this tile on further checks
                    isHovered = true;
                    //find the original tile sprite and set it to a visible scale
                    GameObject.Find("DefendTile").transform.localScale = new Vector3(1, 1, 1);
                    //create a clone of the defend tile at the current location
                    CurrentDefendTile = Instantiate(DefendTile, HoverPosition, transform.rotation);
                    //tag the cloned sprite with the pathfinder tag for cleanup later
                    CurrentDefendTile.tag = ("PathFinder");
                    //find the original defend tile and set it to an invisible scale
                    GameObject.Find("DefendTile").transform.localScale = new Vector3(0, 0, 0);
                }
            }
        }


        //if no entity has been assigned to this character yet and no blue tile has been created, then create a sprite for showing
        //the player that this tile will be selected when the mouse is clicked. Only do so if a sprite has not already been created
        //at this location
        else if (isHovered == false)
        {
            //show that a tile has already been created at this location for further checks
            isHovered = true;

            //find the original selectedTile and set it to a visible scale
            GameObject.Find("SelectedTile").transform.localScale = new Vector3(1, 1, 1);
            //create a clone of the selectedTile at the current location
            CurrentSelectedTile = Instantiate(SelectedTile, HoverPosition, transform.rotation);
            //tag the cloned sprite with PathFinder for cleanup later
            CurrentSelectedTile.tag = ("PathFinder");
            //find the original selected tile and set it to an invisible scale
            GameObject.Find("SelectedTile").transform.localScale = new Vector3(0, 0, 0);
        }
          
	}
	
	
	void OnMouseExit () {
        //if something has been created
		if (isHovered == true)
        {
            
            //destroy all cloned tiles that may have been created at this location
            Destroy(CurrentSelectedTile);
            Destroy(CurrentAttackTile);
            Destroy(CurrentAssistTile);
            Destroy(CurrentMoveTile);
            Destroy(CurrentDefendTile);
            //set the current tile to no longer hovered
            isHovered = false;

        }
	}
}
