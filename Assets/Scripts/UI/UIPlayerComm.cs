using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIPlayerComm : MonoBehaviour
{
    public enum SideTypes { ToLeft, ToRight }

    public void StartFire()
    {
        PlayerMover.SetAimState(true);
        PlayerShooter.StartShoot();
    }

    public void EndFire()
    {
        PlayerMover.SetAimState(false);
        PlayerShooter.EndShoot();
    }

    public void RotateViewLeft()
    {
        PlayerMover.TurnTo(SideTypes.ToLeft);
    }

    public void RotateViewRight()
    {
        PlayerMover.TurnTo(SideTypes.ToRight);
    }

    public void NextWeapon()
    {
        PlayerShooter.NextWeapon();
    }
}
