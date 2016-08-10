using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RotateViewBtn : MonoBehaviour
{
    public enum SideTypes { ToLeft, ToRight }
    public SideTypes Direction;
	
	void Start ()
    {
        Button myBtn = GetComponent<Button>();
        myBtn.onClick.AddListener(() => { PlayerMover.TurnTo(Direction); });
	}
	
}
