//Author: Richard Crisp
//Organisation: N/A (Independent)

using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class GetTiles : MonoBehaviour
{

    //creates a 2D array of tileInfo objects for checking tile details when pathfinding
    //or during combat actions
    public TileSquare[,] tileInfo = null;


    public void Awake()
    {
       //create a memory pointer to the TileMap
       Tilemap tilemap = GetComponent<Tilemap>();

        //creates a memory pointer to the cellBounds component of tilemap
        BoundsInt bounds = tilemap.cellBounds;
        //creates an array of pointers to each of the tilemap blocks
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        tileInfo = new TileSquare[bounds.size.x, bounds.size.y];
        
        //loop through each position for x
        for (int x = 0; x < bounds.size.x; x++)
        {
            
            for (int y = 0; y < bounds.size.y; y++)
            {
                //find the tile at grid position x,y (this is a 1D array so 
                //there is a conversion process)
                TileBase tile = allTiles[x + y * bounds.size.x];
                

                //if a tile isn't null then movement costs can be set
                if (tile != null)
                {
                    tileInfo[x, y] = new TileSquare();
                    //if the tile is grassland or plain set move costs as follows
                    if (tile.name.StartsWith("Grass"))
                    {
                       
                        //infantry movement cost
                        tileInfo[x, y].InfCost = 1;
                        //cavalry movement cost
                        tileInfo[x, y].CavCost = 1;
                        //flying unit movement cost
                        tileInfo[x, y].FlyCost = 1;
                        //amphibious unit movement cost
                        tileInfo[x, y].AmphCost = 1;
                        //serpent unit movement cost
                        tileInfo[x, y].SerpCost = 1;
                    }
                    //if the tile is a dwelling, set move costs as follows
                    else if (tile.name.StartsWith("House"))
                    {
                        //infantry movement cost
                        tileInfo[x, y].InfCost = 1;
                        //cavalry movement cost
                        tileInfo[x, y].CavCost = 1;
                        //flying unit movement cost
                        tileInfo[x, y].FlyCost = 1;
                        //amphibious unit movement cost
                        tileInfo[x, y].AmphCost = 1;
                        //serpent unit movement cost
                        tileInfo[x, y].SerpCost = 1;
                      
                    }
                    //if the tile is a single tree/small copse, set move costs as follows
                    else if (tile.name.StartsWith("Tree"))
                    {
                        //infantry movement cost
                        tileInfo[x, y].InfCost = 1;
                        //cavalry movement cost
                        tileInfo[x, y].CavCost = 2;
                        //flying unit movement cost
                        tileInfo[x, y].FlyCost = 1;
                        //amphibious unit movement cost
                        tileInfo[x, y].AmphCost = 1;
                        //serpent unit movement cost
                        tileInfo[x, y].SerpCost = 2;

                     
                    }
                    //if the tile is a dense forest/jungle, set move costs as follows
                    else if (tile.name.StartsWith("forest"))
                    {
                        //infantry movement cost
                        tileInfo[x, y].InfCost = 1;
                        //cavalry movement cost
                        tileInfo[x, y].CavCost = 3;
                        //flying unit movement cost
                        tileInfo[x, y].FlyCost = 1;
                        //amphibious unit movement cost
                        tileInfo[x, y].AmphCost = 1;
                        //serpent unit movement cost
                        tileInfo[x, y].SerpCost = 2;
                    }
                    //if the tile is part of the sea/ocean, set move costs as follows
                    else if (tile.name.StartsWith("Sea"))
                    {
                        //infantry movement cost
                        tileInfo[x, y].InfCost = 3;
                        //cavalry movement cost
                        tileInfo[x, y].CavCost = 3;
                        //flying unit movement cost
                        tileInfo[x, y].FlyCost = 1;
                        //amphibious unit movement cost
                        tileInfo[x, y].AmphCost = 1;
                        //serpent unit movement cost
                        tileInfo[x, y].SerpCost = 3;
                    }
                    //if the tile is part of the beach, set the move cost as follows
                    else if (tile.name.StartsWith("Beach"))
                    {
                        //infantry movement cost
                        tileInfo[x, y].InfCost = 2;
                        //cavalry movement cost
                        tileInfo[x, y].CavCost = 2;
                        //flying unit movement cost
                        tileInfo[x, y].FlyCost = 1;
                        //amphibious unit movement cost
                        tileInfo[x, y].AmphCost = 1;
                        //serpent unit movement cost
                        tileInfo[x, y].SerpCost = 1;
                    }
                    //if the tile is part of the desert, set the move cost as follows
                    else if (tile.name.StartsWith("Desert"))
                    {
                        //infantry movement cost
                        tileInfo[x, y].InfCost = 2;
                        //cavalry movement cost
                        tileInfo[x, y].CavCost = 2;
                        //flying unit movement cost
                        tileInfo[x, y].FlyCost = 1;
                        //amphibious unit movement cost
                        tileInfo[x, y].AmphCost = 1;
                        //serpent unit movement cost
                        tileInfo[x, y].SerpCost = 1;

                    }
                    //if the tile is part of a mountain, set the move cost as follows
                    else if (tile.name.StartsWith("Mountain"))
                    {
                        //infantry movement cost
                        tileInfo[x, y].InfCost = 3;
                        //cavalry movement cost
                        tileInfo[x, y].CavCost = 4;
                        //flying unit movement cost
                        tileInfo[x, y].FlyCost = 1;
                        //amphibious unit movement cost
                        tileInfo[x, y].AmphCost = 4;
                        //serpent unit movement cost
                        tileInfo[x, y].SerpCost = 2;
                        
                    }
                    //if the tile is part of a shallow river, set the move cost as follows
                    else if (tile.name.StartsWith("ShallowRiver"))
                    {
                        //infantry movement cost
                        tileInfo[x, y].InfCost = 2;
                        //cavalry movement cost
                        tileInfo[x, y].CavCost = 1;
                        //flying unit movement cost
                        tileInfo[x, y].FlyCost = 1;
                        //amphibious unit movement cost
                        tileInfo[x, y].AmphCost = 1;
                        //serpent unit movement cost
                        tileInfo[x, y].SerpCost = 2;
                    }
                    //if the tile is part of a deep river, set the move cost as follows
                    else if (tile.name.StartsWith("DeepRiver"))
                    {
                        //infantry movement cost
                        tileInfo[x, y].InfCost = 3;
                        //cavalry movement cost
                        tileInfo[x, y].CavCost = 3;
                        //flying unit movement cost
                        tileInfo[x, y].FlyCost = 1;
                        //amphibious unit movement cost
                        tileInfo[x, y].AmphCost = 1;
                        //serpent unit movement cost
                        tileInfo[x, y].SerpCost = 3;
                    }
                    //if valid tile cannot be found but the tile is not null, something has been named incorrectly
                    //set movement to 900 to make impassable and for location in debug.
                    else
                    {
                        //infantry movement cost
                        tileInfo[x, y].InfCost = 900;
                        //cavalry movement cost
                        tileInfo[x, y].CavCost = 900;
                        //flying unit movement cost
                        tileInfo[x, y].FlyCost = 900;
                        //amphibious unit movement cost
                        tileInfo[x, y].AmphCost = 900;
                        //serpent unit movement cost
                        tileInfo[x, y].SerpCost = 900;
                        Debug.Log("x:" + x + " y:" + y + " tile has been incorrectly named");
                    }
                }




                //if a tile is null (empty), then
                else
                {
                    //can be used for debugging null tiles
                     Debug.Log("x:" + x + " y:" + y + " tile: (null)");

                }
                
            }
        }
           

    }


}
    