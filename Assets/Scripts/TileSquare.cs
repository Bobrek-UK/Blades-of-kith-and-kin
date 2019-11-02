using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSquare
{
    private int cavCost;
    private int infCost;
    private int flyCost;
    private int amphCost;
    private int serpCost;
    private Character playerCharacter = null;
    private Character allyCharacter = null;
    private Character enemyCharacter = null;
    private int movesLeft;
    private int xRoot;
    private int yRoot;
    private int state;
    private bool allyPresent = false;

 

    public int CavCost
    {
        get
        {
            return cavCost;
        }

        set
        {
            cavCost = value;
        }
    }

    public int InfCost
    {
        get
        {
            return infCost;
        }

        set
        {
            infCost = value;
        }
    }

    public int FlyCost
    {
        get
        {
            return flyCost;
        }

        set
        {
            flyCost = value;
        }
    }

    public int AmphCost
    {
        get
        {
            return amphCost;
        }

        set
        {
            amphCost = value;
        }
    }

    public int SerpCost
    {
        get
        {
            return serpCost;
        }

        set
        {
            serpCost = value;
        }
    }

    public Character PlayerCharacter
    {
        get
        {
            return playerCharacter;
        }

        set
        {
            playerCharacter = value;
        }
    }

    public Character AllyCharacter
    {
        get
        {
            return allyCharacter;
        }

        set
        {
            allyCharacter = value;
        }
    }

    public Character EnemyCharacter
    {
        get
        {
            return enemyCharacter;
        }

        set
        {
            enemyCharacter = value;
        }
    }

    public int MovesLeft
    {
        get
        {
            return movesLeft;
        }

        set
        {
            movesLeft = value;
        }
    }

    public int XRoot
    {
        get
        {
            return xRoot;
        }

        set
        {
            xRoot = value;
        }
    }

    public int YRoot
    {
        get
        {
            return yRoot;
        }

        set
        {
            yRoot = value;
        }
    }

    public int State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
        }
    }

    public bool AllyPresent
    {
        get
        {
            return allyPresent;
        }

        set
        {
            allyPresent = value;
        }
    }
}
