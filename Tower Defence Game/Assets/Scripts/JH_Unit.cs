using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Unit : MonoBehaviour
{
    public GameObject parentTower;
    public int onCurrentTile;
    public float fl_moveSpeed;
    public float fl_climbAmount;
    private JH_Game_Manager gameManager;
    private bool bl_moving;
    private bool bl_climbing;
    private Vector3 v3_moveTowards;
    private Vector3 v3_climbTowards;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = Camera.main.GetComponent<JH_Game_Manager>();
        for (int i = 0; i < parentTower.GetComponent<JH_Grid>().tileList.Length; i++)
        {
            if (transform.position.x == parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x &&
                transform.position.z == parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z)
            {
                onCurrentTile = i;
                parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied = true;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.selectedUnit == gameObject)
        {
            CheckTiles();
            UnitSelected();
        }
        if (bl_climbing)
        {
            
            // Once anim is finished, appear at right position

            transform.position = v3_climbTowards;
            bl_climbing = false;
            Debug.Log("Finished Climbing");
            
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
                parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied = true;
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
                if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color != gameManager.m_canMove.color &&
                    !parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied &&
                    parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileY <= fl_climbAmount / 2)
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
                        parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].GetComponent<JH_Tile>().tileOccupied = false;
                        for (int i = 0; i < parentTower.GetComponent<JH_Grid>().tileList.Length; i++)
                        {
                            if (hit.transform.gameObject == parentTower.GetComponent<JH_Grid>().tileList[i])
                            {
                                onCurrentTile = i;
                                parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied = true;
                                break;
                            }
                        }

                        v3_moveTowards = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
                        if (v3_moveTowards.y > transform.position.y - 0.5f)
                        {
                            v3_climbTowards = new Vector3(v3_moveTowards.x, v3_moveTowards.y + 0.5f, v3_moveTowards.z);
                            bl_climbing = true;
                            Debug.Log("Climbing");
                        }
                        else if (v3_moveTowards.y + 0.5f < transform.position.y)
                        {
                            v3_climbTowards = new Vector3(v3_moveTowards.x, v3_moveTowards.y + 0.5f, v3_moveTowards.z);
                            bl_climbing = true;
                            Debug.Log("Climbing");
                        }
                        else bl_moving = true;

                        v3_moveTowards.y = transform.position.y;
                    }
                }
            }
        }
    }
}
 