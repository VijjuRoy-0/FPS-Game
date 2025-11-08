using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoveredWeapon = null;
    public Ammo hoveredAmmo = null;
    public Throwables hoveredThrowables = null;

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

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            GameObject objectWeHitByRayCast = hit.transform.gameObject;
            //Weapon
            if (objectWeHitByRayCast.GetComponent<Weapon>() && objectWeHitByRayCast.GetComponent<Weapon>().isWeaponActive==false)
            {
                hoveredWeapon = objectWeHitByRayCast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupWeapon(objectWeHitByRayCast.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent <Outline>().enabled = false;
                }
            }
            //Ammo
            if (objectWeHitByRayCast.GetComponent<Ammo>() )
            {
                hoveredAmmo = objectWeHitByRayCast.gameObject.GetComponent<Ammo>();
                hoveredAmmo.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmo);
                    Destroy(objectWeHitByRayCast.gameObject);
                }
            }
            else
            {
                if (hoveredAmmo)
                {
                    hoveredAmmo.GetComponent<Outline>().enabled = false;
                }
            }
            //Throwable
            if (objectWeHitByRayCast.GetComponent<Throwables>())
            {
                hoveredThrowables = objectWeHitByRayCast.gameObject.GetComponent<Throwables>();
                hoveredThrowables.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupThrowables(hoveredThrowables);
                }
            }
            else
            {
                if (hoveredThrowables)
                {
                    hoveredThrowables.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
