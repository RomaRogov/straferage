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
    //private float guideRotation;
    private float gyroAngle;
    private float gyroRotation;
    private static PlayerMover instance;
    private int? lastTouchID;
    private BezierCurve guideInstance;

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
            if (lastTouchID.HasValue && (lastTouchID.Value > (Input.touchCount - 1)))
            {
                lastTouchID = null;
            }

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

        if (Input.GetButtonDown("Fire"))
        {
        	PlayerShooter.StartShoot();
        	SetAimState(true);
        }
        if (Input.GetButtonUp("Fire"))
        {
        	PlayerShooter.EndShoot();
        	SetAimState(false);
        }
        if (Input.GetButtonDown("Turn Left"))
        {
        	TurnTo(UIPlayerComm.SideTypes.ToRight);
        }
        if (Input.GetButtonDown("Turn Right"))
        {
        	TurnTo(UIPlayerComm.SideTypes.ToLeft);
        }
        if (Input.GetButtonDown("Next Weapon"))
        {
        	PlayerShooter.PrevWeapon();
        }
        if (Input.GetButtonDown("Prev Weapon"))
        {
        	PlayerShooter.NextWeapon();
        }
    }
	
	void FixedUpdate ()
    {
    	float mouseX = 0;
        float mouseY = 0;
        if (Application.isEditor)
        {
            mouseX = Input.GetAxis("Horizontal") / (float)Screen.width * 500f;
            mouseY = Input.GetAxis("Vertical") / (float)Screen.height * 500f;
        }

		if (!Application.isEditor && Input.GetMouseButton(0))
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

        //if (aimMode)
        //{
        gyroAngle -= Input.gyro.rotationRateUnbiased.y;
        if (Input.GetButton("Target Left")) { gyroRotation -= 1f; }
        if (Input.GetButton("Target Right")) { gyroRotation += 1f; }
        gyroRotation += gyroAngle * .1f;

        if (Input.GetButton("Stop"))
        {
        	myRigidbody.velocity = Vector3.zero;
        }

        rotation = Mathf.LerpAngle(rotation, rotationTarget, Time.fixedDeltaTime * 10f);
        
        /*if (guideInstance != null)
        {
            guideRotation = Mathf.LerpAngle(guideRotation, Quaternion.LookRotation(guideInstance.GetDirection(guideInstance.GetPointValue(transform.position))).eulerAngles.y, Time.fixedDeltaTime * 10f);
        }*/

        View.transform.position = transform.position + Shift;
        View.transform.rotation = Quaternion.Euler(Vector3.up * (rotation + gyroRotation));
    }

    private void OnTriggerEnter(Collider other)
    {
        BezierCurve possibleGuide = other.GetComponent<BezierCurve>();
        if (possibleGuide != null) { guideInstance = possibleGuide; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (guideInstance != null && guideInstance.GetComponent<Collider>() == other)
        {
            guideInstance = null;
        }
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

    public static void Calibrate()
    {
        instance.gyroAngle = 0;
    }
}
