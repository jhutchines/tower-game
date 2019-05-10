using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Camera_Follow : MonoBehaviour
{
    private GameObject go_camera;
    private Vector3 v3_lookAt;
    // Start is called before the first frame update
    void Start()
    {
        go_camera = Camera.main.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.rotation = go_camera.transform.rotation;
    }
}
