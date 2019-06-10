using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_UnitAttack : MonoBehaviour
{
    public int in_damage;

    // Unit Type enum
    public JH_Game_Manager.unitType unitType;
    // Unit to take damage
    public GameObject damagedEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Attack function

    public void AttackTarget()
    {
        if (unitType == JH_Game_Manager.unitType.Archer)
        {
            in_damage = 2;
        }


        // Animation goes here

        GetComponent<JH_Unit>().animator.Play("ATK");

        damagedEnemy.GetComponent<JH_Unit>().in_health -= in_damage;
        if (damagedEnemy.GetComponent<JH_Unit>().in_health <= 0)
        {
            damagedEnemy.GetComponent<JH_Unit>().StartDeathSequence();
        }


    }
}
