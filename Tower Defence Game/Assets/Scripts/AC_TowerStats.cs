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
    public GameObject[] storedUnits;
    public int spawnNumber;
    public int currentTurn;
    public int currentNum;

    // Start is called before the first frame update
    void Start()
    {
        endTurn = GameObject.Find("Button").GetComponent<AC_EndTurn>();
        currentNum = spawnNumber;
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
                        for (int j = 0; j < storedUnits.Length; j++)
                        {
                            if (storedUnits == null)
                            {
                                storedUnits[i] = towerUnits[j];
                                storedUnits[i].SetActive(false);
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

    public void SpawnUnit()
    {
        if (gameObject.layer != 9)
        {
            return;
        }

        Debug.Log("Spawning New Units");

        for (int i = 0; i < storedUnits.Length; i++)
        {
            if (storedUnits[i] == null && currentNum != 0)
            {
                //Debug.Log("Hi");
                storedUnits[i] = spawnUnit;
                currentNum -= 1;
            }
        }

        Debug.Log("Spawned");
        currentNum = spawnNumber;
    }
}
