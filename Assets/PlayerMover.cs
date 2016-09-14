using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class PlayerMover : MonoBehaviour
{
    public Transform View;
    public Vector3 Shift;

    private Rigidbody myRigidbody;
    private float rotationTarget;
    private float rotation;
    private float gyroRotation;
    private static PlayerMover instance;
    private int lastTouchID;

    private bool aimMode = false;

    void Start ()
    {
        myRigidbody = GetComponent<Rigidbody>();
        instance = this;
        rotation = rotationTarget = View.eulerAngles.y;

        Input.gyro.enabled = true;
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Application.isEditor)
        {
            Touch lastTouch = Array.Find<Touch>(Input.touches, touch => { return (touch.phase == TouchPhase.Began); });

            int touchID = lastTouch.fingerId;
            if (!EventSystem.current.IsPointerOverGameObject(touchID))
            {
                lastTouchID = touchID;
            }
        }
    }
	
	void FixedUpdate ()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX;
            float mouseY;
            if (Application.isEditor)
            {
                mouseX = Input.GetAxis("Mouse X") / (float)Screen.width * 500f;
                mouseY = Input.GetAxis("Mouse Y") / (float)Screen.height * 500f;
            }
            else
            {
                mouseX = Input.GetTouch(lastTouchID).deltaPosition.x / (float)Screen.width * 500f;
                mouseY = Input.GetTouch(lastTouchID).deltaPosition.y / (float)Screen.height * 500f;
            }

            Vector3 forward = View.TransformDirection(Vector3.forward);
            Vector3 right = View.TransformDirection(Vector3.right);
            
            myRigidbody.AddForce((forward * mouseY + right * mouseX) * 30f);
        }

        if (aimMode)
        {
            gyroRotation -= Input.gyro.rotationRateUnbiased.y;
        }
        else
        {
            gyroRotation = Mathf.LerpAngle(gyroRotation, 0, Time.fixedDeltaTime * 5f);
        }

        rotation = Mathf.LerpAngle(rotation, rotationTarget, Time.fixedDeltaTime * 10f);
        View.transform.position = transform.position + Shift;
        View.transform.rotation = Quaternion.Euler(Vector3.up * (rotation + gyroRotation));
	}

    public static void TurnTo(RotateViewBtn.SideTypes side)
    {
        instance.rotationTarget += (side == RotateViewBtn.SideTypes.ToLeft ? -90 : 90);
        instance.gyroRotation = 0;
    }

    public static void SetAimState(bool state)
    {
        instance.aimMode = state;
    }
}
