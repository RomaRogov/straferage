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
    private int? lastTouchID;

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
        if (!Application.isEditor && (Input.touchCount > 0))
        {
            if (lastTouchID.HasValue && (Input.GetTouch(lastTouchID.Value).phase == TouchPhase.Ended))
            {
                lastTouchID = null;
            }

            if (lastTouchID != null) { return; }

            int possibleTouchIndex = Array.FindIndex(Input.touches, touch => { return (touch.phase == TouchPhase.Began); });
            
            if ((possibleTouchIndex >= 0) && !EventSystem.current.IsPointerOverGameObject(Input.touches[possibleTouchIndex].fingerId))
            {
                lastTouchID = Input.touches[possibleTouchIndex].fingerId;
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
                mouseX = (lastTouchID.HasValue ? (Input.GetTouch(lastTouchID.Value).deltaPosition.x / (float)Screen.width * 500f) : 0);
                mouseY = (lastTouchID.HasValue ? (Input.GetTouch(lastTouchID.Value).deltaPosition.y / (float)Screen.height * 500f) : 0);
            }

            mouseX = Mathf.Clamp(mouseX, -10f, 10f);
            mouseY = Mathf.Clamp(mouseY, -10f, 10f);
            Vector3 forward = View.TransformDirection(Vector3.forward);
            Vector3 right = View.TransformDirection(Vector3.right);
            
            myRigidbody.AddForce((forward * mouseY + right * mouseX) * 30f);
            float vel = Mathf.Clamp(myRigidbody.velocity.magnitude, -20f, 20f);
            myRigidbody.velocity = myRigidbody.velocity.normalized * vel;
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

    public static void TurnTo(UIPlayerComm.SideTypes side)
    {
        instance.rotationTarget += (side == UIPlayerComm.SideTypes.ToLeft ? -90 : 90);
        instance.gyroRotation = 0;
    }

    public static void SetAimState(bool state)
    {
        instance.aimMode = state;
    }
}
