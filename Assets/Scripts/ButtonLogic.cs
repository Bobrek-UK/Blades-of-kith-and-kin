using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    public void balancedAttack()
    {
        doDamage(0, 1);
    }

    public void quickAttack()
        {
          doDamage(2, 0.8f);
        }

    public void powerAttack()
    {
        doDamage(-2, 1.2f);
    }

    private void doDamage(int accuracyModifier, float damageModifier)
    { 

        //make pointer to pathfinder
        var path = FindObjectOfType<PathFinder>();

        //find currently selected enemy from pathfinder
        GameObject enemyClicked = path.enemyClicked;
        //find currently selected player
        GameObject playerSelected = path.playerSelected;

        //get character script from selected enemy
        Character enemy = enemyClicked.GetComponent<Character>();
        //get character script from selected ally
        Character player = playerSelected.GetComponent<Character>();

        //ends the attack and closes the menu
        EnemyClicked enemyClickedScript = enemyClicked.GetComponent<EnemyClicked>();
        enemyClickedScript.closeAttackMenu();

        int hitChanceChecker = player.accuracyValue + accuracyModifier - enemy.dodgeValue;
        int hitChance = CalculateHitChance(hitChanceChecker);

        
        

        System.Random rand = new System.Random();
        int attackRoll1 = rand.Next(1, 6);
        int attackRoll2 = rand.Next(1, 6);
        int defendRoll1 = rand.Next(1, 6);
        int defendRoll2 = rand.Next(1, 6);
        int damageDealt = 0;
        string damageNote = "";

        
        //double six on attack roll always results in hit and critical strike
        if (attackRoll1 + attackRoll2==12)
        {
            //crit does 1.5 times damage
            damageDealt = System.Convert.ToInt32(player.attackValue * 1.5 * damageModifier);
            enemy.currentHP = enemy.currentHP - damageDealt;
            damageNote = (damageDealt.ToString()+ "!");
        }
        //for any other dice roll check that the attack landed 
        else if (player.accuracyValue + accuracyModifier + attackRoll1 + attackRoll2 >= enemy.dodgeValue + defendRoll1 + defendRoll2)
        {
            //check for critical strike (10% chance for balanced attack)
            if (rand.Next(0, 100) <= player.criticalHitChance)
            {
                
                damageDealt = System.Convert.ToInt32(player.attackValue*1.5 * damageModifier);
                enemy.currentHP = enemy.currentHP - damageDealt;
                damageNote = (damageDealt.ToString() + "!");
            }
            else
            {
                
                damageDealt = System.Convert.ToInt32(player.attackValue * damageModifier);
                enemy.currentHP = enemy.currentHP - damageDealt;
                damageNote = damageDealt.ToString();
            }
        }
        else
        {
            damageDealt = 0;
            damageNote = "Miss";
        }

        var popText = FindObjectOfType<PopText>();
        Vector3 something = enemyClicked.transform.position;
        popText.animatorOn(damageNote, something);

        StartCoroutine(damageWait());

     

    }

    

    private int CalculateHitChance(int chanceIn)
    {
        int hitChanceChecker = chanceIn;
        int hitChance = 0;
        if (hitChanceChecker >= 10)
        {
            hitChance = 100;
        }
        else if (hitChanceChecker == 9)
        {
            hitChance = 99;
        }
        else if (hitChanceChecker == 8)
        {
            hitChance = 99;
        }
        else if (hitChanceChecker == 7)
        {
            hitChance = 99;
        }
        else if (hitChanceChecker == 6)
        {
            hitChance = 97;
        }
        else if (hitChanceChecker == 5)
        {
            hitChance = 95;
        }
        else if (hitChanceChecker == 4)
        {
            hitChance = 90;
        }
        else if (hitChanceChecker == 3)
        {
            hitChance = 84;
        }
        else if (hitChanceChecker == 2)
        {
            hitChance = 76;
        }
        else if (hitChanceChecker == 1)
        {
            hitChance = 66;
        }
        else if (hitChanceChecker == 0)
        {
            hitChance = 55;
        }
        else if (hitChanceChecker == -1)
        {
            hitChance = 44;
        }
        else if (hitChanceChecker == -2)
        {
            hitChance = 34;
        }
        else if (hitChanceChecker == -3)
        {
            hitChance = 24;
        }
        else if (hitChanceChecker == -4)
        {
            hitChance = 16;
        }
        else if (hitChanceChecker == -5)
        {
            hitChance = 10;
        }
        else if (hitChanceChecker == -6)
        {
            hitChance = 5;
        }
        else if (hitChanceChecker <= -7)
        {
            hitChance = 3;
        }
       
        
        return hitChance;
    }

    IEnumerator damageWait()
    {
        yield return new WaitForSeconds(0.9f);

        var popText = FindObjectOfType<PopText>();
        popText.animatorOff();
    }
}
