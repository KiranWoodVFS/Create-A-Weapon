using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int _speed = 10;
    [SerializeField] private float _mouseSensitivity = 400f;

    private Rigidbody _rb;
    private Vector3 _moveVector;
    private float _xRotation = 0f;

    [Header("Gun")]
    public Transform _gunTip;

    [Header("ShotGun")]
    public GameObject _bulletPrefab;
    public float _shotGunRotationMax;
    public float _shotGunRechargeTime;
    public int _bulletAmount = 8;

    [Header("ExplosiveGun")]
    public GameObject _bulletPrefabAlternate;
    public float _explosiveGunRechargeTime;

    float _waitTime;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _moveVector = Vector3.zero;
        _waitTime = _shotGunRechargeTime + _explosiveGunRechargeTime; // Can always shoot gun immediately

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _waitTime += Time.deltaTime;

        // Player Rotation
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        gameObject.transform.Rotate(Vector3.up * mouseX);

        // Player Movement
        float zMovement = Input.GetAxis("Vertical");
        float xMovement = Input.GetAxis("Horizontal");

        _moveVector = transform.right * xMovement + transform.forward * zMovement;

        // Shoots Gun
        if (Input.GetButtonDown("Fire1")) Fire(false);
        if (Input.GetButtonDown("Fire2")) Fire(true);
    }

    private void Fire(bool isAlternate)
    {
        if (!isAlternate) // Left Click. Fires shotgun (5 shots)
        {
            if (_waitTime > _shotGunRechargeTime)
            {
                Vector3 gunRotation = _gunTip.rotation.eulerAngles;
                Quaternion newRotation = _gunTip.rotation;

                // Creates new rotation for individual shot
                for (int i = -0; i < _bulletAmount; i++)
                {
                    float randomRotationAmountX = UnityEngine.Random.Range(-_shotGunRotationMax, _shotGunRotationMax);
                    float randomRotationAmountY = UnityEngine.Random.Range(-_shotGunRotationMax, _shotGunRotationMax);
                    newRotation = Quaternion.Euler(gunRotation.x + randomRotationAmountX, gunRotation.y + randomRotationAmountY, gunRotation.z);
                    Instantiate(_bulletPrefab, _gunTip.position, newRotation);
                }
                _waitTime = 0;
            }
        }
        else // Fires explosive gun
        {
            if (_waitTime > _explosiveGunRechargeTime)
            {
                Instantiate(_bulletPrefabAlternate, _gunTip.position, _gunTip.rotation);
                _waitTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_moveVector * _speed);
    }
}
