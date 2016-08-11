using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class FireButton : MonoBehaviour
{
    public void OnButtonDown()
    {
        PlayerMover.SetAimState(true);
    }

    public void OnButtonUp()
    {
        PlayerMover.SetAimState(false);
    }
}
