using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour
{
    public Transform View;
    public Vector3 Shift;

    private Rigidbody myRigidbody;
    //private float rotationSpeed;
    private float rotationTarget;
    private float rotation;
    private static PlayerMover instance;

    void Start ()
    {
        myRigidbody = GetComponent<Rigidbody>();
        instance = this;
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
                mouseX = Input.GetTouch(0).deltaPosition.x / (float)Screen.width * 500f;
                mouseY = Input.GetTouch(0).deltaPosition.y / (float)Screen.height * 500f;
            }

            Vector3 forward = View.TransformDirection(Vector3.forward);
            Vector3 right = View.TransformDirection(Vector3.right);
            
            myRigidbody.AddForce((forward * mouseY + right * mouseX) * 40f);

            /*if (Mathf.Abs(mouseY) > Mathf.Abs(mouseX))
            {
                myRigidbody.AddForce(View.TransformDirection(Vector3.forward) * mouseY * 40f);
            }
            else
            {
                if (Mathf.Abs(rotationSpeed) < Mathf.Abs(mouseX))
                { rotationSpeed = mouseX; }
            }*/
        }
        //rotationSpeed = Mathf.Lerp(rotationSpeed, 0f, Time.fixedDeltaTime);
        //rotation += rotationSpeed;
        rotation = Mathf.LerpAngle(rotation, rotationTarget, Time.fixedDeltaTime * 10f);
        View.transform.position = transform.position + Shift;
        View.transform.rotation = Quaternion.Euler(Vector3.up * rotation);
	}

    public static void TurnTo(RotateViewBtn.SideTypes side)
    {
        instance.rotationTarget += (side == RotateViewBtn.SideTypes.ToLeft ? -90 : 90);
    }
}
