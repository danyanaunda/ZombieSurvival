using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterController : MonoBehaviour, IUnit
{
    [SerializeField] private Shell shellCasing;
    [SerializeField] private Transform aimPoint;
    [SerializeField] private int damage;

    public int MaxAmmoCount;
    public event Action<int> OnShot;
    public event Action<float, float> OnReload;
    public event Action OnReloadEnded;

    [Header("Movement")] [Tooltip("Max movement speed when grounded (when not sprinting)")]
    public float MaxSpeedOnGround = 10f;

    [Tooltip("Multiplicator for the sprint speed (based on grounded speed)")]
    public float SprintSpeedModifier = 2f;

    [Header("Rotation")] [Tooltip("Rotation speed for moving the camera")]
    public float RotationSpeed = 200f;

    public Health Health { get; private set; }
    public int Damage => damage;
    public Vector3 CharacterVelocity { get; set; }

    private PlayerInputHandler _inputHandler;
    private CharacterController _characterController;
    private Camera _mainCamera;

    private ShellPool _shellPool;

    private bool _isRealoading;
    private int _currentAmmoCount;
    private float _reloadTime = 5;
    private float _timeInReload;
    private float _reloadStartTime;
    private float _shellCasingEjectionForce = 1f;


    void Awake()
    {
        _shellPool = new ShellPool(shellCasing, MaxAmmoCount);
        _characterController = GetComponent<CharacterController>();
        // _inputHandler = GetComponent<PlayerInputHandler>();
        Health = GetComponent<Health>();

        _characterController.enableOverlapRecovery = true;
    }

    void Start()
    {
        _currentAmmoCount = MaxAmmoCount;
    }

    private void Update()
    {
        if (_inputHandler is null) return;
        
        HandleCharacterMovement();
        HandleAttack();
        HandleReloading();
    }

    public void Init(PlayerInputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }
    
    private void HandleReloading()
    {
        if (!_inputHandler.GetReloadButtonDown()) return;
        if (_isRealoading || _currentAmmoCount == MaxAmmoCount) return;

        StartCoroutine(ReloadingWeapon());
    }

    private void HandleAttack()
    {
        if (_isRealoading) return;
        if (!_inputHandler.GetFireInputDown()) return;

        var shell = _shellPool.Get();

        aimPoint.GetPositionAndRotation(out var position, out var rotation);
        Transform shellTransform = shell.transform;

        shellTransform.SetPositionAndRotation(position, rotation);
        shell.Activate(damage, aimPoint.forward * _shellCasingEjectionForce);

        _currentAmmoCount--;
        OnShot?.Invoke(_currentAmmoCount);

        if (_currentAmmoCount <= 0)
        {
            StartCoroutine(ReloadingWeapon());
        }
    }

    private IEnumerator ReloadingWeapon()
    {
        _isRealoading = true;
        _timeInReload = 0;
        _reloadStartTime = Time.time;

        while (_timeInReload < _reloadTime)
        {
            OnReload?.Invoke(_timeInReload, _reloadTime);
            _timeInReload = Time.time - _reloadStartTime;
            yield return null;
        }

        OnShot?.Invoke(_currentAmmoCount);
        OnReloadEnded?.Invoke();
        _currentAmmoCount = MaxAmmoCount;
        _isRealoading = false;
    }

    void HandleCharacterMovement()
    {
        transform.Rotate(
            new Vector3(0f, (_inputHandler.GetLookInputsHorizontal() * RotationSpeed),
                0f), Space.Self);

        bool isSprinting = _inputHandler.GetSprintInputHeld();
        {
            float speedModifier = isSprinting ? SprintSpeedModifier : 1f;
            Vector3 worldspaceMoveInput = transform.TransformVector(_inputHandler.GetMoveInput());
            CharacterVelocity = worldspaceMoveInput * (MaxSpeedOnGround * speedModifier);
        }

        _characterController.SimpleMove(CharacterVelocity);
    }
}