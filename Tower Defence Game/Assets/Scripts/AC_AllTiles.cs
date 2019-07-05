using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AC_AllTiles : MonoBehaviour
{
    public GameObject[] allTileZones;
    public GameObject[] allTilesList;
    public List<GameObject> currentZoneTiles;
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

        if (allTilesList.Length == 0)
        {
            for (int i = 0; i < allTileZones.Length; i++)
            {
                tiles += allTileZones[i].transform.childCount;
            }

            allTilesList = new GameObject[tiles];

            //for (int i = 0; i < allTileZones.Length; i++)
            //{
            //    for (int j = 0; j < allTileZones[i].transform.childCount; j++)
            //    {
            //        currentZoneTiles.Add(new GameObject[allTileZones[j].transform.childCount]);
            //    }
            //    currentZoneTiles.Add(allTileZones[i].transform.childCount);
            //    for (int j = 0; j < currentZoneTiles.Length; j++)
            //    {
            //        currentZoneTiles[j] = allTileZones[j].transform.GetChild(j).gameObject;
            //        currentZoneTiles[j].GetComponent<JH_Tile>().towerGrid = gameObject;
            //    }

            //    for (int j = 0; j < currentZoneTiles.Length; j++)
            //    {
            //        if (allTilesList[j] != null)
            //        {

            //        }
            //    }

            //    currentZoneTiles.
            //}

            //for (int i = 0; i < allTilesList.Length; i++)
            //{
            //    if (allTilesList[i] != null)
            //    {
            //        for (int j = 0; j < allTileZones[i].transform.childCount; j++)
            //        {
            //            currentZoneTiles = new GameObject[allTileZones[k].transform.childCount];
            //            for (int l = 0; l < currentZoneTiles.Length; l++)
            //            {
            //                currentZoneTiles[l] = allTileZones[l].transform.GetChild(l).gameObject;
            //                currentZoneTiles[l].GetComponent<JH_Tile>().towerGrid = gameObject;
            //            }
            //        }
            //        allTilesList[i] = allTileZones[j];
            //    }
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
