using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AC_TowerStats : MonoBehaviour
{
    private JH_Game_Manager.unitOwnership unitOwnership;

    public int towerHealth;
    public GameObject[] buildTowers;
    public Material playerTowerMat;
    public Material enemyTowerMat;
    public GameObject towerFlag;
    public GameObject[] towerUnits;
    private AC_EndTurn endTurn;
    public GameObject spawnUnit;
    //public GameObject[] storedUnit;
    public int currentTurn;
    public int currentNum;
    public GameObject opposingTower;

    // Unit Setting and Training.
    public int storedUnits;
    public int newUnits;
    public int peasantNum;
    public int soldierNum;
    public int currentUnit;
    public bool isTraining;
    public GameObject trainingInput;
    public int trainingNum;
    public int currentTrainingUnit;
    public int trainingTurns;
    public int currentTrainingTurnsLeft;
    public GameObject setUnit;
    public GameObject peasantUnit;
    public GameObject soldierUnit;

    //Spawn Locations.
    //private AC_UnitZoneSetting unitZoneSetting;
    private JH_Tile tiles;
    public GameObject towerTiles;
    private GameObject[] currentZoneTiles;
    private int currentTile;
    private int currentTileNum;
    public GameObject[] settingTiles;
    //public GameObject[] firstTurnTiles;
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
        //unitZoneSetting = GameObject.Find("UnitZones").GetComponent<AC_UnitZoneSetting>();
        currentNum = newUnits;

        // Set the spawnable locations to an array.
        if (settingTiles.Length == 0)
        {
            settingTiles = new GameObject[towerTiles.transform.childCount];

            for (int i = 0; i < settingTiles.Length; i++)
            {
                currentZoneTiles = new GameObject[towerTiles.transform.childCount];

                currentTile = 0;

                while (currentTile < towerTiles.transform.childCount)
                {
                    if (currentTile < towerTiles.transform.childCount)
                    {
                        currentZoneTiles[currentTile] = towerTiles.transform.GetChild(currentTile).gameObject;
                        currentTile += 1;
                    }
                }

                currentNum = currentTile;

                for (int j = 0; j < currentZoneTiles.Length; j++)
                {
                    for (int k = 0; k < settingTiles.Length; k++)
                    {
                        if (settingTiles[k] == null)
                        {
                            settingTiles[k] = currentZoneTiles[j];
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

        if (towerHealth <= 0 && gameObject.layer == 10)
        {
            Debug.Log(gameObject + " destroyed!");
            KillUnits();
            towerFlag.GetComponent<Renderer>().material = playerTowerMat;
            gameObject.layer = 9;
            SpawnTowers();
        }
        else if (towerHealth <= 0 && gameObject.layer == 10)
        {
            Debug.Log(gameObject + " destroyed!");
            KillUnits();
            towerFlag.GetComponent<Renderer>().material = enemyTowerMat;
            gameObject.layer = 10;
            SpawnTowers();
        }
    }

    void KillUnits()
    {
        if (gameObject.layer == 9)
        {
            for (int i = 0; i < towerUnits.Length; i++)
            {
                if (towerUnits[i].GetComponent<JH_Unit>().unitOwnership == JH_Game_Manager.unitOwnership.Enemy)
                {
                    if (towerUnits[i].GetComponent<JH_UnitAttack>().unitType == JH_Game_Manager.unitType.Peasant)
                    {
                        opposingTower.GetComponent<AC_TowerStats>().peasantNum += 1;
                        Destroy(towerUnits[i]);
                    }
                    else if (towerUnits[i].GetComponent<JH_UnitAttack>().unitType == JH_Game_Manager.unitType.Swordsman)
                    {
                        opposingTower.GetComponent<AC_TowerStats>().soldierNum += 1;
                        Destroy(towerUnits[i]);
                    }
                }

                if (towerUnits[i].GetComponent<JH_Unit>().unitOwnership == JH_Game_Manager.unitOwnership.Player)
                {
                    Destroy(towerUnits[i]);
                }
            }
        }

        if (gameObject.layer == 10)
        {
            for (int i = 0; i < towerUnits.Length; i++)
            {
                if (towerUnits[i] != null)
                {
                    if (towerUnits[i].GetComponent<JH_Unit>().unitOwnership == JH_Game_Manager.unitOwnership.Player)
                    {
                        if (towerUnits[i].GetComponent<JH_UnitAttack>().unitType == JH_Game_Manager.unitType.Peasant)
                        {
                            opposingTower.GetComponent<AC_TowerStats>().peasantNum += 1;
                            Destroy(towerUnits[i]);
                        }
                        else if (towerUnits[i].GetComponent<JH_UnitAttack>().unitType == JH_Game_Manager.unitType.Swordsman)
                        {
                            opposingTower.GetComponent<AC_TowerStats>().soldierNum += 1;
                            Destroy(towerUnits[i]);
                        }
                    }

                    if (towerUnits[i].GetComponent<JH_Unit>().unitOwnership == JH_Game_Manager.unitOwnership.Enemy)
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
        if (currentTurn == 0)
        {
            if (peasantNum > 0)
            {
                Debug.Log("Peasant Spawning");
                setUnit = peasantUnit;
                currentUnit = 1;
                SettablePositions();
            }
        }
    }

    public void SetSoldier()
    {
        if (currentTurn == 0)
        {
            if (soldierNum > 0)
            {
                Debug.Log("Soldier Spawning");
                setUnit = soldierUnit;
                currentUnit = 2;
                SettablePositions();
            }
        }
    }

    public void TrainOne()
    {

        if (currentTurn == 0)
        {
            if (peasantNum > 0 && !isTraining)
            {
                Debug.Log("Training One Unit");
                // Change for number selected.
                trainingNum = 1;
                TrainPeasant();
            }
            else
            {
                return;
            }
        }
    }

    public void TrainTwo()
    {
        if (currentTurn == 0)
        {
            if (peasantNum > 1 && !isTraining)
            {
                Debug.Log("Training Two Unit");
                // Change for number selected.
                trainingNum = 2;
                TrainPeasant();
            }
            else
            {
                return;
            }
        }
    }
    public void TrainThree()
    {
        if (currentTurn == 0)
        {
            if (peasantNum > 2 && !isTraining)
            {
                // Change for number selected.
                trainingNum = 3;
                TrainPeasant();
            }
            else
            {
                return;
            }
        }
    }
    public void TrainFour()
    {
        if (currentTurn == 0)
        {
            if (peasantNum > 3 && !isTraining)
            {
                // Change for number selected.
                trainingNum = 4;
                TrainPeasant();
            }
            else
            {
                return;
            }
        }
    }
    public void TrainFive()
    {
        if (currentTurn == 0)
        {
            if (peasantNum > 4 && !isTraining)
            {
                // Change for number selected.
                trainingNum = 5;
                TrainPeasant();
            }
            else
            {
                return;
            }
        }
    }
    public void TrainSix()
    {
        if (currentTurn == 0)
        {
            if (peasantNum > 5 && !isTraining)
            {
                // Change for number selected.
                trainingNum = 6;
                TrainPeasant();
            }
            else
            {
                return;
            }
        }
    }

    public void TrainPeasant()
    {
        Debug.Log("Training Peasants");
        // Reduce peasant number.
        peasantNum -= trainingNum;
        currentTrainingTurnsLeft = trainingTurns;
        isTraining = true;
    }

    public void Training()
    {       
        if (currentTrainingTurnsLeft == 1)
        {
            Debug.Log("Trained");            
            soldierNum += trainingNum;
            isTraining = false;          
        }
        else
        {
            Debug.Log("Training");
            currentTrainingTurnsLeft -= 1;
        }
    }

    //public void FirstTurnTiles()
    //{
    //    Debug.Log("Turning off tiles");

    //    for (int i = 0; i < firstTurnTiles.Length; i++)
    //    {
    //        if (firstTurnTiles[i].GetComponent<JH_Tile>().initialBattleMove)
    //        {
    //            firstTurnTiles[i].layer = 0;
    //        }
    //    }
    //}

    public void SettablePositions()
    {
        if (endTurn.firstTurn)
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
        else
        {
            Debug.Log("Highlighting Tiles");

            gameObject.GetComponent<JH_Grid>().spawningUnit = true;

            // Turns the availble tiles green.
            for (int i = 0; i < settingTiles.Length; i++)
            {
                if (settingTiles[i].GetComponent<JH_Tile>().initialBattleMove == false)
                {
                    Debug.Log("Tile Turning");
                    settingTiles[i].GetComponent<JH_Tile>().Spawning();
                }
            }

            //
            unitSetting = true;
        }
    }

    public void SpawnUnit()
    {
        unitSetting = false;

        // Instantiate chosen unit at location.
        GameObject newUnit = Instantiate(setUnit, new Vector3(setPosition.x, setPosition.y + 0.5f, setPosition.z), Quaternion.identity);
        newUnit.GetComponent<JH_Unit>().parentTower = gameObject;

        // Reduce the number of units in tower.
        if (currentUnit == 1)
        {
            peasantNum -= 1;
        }
        else if (currentUnit == 2)
        {
            soldierNum -= 1;
        }
    }

}
