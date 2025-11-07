using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }
    private void Update()
    {
        foreach(GameObject weaponSlot in weaponSlots)
        {
            if(weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }
    }
    public void PickupWeapon(GameObject pickUpWeapon)
    {
        AddWeaponIntoActiveAlot(pickUpWeapon);
    }
    private void AddWeaponIntoActiveAlot(GameObject pickupWeapon)
    {
        DropCurretWeapon(pickupWeapon);
        pickupWeapon.transform.SetParent(activeWeaponSlot.transform,false);

        Weapon weapon = pickupWeapon.GetComponent<Weapon>();

        pickupWeapon.transform.localPosition = new Vector3(weapon.spwanPosition.x, weapon.spwanPosition.y, weapon.spwanPosition.z);
        pickupWeapon.transform.localRotation = Quaternion.Euler(weapon.spwanRotation.x, weapon.spwanRotation.y, weapon.spwanRotation.z);
        weapon.isWeaponActive = true;
        weapon.animator.enabled = true;
    }


    private void DropCurretWeapon(GameObject pickupWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isWeaponActive = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickupWeapon.transform.localRotation;
        }
    }
    public void SwitchActiveSlot(int slotNumber)
    {
        if(activeWeaponSlot.transform.childCount>0)
        {
            Weapon curretWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            curretWeapon.isWeaponActive=false;
        }
        activeWeaponSlot = weaponSlots[slotNumber];
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isWeaponActive=true;
        }
    }

    internal void PickupAmmo(Ammo ammo)
    {
        switch (ammo.ammoType)
        {
            case Ammo.AmmoType.PistoleAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case Ammo.AmmoType.RifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
        }
    }

    internal void DecreaseTotalAmmo(int bulletsToDecrese, Weapon.WeaponModel thisWeaponmodel)
    {
        switch (thisWeaponmodel)
        {
            case Weapon.WeaponModel.Pistol:
                totalPistolAmmo -= bulletsToDecrese;
                break;
            case Weapon.WeaponModel.M16:
                totalRifleAmmo -= bulletsToDecrese;
                break;
        }
    }
    public int CheckAmmoLeftfor(Weapon.WeaponModel thisWeaponmodel)
    {
        switch (thisWeaponmodel)
        {
            case Weapon.WeaponModel.Pistol:
                return totalPistolAmmo;
            case Weapon.WeaponModel.M16:
                return totalRifleAmmo;

            default:
                return 0;
        }

    }
}
