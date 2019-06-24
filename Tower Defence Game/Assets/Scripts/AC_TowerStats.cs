using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AC_TowerStats : MonoBehaviour
{
    public int towerHealth;
    public GameObject[] buildTowers;
    public Material playerTowerMat;
    public Material playerZoneMat;
    public GameObject[] towerUnits;
    private AC_EndTurn endTurn;
    public GameObject spawnUnit;
    public GameObject[] storedUnit;
    public int currentTurn;
    public int currentNum;

    // Unit Setting and Training.
    public int storedUnits;
    public int newUnits;
    public int peasantNum;
    public int soldierNum;
    public int trainingNum;
    public GameObject setUnit;
    public GameObject peasantUnit;
    public GameObject soldierUnit;

    //Spawn Locations.
    public GameObject[] settingTiles;
    public GameObject chosenTile;
    public bool unitSetting;
    public Vector3 setPosition;
    public Camera mainCamera;
    public Ray mouseRay;
    public RaycastHit objectHit;
    public LayerMask spawnPositionLayer;


    // Start is called before the first frame update
    void Start()
    {
        endTurn = GameObject.Find("Button").GetComponent<AC_EndTurn>();
        currentNum = newUnits;
    }

    // Update is called once per frame
    void Update()
    {
        // Changes player turn between two players, 0 is player 1 and 1 is player 2. This can be slightly edited for more then two player if nessarsary.
        if (endTurn.turnCounter != currentTurn)
        {
            currentTurn = endTurn.turnCounter;

            if (currentTurn == 0)
            {
                CreateNewUnits();
            }
        }

        if (unitSetting == true && Input.GetMouseButtonDown(0))
        {
            ChooseTile();
            if (chosenTile != null)
            {
            // Take the selected tile and make setPosition the selcted tiles location.
            setPosition = chosenTile.transform.position;
            // Call spawn unit function.
            SpawnUnit();
            }
        }

    }

    public void TowerDeath()
    {
        if (towerHealth <= 0)
        {
            KillUnits();
            gameObject.GetComponent<Renderer>().material = playerTowerMat;
            gameObject.layer = 9;
            //transform.Find("Zone").GetComponent<Renderer>().material = playerZoneMat;
            //transform.Find("Zone").gameObject.layer = 11;
            SpawnTowers();
        }
    }

    void KillUnits()
    {
        if (gameObject.layer == 10)
        {
            for (int i = 0; i < towerUnits.Length; i++)
            {
                if (towerUnits[i] != null)
                {
                    if (towerUnits[i].layer == 13)
                    {
                        for (int j = 0; j < storedUnit.Length; j++)
                        {
                            if (storedUnit == null)
                            {
                                storedUnit[i] = towerUnits[j];
                                storedUnit[i].SetActive(false);
                            }
                        }                   
                    }

                    if (towerUnits[i].layer == 14)
                    {
                        Destroy(towerUnits[i]);
                    }
                }
            }
        }
    }

    void SpawnTowers()
    {
        for (int i = 0; i < buildTowers.Length; i++)
        {
            buildTowers[i].SetActive(true);
        }
    }

    public void CreateNewUnits()
    {
        if (gameObject.layer != 9)
        {
            return;
        }

        Debug.Log("Spawning New Units");
        peasantNum += newUnits;

        //for (int i = 0; i < storedUnit.Length; i++)
        //{
        //    if (storedUnit[i] == null && currentNum != 0)
        //    {
        //        //Debug.Log("Hi");
        //        storedUnit[i] = spawnUnit;
        //        currentNum -= 1;
        //    }
        //}
        //currentNum = newUnits;
        //Debug.Log("Spawned");

    }

    public void ChooseTile()
    {
        mainCamera = Camera.main;

        // Ray assigned to where the on the screen the mouse is clicked.
        mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Checks to see if ray intersected with any objects on the player layer.
        if (Physics.Raycast(mouseRay, out objectHit, Mathf.Infinity, spawnPositionLayer))
        {
            // Draws a yellow line from the camera to the clicked location when a player obejct is hit.
            Debug.DrawRay(mouseRay.origin, mouseRay.direction * objectHit.distance, Color.yellow);
            Debug.Log("Left Did Hit");
            // Does damage to hit object.
            chosenTile = objectHit.transform.gameObject;
        }
        else
        {
            // Draws a white line from the camera to the clicked location when a player obejct is not hit.
            Debug.DrawRay(mouseRay.origin, mouseRay.direction * 1000, Color.white);
            Debug.Log("Left Did not Hit");
        }
    }

    public void SetPeasant()
    {
        if (peasantNum > 0)
        {
            Debug.Log("Peasant Spawning");
            setUnit = peasantUnit;
            SettablePositions();
        }
    }

    public void SetSoldier()
    {
        if (soldierNum > 0)
        {
            Debug.Log("Soldier Spawning");
            setUnit = soldierUnit;
            SettablePositions();
        }
    }
    public void SettablePositions()
    {
        Debug.Log("Highlighting Tiles");

        gameObject.GetComponent<JH_Grid>().spawningUnit = true;

        // Turns the availble tiles green.
        for (int i = 0; i < settingTiles.Length; i++)
        {
            Debug.Log("Tile Turning");
            settingTiles[i].GetComponent<JH_Tile>().Spawning();
        }

        //
        unitSetting = true;
    }

    public void SpawnUnit()
    {
        unitSetting = false;

        // Instantiate chosen unit at location.
        GameObject newUnit = Instantiate(setUnit, new Vector3(setPosition.x, setPosition.y + 0.5f, setPosition.z), Quaternion.identity);
        newUnit.GetComponent<JH_Unit>().parentTower = gameObject;

        // Reduce the number of units in tower.
    }

}
