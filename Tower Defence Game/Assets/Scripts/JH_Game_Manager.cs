using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Game_Manager : MonoBehaviour
{
    public enum gridOwnership
    {
        None,
        Player,
        Enemy
    }

    public enum currentTurn
    {
        None,
        Player,
        Enemy
    }

    public currentTurn thisTurn;

    public bool inBattle;
    public GameObject selectedUnit;

    public Material m_canMove;
    public Material m_cannotMove;
    public Material m_checkMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
