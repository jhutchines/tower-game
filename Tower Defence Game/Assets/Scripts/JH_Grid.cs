using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Grid : MonoBehaviour
{
    public JH_Game_Manager.gridOwnership gridOwnership;
    private JH_Game_Manager gameManager;

    public GameObject[] tileList;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = Camera.main.GetComponent<JH_Game_Manager>();
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
