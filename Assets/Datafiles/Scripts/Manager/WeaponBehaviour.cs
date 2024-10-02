using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Gun,
    Grenade,
    PlayerGunProjectile
}

public class WeaponBehaviour : MonoBehaviour
{
    public WeaponType weaponType;
    private Rigidbody rb;

    public Vector3 _grenadeTarget;

    public GameObject _gunPoint;

    public GameObject _projectile;
    public GameObject _projectileInstance;

    public GameObject _GunExplosivePrefab;
    public GameObject _GunExplosiveInstance;

    public GameObject _bombExplosivePrefab;
    public GameObject _bombExplosiveInstance;

    Coroutine _gunDelayExplosion;

    private void Start()
    {
        _projectile = Resources.Load<GameObject>("Prefabs/Weapons/Projectile");
        rb = GetComponent<Rigidbody>();
        if(weaponType == WeaponType.PlayerGunProjectile)
        {
            _GunExplosivePrefab = Resources.Load<GameObject>("Prefabs/Weapons/GunShotExplosion");
        }
        else if (weaponType == WeaponType.Grenade)
        {
            _bombExplosivePrefab = Resources.Load<GameObject>("Prefabs/Weapons/BombExplosionFx");
        }

        //ThrowGrenade();
    }

    private void ThrowGrenade()
    {
        if (rb != null)
        {
            // Calculate the throw direction (forward with some upward force)
            Vector3 throwDirection = transform.forward * 15f + transform.up * 5f;

            // Apply the calculated force to the grenade's Rigidbody
            rb.AddForce(throwDirection, ForceMode.Impulse);
        }
    }

    public void ShootBullet()
    {
        _projectileInstance = Instantiate(_projectile, _gunPoint.transform.position, _gunPoint.transform.rotation);
        _projectileInstance.GetComponent<Rigidbody>().AddForce(_projectileInstance.transform.forward * 2, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (weaponType == WeaponType.PlayerGunProjectile && _GunExplosivePrefab != null)
        {
            _GunExplosiveInstance = Instantiate(_GunExplosivePrefab, transform.position, Quaternion.identity);
            Destroy(_GunExplosiveInstance,5f);
            Destroy(gameObject);
        } 
        else if (weaponType == WeaponType.PlayerGunProjectile)
        {
            _GunExplosivePrefab = Resources.Load<GameObject>("Prefabs/Weapons/GunShotExplosion");
            if( _GunExplosivePrefab != null)
            {
                _GunExplosiveInstance = Instantiate(_GunExplosivePrefab, transform.position, Quaternion.identity);
                Destroy(_GunExplosiveInstance);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("GunExplosive is null");
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (weaponType == WeaponType.Grenade && collision.gameObject.CompareTag("Floor"))
        {
            if (_gunDelayExplosion == null)
            {
                _gunDelayExplosion = StartCoroutine(DelayExplosion());
            }
        }*/
        if (weaponType == WeaponType.Grenade && collision.gameObject.CompareTag("Floor"))
        {
            if(Vector3.Distance(transform.position, _grenadeTarget) < 0.5f
                && _gunDelayExplosion == null)
            {
                _gunDelayExplosion = StartCoroutine(DelayExplosion());
            }
        }
    }

    private IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(0.1f);
        _bombExplosiveInstance = Instantiate(_bombExplosivePrefab, transform.position, Quaternion.identity);
        Destroy(_bombExplosiveInstance, 5f);
        Destroy(gameObject);
    }
}
