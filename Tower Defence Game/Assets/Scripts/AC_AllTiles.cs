﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AC_AllTiles : MonoBehaviour
{
    public GameObject allTiles;
    public GameObject[] allTileZones;
    public int tiles;
    public GameObject[] allTilesList;
    public int currentZone; 
    public GameObject[] currentZoneTiles;
    public int currentTile;
    public int currentNum;

    // Start is called before the first frame update
    void Start()
    {
        if (allTileZones.Length == 0)
        {
            allTileZones = new GameObject[gameObject.transform.childCount];
            for (int i = 0; i < allTileZones.Length; i++)
            {
                allTileZones[i] = gameObject.transform.GetChild(i).gameObject;
            }
        }

        if (allTilesList.Length == 0)
        {
            for (int i = 0; i < allTileZones.Length; i++)
            {
                tiles += allTileZones[i].transform.childCount;
            }

            allTilesList = new GameObject[tiles];

            for (int i = 0; i < allTileZones.Length; i++)
            {
                currentZone = allTileZones[i].transform.childCount;

                currentZoneTiles = new GameObject[currentZone];

                currentTile = 0;
               
                while (currentTile < currentZone)
                {                  
                    if (currentTile < allTiles.transform.GetChild(i).gameObject.transform.childCount)
                    {
                        currentZoneTiles[currentTile] = allTiles.transform.GetChild(i).gameObject.transform.GetChild(currentTile).gameObject;
                        currentTile += 1;
                    }
                }

                currentNum = currentTile;

                for (int j = 0; j < currentZoneTiles.Length; j++)
                {
                    for (int k = 0; k < allTilesList.Length; k++)
                    {
                        if (allTilesList[k] == null)
                        {
                            allTilesList[k] = currentZoneTiles[j];
                            break;
                        }
                    }
                }

                for (int j = 0; j < currentZoneTiles.Length; j++)
                {
                    currentZoneTiles[j] = null;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
