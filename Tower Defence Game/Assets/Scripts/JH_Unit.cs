using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Unit : MonoBehaviour
{
    public GameObject parentTower;
    public int onCurrentTile;
    public float fl_moveSpeed;
    public float fl_climbAmount;
    public int in_movement;
    public int in_health;

    private JH_Game_Manager gameManager;
    private Vector3 v3_moveTowards;
    private Vector3 v3_climbTowards;
    public JH_Game_Manager.unitOwnership unitOwnership;
    private int in_startingMovement;
    private GameObject go_changePlaces;
    private Vector3 v3_childPosition;
    private bool bl_deathSequence;
    private Vector3 v3_deathMove;

    [HideInInspector] public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        gameManager = Camera.main.GetComponent<JH_Game_Manager>();

        // Finds the tile this unit is on when the game starts
        for (int i = 0; i < parentTower.GetComponent<JH_Grid>().tileList.Length; i++)
        {
            if (transform.position.x == parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x &&
                transform.position.z == parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z)
            {
                onCurrentTile = i;
                parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied = gameObject;
                break;
            }
        }
        in_startingMovement = in_movement;

        v3_childPosition = new Vector3(0, -0.9f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Checks surrounded tiles of the selected unit
        if (gameManager.selectedUnit == gameObject)
        {
            CheckTiles();
            UnitSelected();
        }
        
        // Moves towards the clicked location
        if (animator.GetBool("isWalking"))
        {
            transform.position = Vector3.MoveTowards(transform.position, v3_moveTowards, fl_moveSpeed);
            if (transform.position == v3_moveTowards) animator.SetBool("isWalking", false);
        }

        // Places the character back into the center of the tile if they get displaced
        if (transform.GetChild(0).transform.localPosition.x >= 1 ||
            transform.GetChild(0).transform.localPosition.x <= -1 ||
            transform.GetChild(0).transform.localPosition.z >= 1 ||
            transform.GetChild(0).transform.localPosition.z <= -1)
        {
            transform.GetChild(0).transform.localPosition = v3_childPosition;
        }

        // Climbs up higher tiles
        if (animator.GetBool("isClimbing"))
        {
            ClimbingFinished();
        }

        // Destroys the unit
        if (bl_deathSequence) UnitDeath();
    }


    void CheckTiles()
    {
        // Checks to see what tile the unit is currently on
        for (int i = 0; i < parentTower.GetComponent<JH_Grid>().tileList.Length; i++)
        {
            if (transform.position.x == parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x &&
                transform.position.z == parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z)
            {
                onCurrentTile = i;
                parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied = gameObject;
            }

            // Checks surrounding tiles
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
                        // Sets tile to red if occupied by an enemy unit
                        if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied.GetComponent<JH_Unit>().unitOwnership
                        == JH_Game_Manager.unitOwnership.Enemy)
                        {
                            parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color = gameManager.m_canAttack.color;
                        }
                        // Allows friendly units to swap places if they have enough moves left
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
                        // Sets tile to green if unit can move there
                        if (in_movement > 0)
                        {
                            parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color = gameManager.m_checkMove.color;
                        }
                        // Removes tile colour if not able to attack or move there
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
                // Sets tile to original colour if not next to unit
                if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color !=
                    gameManager.m_cannotMove.color)
                {
                    parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color =
                        parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().c_startColor;
                }
            }

            // Checks to see if an enemy is within range, and sets tile to red if so
            if (GetComponent<JH_UnitAttack>().fl_range > 1)
            {
                if ((parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x <= 
                    parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x + GetComponent<JH_UnitAttack>().fl_range &&
                        parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.x >= 
                        parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x - GetComponent<JH_UnitAttack>().fl_range) &&
                        (parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z <= 
                        parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z + GetComponent<JH_UnitAttack>().fl_range &&
                        parentTower.GetComponent<JH_Grid>().tileList[i].transform.position.z >= 
                        parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z - GetComponent<JH_UnitAttack>().fl_range))
                {
                    
                    if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied != null)
                    {
                        if (parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<JH_Tile>().tileOccupied.GetComponent<JH_Unit>().unitOwnership ==
                        JH_Game_Manager.unitOwnership.Enemy)
                        {
                            parentTower.GetComponent<JH_Grid>().tileList[i].GetComponent<Renderer>().material.color = gameManager.m_canAttack.color;
                        }
                    }

                    // Sets tile back to original colour
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
        // Creates raycast on left click to see what the player is clicking on
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.transform.GetComponent<JH_Tile>() != null)
                {
                    // Checks to make sure the unit is not already moving
                    if (hit.transform.GetComponent<Renderer>().material.color == gameManager.m_canMove.color && !animator.GetBool("isWalking"))
                    {
                        // Swaps position with a friendly unit
                        if (hit.transform.GetComponent<JH_Tile>().tileOccupied != null)
                        {
                            go_changePlaces = hit.transform.GetComponent<JH_Tile>().tileOccupied;
                            go_changePlaces.GetComponent<JH_Unit>().v3_moveTowards =
                            new Vector3(parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.x,
                                        go_changePlaces.transform.position.y,
                                        parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].transform.position.z);
                        }

                        // Sets current tile as unoccupied and sets new tile to occupied
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

                        // Sets position for unit to move to
                        v3_moveTowards = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);

                        // Checks if the unit requires to climb to reach the new tile
                        if (!animator.GetBool("isClimbing"))
                        {
                            if (v3_moveTowards.y > transform.position.y - 0.5f)
                            {
                                v3_climbTowards = new Vector3(v3_moveTowards.x, v3_moveTowards.y + 0.5f, v3_moveTowards.z);
                                animator.SetBool("isClimbing", true);
                                Debug.Log("Climbing");
                            }
                            else if (v3_moveTowards.y + 0.5f < transform.position.y)
                            {
                                v3_climbTowards = new Vector3(v3_moveTowards.x, v3_moveTowards.y + 0.5f, v3_moveTowards.z);
                                animator.SetBool("isClimbing", true);
                                Debug.Log("Climbing");
                            }
                            else
                            {
                                animator.SetBool("isWalking", true);
                                if (go_changePlaces != null)
                                {
                                    go_changePlaces.GetComponent<JH_Unit>().in_movement--;
                                    go_changePlaces.GetComponent<JH_Unit>().animator.SetBool("isWalking", true);
                                }
                            }
                        }

                        go_changePlaces = null;
                        v3_moveTowards.y = transform.position.y;
                    }

                    // Allows the unit to attack an enemy
                    if (hit.transform.GetComponent<Renderer>().material.color == gameManager.m_canAttack.color)
                    {
                        GetComponent<JH_UnitAttack>().damagedEnemy = hit.transform.GetComponent<JH_Tile>().tileOccupied;
                        GetComponent<JH_UnitAttack>().AttackTarget();
                    }
                }
            }
        }
    }

    // Unit takes damage if falling down too high
    public void ClimbingFinished()
    {
        if (transform.position.y > v3_climbTowards.y + 0.5f) in_health--;
        transform.position = v3_climbTowards;
        animator.SetBool("isClimbing", false);
        Debug.Log("Finished Climbing");

        if (in_health <= 0)
        {
            animator.Play("Death");
            StartCoroutine(DeathSequence());
        }
    }


    public void StartDeathSequence()
    {
        StartCoroutine(DeathSequence());
    }

    // Unit stays on the map for a time after dying
    IEnumerator DeathSequence()
    {
        parentTower.GetComponent<JH_Grid>().tileList[onCurrentTile].GetComponent<JH_Tile>().tileOccupied = null;
        yield return new WaitForSeconds(5);
        v3_deathMove = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
        bl_deathSequence = true;
    }

    // Makes the dead unit move under the tile before removing
    void UnitDeath()
    {
        transform.position = Vector3.MoveTowards(transform.position, v3_deathMove, 0.05f);
        if (transform.position == v3_deathMove) Destroy(gameObject);
    }
}
 