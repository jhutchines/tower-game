using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AC_EndTurn : MonoBehaviour
{
    public Button endTurnButton;
    public int turnCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnChange()
    {
        if (turnCounter == 0)
        {
            turnCounter = 1;
        }
        else
        {
            turnCounter = 0;
        }
    }
}
