using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AC_AllTiles : MonoBehaviour
{
    public GameObject[] allTileZones;
    public GameObject[] allTilesList;
    public List<GameObject> currentZoneTiles;
    public GameObject allTiles;
    public int currentTile;
    public int tiles;

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

        //if (allTilesList.Length == 0)
        //{
        //    for (int i = 0; i < allTileZones.Length; i++)
        //    {
        //        tiles += allTileZones[i].transform.childCount;
        //    }

        //    allTilesList = new GameObject[tiles];

        //    for (int i = 0; i < allTileZones.Length; i++)
        //    {
        //        currentTile = 0;

        //        if (allTiles.transform.GetChild(i).gameObject.transform.GetChild(currentTile).gameObject != null)
        //        {
        //            currentZoneTiles.Add(allTiles.transform.GetChild(i).gameObject.transform.GetChild(currentTile).gameObject);
        //            currentTile += 1;
        //        }

        //        //allTilesList[j].GetComponent<JH_Tile>().towerGrid = gameObject;

        //        for (int j = 0; j < currentZoneTiles.Capacity; j++)
        //        {
        //            for (int k = 0; k < allTilesList.Length; k++)
        //            {
        //                if (allTilesList[k] == null)
        //                {
        //                    allTilesList[k] = currentZoneTiles[j];
        //                }
        //                else
        //                {
        //                    k += 1;
        //                }
        //            }
        //        }

        //        currentZoneTiles.Clear();               
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
