using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AC_EndTurn : MonoBehaviour
{
    private AC_TowerStats towerStats;
    public GameObject[] go_Towers;
    public Button endTurnButton;
    public int turnCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnChange()
    {
        if (turnCounter == 0)
        {
            turnCounter = 1;
        }
        else
        {
            turnCounter = 0;
        }

        if (turnCounter == 0)
        {
            for (int i = 0; i < go_Towers.Length; i++)
            {
                towerStats = go_Towers[i].GetComponent<AC_TowerStats>();

                if (towerStats.isTraining)
                {
                    towerStats.Training();
                    Debug.Log(go_Towers[i] + " Training Turns Left " + towerStats.currentTrainingTurnsLeft);
                }
            }
        }
    }
}
