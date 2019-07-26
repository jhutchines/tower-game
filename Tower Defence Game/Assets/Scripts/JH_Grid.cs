using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Grid : MonoBehaviour
{
    public JH_Game_Manager.gridOwnership gridOwnership;
    public GameObject go_grid;
    private JH_Game_Manager gameManager;

    public GameObject[] tileList;

    public bool spawningUnit;

    public GameObject[] towerZones;
    private int tiles;
    public GameObject[] towerTilesList;
    private GameObject[] currentZoneTiles;
    private int currentTile;
    private int currentNum;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = Camera.main.GetComponent<JH_Game_Manager>();
        //if (tileList.Length == 0)
        //{
        //    tileList = new GameObject[go_grid.transform.childCount];
        //    for (int i = 0; i < tileList.Length; i++)
        //    {
        //        tileList[i] = go_grid.transform.GetChild(i).gameObject;
        //        tileList[i].GetComponent<JH_Tile>().towerGrid = gameObject;
        //    }
        //}

        if (towerTilesList.Length == 0)
        {
            for (int i = 0; i < towerZones.Length; i++)
            {
                tiles += towerZones[i].transform.childCount;
            }

            towerTilesList = new GameObject[tiles];

            for (int i = 0; i < towerZones.Length; i++)
            {
                currentZoneTiles = new GameObject[towerZones[i].transform.childCount];

                currentTile = 0;

                while (currentTile < towerZones[i].transform.childCount)
                {
                    if (currentTile < towerZones[i].gameObject.transform.childCount)
                    {
                        currentZoneTiles[currentTile] = towerZones[i].gameObject.transform.GetChild(currentTile).gameObject;
                        currentTile += 1;
                    }
                }

                currentNum = currentTile;

                for (int j = 0; j < currentZoneTiles.Length; j++)
                {
                    for (int k = 0; k < towerTilesList.Length; k++)
                    {
                        if (towerTilesList[k] == null)
                        {
                            towerTilesList[k] = currentZoneTiles[j];
                            towerTilesList[k].GetComponent<JH_Tile>().towerGrid = gameObject;
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
        if (gameManager.thisTurn == JH_Game_Manager.currentTurn.Player) PlayerTurn();
    }

    void PlayerTurn()
    {

    }
}
