//Author: Richard Crisp
//Organisation: N/A (Independent)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{
    //the boolean allySelected is used for determining whether to open normal pathfinder grid or enemy grid.
    public bool playerUnitSelected = false;
    public bool npcUnitSelected = false;
    public GameObject enemyClicked = null;
    public GameObject playerSelected = null;

    //GameObjects for holding and cloning the grid tiles
    //blue tile for ally movement
    private GameObject BlueTile;
    public GameObject NewBlueTile;

    //red tile for ally can attack enemy or vice versa
    private GameObject RedTile;
    public GameObject NewRedTile;

    //green tile for ally can support ally or enemy can support enemy
    private GameObject GreenTile;
    public GameObject NewGreenTile;

    //orange tile for enemy movement
    private GameObject OrangeTile;
    public GameObject NewOrangeTile;

    

    //A vector3 for holding a position value and a Quarternion for holding a rotation value
    private Vector3 Positioner;
    private Quaternion Rotation;

    //the final position values hold values for the position of the movement tile clicked in the pathfinder
    //ie, where the player wants the selected unit to move TO
    public int finalPositionX;
    public int finalPositionY;

    //the start position values hold values for the position of the selected unit when the pathfinder is clicked
    //for allies, this indicates where the selected unit will move FROM
    public int startPositionX;
    public int startPositionY;
    private int targetPositionX;
    private int targetPositionY;
    public int characterSpeed;
    public int charRange;
    public int unitType;
    public bool movementInProgress = false;

    //initialize an array for holding the root tile location from which a tile was accessed, for x and y
    //!!!!!!!!!!!!!!!!!!!!!!
    //public int[,] xRoot = new int[60, 30];
    //public int[,] yRoot = new int[60, 30];

    //creates a 2d array for holding information about characters or buildings on a tile
    // 0 = nothing, 1 = ally. 2 = enemy, 3 = neutral character, 4 = building, 5 = event.
    //public int[,] entityPresent = new int[60, 30];

    //at awake set values for the empty position and rotation so that instantiating tiles works later
    //set the prefab that will be used for each gameobject so that the correct sprite is loaded
    public void Awake()
    {
        BlueTile = GameObject.Find("BlueTile");
        RedTile = GameObject.Find("RedTile");
        GreenTile = GameObject.Find("GreenTile");
        OrangeTile = GameObject.Find("OrangeTile");
        Positioner = GameObject.Find("GreenTile").transform.position;
        Rotation = GameObject.Find("GreenTile").transform.rotation;
    }


    //the PathFind class determines which subroutine should be executed based on the units type
    //as enemy or ally and then as infantry, cavalry, etc.
    public void PathFind()
    {
        
            //unit type 1 indicates an infantry unit
            if (unitType == 1)
            {
                PathFindInfantry();
            }
      
    }

    //PathFindInfantry runs a pathfinder for allied infantry units
    public void PathFindInfantry()
    {
        //create a pointer to GetTiles in memory
        var tiles = FindObjectOfType<GetTiles>();

        //sets the z position of positioner to -1
        //this puts grid tiles in front of the tile map, but behind unit sprites.
        Positioner.z = -1f;

        //create temporary fields for holding xPosition and yPosition when altering xCP or yCP fields.
        //not to be confused with xCP and yCP which form the current position being checked
        int xPos;
        int yPos;

        //make first checkPoint the start position
        int xCP = startPositionX; //xCP is checkPoint position on x axis
        int yCP = startPositionY; //yXP is checkPoint position on y axis

        //set state of CheckPoint to 1
        tiles.tileInfo[xCP, yCP].State = 1;

        //initialize a value for checking for most efficient movement path
        int moveCheck = 0;


        //ensure each time this function is loaded, "end" is set to false
        bool end = false;

        //create a memory pointer to the TileMap
        Tilemap tilemap = GetComponent<Tilemap>();

        //creates a memory pointer to the cellBounds component of tilemap
        BoundsInt bounds = tilemap.cellBounds;

        //go through every tile on the grid and set the movesLeft value to -600
        //this ensures that the grid is reset before each execution of the pathfinder
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                //set moves left to -600 for every tile except checkpoint. This will be used to check that a tile has been checked
                //any tile that has not been checked will remain at -600. All checked tiles will be changed to a specific value.
                //any movesLeft value of 0 or more indicates that the tile can be reached.
                tiles.tileInfo[x, y].MovesLeft = -600;
                tiles.tileInfo[x, y].XRoot = -10;
                tiles.tileInfo[x, y].YRoot = 10;
                //a state of 1 indicates that a tile has not yet gone through any of it's adjacent tile checks
                tiles.tileInfo[x, y].State = 1;
                tiles.tileInfo[x, y].AllyPresent = false;
            }
        }

        //set root of checkpoint tile to -1 to let program know when all paths have been checked
        //a trigger will be set at end of loop to set end = true if xRoot and yRoot == -1
        tiles.tileInfo[xCP, yCP].XRoot = -1;
        tiles.tileInfo[xCP, yCP].YRoot = -1;

        //set the movesLeft on starting position to the movement speed of character
        tiles.tileInfo[xCP, yCP].MovesLeft = characterSpeed;
        //Debug.Log("xCP = " + xCP + ", yCP = " + yCP);

        bool boundsViolated = false;

        //the pathfinder will continue to run until end == true
        while (end == false)
        {

            //first check if state has reached state 5. If so, return to stored root tile.
            if (tiles.tileInfo[xCP, yCP].State == 5)
            {
                //if the root of the current tile is -1, then the pathfinder needs to end
                if (tiles.tileInfo[xCP, yCP].XRoot == -1)
                {
                    end = true;
                }
                //if the root of the current tile is not -1, then the pathfinder needs to return to the root tile
                else
                {
                    xPos = tiles.tileInfo[xCP, yCP].XRoot; //temporarily holds values for the x root destination from this tile
                    yPos = tiles.tileInfo[xCP, yCP].YRoot; //temporarily holds values for the y root destination from this tile
                    xCP = xPos;
                    yCP = yPos;
                }
            }



            /////////////////START OF NORTH CHECK
            #region North Check
            //if state is equal to one, then check tile to North (or y+1)
            else
            {
                if (tiles.tileInfo[xCP, yCP].State == 1)
                {
                    //Debug.Log("I'm in North Check");
                    //before anything else, change the state for this tile to two, so that it changes on next loop
                    tiles.tileInfo[xCP, yCP].State = 2;

                    //don't check north if the current tile is on the north edge of the map
                    if (yCP != (bounds.size.y - 1))
                    {
                        //Debug.Log("I'm not out of bounds to the north");
                        boundsViolated = false;

                        //only do this loop if not one tile south of start position
                        if (!(yCP == startPositionY - 1 && xCP == startPositionX))
                        {
                            //Debug.Log("bounds not violated");
                            boundsViolated = false;
                            targetPositionX = xCP;
                            targetPositionY = yCP + 1;
                        }
                        else
                        {
                            //don't check the starting tile
                            boundsViolated = true;
                        }


                    }
                    else
                    {


                        //don't attempt to check a tile outside of bounds
                        boundsViolated = true;
                    }
                }
                #endregion
                //END OF North CHECK


                ///////////////START OF SOUTH CHECK
                #region South Check
                else if (tiles.tileInfo[xCP, yCP].State == 2)

                {
                    //before anything else, change the state for this tile to three, so that it changes on next loop
                    tiles.tileInfo[xCP, yCP].State = 3;


                    //make sure checked tile is inside bounds
                    if (yCP != 0)
                    {

                        //only do this loop if not one tile north of start position
                        if (!(yCP == startPositionY + 1 && xCP == startPositionX))
                        {
                            boundsViolated = false;
                            targetPositionX = xCP;
                            targetPositionY = yCP - 1;


                        }
                        else
                        {
                            //don't check the starting tile
                            boundsViolated = true;
                        }


                    }
                    else
                    {


                        //don't attempt to check a tile outside of bounds
                        boundsViolated = true;
                    }
                }
                #endregion
                //END OF SOUTH CHECK


                /////////////////START OF EAST CHECK
                #region East Check
                //if state is equal to three, then check tile to East (or x+1)
                else if (tiles.tileInfo[xCP, yCP].State == 3)
                {
                    //before anything else, change the state for this tile to two, so that it changes on next loop
                    tiles.tileInfo[xCP, yCP].State = 4;



                    if (xCP != bounds.size.x - 1)
                    {
                        //only do this loop if current tile not one tile west of start position
                        if (!(xCP == startPositionX - 1 && yCP == startPositionY))
                        {
                            boundsViolated = false;
                            targetPositionX = xCP + 1;
                            targetPositionY = yCP;


                        }
                        else
                        {
                            //don't check the starting tile
                            boundsViolated = true;
                        }


                    }
                    else
                    {


                        //don't attempt to check a tile outside of bounds
                        boundsViolated = true;
                    }
                }
                #endregion
                //END OF East CHECK


                ///////////////START OF WEST CHECK
                #region West Check
                else if (tiles.tileInfo[xCP, yCP].State == 4)
                {
                    //before anything else, change the state for this tile to five, so that it changes on next loop
                    tiles.tileInfo[xCP, yCP].State = 5;


                    if (xCP != 0)
                    {
                        //only do this check if tile is not one point east of start position
                        if (!(xCP == startPositionX + 1 && yCP == startPositionY))
                        {

                            boundsViolated = false;
                            targetPositionX = xCP - 1;
                            targetPositionY = yCP;

                        }
                        else
                        {
                            //don't check the starting tile
                            boundsViolated = true;
                        }


                    }
                    else
                    {


                        //don't attempt to check a tile outside of bounds
                        boundsViolated = true;
                    }
                }
                #endregion
                //END OF WEST CHECK

                #region Pathfinder Logic
                //if bounds not viloated do tile check
                if (boundsViolated == false)
                {
                    //an enemy inhabits tile if enemy character is not null
                    if (tiles.tileInfo[targetPositionX, targetPositionY].EnemyCharacter != null)
                    {
                        if (tiles.tileInfo[targetPositionX, targetPositionY].XRoot != -999)
                        {
                            if (xCP == startPositionX && yCP == startPositionY)
                            {
                                tiles.tileInfo[targetPositionX, targetPositionY].MovesLeft = -400;
                                tiles.tileInfo[targetPositionX, targetPositionY].XRoot = -999;
                            }
                            //because allies can be travelled through, ensure that the current tile does not contain an ally
                            //because although allies can be travelled THROUGH, two units cannot occupy the same space.
                            //if no ally is present on the current tile then set movesLeft to -400 on target tile so red tile will be loaded
                            if (tiles.tileInfo[xCP, yCP].PlayerCharacter == null && tiles.tileInfo[xCP, yCP].AllyCharacter == null)
                            {
                                tiles.tileInfo[targetPositionX, targetPositionY].MovesLeft = -400;
                                //setting the root tile of the enemy tile, allows for movement and attack to be handled in a single action
                                //rather than forcing the player to move then attack as two seperate actions
                                if (tiles.tileInfo[targetPositionX, targetPositionY].XRoot != -999)
                                {
                                    if (tiles.tileInfo[targetPositionX, targetPositionY].XRoot == -10)
                                    {
                                        tiles.tileInfo[targetPositionX, targetPositionY].XRoot = xCP;
                                        tiles.tileInfo[targetPositionX, targetPositionY].YRoot = yCP;
                                    }
                                    else if (tiles.tileInfo[tiles.tileInfo[targetPositionX, targetPositionY].XRoot, tiles.tileInfo[targetPositionX, targetPositionY].YRoot].MovesLeft
                                             < tiles.tileInfo[xCP, yCP].MovesLeft)
                                    {
                                        tiles.tileInfo[targetPositionX, targetPositionY].XRoot = xCP;
                                        tiles.tileInfo[targetPositionX, targetPositionY].YRoot = yCP;
                                    }
                                }
                            }
                        }



                    }

                    //if ally inhabits tile do ally / other player check
                    else if (tiles.tileInfo[targetPositionX, targetPositionY].AllyCharacter != null || tiles.tileInfo[targetPositionX, targetPositionY].PlayerCharacter != null)
                    {
                        if (xCP == startPositionX && yCP == startPositionY)
                        {
                            tiles.tileInfo[targetPositionX, targetPositionY].AllyPresent = true;
                        }
                        //because allies can be travelled through, ensure that the current tile does not contain an ally
                        //because although allies can be travelled THROUGH, two units cannot occupy the same space.
                        //if no ally is present on the current tile then set entityPresent 11 to target tile so green tile will be loaded                           
                        else if ((tiles.tileInfo[xCP, yCP].AllyCharacter == null && tiles.tileInfo[xCP, yCP].PlayerCharacter == null))
                        {
                            //set ally present so that the pathfinder will load a green tile at this position
                            tiles.tileInfo[targetPositionX, targetPositionY].AllyPresent = true;
                            moveCheck = tiles.tileInfo[xCP, yCP].MovesLeft - tiles.tileInfo[targetPositionX, targetPositionY].InfCost;
                            if (tiles.tileInfo[targetPositionX, targetPositionY].MovesLeft < moveCheck)
                            {
                                tiles.tileInfo[targetPositionX, targetPositionY].MovesLeft = moveCheck;
                                tiles.tileInfo[targetPositionX, targetPositionY].XRoot = xCP;
                                tiles.tileInfo[targetPositionX, targetPositionY].YRoot = yCP;
                                if (tiles.tileInfo[targetPositionX, targetPositionY].MovesLeft > 0)
                                {
                                    xCP = targetPositionX;
                                    yCP = targetPositionY;
                                    tiles.tileInfo[xCP, yCP].State = 1;
                                }
                            }
                        }

                    }



                    //if no ally or enemy inhabits tile move on to other checks
                    else
                    {

                        //get movement cost of tile (this could be infCost, cavCost etc depending on character
                        //for now use infCost. Create a move check by subtrating moveCost from the movesLeft on this tile
                        moveCheck = tiles.tileInfo[xCP, yCP].MovesLeft - tiles.tileInfo[targetPositionX, targetPositionY].InfCost;

                        //check to see if tile has already been checked from another route
                        if (tiles.tileInfo[targetPositionX, targetPositionY].MovesLeft != -600)
                        {
                            //if tile has already been checked only update preferred route if this route is more efficient
                            if (tiles.tileInfo[targetPositionX, targetPositionY].MovesLeft < moveCheck)
                            {
                                tiles.tileInfo[targetPositionX, targetPositionY].MovesLeft = moveCheck;

                                //sets root tile of tile being check to current checkPoint
                                tiles.tileInfo[targetPositionX, targetPositionY].XRoot = xCP;
                                tiles.tileInfo[targetPositionX, targetPositionY].YRoot = yCP;

                                //makes this tile the new checkpoint
                                xCP = targetPositionX;
                                yCP = targetPositionY;

                                //because the optimal route to this tile has been changed, reset state to 1
                                tiles.tileInfo[xCP, yCP].State = 1;

                            }
                        }


                        //if this tile was not previously checked, record that it has been checked
                        else
                        {
                            //check that there is enough remaining movement to reach this tile
                            if (moveCheck >= 0)
                            {
                                tiles.tileInfo[targetPositionX, targetPositionY].MovesLeft = moveCheck;

                                //sets root tile of tile being check to current checkPoint
                                tiles.tileInfo[targetPositionX, targetPositionY].XRoot = xCP;
                                tiles.tileInfo[targetPositionX, targetPositionY].YRoot = yCP;

                                //only move to this tile to check adjacent tiles if there is some movement range left.
                                //if (moveCheck > 0)
                                //{
                                //makes this tile the new checkpoint
                                xCP = targetPositionX;
                                yCP = targetPositionY;

                                //because a route to this tile has been established, set state to 1
                                tiles.tileInfo[xCP, yCP].State = 1;


                            }
                            else
                            {
                                tiles.tileInfo[targetPositionX, targetPositionY].MovesLeft = moveCheck;



                            }
                        }
                    }
                }
            }





        }
        #endregion




        #region Create PathFinder Grid
        // set the prefab size for cloning
        GameObject.Find("BlueTile").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("RedTile").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("GreenTile").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("OrangeTile").transform.localScale = new Vector3(1, 1, 1);

        //Debug.Log("movesLeft 5,8 = " + movesLeft[5, 8]);



        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                //Debug.Log(x + "," + y + " state: " + tiles.tileInfo[x, y].State);

                if (x == xCP && y == yCP)
                {
                    //area for entering code for altering the tile on which the currently selected character is stood
                }
                else
                {
                    //if Tile contains an ally produce green overlay
                    //moves left of indicates enemy present
                    if (tiles.tileInfo[x, y].AllyPresent == true)
                    {

                        Positioner.x = 0f + x;
                        Positioner.y = 0f + y;

                        if (playerUnitSelected == true)
                        {
                            NewGreenTile = Instantiate(GreenTile, Positioner, Rotation);
                            NewGreenTile.tag = "PathFinder";
                            NewGreenTile.transform.localScale = new Vector3(1, 1, 1);
                        }
                        else
                        {
                            NewRedTile = Instantiate(RedTile, Positioner, Rotation);
                            NewRedTile.tag = "PathFinder";
                            NewRedTile.transform.localScale = new Vector3(1, 1, 1);
                        }

                        //Debug.Log("Ally entity detected here at " + x + "," + y);

                    }
                    //if tile contains enemy produce red overlay
                    //movesleft of -400 indicates enemy present
                    else if (tiles.tileInfo[x, y].MovesLeft == -400)
                    {
                        //for debugging movement left and reachable tiles
                        //Debug.Log("x:" + x + " y:" + y + " Contains an ally");

                        Positioner.x = 0f + x;
                        Positioner.y = 0f + y;

                        if (playerUnitSelected == true)
                        {
                            NewRedTile = Instantiate(RedTile, Positioner, Rotation);
                            NewRedTile.tag = "PathFinder";
                            NewRedTile.transform.localScale = new Vector3(1, 1, 1);
                        }
                        else
                        {
                            NewGreenTile = Instantiate(GreenTile, Positioner, Rotation);
                            NewGreenTile.tag = "PathFinder";
                            NewGreenTile.transform.localScale = new Vector3(1, 1, 1);
                        }

                        //Debug.Log("the root position of " + x +"," + y + " is " + xRoot[x,y] + "," + yRoot[x,y]);

                        //entityPresent[x, y] = 1;

                        //Debug.Log("Enemy entity detected here at " + x + "," + y);

                    }
                    else if (tiles.tileInfo[x, y].MovesLeft >= 0)
                    {

                        //for debugging movement left and reachable tiles
                        //Debug.Log("x:" + x + " y:" + y + " Can be Moved to");
                        //Debug.Log("movesLeft at " + x + "," + y + "= " + tiles.tileInfo[x, y].MovesLeft);

                        Positioner.x = 0f + x;
                        Positioner.y = 0f + y;

                        if (playerUnitSelected == true)
                        {
                            NewBlueTile = Instantiate(BlueTile, Positioner, Rotation);
                            NewBlueTile.tag = "PathFinder";
                            NewBlueTile.transform.localScale = new Vector3(1, 1, 1);
                        }
                        else
                        {
                            NewOrangeTile = Instantiate(OrangeTile, Positioner, Rotation);
                            NewOrangeTile.tag = "PathFinder";
                            NewOrangeTile.transform.localScale = new Vector3(1, 1, 1);
                        }


                    }
                }

            }
        }




        // Hide the original prefab used for cloning by setting it's scale to 0,0,0
        GameObject.Find("BlueTile").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("RedTile").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("GreenTile").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("OrangeTile").transform.localScale = new Vector3(0, 0, 0);
        //EntityResetter();

        #endregion

    }



    //method that determines whether unit attacking is melee or ranged of a specific distance
    //or a healer
    public void AttackSelector()
    {
        if (playerUnitSelected == true)
        {
            //indicates the attack position of the selected ally
            //entityPresent[startPositionX, startPositionY] = 99;
        }
        if (charRange == 1)
        {
            MeleeAttack();
        }
        if (charRange == 2)
        {
            BowAttack();
        }
        if (charRange == 3)
        {
            MagicAttack();
        }
        if (charRange == 4)
        {
            MusketAttack();
        }


    }

    //method for unit with 1 attack range
    public void MeleeAttack()
    {
        var tiles = FindObjectOfType<GetTiles>();
        //create a memory pointer to the TileMap
        Tilemap tilemap = GetComponent<Tilemap>();

        //creates a memory pointer to the cellBounds component of tilemap
        BoundsInt bounds = tilemap.cellBounds;
        if (startPositionX != bounds.size.x - 1)
        {
            if (tiles.tileInfo[startPositionX + 1, startPositionY].EnemyCharacter != null)
            {
                Positioner.x = startPositionX + 1;
                Positioner.y = startPositionY;

                NewRedTile = Instantiate(RedTile, Positioner, Rotation);
                NewRedTile.tag = "PathFinder";
                NewRedTile.transform.localScale = new Vector3(1, 1, 1);

                //entityPresent[startPositionX + 1, startPositionY] = 22;

            }
            else if (tiles.tileInfo[startPositionX + 1, startPositionY].AllyCharacter != null || tiles.tileInfo[startPositionX + 1, startPositionY].PlayerCharacter != null)
            {
                Positioner.x = startPositionX + 1;
                Positioner.y = startPositionY;

                //entityPresent[startPositionX + 1, startPositionY] = 11;


                NewGreenTile = Instantiate(GreenTile, Positioner, Rotation);
                NewGreenTile.tag = "PathFinder";
                NewGreenTile.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if (startPositionX != 0)
        {
            if (tiles.tileInfo[startPositionX - 1, startPositionY].EnemyCharacter != null)
            {
                Positioner.x = startPositionX - 1;
                Positioner.y = startPositionY;

                NewRedTile = Instantiate(RedTile, Positioner, Rotation);
                NewRedTile.tag = "PathFinder";
                NewRedTile.transform.localScale = new Vector3(1, 1, 1);

                //entityPresent[startPositionX - 1, startPositionY] = 22;
            }
            if (tiles.tileInfo[startPositionX - 1, startPositionY].AllyCharacter != null || tiles.tileInfo[startPositionX - 1, startPositionY].PlayerCharacter != null)
            {
                Positioner.x = startPositionX - 1;
                Positioner.y = startPositionY;


                NewGreenTile = Instantiate(GreenTile, Positioner, Rotation);
                NewGreenTile.tag = "PathFinder";
                NewGreenTile.transform.localScale = new Vector3(1, 1, 1);

                //entityPresent[startPositionX - 1, startPositionY] = 11;
            }
        }
        if (startPositionY != bounds.size.y - 1)
        {
            if (tiles.tileInfo[startPositionX, startPositionY + 1].EnemyCharacter != null)
            {
                Positioner.x = startPositionX;
                Positioner.y = startPositionY + 1;

               
                    NewRedTile = Instantiate(RedTile, Positioner, Rotation);
                    NewRedTile.tag = "PathFinder";
                    NewRedTile.transform.localScale = new Vector3(1, 1, 1);
               
                //entityPresent[startPositionX, startPositionY +1] = 22;
            }
            if (tiles.tileInfo[startPositionX, startPositionY + 1].AllyCharacter != null || tiles.tileInfo[startPositionX, startPositionY + 1].PlayerCharacter != null)
            {
                Positioner.x = startPositionX;
                Positioner.y = startPositionY + 1;


                NewGreenTile = Instantiate(GreenTile, Positioner, Rotation);
                NewGreenTile.tag = "PathFinder";
                NewGreenTile.transform.localScale = new Vector3(1, 1, 1);

                //entityPresent[startPositionX, startPositionY+1] = 11;
            }
        }
        if (startPositionY != 0)
        {
            if (tiles.tileInfo[startPositionX, startPositionY - 1].EnemyCharacter != null)
            {
                Positioner.x = startPositionX;
                Positioner.y = startPositionY - 1;

                NewRedTile = Instantiate(RedTile, Positioner, Rotation);
                NewRedTile.tag = "PathFinder";
                NewRedTile.transform.localScale = new Vector3(1, 1, 1);

                //entityPresent[startPositionX, startPositionY-1] = 22;
            }
            if (tiles.tileInfo[startPositionX, startPositionY - 1].AllyCharacter != null || tiles.tileInfo[startPositionX, startPositionY - 1].PlayerCharacter != null)
            {
                Positioner.x = startPositionX;
                Positioner.y = startPositionY - 1;


                NewGreenTile = Instantiate(GreenTile, Positioner, Rotation);
                NewGreenTile.tag = "PathFinder";
                NewGreenTile.transform.localScale = new Vector3(1, 1, 1);

                //entityPresent[startPositionX, startPositionY-1] = 11;
            }
        }




    }

    //method for unit with 2 attack range only
    public void BowAttack()
    {

    }

    //method for unit with 1-3 attack range
    public void MagicAttack()
    {

    }

    //method for unit with 2-4 attack range
    public void MusketAttack()
    {

    }

    public void EntityResetter()
    {
        var tiles = FindObjectOfType<GetTiles>();
        //create a memory pointer to the TileMap
        Tilemap tilemap = GetComponent<Tilemap>();

        //creates a memory pointer to the cellBounds component of tilemap
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
               tiles.tileInfo[x, y].AllyPresent = false;
               tiles.tileInfo[x, y].MovesLeft = -600;
            }
        }
        movementInProgress = false;
    }

    public void PathFinderResetter()
    {
        //This is used for deleting pathfinder tiles from other objects
        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("PathFinder"))
        {
            Destroy(fooObj);
        }
    }

    //if the user clicks on his own character
    public void SelectedCharacterClicked()
    {
        // do stuff


        //at end of code load the entity resetter
        EntityResetter();
    }

}
