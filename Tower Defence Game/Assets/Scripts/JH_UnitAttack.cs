using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_UnitAttack : MonoBehaviour
{
    public int in_damage;
    public float fl_range;

    // Unit Type enum
    public JH_Game_Manager.unitType unitType;
    // Unit to take damage
    public GameObject damagedEnemy;

    // Start is called before the first frame update
    void Start()
    {
        UnitSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Sets up the amount of damage to be done by a unit
    void UnitSetup()
    {
        switch(unitType)
        {
            case JH_Game_Manager.unitType.Peasant:
                {
                    in_damage = 1;
                    fl_range = 1;
                }
                break;

            case JH_Game_Manager.unitType.Swordsman:
                {
                    in_damage = 2;
                    fl_range = 1;
                }
                break;

            case JH_Game_Manager.unitType.Shieldsman:
                {
                    in_damage = 1;
                    fl_range = 1;
                }
                break;

            case JH_Game_Manager.unitType.Archer:
                {
                    in_damage = 2;
                    fl_range = 2;
                }
                break;
        }
    }

    // Plays animation and deals damage to enemy
    public void AttackTarget()
    { 
       
        GetComponent<JH_Unit>().animator.Play("ATK");

        damagedEnemy.GetComponent<JH_Unit>().in_health -= in_damage;
        if (damagedEnemy.GetComponent<JH_Unit>().in_health <= 0)
        {
            damagedEnemy.GetComponent<JH_Unit>().StartDeathSequence();
        }


    }
}
