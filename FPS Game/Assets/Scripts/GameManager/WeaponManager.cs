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

    [Header("Throwables General")]
    public float throwFroce = 40f;
    public float froceMultiplier =0f;
    public float froceMultiplierLimit = 2f;
    
    public GameObject throwbleSpwan;

    [Header("Lethals")]
    public int maxLeathals = 2;
    public int lethalsCount = 0;
    public Throwables.ThrowableType equippedLethalType;
    public GameObject grenadePrefab;

    [Header("Tacticals")]
    public int maxTacticals = 2;
    public int tacticalCount = 0;
    public Throwables.ThrowableType equippedTacticalType;
    public GameObject SomkeGrenadePrefab;

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
        equippedLethalType = Throwables.ThrowableType.None;
        equippedTacticalType = Throwables.ThrowableType.None;
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
        if (Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.T))
        {
            froceMultiplier += Time.deltaTime;
            if(froceMultiplier > froceMultiplierLimit)
            {
                froceMultiplier= froceMultiplierLimit;
            }
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            if(lethalsCount > 0)
            {
                ThrowLeathal();
            }
            froceMultiplier = 0;
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (tacticalCount > 0)
            {
                ThrowTactical();
            }
            froceMultiplier = 0;
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

    public void PickupThrowables(Throwables throwables)
    {
        switch (throwables.throwableType)
        {
            case Throwables.ThrowableType.Grenade:
                PickUpThrowablesAsLethal(Throwables.ThrowableType.Grenade);
                break;
            case Throwables.ThrowableType.SmokeGrenade:
                PickUpThrowablesAsTactical(Throwables.ThrowableType.SmokeGrenade);
                break;
        }
    }

    private void PickUpThrowablesAsTactical(Throwables.ThrowableType tactical)
    {
        if (equippedTacticalType == tactical || equippedTacticalType == Throwables.ThrowableType.None)
        {
            equippedTacticalType = tactical;
            if (tacticalCount < maxTacticals)
            {
                tacticalCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowables.gameObject);
                HUDManager.Instance.UpdateThrowablesUI();
            }
            else
            {
                print("Tactical limit reached");
            }
        }
    }

    private void PickUpThrowablesAsLethal(Throwables.ThrowableType lethal)
    {
        if(equippedLethalType == lethal || equippedLethalType == Throwables.ThrowableType.None)
        {
            equippedLethalType = lethal;
            if(lethalsCount< maxLeathals)
            {
                lethalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowables.gameObject);
                HUDManager.Instance.UpdateThrowablesUI();
            }
            else
            {
                print("Lethal limit reached");
            }
        }
    }

   
    private void ThrowLeathal()
    {
        GameObject leathalPrefab = GetThrowablePrefab(equippedLethalType);
        GameObject throwable = Instantiate(leathalPrefab,throwbleSpwan.transform.position,Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwFroce * froceMultiplier),ForceMode.Impulse);
       
        throwable.GetComponent<Throwables>().hasbeenThrown=true;

        lethalsCount -= 1;

        if(lethalsCount <= 0)
        {
            equippedLethalType=Throwables.ThrowableType.None;
        }
        HUDManager.Instance.UpdateThrowablesUI();
    }

    private void ThrowTactical()
    {
        GameObject tacticalPrefab = GetThrowablePrefab(equippedTacticalType);
        GameObject throwable = Instantiate(tacticalPrefab, throwbleSpwan.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwFroce * froceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwables>().hasbeenThrown = true;

        tacticalCount -= 1;

        if (tacticalCount <= 0)
        {
            equippedTacticalType = Throwables.ThrowableType.None;
        }
        HUDManager.Instance.UpdateThrowablesUI();
    }

    private GameObject GetThrowablePrefab(Throwables.ThrowableType throwableType)
    {
        switch (throwableType)
        {
            case Throwables.ThrowableType.Grenade:
                return grenadePrefab;

            case Throwables.ThrowableType.SmokeGrenade:
                return SomkeGrenadePrefab;
        }
        return new();
    }
}
