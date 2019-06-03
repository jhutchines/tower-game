using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Unit : MonoBehaviour
{
    public GameObject parentTower;
    public int onCurrentTile;
    public float fl_moveSpeed;
    public float fl_climbAmount;
    public float fl_range;
    public int in_movement;
    public int in_health;

    private JH_Game_Manager gameManager;
    private bool bl_moving;
    private bool bl_climbing;
    private Vector3 v3_moveTowards;
    private Vector3 v3_climbTowards;
    public JH_Game_Manager.unitOwnership unitOwnership;
    private int in_startingMovement;
    private GameObject go_changePlaces;

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
                parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied = gameObject;
                return;
            }
        }
        in_startingMovement = in_movement;
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
                parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied = gameObject;
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
                    parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileY <= fl_climbAmount / 2)
                {
                    if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied != null)
                    {
                        if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied.GetComponent<JH_Unit>().unitOwnership
                        == JH_Game_Manager.unitOwnership.Enemy)
                        {
                            parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color = gameManager.m_canAttack.color;
                        }
                        else
                        {
                            if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied.GetComponent<JH_Unit>().in_movement
                                > 0 && in_movement > 0)
                            {
                                parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color = gameManager.m_checkMove.color;
                            }
                        }
                    }
                    else
                    {
                        if (in_movement > 0)
                        {
                            parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color = gameManager.m_checkMove.color;
                        }
                        else
                        {
                            if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color != gameManager.m_cannotMove.color)
                            {
                                parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color =
                                parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().c_startColor;
                            }
                        }
                    }
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
            if (fl_range > 1)
            {
                if ((parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x <= 
                    parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x + fl_range &&
                        parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x >= 
                        parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x - fl_range) &&
                        (parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z <= 
                        parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z + fl_range &&
                        parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z >= 
                        parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z - fl_range))
                {
                    
                    if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied != null)
                    {
                        if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied.GetComponent<JH_Unit>().unitOwnership ==
                        JH_Game_Manager.unitOwnership.Enemy)
                        {
                            parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color = gameManager.m_canAttack.color;
                        }
                    }
                    else
                    {
                        if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color == gameManager.m_canAttack.color)
                        {
                            parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color =
                            parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().c_startColor;
                        }
                    }
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
                        if (hit.transform.GetComponent<JH_Tile>().tileOccupied != null)
                        {
                            go_changePlaces = hit.transform.GetComponent<JH_Tile>().tileOccupied;
                            go_changePlaces.GetComponent<JH_Unit>().v3_moveTowards =
                            new Vector3(parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x,
                                        go_changePlaces.transform.position.y,
                                        parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z);
                        }
                        parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].GetComponent<JH_Tile>().tileOccupied = null;
                        for (int i = 0; i < parentTower.GetComponent<JH_Grid>().tileList.Length; i++)
                        {
                            if (hit.transform.gameObject == parentTower.GetComponent<JH_Grid>().tileList[i])
                            {
                                onCurrentTile = i;
                                parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied = gameObject;
                                break;
                            }
                        }
                        in_movement--;

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
                        else
                        {
                            bl_moving = true;
                            if (go_changePlaces != null)
                            {
                                go_changePlaces.GetComponent<JH_Unit>().in_movement--;
                                go_changePlaces.GetComponent<JH_Unit>().bl_moving = true;
                            }
                        }

                        go_changePlaces = null;
                        v3_moveTowards.y = transform.position.y;
                    }
                }
            }
        }
    }
}
 