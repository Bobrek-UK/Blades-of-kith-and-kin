//Author: Richard Crisp
//Organisation: N/A (Independent)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClicked : MonoBehaviour {

    [SerializeField]
    private int enemySpeed;
    [SerializeField]
    private int enemyRange;
    [SerializeField]
    private int enemyType; //infantry, cavalry, etc

    GameObject SelectedChar;
    GameObject CurrentSelectedChar;

    private void Awake()
    {
        SelectedChar = GameObject.Find("SelectedChar");
    }

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

        //at start of code
        var path = FindObjectOfType<PathFinder>();
        var tiles = FindObjectOfType<GetTiles>();

        if (path.playerUnitSelected == false)
        {
            if (path.npcUnitSelected == true)
            {
                path.npcUnitSelected = false;
                path.EntityResetter();
                path.PathFinderResetter();
            }
            else
            {
                path.npcUnitSelected = true;
            }
        }

        //clear enemy tags from any selected enemy
        foreach (GameObject selectedEnemies in GameObject.FindGameObjectsWithTag("EnemyClicked"))
        {
            selectedEnemies.tag = "UnSelected";
        }
        //tag this enemy as selected
        tag = "EnemyClicked";

        //Debug.Log("the detected position of the enemy is " + (int)transform.position.x + "," + (int)transform.position.y);

        if (tiles.tileInfo[(int)transform.position.x, (int)transform.position.y].MovesLeft == -400)
        {
            if (path.playerUnitSelected == true)
            {
                
              
                //if movement of a character is not currently in progress the run this code
                if (path.movementInProgress == false)
                {
                    //create a pointer to the character gameObject which is tagged as being selected by the player
                    //(this is the character that they want to move)
                    var mover = GameObject.FindGameObjectWithTag("Selected");

                    Debug.Log("Enemy locations set to = " + transform.position.x + "," + transform.position.y);
                    Debug.Log("enemy root x,y = " + tiles.tileInfo[(int)transform.position.x, (int)transform.position.y].XRoot
                              + "," + tiles.tileInfo[(int)transform.position.x, (int)transform.position.y].YRoot);
                    if (tiles.tileInfo[(int)transform.position.x, (int)transform.position.y].XRoot != -999)
                    {

                        //use this Vector3 position and feed it into the integer fields for final position 
                        //inside the pathFinderscript
                        path.finalPositionX = tiles.tileInfo[(int)gameObject.transform.position.x, (int)gameObject.transform.position.y].XRoot;
                        path.finalPositionY = tiles.tileInfo[(int)gameObject.transform.position.x, (int)gameObject.transform.position.y].YRoot;



                        

                        //find the SelectAlly script within this gameObject and create a pointer to it
                        var allyGo = mover.GetComponent<PlayerClicked>();

                        //movement is about to commence, so set that movement is in progress to prevent multiple movement
                        //inputs for the player before the current sequence is completed
                        path.movementInProgress = true;
                        //load the characterAction method inside the SelectAlly script in the tagged gameObject
                        allyGo.CharacterAction();

                        tiles.tileInfo[(int)gameObject.transform.position.x, (int)gameObject.transform.position.y].XRoot = -999;

                    }
                                        

                }
            }

            //start menu wait co-routine, so that the menu will not open until player unit
            //has finished moving
            StartCoroutine(MenuWait());
            

        }

        else
        {
            var pathFind = FindObjectOfType<PathFinder>();
            if (pathFind.movementInProgress == false)
            {
                path.playerUnitSelected = false;


                foreach (GameObject selObj in GameObject.FindGameObjectsWithTag("Selected"))
                {
                    selObj.tag = "UnSelected";
                }


                foreach (GameObject highlight in GameObject.FindGameObjectsWithTag("SelectionHighlighter"))
                {
                    Destroy(highlight);
                }

                //This is used for deleting the pathfinder tiles - this needs to be moved
                foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("PathFinder"))
                {
                    Destroy(fooObj);
                }


                //Debug.Log("character position for " + gameObject.name + " is " + (int)transform.position.x + "," + (int)transform.position.y);


                pathFind.startPositionX = (int)transform.position.x;
                pathFind.startPositionY = (int)transform.position.y;
                pathFind.characterSpeed = enemySpeed;
                pathFind.charRange = enemyRange;
                pathFind.unitType = enemyType; // is it infantry, cavalry, etc?

                GameObject.Find("SelectedChar").transform.localScale = new Vector3(1, 1, 1);

                CurrentSelectedChar = Instantiate(SelectedChar, transform.position, transform.rotation);
                CurrentSelectedChar.tag = "SelectionHighlighter";

                GameObject.Find("SelectedChar").transform.localScale = new Vector3(0, 0, 0);

                //Debug.Log(pathFind.playerUnitSelected);

                pathFind.PathFind();
            }

        }
    }

    public void attackMenu()
    {
        Vector3 tempLocation = gameObject.transform.position;
        

        var quickAttack = GameObject.Find("Canvas/QuickAttackButton");
        tempLocation.y = tempLocation.y - 1.2f;
        quickAttack.transform.position = tempLocation;
        quickAttack.transform.localScale = new Vector3(0.02f,0.035f,1);
        var balancedAttack = GameObject.Find("Canvas/BalancedAttackButton");
        tempLocation.y = tempLocation.y - 1f;
        balancedAttack.transform.localScale = new Vector3(0.02f, 0.035f, 1);
        balancedAttack.transform.position = tempLocation;
        var powerAttack = GameObject.Find("Canvas/PowerAttackButton");
        tempLocation.y = tempLocation.y - 1;
        powerAttack.transform.localScale = new Vector3(0.02f, 0.035f, 1);
        powerAttack.transform.position = tempLocation;


        //this is where the enemy has been attacked by the player   - CODE TO FOLLOW                
        foreach (GameObject selectedChar in GameObject.FindGameObjectsWithTag("SelectionHighlighter"))
        {
            Destroy(selectedChar);
        }

        var path = FindObjectOfType<PathFinder>();
        path.enemyClicked = this.gameObject;
        path.EntityResetter();
        path.PathFinderResetter();
        path.playerUnitSelected = false;
    }

    public void closeAttackMenu()
    {
        var quickAttack = GameObject.Find("Canvas/QuickAttackButton");
        quickAttack.transform.localScale = new Vector3(0, 1, 1);
        var balancedAttack = GameObject.Find("Canvas/BalancedAttackButton");
        balancedAttack.transform.localScale = new Vector3(0, 1, 1);
        var powerAttack = GameObject.Find("Canvas/PowerAttackButton");
        powerAttack.transform.localScale = new Vector3(0, 1, 1);
    }
    

    //MenuWait prevents a menu from opening until unit movement has completed
     IEnumerator MenuWait()
    {
        //  Debug.Log("I'm here");
        //Find the currently selected player unit's game object
        var mover = GameObject.FindGameObjectWithTag("Selected");
        //find the SelectAlly script within this gameObject and create a pointer to it
        var allyGo = mover.GetComponent<PlayerClicked>();

        //don't allow this code to continue until the player unit stops moving
        yield return new WaitUntil(() => allyGo.isMoving == false);

        //open the player attack menu
        attackMenu();
    }
}
