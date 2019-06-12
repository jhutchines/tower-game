using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Tile : MonoBehaviour
{
    public bool initialBattleMove;

    public int tileX;
    public int tileZ;

    public float tileY;

    public GameObject tileOccupied;

    public Color c_startColor;
    private JH_Game_Manager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        tileX = Mathf.RoundToInt(transform.position.x);
        tileZ = Mathf.RoundToInt(transform.position.z);
        tileY = transform.position.y;
        c_startColor = GetComponent<Renderer>().material.color;
        gameManager = Camera.main.GetComponent<JH_Game_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.selectedUnit == null)
        {
            GetComponent<Renderer>().material.color = c_startColor;
        }
    }

    void CheckMove()
    {

    }

    // Changes colour of the tile depending on if the unit can move there
    private void OnMouseEnter()
    {
        if (gameManager.selectedUnit != null)
        {
            if (GetComponent<Renderer>().material.color == gameManager.m_checkMove.color)
            {
                GetComponent<Renderer>().material.color = gameManager.m_canMove.color;
            }
            else
            {
                GetComponent<Renderer>().material.color = gameManager.m_cannotMove.color;
            }
        }
    }

    // Returns tile colour to the correct colour when the mouse is no longer over it
    private void OnMouseExit()
    {
        if (GetComponent<Renderer>().material.color != gameManager.m_canMove.color)
        {
            GetComponent<Renderer>().material.color = c_startColor;
        }
        else GetComponent<Renderer>().material.color = gameManager.m_checkMove.color;
    }
}
