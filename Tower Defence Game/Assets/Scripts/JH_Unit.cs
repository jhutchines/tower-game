using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Unit : MonoBehaviour
{
    private AC_AllTiles allTiles;
    public GameObject[] towerUnits;
    public GameObject occupiedTile;
    public GameObject occupiedGrid;
    public GameObject occupiedTower;
    public GameObject currentTower;

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
        allTiles = GameObject.Find("AllTiles").GetComponent<AC_AllTiles>();

        animator = transform.GetChild(0).GetComponent<Animator>();
        gameManager = Camera.main.GetComponent<JH_Game_Manager>();

        // Finds the tile this unit is on when the game starts
        for (int i = 0; i < allTiles.allTilesList.Length; i++)
        {
            if (transform.position.x == allTiles.allTilesList[i].transform.position.x &&
                transform.position.z == allTiles.allTilesList[i].transform.position.z)
            {
                onCurrentTile = i;
                allTiles.allTilesList[i].GetComponent<JH_Tile>().tileOccupied = gameObject;
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
        for (int i = 0; i < allTiles.allTilesList.Length; i++)
        {
            if (transform.position.x == allTiles.allTilesList[i].transform.position.x &&
                transform.position.z == allTiles.allTilesList[i].transform.position.z)
            {
                onCurrentTile = i;
                allTiles.allTilesList[i].GetComponent<JH_Tile>().tileOccupied = gameObject;

                occupiedTile = allTiles.allTilesList[i];
                // Checks to see what tiles parent is and add the unit to the towers unit array.
                ParentTowerUpdate();
            }               

            // Checks surrounding tiles
            if ((allTiles.allTilesList[onCurrentTile].transform.position.x + 1 == 
                allTiles.allTilesList[i].transform.position.x &&
                allTiles.allTilesList[onCurrentTile].transform.position.z ==
                allTiles.allTilesList[i].transform.position.z) ||
                (allTiles.allTilesList[onCurrentTile].transform.position.x - 1 ==
                allTiles.allTilesList[i].transform.position.x &&
                allTiles.allTilesList[onCurrentTile].transform.position.z ==
                allTiles.allTilesList[i].transform.position.z) || 
                (allTiles.allTilesList[onCurrentTile].transform.position.z + 1 ==
                allTiles.allTilesList[i].transform.position.z &&
                allTiles.allTilesList[onCurrentTile].transform.position.x ==
                allTiles.allTilesList[i].transform.position.x) ||
                (allTiles.allTilesList[onCurrentTile].transform.position.z - 1 ==
                allTiles.allTilesList[i].transform.position.z &&
                allTiles.allTilesList[onCurrentTile].transform.position.x ==
                allTiles.allTilesList[i].transform.position.x) ||
                (allTiles.allTilesList[onCurrentTile].transform.position.z - 1 ==
                allTiles.allTilesList[i].transform.position.z &&
                allTiles.allTilesList[onCurrentTile].transform.position.x - 1 ==
                allTiles.allTilesList[i].transform.position.x) ||
                (allTiles.allTilesList[onCurrentTile].transform.position.z - 1 ==
                allTiles.allTilesList[i].transform.position.z &&
                allTiles.allTilesList[onCurrentTile].transform.position.x + 1 ==
                allTiles.allTilesList[i].transform.position.x) ||
                (allTiles.allTilesList[onCurrentTile].transform.position.z + 1 ==
                allTiles.allTilesList[i].transform.position.z &&
                allTiles.allTilesList[onCurrentTile].transform.position.x - 1 ==
                allTiles.allTilesList[i].transform.position.x) ||
                (allTiles.allTilesList[onCurrentTile].transform.position.z + 1 ==
                allTiles.allTilesList[i].transform.position.z &&
                allTiles.allTilesList[onCurrentTile].transform.position.x + 1 ==
                allTiles.allTilesList[i].transform.position.x))
            {

                if (allTiles.allTilesList[i].GetComponent<Renderer>().material.color != gameManager.m_canMove.color &&
                    allTiles.allTilesList[i].GetComponent<JH_Tile>().tileY <= fl_climbAmount / 2)
                {
                    if (allTiles.allTilesList[i].GetComponent<JH_Tile>().tileOccupied != null)
                    {
                        // Sets tile to red if occupied by an enemy unit
                        if (allTiles.allTilesList[i].GetComponent<JH_Tile>().tileOccupied.GetComponent<JH_Unit>().unitOwnership
                        == JH_Game_Manager.unitOwnership.Enemy)
                        {
                            allTiles.allTilesList[i].GetComponent<Renderer>().material.color = gameManager.m_canAttack.color;
                        }
                        // Allows friendly units to swap places if they have enough moves left
                        else
                        {
                            if (allTiles.allTilesList[i].GetComponent<JH_Tile>().tileOccupied.GetComponent<JH_Unit>().in_movement
                                > 0 && in_movement > 0)
                            {
                                allTiles.allTilesList[i].GetComponent<Renderer>().material.color = gameManager.m_checkMove.color;
                            }
                        }
                        Debug.Log("Tile " + allTiles.allTilesList[i].gameObject + " occupied by " 
                                  + allTiles.allTilesList[i].GetComponent<JH_Tile>().tileOccupied);
                    }
                    else
                    {
                        // Sets tile to green if unit can move there
                        if (in_movement > 0)
                        {
                            allTiles.allTilesList[i].GetComponent<Renderer>().material.color = gameManager.m_checkMove.color;
                        }
                        // Removes tile colour if not able to attack or move there
                        else
                        {
                            if (allTiles.allTilesList[i].GetComponent<Renderer>().material.color != gameManager.m_cannotMove.color)
                            {
                                allTiles.allTilesList[i].GetComponent<Renderer>().material.color =
                                allTiles.allTilesList[i].GetComponent<JH_Tile>().c_startColor;
                            }
                        }
                    }
                }
            }
            else
            {
                // Sets tile to original colour if not next to unit
                if (allTiles.allTilesList[i].GetComponent<Renderer>().material.color !=
                    gameManager.m_cannotMove.color)
                {
                    allTiles.allTilesList[i].GetComponent<Renderer>().material.color =
                        allTiles.allTilesList[i].GetComponent<JH_Tile>().c_startColor;
                }
            }

            // Checks to see if an enemy is within range, and sets tile to red if so
            if (GetComponent<JH_UnitAttack>().fl_range > 1)
            {
                if ((allTiles.allTilesList[i].transform.position.x <= 
                    allTiles.allTilesList[onCurrentTile].transform.position.x + GetComponent<JH_UnitAttack>().fl_range &&
                        allTiles.allTilesList[i].transform.position.x >= 
                        allTiles.allTilesList[onCurrentTile].transform.position.x - GetComponent<JH_UnitAttack>().fl_range) &&
                        (allTiles.allTilesList[i].transform.position.z <= 
                        allTiles.allTilesList[onCurrentTile].transform.position.z + GetComponent<JH_UnitAttack>().fl_range &&
                        allTiles.allTilesList[i].transform.position.z >= 
                        allTiles.allTilesList[onCurrentTile].transform.position.z - GetComponent<JH_UnitAttack>().fl_range))
                {
                    
                    if (allTiles.allTilesList[i].GetComponent<JH_Tile>().tileOccupied != null)
                    {
                        if (allTiles.allTilesList[i].GetComponent<JH_Tile>().tileOccupied.GetComponent<JH_Unit>().unitOwnership ==
                        JH_Game_Manager.unitOwnership.Enemy)
                        {
                            allTiles.allTilesList[i].GetComponent<Renderer>().material.color = gameManager.m_canAttack.color;
                        }
                    }

                    // Sets tile back to original colour
                    else
                    {
                        if (allTiles.allTilesList[i].GetComponent<Renderer>().material.color == gameManager.m_canAttack.color)
                        {
                            allTiles.allTilesList[i].GetComponent<Renderer>().material.color =
                            allTiles.allTilesList[i].GetComponent<JH_Tile>().c_startColor;
                        }
                    }
                }
            }
        }
    }

    public void ParentTowerUpdate()
    {
        Debug.Log("Updating " + gameObject + "'s Parent.");

        occupiedGrid = occupiedTile.GetComponent<JH_Tile>().towerGrid.GetComponent<JH_Grid>().go_grid;

        if (occupiedTower != null)
        {          
            occupiedTower = occupiedTile.GetComponent<JH_Tile>().towerGrid;

            if (occupiedGrid != currentTower.GetComponent<JH_Grid>().go_grid)
            {
                Debug.Log(gameObject + " in new tile zone.");

                towerUnits = occupiedTower.GetComponent<AC_TowerStats>().towerUnits;

                // Finds the unit in the tower array and removes it.
                for (int i = 0; i < towerUnits.Length; i++)
                {
                    if (towerUnits[i] == gameObject)
                    {
                        Debug.Log(gameObject + " removed from " + occupiedTower);
                        towerUnits[i] = null;
                        currentTower = occupiedTower;
                        break;
                    }
                }

                towerUnits = occupiedTower.GetComponent<AC_TowerStats>().towerUnits;

                // Sees if the unit is already in the tower array.
                for (int i = 0; i < towerUnits.Length; i++)
                {
                    if (towerUnits[i] == gameObject)
                    {
                        Debug.Log(gameObject + " already in tower.");
                        currentTower = occupiedTower;
                        break;
                    }
                }

                // Finds a space in the tower array for the unit and places it into the array.
                for (int i = 0; i < towerUnits.Length; i++)
                {
                    Debug.Log(gameObject + " added to " + occupiedTower);

                    if (towerUnits[i] == null)
                    {
                        towerUnits[i] = gameObject;
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.Log(gameObject + "'s First Parenting");

            occupiedTower = occupiedTile.GetComponent<JH_Tile>().towerGrid;

            towerUnits = occupiedTower.GetComponent<AC_TowerStats>().towerUnits;

            // Sees if the unit is already in the tower array.
            for (int i = 0; i < towerUnits.Length; i++)
            {
                if (towerUnits[i] == gameObject)
                {
                    currentTower = occupiedTower;
                    return;
                }
            }

            // Finds a space in the tower array for the unit and places it into the array.
            for (int i = 0; i < towerUnits.Length; i++)
            {
                if (towerUnits[i] == null)
                {
                    Debug.Log(gameObject + " added to " + occupiedTower);
                    towerUnits[i] = gameObject;
                    break;
                }
            }
        }

        currentTower = occupiedTower;
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
                            new Vector3(allTiles.allTilesList[onCurrentTile].transform.position.x,
                                        go_changePlaces.transform.position.y,
                                        allTiles.allTilesList[onCurrentTile].transform.position.z);
                        }

                        // Sets current tile as unoccupied and sets new tile to occupied
                        allTiles.allTilesList[onCurrentTile].GetComponent<JH_Tile>().tileOccupied = null;
                        for (int i = 0; i < allTiles.allTilesList.Length; i++)
                        {
                            if (hit.transform.gameObject == allTiles.allTilesList[i])
                            {
                                onCurrentTile = i;
                                allTiles.allTilesList[i].GetComponent<JH_Tile>().tileOccupied = gameObject;
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
        allTiles.allTilesList[onCurrentTile].GetComponent<JH_Tile>().tileOccupied = null;
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
 