using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;

    [SerializeField] float rotateSpeed; 
    
    //Clamp camera so it doesn't flip over
    [SerializeField] float topCameraClamp = 25;
    [SerializeField] float botCameraClamp = 335;

    //Late update so it happens after the player moves in update
    void LateUpdate()
    {
        //Rotate target based on Mouse X & Y
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        //target.Rotate(0, horizontal, 0);
        player.Rotate(0, horizontal, 0);

        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        target.Rotate(vertical, 0, 0);

        //Clamps Vertical Camera Rotation
        if(target.rotation.eulerAngles.x > topCameraClamp && target.rotation.eulerAngles.x < 180f)
        {
            target.rotation = Quaternion.Euler(topCameraClamp, 0, 0);
        }
        if (target.rotation.eulerAngles.x > 180 && target.rotation.eulerAngles.x < botCameraClamp)
        {
            target.rotation = Quaternion.Euler(botCameraClamp, 0, 0);
        }

        //Rotate the camera based on target rotation & keeps it near the target
        float desiredYAngle = player.eulerAngles.y;
        float desiredXAngle = target.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offset);

        transform.LookAt(target);
    }
}
