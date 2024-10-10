using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Gun,
    Grenade,
    PlayerGunProjectile,
    Crossbow,
    CrossbowProjectile
}

public class WeaponBehaviour : MonoBehaviour
{
    public WeaponType weaponType;
    private Rigidbody rb;

    public Vector3 _grenadeTarget;

    public GameObject _gunPoint;
    public Transform _crossbowArrowSpts;

    public LayerMask _gunProjectilelayerMask;
    public GameObject _middleProjectile;
    public GameObject _leftProjectile;
    public GameObject _rightProjectile;
    public GameObject _muzzleVFX;
    public GameObject _projectileInstance;

    public GameObject _crossbowProjectile;
    public GameObject _crossbowProjectileInstance;

    public GameObject _GunExplosivePrefab;
    public GameObject _GunExplosiveInstance;

    public GameObject _bombExplosivePrefab;
    public GameObject _bombExplosiveInstance;

    public Vector3 _startposition;

    Coroutine _gunDelayExplosion;

    public float _bulletSpeed = 1.2f;
    public float _bulletRange = 1;
    public float _distanceTravelled;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (weaponType == WeaponType.Gun)
        {
            _middleProjectile = Resources.Load<GameObject>("Prefabs/Weapons/Projectile");
            _muzzleVFX = Resources.Load<GameObject>("Prefabs/Weapons/MuzzleEffect");
        }
        else if (weaponType == WeaponType.PlayerGunProjectile)
        {
            _GunExplosivePrefab = Resources.Load<GameObject>("Prefabs/Weapons/GunShotExplosion");
            _startposition = transform.position;
        }else if (weaponType == WeaponType.CrossbowProjectile)
        {
            _startposition = transform.position;
        }
        else if (weaponType == WeaponType.Grenade)
        {
            _bombExplosivePrefab = Resources.Load<GameObject>("Prefabs/Weapons/BombExplosionFx");
        }
        else if (weaponType == WeaponType.Crossbow)
        {
            _crossbowProjectile = Resources.Load<GameObject>("Prefabs/Weapons/Arrow_1_Red");
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

    private void Update()
    {
        if(weaponType == WeaponType.PlayerGunProjectile)
        {
            _distanceTravelled = Vector3.Distance(_startposition, transform.position);
            if(_distanceTravelled >=
                _bulletRange)
            {
                Destroy(gameObject);
            }
            else if(_projectileInstance == null)
            {
                Debug.LogError($" Projectile Instance is null");
            }
        }else if (weaponType == WeaponType.CrossbowProjectile)
        {
            _distanceTravelled = Vector3.Distance(_startposition, transform.position);
            if (_distanceTravelled >=
                _bulletRange)
            {
                Destroy(gameObject);
            }
            else if (_projectileInstance == null)
            {
                Debug.LogError($" Projectile Instance is null");
            }
        }
    }

    public void ShootBullet()
    {
        _projectileInstance = Instantiate(_middleProjectile, _gunPoint.transform.position, _gunPoint.transform.rotation);
        Destroy(Instantiate(_muzzleVFX, _gunPoint.transform.position, _gunPoint.transform.rotation), 3f);
        _projectileInstance.GetComponent<Rigidbody>().AddForce(_projectileInstance.transform.forward * _bulletSpeed, ForceMode.Impulse);

        Vector3 leftOffset = _gunPoint.transform.right * -0.08f;
        Quaternion leftRotation = Quaternion.Euler(0, -15, 0) * _gunPoint.transform.rotation; // Adjust rotation by -15 degrees
        _leftProjectile = Instantiate(_middleProjectile, _gunPoint.transform.position + leftOffset, leftRotation);
        _leftProjectile.GetComponent<Rigidbody>().AddForce(_leftProjectile.transform.forward * _bulletSpeed, ForceMode.Impulse);

        Vector3 rightOffset = _gunPoint.transform.right * 0.08f;
        Quaternion rightRotation = Quaternion.Euler(0, 15, 0) * _gunPoint.transform.rotation; // Adjust rotation by +15 degrees
        _rightProjectile = Instantiate(_middleProjectile, _gunPoint.transform.position+ rightOffset, rightRotation);
        _rightProjectile.GetComponent<Rigidbody>().AddForce(_rightProjectile.transform.forward * _bulletSpeed, ForceMode.Impulse);
    }

    public void ShootArrow()
    {
        _crossbowProjectileInstance = Instantiate(_crossbowProjectile, _crossbowArrowSpts.position, _crossbowArrowSpts.rotation);
        _crossbowProjectileInstance.GetComponent<Rigidbody>().
            AddForce(_crossbowProjectileInstance.transform.forward * _bulletSpeed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (weaponType == WeaponType.PlayerGunProjectile && _GunExplosivePrefab != null && other.gameObject.tag != gameObject.tag)
        {
            Debug.LogError($"Collided with {other.gameObject.tag}");
            _GunExplosiveInstance = Instantiate(_GunExplosivePrefab, transform.position, Quaternion.identity);
            Destroy(_GunExplosiveInstance,5f);
            Destroy(gameObject);
        } 
        else if (weaponType == WeaponType.PlayerGunProjectile && other.gameObject.tag != gameObject.tag)
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
