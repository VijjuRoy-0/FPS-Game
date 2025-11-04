using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
   public float mouseSensitivity = 500f;

    float xRotation = 0;
    float yRotation = 0;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity *Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y")* mouseSensitivity *Time.deltaTime;

        //Rotate around the x-axis (look up and Down)
        xRotation -= mouseY;

        //Clap the Rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //Rotate around the y-axis
        yRotation += mouseX;

        // Apply rotation to player
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
