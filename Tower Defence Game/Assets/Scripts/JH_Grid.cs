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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = Camera.main.GetComponent<JH_Game_Manager>();
        if (tileList.Length == 0)
        {
            tileList = new GameObject[go_grid.transform.childCount];
            for (int i = 0; i < tileList.Length; i++)
            {
                tileList[i] = go_grid.transform.GetChild(i).gameObject;
                tileList[i].GetComponent<JH_Tile>().towerGrid = gameObject;
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
