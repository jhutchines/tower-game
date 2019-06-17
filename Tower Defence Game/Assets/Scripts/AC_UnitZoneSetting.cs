using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AC_UnitZoneSetting : MonoBehaviour
{
    private AC_TowerStats towerStats;
    public bool firstTurn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject + " Entered");

        if (gameObject.layer == 11)
        {
            if (other.gameObject.layer == 13)
            {
                towerStats = gameObject.GetComponentInParent<AC_TowerStats>();

                for (int i = 0; i < towerStats.towerUnits.Length; i++)
                {
                    if (towerStats.towerUnits[i] == null)
                    {
                        towerStats.towerUnits[i] = other.gameObject;
                        return;
                    }
                }
            }

            if (other.gameObject.layer == 14)
            {

            }
        }

        if (gameObject.layer == 12)
        {
            if (other.gameObject.layer == 13)
            {
                if (firstTurn == true)
                {
                    // Cant spawn here.
                }
            }

            if (other.gameObject.layer == 14)
            {
                towerStats = gameObject.GetComponentInParent<AC_TowerStats>();

                for (int i = 0; i < towerStats.towerUnits.Length; i++)
                {
                    if (towerStats.towerUnits[i] == null)
                    {
                        towerStats.towerUnits[i] = other.gameObject;
                        return;
                    }
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (gameObject.layer == 11)
        {
            if (other.gameObject.layer == 13)
            {
                towerStats = gameObject.GetComponentInParent<AC_TowerStats>();

                for (int i = 0; i < towerStats.towerUnits.Length; i++)
                {
                    if (towerStats.towerUnits[i] == other.gameObject)
                    {
                        towerStats.towerUnits[i] = null;
                        return;
                    }
                }
            }

            if (other.gameObject.layer == 14)
            {

            }
        }

        if (gameObject.layer == 12)
        {
            if (other.gameObject.layer == 13)
            {

            }

            if (other.gameObject.layer == 14)
            {
                towerStats = gameObject.GetComponentInParent<AC_TowerStats>();

                for (int i = 0; i < towerStats.towerUnits.Length; i++)
                {
                    if (towerStats.towerUnits[i] == other.gameObject)
                    {
                        towerStats.towerUnits[i] = null;
                        return;
                    }
                }
            }
        }
    }
}
