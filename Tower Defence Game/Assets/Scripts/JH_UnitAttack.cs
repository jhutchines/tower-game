using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_UnitAttack : MonoBehaviour
{
    public int in_damage;

    // -- Unit Type enum
    public JH_Game_Manager.unitType unitType;
    // -- Unit to take damage
    public GameObject damagedEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // -- Attack function

    void AttackExample()
    {
        if (unitType == JH_Game_Manager.unitType.Archer)
        {
            in_damage = 2;
        }

        damagedEnemy.GetComponent<JH_Unit>().in_health = in_damage;


    }
}
