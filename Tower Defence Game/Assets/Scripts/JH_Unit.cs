using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Unit : MonoBehaviour
{
    public GameObject parentTower;
    public int onCurrentTile;
    public float fl_moveSpeed;
    private JH_Game_Manager gameManager;
    public bool bl_moving;
    private Vector3 v3_moveTowards;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = Camera.main.GetComponent<JH_Game_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.selectedUnit == gameObject)
        {
            CheckTiles();
            UnitSelected();
        }
        if (bl_moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, v3_moveTowards, fl_moveSpeed);
            if (transform.position == v3_moveTowards) bl_moving = false;
        }
    }

    void CheckTiles()
    {
        for (int i = 0; i < parentTower.GetComponent<JH_Grid>().tileList.Length; i++)
        {
            if (transform.position.x == parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x &&
                transform.position.z == parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z)
            {
                onCurrentTile = i;
            }

            if ((parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x + 1 == 
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x &&
                parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z) ||
                (parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x - 1 ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x &&
                parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z) || 
                (parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z + 1 ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z &&
                parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x) ||
                (parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z - 1 ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z &&
                parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x) ||
                (parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z - 1 ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z &&
                parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x - 1 ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x) ||
                (parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z - 1 ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z &&
                parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x + 1 ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x) ||
                (parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z + 1 ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z &&
                parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x - 1 ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x) ||
                (parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z + 1 ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z &&
                parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x + 1 ==
                parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x))
            {
                if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color != gameManager.m_canMove.color)
                {
                    parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color = gameManager.m_checkMove.color;
                }
            }
            else
            {
                if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color !=
                    gameManager.m_cannotMove.color)
                {
                    parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color =
                        parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().c_startColor;
                }
            }
        }
    }

    void UnitSelected()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.transform.GetComponent<JH_Tile>() != null)
                {
                    if (hit.transform.GetComponent<Renderer>().material.color == gameManager.m_canMove.color && !bl_moving)
                    {
                        v3_moveTowards = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                        bl_moving = true;
                    }
                }
            }
        }
    }
}
 