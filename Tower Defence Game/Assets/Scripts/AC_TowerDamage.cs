using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AC_TowerDamage : MonoBehaviour
{
    // 
    public LayerMask enemyLayerMask;
    // The main camera that acst as the player's eyes.
    public Camera mainCamera;
    // Ray that fires from the camera to the map when they click.
    public Ray mouseRay;
    // Holds were the player has clicked on the map.
    public RaycastHit objectHit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Checks to see if the left mouse button has been clicked and calls function if it has.
        if (Input.GetMouseButtonDown(1))
        {
            // Calls left click function that deals with unit slection.
            Debug.Log("Left Click");
            RightMouseClick();
        }  
    }

    public void RightMouseClick()
    {
        // Ray assigned to where the on the screen the mouse is clicked.
        mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Checks to see if ray intersected with any objects on the player layer.
        if (Physics.Raycast(mouseRay, out objectHit, Mathf.Infinity, enemyLayerMask))
        {
            // Draws a yellow line from the camera to the clicked location when a player obejct is hit.
            Debug.DrawRay(mouseRay.origin, mouseRay.direction * objectHit.distance, Color.yellow);
            Debug.Log("Left Did Hit");
            // Does damage to hit object.
            objectHit.collider.gameObject.GetComponent<AC_TowerStats>().towerHealth -= 1;
            objectHit.collider.gameObject.GetComponent<AC_TowerStats>().TowerDeath();
        }
        else
        {
            // Draws a white line from the camera to the clicked location when a player obejct is not hit.
            Debug.DrawRay(mouseRay.origin, mouseRay.direction * 1000, Color.white);
            Debug.Log("Left Did not Hit");
        }
    }
}
