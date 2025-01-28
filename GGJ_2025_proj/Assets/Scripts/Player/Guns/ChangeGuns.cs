using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGuns : MonoBehaviour
{
    [Header("Weapon Paths")]
    [SerializeField] private List<GameObject> A_Type;
    [SerializeField] private List<GameObject> B_Type;
    [SerializeField] private List<GameObject> E_Type;

    [Header("Weapon Locations")]
    [SerializeField] private List<Transform> locations;

    private CurrentWeapon AWeapon = new CurrentWeapon();
    private CurrentWeapon BWeapon = new CurrentWeapon();
    private CurrentWeapon EWeapon = new CurrentWeapon();




    public void getWeapon()
    {

    }
}

public struct CurrentWeapon
{
    public int index;
    public WeaponType type;

}
public enum WeaponType
{
    A,
    B,
    E
}