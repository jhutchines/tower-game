using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_Update_UI : MonoBehaviour
{
    public Text towerName;
    public Text towerHealth;
    public Text towerPeasants;
    private GameObject go_camera;

    // Start is called before the first frame update
    void Start()
    {
        go_camera = Camera.main.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        towerName.text = go_camera.GetComponent<JH_Camera_Controls>().go_moveTowards.transform.parent.parent.GetComponent<JH_Tower_Stats>().towerName;
        towerHealth.text = "Health: " + go_camera.GetComponent<JH_Camera_Controls>().go_moveTowards.transform.parent.parent.GetComponent<JH_Tower_Stats>().towerHealth;
        towerPeasants.text = "Number of peasants: " + go_camera.GetComponent<JH_Camera_Controls>().go_moveTowards.transform.parent.parent.GetComponent<JH_Tower_Stats>().towerPeasants;
    }
}
