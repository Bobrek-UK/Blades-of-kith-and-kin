//Author: Richard Crisp
//Organisation: N/A (Independent)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    //create a serialized field for holding the enemy type (1 for ally, 2 for enemy)
    [SerializeField]
    protected int charType;
    //creates a serialized field for holding the maximum HP of a character 
    //(this will need to be changed once a levelling system is introduced)
    [SerializeField]
    private int maxHP;
    //create a public field for the current HP, as this will need to be changed by other functions
    public int currentHP;

    //create a serialized field for holding the maximum Mana of a player
    //(this will need to be changed once a levelling system is introduced)
    [SerializeField]
    private int maxMP;
    //create a public field for the current HP, as this will need to be changed by other functions
    public int currentMP;

    //create fields for the characters attack, defense etc values that can be changed in the inspector
    //(this will need to be changed once a levelling system is introduced)
    [SerializeField]
    public int attackValue;
    [SerializeField]
    public int defenseValue;
    [SerializeField]
    public int accuracyValue;
    [SerializeField]
    public int dodgeValue;
    [SerializeField]
    public int criticalHitChance;

   
    private float lerpSpeed = 2;
    [SerializeField]
    private float lerpHP;


      

    //when the program starts, load entity present values into the pathfinder array
    void Start() {

        lerpHP = currentHP;
        //first, create a pointer to the PathFinder script in memory
        var path = FindObjectOfType<PathFinder>();
        
        //if the char type has been set to 1, this is an ally        
        if (charType == 1)
        {
            var tiles = FindObjectOfType<GetTiles>();
            //so set the entitypresent value of this grid position to 1
            tiles.tileInfo[(int)transform.position.x, (int)transform.position.y].PlayerCharacter = this;
        }

        //if the char type has been set to 2, this is an enemy
        else if (charType == 2)
        {
            var tiles = FindObjectOfType<GetTiles>();
            //so set the entitypresent value of this grid position to 1
            tiles.tileInfo[(int)transform.position.x, (int)transform.position.y].EnemyCharacter = this;
        }
          
    }

  

    private void Update()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            
            if (currentHP != lerpHP)
            {
                lerpHP = Mathf.Lerp(lerpHP, currentHP, Time.deltaTime * lerpSpeed);
                gameObject.transform.GetChild(0).transform.GetChild(1).transform.localScale = new Vector3(lerpHP / maxHP, 1, 1);
            }
        }
    }


}

