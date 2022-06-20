using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCameraController : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerCamera;

    public float sensivityX = 200f;
    public float sensivityY = 200f;
    private float rotation = 0;
    
    void Start()
    {
        if (!isLocalPlayer) return;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCamera.SetActive(true);
    }
    void Update()
    {
        if (!isLocalPlayer) return;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            gameObject.transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensivityX * Time.deltaTime, 0);
            rotation += Input.GetAxis("Mouse Y") * sensivityY * Time.deltaTime;
            rotation = Mathf.Clamp(rotation, -90, 90);
            playerCamera.transform.localRotation = Quaternion.Euler(new Vector3(-rotation, 0, 0));
        }
    }
}
