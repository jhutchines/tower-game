using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Camera_Controls : MonoBehaviour
{
    private GameObject go_camera;
    private GameObject go_cameraParent;
    public float fl_doubleClickTime;

    [Header("Camera Speed")]
    public float cameraSpeed;
    public float cameraRotation;
    public float cameraZoomSpeed;

    [Header("Zooming")]
    public float maxZoom;
    public float minZoom;
    private float cameraZoom;

    private bool bl_countTime;
    public float fl_countTime;
    private bool bl_moveTowards;
    private GameObject go_moveTowards;
    private float fl_zoomSpeed;
    // Start is called before the first frame update
    void Start()
    {
        go_camera = transform.GetChild(0).gameObject;
        go_cameraParent = transform.parent.gameObject;
        cameraZoom = Mathf.RoundToInt(transform.localPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        CameraLocked();
        ZoomCamera();
    }

    void CameraLocked()
    {
        // Defines Speed of Camera (only moves forward/back & left/right, no up/down.) public variable that can be changed in editor
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * cameraSpeed, 
                                        0, 
                                        Input.GetAxis("Vertical") * cameraSpeed));
        // Defines Rotation of Camera (Q&E) public variable that can be changed in editor
        transform.Rotate(new Vector3(0, Input.GetAxis("Rotation") * cameraRotation, 0));

        cameraZoom = Mathf.Clamp(cameraZoom += Input.GetAxis("Mouse ScrollWheel"), minZoom, maxZoom);
        Vector3 zoomCamera = new Vector3(transform.localPosition.x, cameraZoom, transform.localPosition.z);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, zoomCamera, cameraZoomSpeed);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) bl_moveTowards = false;
    }

    void ZoomCamera()
    {

        if (Input.GetMouseButtonUp(0))
        {


            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.transform.tag == "Tower")
                {
                    go_moveTowards = hit.transform.GetChild(0).GetChild(0).gameObject;
                    if (fl_countTime <= fl_doubleClickTime && fl_countTime != 0)
                    {
                        if (go_moveTowards != null) bl_moveTowards = true;
                        bl_countTime = false;
                        fl_countTime = 0;
                    }
                    bl_countTime = true;
                }
                else
                {
                    go_moveTowards = null;
                }
            }
        }

        if (bl_countTime)
        {
            fl_countTime += Time.deltaTime;
            if (fl_countTime > fl_doubleClickTime)
            {
                bl_countTime = false;
                fl_countTime = 0;
            }
        }

        

        if (go_moveTowards != null)
        {
            
            if (bl_moveTowards)
            {
                transform.position = Vector3.MoveTowards(transform.position, go_moveTowards.transform.position, fl_zoomSpeed);
            }
            else
            {
                if (Vector3.Distance(transform.position, go_moveTowards.transform.position) > 40)
                {
                    fl_zoomSpeed = 1f;
                }
                else if (Vector3.Distance(transform.position, go_moveTowards.transform.position) > 20)
                {
                    fl_zoomSpeed = 0.75f;
                }
                else
                {
                    fl_zoomSpeed = 0.5f;
                }
            }
        }
        else bl_moveTowards = false;
    }
}
