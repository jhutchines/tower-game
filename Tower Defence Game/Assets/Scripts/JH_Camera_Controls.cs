using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Camera_Controls : MonoBehaviour
{
    private GameObject go_camera;
    private GameObject go_cameraParent;
    public float fl_doubleClickTime;
    public float fl_countTime;

    [Header("Camera Speed")]
    public float cameraSpeed;
    public float cameraRotation;
    public float cameraZoomSpeed;
    private float startCameraSpeed;
    private float startCameraRotation;

    [Header("Zooming")]
    public float maxZoom;
    public float minZoom;
    private float cameraZoom;

    [Header("Clamps")]
    public float maxDistance;

    private bool bl_countTime;
    private bool bl_moveTowards;
    private GameObject go_moveTowards;
    private float fl_zoomSpeed;
    // Start is called before the first frame update
    void Start()
    {
        go_camera = transform.GetChild(0).gameObject;
        go_cameraParent = transform.parent.gameObject;
        cameraZoom = Mathf.RoundToInt(transform.localPosition.y);
        startCameraSpeed = cameraSpeed;
        startCameraRotation = cameraRotation;
    }

    // Update is called once per frame
    void Update()
    {
        CameraLocked();
        ZoomCamera();
    }

    void CameraLocked()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            cameraSpeed = startCameraSpeed * 2;
            cameraRotation = startCameraRotation * 1.5f;
        }
        else
        {
            cameraSpeed = startCameraSpeed;
            cameraRotation = startCameraRotation;
        }

        // Defines Speed of Camera (only moves forward/back & left/right, no up/down.) public variable that can be changed in editor
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * cameraSpeed, 
                                        0, 
                                        Input.GetAxis("Vertical") * cameraSpeed));
        // Defines Rotation of Camera (Q&E) public variable that can be changed in editor
        transform.Rotate(new Vector3(0, Input.GetAxis("Rotation") * cameraRotation, 0));

        // Stops the camera from being able to move too far away
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -maxDistance, maxDistance);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -maxDistance, maxDistance);
        transform.position = clampedPosition;

        cameraZoom = Mathf.Clamp(cameraZoom += Input.GetAxis("Mouse ScrollWheel"), minZoom, maxZoom);
        Vector3 zoomCamera = new Vector3(transform.localPosition.x, cameraZoom, transform.localPosition.z);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, zoomCamera, cameraZoomSpeed);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) bl_moveTowards = false;
    }

    void ZoomCamera()
    {
        // When click is released, zoom in on a defined object such as "tower"
        if (Input.GetMouseButtonUp(0))
        {

            // Convert current camera position of mouse on screen, moves forward towards objects selected
            // Starts ray at mouse position to check what the player has clicked on
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                // Remember to check for components instead of tags, as more efficient
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
            }
        }

        // Counts time between clicks
        if (bl_countTime)
        {
            fl_countTime += Time.deltaTime;
            if (fl_countTime > fl_doubleClickTime)
            {
                bl_countTime = false;
                fl_countTime = 0;
            }
        }

        
        // If movetowards has a value
        if (go_moveTowards != null)
        {
            // Activate movetowards, if player has double clicked
            if (bl_moveTowards)
            {
                transform.position = Vector3.MoveTowards(transform.position, go_moveTowards.transform.position, fl_zoomSpeed);
                // Sets zoom to 20 if it was greater than 30
                if (transform.position == go_moveTowards.transform.position && cameraZoom > 30) cameraZoom = 20;
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
    }
}
