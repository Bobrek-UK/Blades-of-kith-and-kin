//Author: Richard Crisp
//Organisation: N/A (Independent)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClicked : MonoBehaviour
{

    private Vector3 characterPosition;
    [SerializeField]
    private Transform SpawnPoint;
    [SerializeField]
    private GameObject SelectedChar;
    private GameObject CurrentSelectedChar;
    private bool characterMoved = false;
    
    Vector3 destination;

    [SerializeField]
    private int characterSpeed;
    [SerializeField]
    private int characterRange;
    [SerializeField]
    private int unitType; // is it infantry, cavalry, etc?

    private int[] nextTileX = new int[20];
    private int[] nextTileY = new int[20];

    private int rootCheck;

    public int i = 0;

    private int r = 0;

    public bool isMoving = false;

    // Use this for initialization
    void OnMouseDown()
    {
        

        foreach (var hover in FindObjectsOfType<HoverOver>())
        {
            hover.ClearObjects();
        }
        //delete selection highlighter to remove any leftover artefacts
        foreach (GameObject highlight in GameObject.FindGameObjectsWithTag("SelectionHighlighter"))
        {
            Destroy(highlight);
        }
        var pathFind = FindObjectOfType<PathFinder>();
        var tiles = FindObjectOfType<GetTiles>();


        #region ally support action

        //if ally character has been found to be in range by pathfinder do the following
        if (tiles.tileInfo[(int)transform.position.x, (int)transform.position.y].AllyPresent == true)
        {
            pathFind.EntityResetter();
            pathFind.PathFinderResetter();
            pathFind.playerUnitSelected = false;

            //ally support action code or reference to support method will go here
        }

        #endregion


        //if the character previously clicked has been clicked again, cancel move (entity reset)
        else if (pathFind.playerUnitSelected == true)
        {
            if (pathFind.startPositionX == (int)transform.position.x && pathFind.startPositionY == (int)transform.position.y)
            {
                pathFind.EntityResetter();
                pathFind.PathFinderResetter();
                pathFind.playerUnitSelected = false;
            }
        }
        else
        {
            
            if (pathFind.movementInProgress == false)
            {
                pathFind.playerSelected = gameObject;
                //run pathfinder routine if character not yet moved
                if (characterMoved == false)
                {
                    pathFind.playerUnitSelected = true;
                    //clear selected tag from any selected ally
                    foreach (GameObject selObj in GameObject.FindGameObjectsWithTag("Selected"))
                    {
                        selObj.tag = "UnSelected";
                    }
                    gameObject.tag = "Selected";

                    //clear enemy tags from any selected enemy
                    foreach (GameObject selectedEnemies in GameObject.FindGameObjectsWithTag("EnemyClicked"))
                    {
                        selectedEnemies.tag = "UnSelected";
                    }



                    pathFind.PathFinderResetter();


                    characterPosition = gameObject.transform.position;

                    //Debug.Log("character position for " + gameObject.name + " is " + (int)characterPosition.x + "," + (int)characterPosition.y);


                    pathFind.startPositionX = (int)characterPosition.x;
                    pathFind.startPositionY = (int)characterPosition.y;
                    pathFind.characterSpeed = characterSpeed;
                    pathFind.charRange = characterRange;
                    pathFind.unitType = unitType; // is it infantry, cavalry, etc?


                    //make sure tile snaps correctly to the grid
                    characterPosition.x = (int)characterPosition.x;
                    characterPosition.y = (int)characterPosition.y;
                    characterPosition.z = -3;
                    SpawnPoint.position = characterPosition;

                    GameObject.Find("SelectedChar").transform.localScale = new Vector3(1, 1, 1);

                    CurrentSelectedChar = Instantiate(SelectedChar, SpawnPoint.position, SpawnPoint.rotation);
                    CurrentSelectedChar.tag = "SelectionHighlighter";

                    GameObject.Find("SelectedChar").transform.localScale = new Vector3(0, 0, 0);

                    pathFind.PathFind();
                }


                else
                {

                    //This is used for deleting the pathfinder tiles - this needs to be moved
                    foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("PathFinder"))
                    {
                        Destroy(fooObj);
                    }



                }


            }
            else
            {
                //if this is the currently selected tile
                if (tag == "Selected")
                {
                    if (isMoving == false)
                    {
                        //This is used for deleting pathfinder tiles from other objects
                        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("PathFinder"))
                        {
                            Destroy(fooObj);
                        }

                        var path = FindObjectOfType<PathFinder>();
                        // path.SelectedCharacterClicked();
                    }
                }
            }
        }

        //if movement is current in progress 
       

    }


    public void CharacterAction()
    {
        //do the movement stuff here
        
        
        var pathFind = FindObjectOfType<PathFinder>();

        


            var tiles = FindObjectOfType<GetTiles>();
            tiles.tileInfo[pathFind.startPositionX, pathFind.startPositionY].PlayerCharacter = null;

            i = 0;
            nextTileX[i] = tiles.tileInfo[pathFind.finalPositionX, pathFind.finalPositionY].XRoot;
            nextTileY[i] = tiles.tileInfo[pathFind.finalPositionX, pathFind.finalPositionY].YRoot;
            //Debug.Log("final position x = " + finalPositionX + ". Final position y = " + finalPositionY);
            rootCheck = tiles.tileInfo[pathFind.finalPositionX, pathFind.finalPositionY].XRoot;
            //Debug.Log("At i = " + i + ".  x rootCheck = " + rootCheck + ",  and nextTile = " + nextTileX[i] + "," + nextTileY[i]);
            //Debug.Log("At i = " + i + " y root Check = " + pathFind.yRoot[finalPositionX, finalPositionY]);


            while (rootCheck != -1)
            {
                i++;
                nextTileX[i] = tiles.tileInfo[nextTileX[i - 1], nextTileY[i - 1]].XRoot;
                nextTileY[i] = tiles.tileInfo[nextTileX[i - 1], nextTileY[i - 1]].YRoot;
                //  Debug.Log("nextTileX[i] = " + nextTileX[i] + ". nextTileY[i] = " + nextTileY[i]);
                rootCheck = nextTileX[i];
                //    Debug.Log("At i = " + i + ".  rootCheck = " + rootCheck + ",  and nextTile = " + nextTileX[i] + "," + nextTileY[i]);

            }
            i--;
            i--;
            if (i == -1)
            {
                destination.x = pathFind.finalPositionX;
                destination.y = pathFind.finalPositionY;
                destination.z = -5f;
            }
            else
            {
                destination = new Vector3(nextTileX[i], nextTileY[i], -5f);

            }
            isMoving = true;

            foreach (GameObject highlight in GameObject.FindGameObjectsWithTag("SelectionHighlighter"))
            {
                Destroy(highlight);
            }


            var mover = GameObject.FindGameObjectWithTag("Selected");
            //pathFind.entityPresent[(int)mover.transform.position.x, (int)mover.transform.position.y] = 0;


            Debug.Log("Destination = " + destination.x + "," + destination.y);
        
    }


    
    // Update is called once per frame
    void Update()
    {
        
        if (isMoving == true)
            
        {
            
            //Debug.Log(r);
            r++;

            if (transform.position != destination)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, 4f * Time.deltaTime);
            }
            else
            {
                i--;
                if (i >= 0)
                {
                    destination.x = nextTileX[i];
                    destination.y = nextTileY[i];
                    
                }
                else if (i == -1)
                {
                    var path = FindObjectOfType<PathFinder>();
                    destination.x = path.finalPositionX;
                    destination.y = path.finalPositionY;
                    

                }
                else
                {
                    i = 0;
                    isMoving = false;
                    var path = FindObjectOfType<PathFinder>();
                    var tiles = FindObjectOfType<GetTiles>();
                    //path.entityPresent[(int)transform.position.x, (int)transform.position.y] = 1;
                    //This is used for deleting the pathfinder tiles - this needs to be moved
                    foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("PathFinder"))
                    {
                        Destroy(fooObj);
                    }

                    characterPosition = transform.position;
                    path.startPositionX = (int)characterPosition.x;
                    path.startPositionY = (int)characterPosition.y;
                    tiles.tileInfo[path.startPositionX, path.startPositionY].PlayerCharacter = gameObject.GetComponent<Character>();
                    

                    GameObject.Find("SelectedChar").transform.localScale = new Vector3(1, 1, 1);

                    CurrentSelectedChar = Instantiate(SelectedChar, characterPosition, transform.rotation);
                    CurrentSelectedChar.tag = "SelectionHighlighter";

                    GameObject.Find("SelectedChar").transform.localScale = new Vector3(0, 0, 0);

                    path.startPositionX = (int)gameObject.transform.position.x;
                    path.startPositionY = (int)gameObject.transform.position.y;
                    path.AttackSelector();
                }
            }
        }
       
    }
}
