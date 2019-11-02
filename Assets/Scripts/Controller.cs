using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Character attackedCharacter;
    private bool enemyInRange = false;
    private bool allyCharacterSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAttackedCharacter(Character charIn)
    {
        attackedCharacter = charIn;
    }

    public Character GetAttackedCharacter()
    {
        return attackedCharacter;
    }

    public void SetEnemyInRange(bool isEnemyInRange)
    {
        enemyInRange = isEnemyInRange;
    }

    public bool GetEnemyInRange()
    {
        return enemyInRange;
    }

    public void SetAllyCharacterSelected(bool allySelected)
    {
        allyCharacterSelected = allySelected;
    }

    public bool SetAllyCharacterSelected()
    {
        return allyCharacterSelected;
    }
}

