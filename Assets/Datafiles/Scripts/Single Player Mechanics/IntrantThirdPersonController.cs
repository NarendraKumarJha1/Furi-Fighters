using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;
using UnityEngine.AI;
using Cinemachine;

public enum MAP
{
    WEEK1,
    WEEK2,
    WEEK3,
    WEEK4,
    WEEK5,
    WEEK6,
    WEEK7,
    WEEK8,
    WEEK9,
    WEEK10,
    WEEK11,
    WEEK12,
    WEEK13,
    WEEK14,
    WEEK15,
    WEEK16,
    WEEK17,
    WEEK18,
    WEEK19,
    WEEK20,
    WEEK21,
    WEEK22,
    WEEK23,
    WEEK24,
    WEEK25
}

public class PlayerDataJson
{
    public string nickName;
    public string name;
    public bool _collectedBreakPotion;
    public bool _brokeWall;
    public bool _receivedTheraBox;
    public bool _receivedTheraCode;
    public bool _receivedAsimanaCodex;
    public bool _placedTheraCode;
    public bool _mainFrameEnabled;
    public bool _beenInAlkemiaChamber;
    public bool _headingToTerathsGate;
    public bool _beenToRecoletteMirror;
    public bool _collectedStarDevice;
    public bool _inOrbsOfKinesis;
    public bool _collectedMiniGameOrbs;
    public bool _insideOrbsEmitter;
    public bool _placedOrbs;
    public bool _collectedLastOrb;
    public bool _fightedKaire;
    public bool _playerenteredSafetyEnclosure;
    public bool _collectedClaw;
    public bool _fixedStoneCourtyard;
    public bool _killedBossMonster;
    public bool _collectedMagicalKey;
    public bool _unlockedMagicalDoor;
    public bool _foundThrowingStar;
    public bool _OrinAndRasveusFixedMira;
    public bool _meronNftOne;
    public bool _collectedAssembledMeronNft;
    public bool _hasPowerPotion;
    public int _switchedOnCount;
    public int _collectedTherCode;
    public int _collectedOrbs;
    public int _magicScrolls;
    public int _collectedAdacode;
    public int _forearmsArmor;
    public int _bodyArmor;
    public int _ShoulderArmor;
    public int _collectedClawCount;
    public int _completedMissionNo;
    public int _collectedChest;
    public int _monsterSlain;
}

public class credentialClass
{
    public string type;
    public string project_id;
    public string private_key_id;
    public string private_key;
    public string client_email;
    public string client_id;
    public string auth_uri;
    public string token_uri;
    public string auth_provider_x509_cert_url;
    public string client_x509_cert_url;
    public string universe_domain;
}

public class IntrantThirdPersonController : IntrantThirdPersonAnimator
{
    public static IntrantThirdPersonController instance;
    [SerializeField] public IntrantPlayerHealthManager _playerHealthManager;
    [SerializeField] public CinemachineVirtualCamera battleArenaFreelookCamera;
    private Rigidbody _rb;

    //Action
    public static event Action CollectedAsimanaCodex;
    public static event Action UnlockedDoor;
    public static event Action LoadedPlayerData;
    public static event Action Mission;

    public VideoPlayer _cutsceneVideoPlayer;
    [SerializeField] private List<GameObject> _enemyController;
    [SerializeField] public GameObject[] externalCanvases;
    [SerializeField] public GameObject _virtualCamera;
    [SerializeField] public GameObject _kairRef;
    private GameObject _shieldFxInstance = null;
    [SerializeField] private GameObject _gunPrefab = null;
    [SerializeField] private GameObject _crossbowPrefab = null;
    [SerializeField] private GameObject _crossbowInstance = null;
    [SerializeField] private GameObject _gunInstance = null;
    [SerializeField] private GameObject _grenadePrefab = null;
    [SerializeField] private GameObject _grenadeInstance = null;
    [SerializeField] private GameObject _activeWeaponInstance = null;
    public GameObject rightHandAnchor = null;


    [Header("UI Ref")]
    public Button _signatureAttackButton;
    public FixedJoystick _grenadeButton;
    public FixedJoystick _shootButton;
    public FixedJoystick _crossbowButton;
    public Image _shootCooldownImage;
    public Image _bombCooldownImage;
    public Image _signatureCooldownImage;
    public TMP_Text _ObjectInfoContent;
    public TMP_Text _ObjectInfoTitle;
    public Image _otherInformationPic;
    public CanvasGroup _shoulder;
    public CanvasGroup[] _forearms;
    public CanvasGroup _body;
    public GameObject _playerMinimapPointer;
    public GameObject _checkPointFx;
    public GameObject _grenadeProjectionMarker;
    public GameObject _gunProjectionMarker;
    public GameObject _crossbowMarker;
    public Collider _hammerCollider;
    public Collider _PlayerShield;
    public Transform _cleaveSpts;
    public Transform _shieldFxParent;
    public Transform _informationPanelSpts;
    public Transform _ingamePotionSpawn;

    public bool isShieldActive;
    public bool isAttacking;
    public bool canSprint = true;
    private readonly float shieldCooldown = 2.5f;
    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private static readonly int CrossShootHash = Animator.StringToHash("CrossbowShoot");
    private static readonly int BombHash = Animator.StringToHash("ThrowGrenade");
    private static readonly int SignatureHash = Animator.StringToHash("SignatureAttack");
    private static readonly int PowerUpHash = Animator.StringToHash("PowerUp");
    private static readonly int ShieldHash = Animator.StringToHash("Shield");
    private static readonly int DeadHash = Animator.StringToHash("Died");
    private static readonly int StrafeNormalHash = Animator.StringToHash("StrafeNormal");
    private static readonly int ConfusedHash = Animator.StringToHash("Confused");
    private static readonly int RunningGunAimHash = Animator.StringToHash("RunningGunAim");
    private static readonly int RunningGrenadeAimHash = Animator.StringToHash("RunningGrenadeAim");
    private static readonly int IdleGunAimHash = Animator.StringToHash("IdleGunAim");
    private static readonly int IdleGrenadeAimHash = Animator.StringToHash("IdleGrenadeAim") ;
    private static readonly int IdleGrenadeHash = Animator.StringToHash("IdleGrenade") ;
    private static readonly int IdleGunHash = Animator.StringToHash("IdleGun") ;
    private static readonly int StrafeGunHash = Animator.StringToHash("StrafeGun");

    [Header("Bool")]
    public bool _paused;
    public bool _shallPause;
    public bool _playingCinematic;
    public bool _shieldUp;
    public bool isMobilePlatform;
    public bool _gameFinished;
    public bool _isAnimating;
    public bool _collectedBreakPotion;
    public bool _brokeWall;
    public bool _receivedTheraBox;
    public bool _receivedTheraCode;
    public bool _receivedAsimanaCodex;
    public bool _placedTheraCode;
    public bool _jumpButtonPressed;
    public bool _mainFrameEnabled;
    public bool _beenInAlkemiaChamber;
    public bool _headingToTerathsGate;
    public bool _attackOneOnCooldown;
    public bool _attackTwoOnCooldown;
    public bool _shieldOnCooldown;
    public bool _beenToRecoletteMirror;
    public bool _collectedStarDevice;
    public bool _inOrbsOfKinesis;
    public bool _collectedMiniGameOrbs;
    public bool _insideOrbsEmitter;
    public bool _placedOrbs;
    public bool _collectedLastOrb;
    public bool _fightedKaire;
    public bool _playerenteredSafetyEnclosure;
    public bool _collectedClaw;
    public bool _fixedStoneCourtyard;
    public bool _killedBossMonster;
    public bool _collectedMagicalKey;
    public bool _unlockedMagicalDoor;
    public bool _foundThrowingStar;
    public bool _OrinAndRasveusFixedMira;
    public bool _meronNftOne;
    public bool _collectedAssembledMeronNft;
    public bool _hasPowerPotion;
    public bool isActuallyMoving;
    

    [Header("Materials")]
    public Material _alkemmanaSwitchonMat;

    [Header("Count")]
    public int _monsterSlain;
    public float _elapsedTime;
    public Vector3 previousPosition = new Vector3();
    private Vector3 lastPosition = new Vector3();
    Vector3 pushDirection;
    public Vector3 positionBeforeTeleport;
    public Vector3 grenadeThrowDirection;
    public Vector3 gunShotDirection;
    public Vector3 crossbowShotDirection;
    private Vector3 grenadeInput;
    private Vector3 gunInput;
    private Vector3 crossbowInput;

    [Header("Panel")]
    public GameObject closestEnemy;

    [Header("Values")]
    public float movement_Speed = 3f;
    public float grenadeThrowRange = 1f;
    private float idleThreshold = 5f;
    private float idleTime;
    public float gunShotThrowRange = 1f;
    public string currentAreaName = "";
    public Stack<string> visitedAreas = new Stack<string>();
    public Dictionary<string, bool> playedCinematics = new Dictionary<string, bool>();
    public List<string> missions = new List<string>();
    public Stack<string> knownMonsters = new Stack<string>();
    public MAP _map;

    [Header("Ground Slam Settings")]
    public readonly float jumpForce = 6f;
    public float slamHeight = 2f; // Minimum height to perform a slam
    public float slamForce = 20f; // Downward force applied during slam
    public float slamRadius = 3f; // AoE radius for the slam
    public float slamDamage = 10f;
    public LayerMask damageableLayer;

    #region UI

    public TMP_Text _theraCount;
    public TMP_Text _stepCounttxt;
    public Image _attackOneCooldown;
    public Image _attackTwoCooldown;
    public Image _shieldCooldown;
    public Image _sprintCooldown;
    public Button _showInformation;
    public Button _dontShowInfromation;
    public Button _changeLayout;
    public Button _resetLayout;
    public Sprite _ferraptorPic;
    public Sprite _minaborPic;
    public Sprite _wrathonPic;
    public Sprite _armachillonPic;
    public Sprite _theracodePic;
    public Sprite _theraboxPic;
    public Sprite _magicScrollPic;
    public Sprite _asimanacodexPic;
    public Sprite _fortressPic;

    #endregion

    public bool pause
    {
        get => _paused;
        set => _paused = value;
    }

    public bool shallPause
    {
        get => _shallPause;
        set => _shallPause = value;
    }
    public bool playingCinematic
    {
        get => _playingCinematic;
        set => _playingCinematic = value;
    }
    private void Awake()
    {

    }

    private void Start()
    {
        instance = this;
        shallPause = true;

        #region Listeners

        Joystick.Dropped += Throw;

        _signatureAttackButton.onClick.AddListener(() =>
        {
            Kick();
            //JumpSlam();
        });
        #endregion

        #region Load prefab

        if (_grenadePrefab == null)
        {
            _grenadePrefab = Resources.Load<GameObject>("Prefabs/Weapons/RGD-5");
        }

        #endregion

        #region GetBehaviourComponent

        _rb = GetComponent<Rigidbody>();
        _playerHealthManager = GetComponent<IntrantPlayerHealthManager>();
        #endregion

        lastPosition = transform.position; // Initialize the last position
        idleTime = 0f; // Reset idle time
    }

    private void Kick()
    {
        if (!_signatureCooldownImage.gameObject.activeSelf)
        {
            animator.SetBool(SignatureHash, true);
            StartCoroutine(StartCooldown(5f, _signatureCooldownImage, _signatureAttackButton, true));
        }
    }

    private void Kicked()
    {
        animator.SetBool(SignatureHash, false);
    }

    private void Shoot()
    {
        if (input != Vector3.zero)
        {
            animator.SetBool(RunningGunAimHash, false);
            animator.SetBool(RunningGrenadeAimHash, false);
            animator.SetBool(IdleGunAimHash, false);
            animator.SetBool(IdleGrenadeAimHash, false);
            animator.SetBool(ConfusedHash, false);
        }
        else if(input == Vector3.zero)
        {
            animator.SetBool(RunningGunAimHash, false);
            animator.SetBool(RunningGrenadeAimHash, false);
            animator.SetBool(IdleGunAimHash, false);
            animator.SetBool(IdleGrenadeAimHash, false);
            animator.SetBool(ConfusedHash, false);
        }
        animator.SetBool(ShootHash, true);
        //StartCoroutine(StartCooldown(5f, _shootCooldownImage, _shootButton, true));
        //if there is no active weapon
       
        if (_activeWeaponInstance == null)
        {
            animator.SetBool(StrafeNormalHash, true);
            animator.SetBool(StrafeGunHash, false);
        }
        else if (_activeWeaponInstance != null)
        {
            animator.SetBool(StrafeNormalHash, false);
            animator.SetBool(StrafeGunHash, true);
        }
    }

    public void ShootBulletInit()
    {
        if (_gunInstance != null)
        {
            _gunInstance.GetComponent<WeaponBehaviour>().ShootBullet();
            //Destroy(_gunInstance);
        }
        animator.SetBool(ShootHash, false);
    }

    private void CrossBowShoot()
    {
        if (input != Vector3.zero)
        {
            animator.SetBool(RunningGunAimHash, false);
            animator.SetBool(RunningGrenadeAimHash, false);
            animator.SetBool(IdleGunAimHash, false);
            animator.SetBool(IdleGrenadeAimHash, false);
            animator.SetBool(ConfusedHash, false);
        }
        else if (input == Vector3.zero)
        {
            animator.SetBool(RunningGunAimHash, false);
            animator.SetBool(RunningGrenadeAimHash, false);
            animator.SetBool(IdleGunAimHash, false);
            animator.SetBool(IdleGrenadeAimHash, false);
            animator.SetBool(ConfusedHash, false);
        }
        animator.SetBool(CrossShootHash, true);
        //StartCoroutine(StartCooldown(5f, _shootCooldownImage, _shootButton, true));
        //if there is no active weapon
       
        if (_activeWeaponInstance == null)
        {
            animator.SetBool(StrafeNormalHash, true);
            animator.SetBool(StrafeGunHash, false);
        }
        else if (_activeWeaponInstance != null)
        {
            animator.SetBool(StrafeNormalHash, false);
            animator.SetBool(StrafeGunHash, true);
        }
    }

    public void CrossbowShootInit()
    {
        if (_crossbowInstance != null)
        {
            _crossbowInstance.GetComponent<WeaponBehaviour>().ShootArrow();
            animator.SetBool(CrossShootHash, false);
            //Destroy(_crossbowInstance);
        }
        else
        {
            Debug.LogError("_crossbowInstance is null");
        }
    }

    private void Throw(string val)
    {
        idleTime = 0;

        Debug.LogError("throwing");
        animator.SetBool(ConfusedHash, false);
        animator.SetBool(IdleGunHash, false);
        animator.SetBool(IdleGrenadeHash, false);
        if(_activeWeaponInstance == null)
        {
            animator.SetBool(StrafeNormalHash, true);
            animator.SetBool(StrafeGunHash, false);
        }
        else if(_activeWeaponInstance != null)
        {
            animator.SetBool(StrafeNormalHash, false);
            animator.SetBool(StrafeGunHash, true);
        }
        if (val == "Grenade Joystick")
        {
            //Destroy(_activeWeaponInstance);

            Debug.LogError("throwing grenade");
            if (input != Vector3.zero)
            {
                animator.SetBool(RunningGunAimHash, false);
                animator.SetBool(RunningGrenadeAimHash, false);
                animator.SetBool(IdleGunAimHash, false);
                animator.SetBool(IdleGrenadeAimHash, false);
                animator.SetBool(ConfusedHash, false);
            }
            else if (input == Vector3.zero)
            {
                animator.SetBool(RunningGunAimHash, false);
                animator.SetBool(RunningGrenadeAimHash, false);
                animator.SetBool(IdleGunAimHash, false);
                animator.SetBool(IdleGrenadeAimHash, false);
                animator.SetBool(ConfusedHash, false);
            }
            animator.SetBool(BombHash, true);
            //StartCoroutine(StartCooldown(5f, _bombCooldownImage, _grenadeButton, true));
        }
        else if (val == "Gun Joystick")
        {
            Debug.LogError("Shooting");
            Shoot();
        }
        else if (val == "Crossbow Joystick")
        {
            Debug.LogError("Crossbow shoot");
            CrossBowShoot();
        }
    }

    public void InstantiateAndThrow()
    {
        Destroy(_activeWeaponInstance);
        if (_activeWeaponInstance == null)
        {
            _grenadeInstance = Instantiate(_grenadePrefab, rightHandAnchor.transform.position, rightHandAnchor.transform.rotation);
            _activeWeaponInstance = _grenadeInstance;
        }

        grenadeThrowDirection = CalculateThrowDirection(rightHandAnchor.transform.position,
            _grenadeProjectionMarker.transform.position,
            1.5f,
            1f);
        _grenadeInstance.GetComponent<Rigidbody>().AddForce(grenadeThrowDirection, ForceMode.VelocityChange);
        _grenadeInstance.GetComponent<WeaponBehaviour>()._grenadeTarget = _grenadeProjectionMarker.transform.position;

        animator.SetBool(BombHash, false);
        if (input != Vector3.zero)
        {
            animator.SetBool(RunningGunAimHash, false);
            animator.SetBool(RunningGrenadeAimHash, false);
            animator.SetBool(IdleGunAimHash, false);
            animator.SetBool(IdleGrenadeAimHash, false);
            animator.SetBool(ConfusedHash, false);
        }
        else if (input == Vector3.zero)
        {
            animator.SetBool(RunningGunAimHash, false);
            animator.SetBool(RunningGrenadeAimHash, false);
            animator.SetBool(IdleGunAimHash, false);
            animator.SetBool(IdleGrenadeAimHash, false);
            animator.SetBool(ConfusedHash, false);
        }
        if (_activeWeaponInstance == null)
        {
            animator.SetBool(StrafeNormalHash, true);
            animator.SetBool(StrafeGunHash, false);
        }
        else if (_activeWeaponInstance != null)
        {
            animator.SetBool(StrafeNormalHash, false);
            animator.SetBool(StrafeGunHash, true);
        }
    }

    public void AttackStatus(int val)
    {
        if (val == 0)
        {
            attacking = false;
        }
        else if (val == 1)
        {
            attacking = true;
        }
    }

    private Vector3 CalculateThrowDirection(Vector3 start, Vector3 target, float throwForce, float arcHieght)
    {
        // Calculate the flat direction (horizontal, xz-plane)
        Vector3 flatDirection = new Vector3(target.x - start.x, 0, target.z - start.z);
        float distanceXZ = flatDirection.magnitude;

        // Calculate the height difference
        float yOffset = target.y - start.y;

        // Gravity (positive value)
        float gravity = -Physics.gravity.y;

        // Calculate the time to reach the target (t is based on the maxFlightTime)
        float flightTime = 1f;

        // Calculate the initial velocity in the xz-plane
        Vector3 velocityXZ = flatDirection.normalized * (distanceXZ / flightTime);

        // Calculate the initial velocity in the y-axis (vertical velocity)
        float velocityY = (yOffset + 0.5f * gravity * Mathf.Pow(flightTime, 2)) / flightTime;

        // Combine the horizontal and vertical velocities
        Vector3 velocity = velocityXZ;
        velocity.y = velocityY;

        return velocity;
    }



    #region Trigger

    private void OnTriggerEnter(Collider other)
    {

    }

    #endregion

    #region Pause and Resume

    public void Resume()
    {
        //Time.timeScale = 1f;
        pause = false;
        Debug.Log("Resuming game..");
    }

    public void Pause()
    {
        //Time.timeScale = 0f;
        pause = true;
        Debug.Log("Resuming game..");
    }

    #endregion

    public virtual void ControlAnimatorRootMotion()
    {
        if (!enabled) return;

        if (inputSmooth == Vector3.zero)
        {
            transform.position = animator.rootPosition;
            transform.rotation = animator.rootRotation;
        }

        if (useRootMotion)
            MoveCharacter(moveDirection);
    }

    public virtual void ControlLocomotionType()
    {
        if (pause) return;
        if (lockMovement) return;

        if ((locomotionType.Equals(LocomotionType.FreeWithStrafe) && !isStrafing) ||
            locomotionType.Equals(LocomotionType.OnlyFree))
        {
            SetControllerMoveSpeed(freeSpeed);
            SetAnimatorMoveSpeed(freeSpeed);
        }
        else if (locomotionType.Equals(LocomotionType.OnlyStrafe) ||
                 (locomotionType.Equals(LocomotionType.FreeWithStrafe) && isStrafing))
        {
            isStrafing = true;
            SetControllerMoveSpeed(strafeSpeed);
            SetAnimatorMoveSpeed(strafeSpeed);
        }

        if (!useRootMotion)
            MoveCharacter(moveDirection);
    }

    public virtual void ControlRotationType()
    {
        if (pause) return;
        if (lockRotation) return;

        var validInput = input != Vector3.zero ||
                         (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

        if (validInput)
        {
            // calculate input smooth
            inputSmooth = Vector3.Lerp(inputSmooth, input,
                (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);

            var dir = ((isStrafing && (!isSprinting || sprintOnlyFree == false)) ||
                       (freeSpeed.rotateWithCamera && input == Vector3.zero)) && rotateTarget
                ? rotateTarget.forward
                : moveDirection;
            RotateToDirection(dir);
        }
    }

    public virtual void UpdateMoveDirection(Transform referenceTransform = null)
    {
        if (pause) return;
        if (input.magnitude <= 0.01)
        {
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero,
                (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
            return;
        }

        if (referenceTransform && !rotateByWorld)
        {
            //get the right-facing direction of the referenceTransform
            var right = referenceTransform.right;
            right.y = 0;
            //get the forward direction relative to referenceTransform Right
            var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
            // determine the direction the player will face based on input and the referenceTransform's right and forward directions
            moveDirection = inputSmooth.x * right + inputSmooth.z * forward;
        }
        else
        {
            moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
        }
    }

    public virtual void Sprint(bool value)
    {
        if (pause) return;

        if (!canSprint)
        {
            isSprinting = canSprint;
            return;
        }

        var sprintConditions = input.sqrMagnitude > 0.1f && isGrounded &&
                               !(isStrafing && !strafeSpeed.walkByDefault && (horizontalSpeed >= 0.5 ||
                                                                              horizontalSpeed <= -0.5 ||
                                                                              verticalSpeed <= 0.1f));

        if (value && sprintConditions)
        {
            if (input.sqrMagnitude > 0.1f)
            {
                if (isGrounded && useContinuousSprint)
                    isSprinting = !isSprinting;
                else if (!isSprinting) isSprinting = true;
            }
            else if (!useContinuousSprint && isSprinting)
            {
                isSprinting = false;
            }
        }
        else if (isSprinting)
        {
            isSprinting = false;
        }
    }

    public virtual void Strafe()
    {
        if (pause) return;
        isStrafing = !isStrafing;
    }

    public virtual void Jump()
    {
        if (pause) return;
        // trigger jump behaviour
        jumpCounter = jumpTimer;
        isJumping = true;

        // trigger jump animations
        if (input.sqrMagnitude < 0.1f)
            animator.CrossFadeInFixedTime("Jump", 0.1f);
        else
            animator.CrossFadeInFixedTime("JumpMove", .2f);
    }

    private IEnumerator DisableAnimation(int AttackingHash, float delay)
    {
        isAttacking = true;
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }


    private IEnumerator StartCooldown(float cooldownDuration,
        Image _coolDownImage,
        Button _attackButton,
        bool val)
    {
        var cooldownTimer = cooldownDuration;

        while (cooldownTimer > 0f)
        {
            // Update the cooldown timer
            cooldownTimer -= Time.deltaTime;
            _coolDownImage.fillAmount = cooldownTimer / cooldownDuration;
            yield return null; // Wait for the next frame
        }

        // Cooldown is over, allow the button to be interactable again
        _attackButton.interactable = true;
        _coolDownImage.gameObject.SetActive(false);
        val = false;
    }

    private void FixedUpdate()
    {
        useRootMotion = pause ? !pause : !pause;

        HandleGrenadeThrowInput();

        HandleGunThrowInput();

        HandleCrossBowInput();

        CheckIdleState();

        //SignatureAttack();

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<IntrantPlayerInput>().currentPlatform = PlatformType.PC;
        }

        if (!shallPause && pause)
        {
            pause = false;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {

        }

        if (pause)
        {
            input = Vector3.zero;
            SetAnimatorToIdle();
        }
        if (isAttacking)
        {
            input = Vector3.zero;
            SetAnimatorToIdle();
        }
    }

    public void JumpSlam()
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, jumpForce, _rb.linearVelocity.z); // Apply jump force
        //animator.SetBool(SignatureHash, false);

        //_rb.linearVelocity = new Vector3(0, -slamForce, 0); // Forcefully slam down
    }

    public void SlamCompleted()
    {
        animator.SetBool(SignatureHash, false);
    }

    void GroundSlam()
    {
        animator.SetTrigger("GroundSlam"); // Play slam animation
        _rb.linearVelocity = new Vector3(0, -slamForce, 0); // Forcefully slam down

        // AoE Damage when the monkey lands
        //Invoke(nameof(PerformGroundSlamEffect), 0.2f); // Delay to match animation timing
    }

    void CheckIdleState()
    {
        // Check if the player's position has changed
        if (Vector3.Distance(transform.position, lastPosition) > 0.01f)
        {
            // Player is moving, reset idle time
            idleTime = 0f;
            lastPosition = transform.position; // Update last position
            animator.SetBool(DeadHash, false); // Ensure idle animation stops
                animator.SetBool(ConfusedHash, false);
        }
        else if(_activeWeaponInstance == null)
        {
            // Player is not moving
            idleTime += Time.deltaTime; // Increment idle time

            if (idleTime >= idleThreshold)
            {
                // Trigger idle animation
                animator.SetBool(ConfusedHash, true);
            }
        }
    }

    private void HandleCrossBowInput()
    {
        crossbowInput = new Vector3(_crossbowButton.Horizontal, 0, _crossbowButton.Vertical);
        if (crossbowInput != Vector3.zero)
        {
            _crossbowMarker.SetActive(true);
            PlaceMarker(2);
        }
        else if (crossbowInput == Vector3.zero && _crossbowMarker.activeSelf)
        {
            _crossbowMarker.SetActive(false);
        }
    }

    private void HandleGunThrowInput()
    {
        gunInput = new Vector3(_shootButton.Horizontal, 0, _shootButton.Vertical);
        if (gunInput != Vector3.zero)
        {
            _gunProjectionMarker.SetActive(true);
            PlaceMarker(0);
        }
        else if (gunInput == Vector3.zero && _gunProjectionMarker.activeSelf)
        {
            _gunProjectionMarker.SetActive(false);
        }
    }

    private void HandleGrenadeThrowInput()
    {
        grenadeInput = new Vector3(_grenadeButton.Horizontal, 0, _grenadeButton.Vertical);
        if (grenadeInput != Vector3.zero)
        {
            _grenadeProjectionMarker.SetActive(true);
            PlaceMarker(1);
        }
        else if (grenadeInput == Vector3.zero && _grenadeProjectionMarker.activeSelf)
        {
            _grenadeProjectionMarker.SetActive(false);
        }
    }

    private void PlaceMarker(int val)
    {
        //0 for gun
        //1 for grenade
        //2 for crossbow

        if (val == 0)
        {
            if (_activeWeaponInstance != null)
            {
                if (_gunPrefab == null)
                {
                    _gunPrefab = Resources.Load<GameObject>("Prefabs/Weapons/AK74");
                }
                if (_activeWeaponInstance.name != _gunPrefab.name + "(Clone)")
                {
                    Destroy(_activeWeaponInstance);
                    _gunInstance = Instantiate(_gunPrefab, rightHandAnchor.transform);
                    _activeWeaponInstance = _gunInstance;
                }
                else if (_activeWeaponInstance.name == _gunPrefab.name)
                {
                    _gunInstance = _activeWeaponInstance;
                }
            }
            else if (_activeWeaponInstance == null)
            {
                if (_gunPrefab == null)
                {
                    _gunPrefab = Resources.Load<GameObject>("Prefabs/Weapons/AK74");
                }
                _gunInstance = Instantiate(_gunPrefab, rightHandAnchor.transform);
                _activeWeaponInstance = _gunInstance;
            }

            if (input != Vector3.zero)
            {
                animator.SetBool(RunningGunAimHash, true);
                animator.SetBool(RunningGrenadeAimHash, false);
                animator.SetBool(IdleGunAimHash, false);
                animator.SetBool(IdleGrenadeAimHash, false);
                animator.SetBool(ConfusedHash, false);
            }
            else if (input == Vector3.zero)
            {
                animator.SetBool(RunningGunAimHash, false);
                animator.SetBool(RunningGrenadeAimHash, false);
                animator.SetBool(IdleGunAimHash, true);
                animator.SetBool(IdleGrenadeAimHash, false);
                animator.SetBool(ConfusedHash, false);
            }
            gunShotDirection = new Vector3(_shootButton.Horizontal, 0, _shootButton.Vertical).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(gunShotDirection);

            _gunProjectionMarker.transform.rotation = Quaternion.Slerp(_gunProjectionMarker.transform.rotation,
                targetRotation * Quaternion.Euler(0, 100, 0),
                5f * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                targetRotation,
                5f * Time.deltaTime);
        }
        else if (val == 1)
        {
            if (_activeWeaponInstance == null)
            {
                _grenadeInstance = Instantiate(_grenadePrefab, rightHandAnchor.transform.position, rightHandAnchor.transform.rotation, rightHandAnchor.transform);
                _grenadeInstance.GetComponent<Rigidbody>().isKinematic = true;
                _activeWeaponInstance = _grenadeInstance;
            }

            if (input != Vector3.zero)
            {
                animator.SetBool(RunningGunAimHash, false);
                animator.SetBool(RunningGrenadeAimHash, true);
                animator.SetBool(IdleGunAimHash, false);
                animator.SetBool(IdleGrenadeAimHash, false);
                animator.SetBool(ConfusedHash, false);
            }
            else if (input == Vector3.zero)
            {
                animator.SetBool(RunningGunAimHash, false);
                animator.SetBool(RunningGrenadeAimHash, false);
                animator.SetBool(IdleGunAimHash, false);
                animator.SetBool(IdleGrenadeAimHash, true);
                animator.SetBool(ConfusedHash, false);
            }
            grenadeThrowDirection = new Vector3(_grenadeButton.Horizontal, 0, _grenadeButton.Vertical).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(grenadeThrowDirection);

            _grenadeProjectionMarker.transform.position = new Vector3(transform.position.x + grenadeInput.x * grenadeThrowRange,
                _grenadeProjectionMarker.transform.position.y,
                transform.position.z + grenadeInput.z * grenadeThrowRange);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                targetRotation,
                5f * Time.deltaTime);
        }
        else if (val == 2)
        {
            if (_activeWeaponInstance != null)
            {
                if (_crossbowPrefab == null)
                {
                    _crossbowPrefab = Resources.Load<GameObject>("Prefabs/Weapons/Crossbow_1_Red");
                }
                if (_activeWeaponInstance.name != _crossbowPrefab.name + "(Clone)")
                {
                    Destroy(_activeWeaponInstance);
                    _crossbowInstance = Instantiate(_crossbowPrefab, rightHandAnchor.transform);
                    _activeWeaponInstance = _crossbowInstance;
                }
                else if (_activeWeaponInstance.name == _crossbowPrefab.name)
                {
                    _crossbowInstance = _activeWeaponInstance;
                }
            }
            else if (_activeWeaponInstance == null)
            {
                if (_crossbowPrefab == null)
                {
                    _crossbowPrefab = Resources.Load<GameObject>("Prefabs/Weapons/Crossbow_1_Red");
                }
                _crossbowInstance = Instantiate(_crossbowPrefab, rightHandAnchor.transform);
                _activeWeaponInstance = _crossbowInstance;
            }

            if (input != Vector3.zero)
            {
                animator.SetBool(RunningGunAimHash, true);
                animator.SetBool(RunningGrenadeAimHash, true);
                animator.SetBool(IdleGunAimHash, false);
                animator.SetBool(IdleGrenadeAimHash, false);
                animator.SetBool(ConfusedHash, false);
            }
            else if (input == Vector3.zero)
            {
                animator.SetBool(RunningGunAimHash, false);
                animator.SetBool(RunningGrenadeAimHash, false);
                animator.SetBool(IdleGunAimHash, true);
                animator.SetBool(IdleGrenadeAimHash, true);
                animator.SetBool(ConfusedHash, false);
            }
            crossbowShotDirection = new Vector3(_crossbowButton.Horizontal, 0, _crossbowButton.Vertical).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(crossbowShotDirection);

            _crossbowMarker.transform.rotation = Quaternion.Slerp(_crossbowMarker.transform.rotation,
                targetRotation * Quaternion.Euler(0, 100, 0),
                5f * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    public void SetIdlePostAttack()
    {
        if (_activeWeaponInstance != null )
        {
            if(_crossbowPrefab != null)
            {
                if (_activeWeaponInstance.name == _crossbowPrefab.name + "(Clone)")
                {
                    animator.SetBool(IdleGunHash , true);
                }
            }
            else if (_gunPrefab != null)
            {
                if (_activeWeaponInstance.name == _gunPrefab.name + "(Clone)")
                {
                    animator.SetBool(IdleGunHash, true);
                }
            }
            else if (_grenadePrefab != null)
            {
                if (_activeWeaponInstance.name == _grenadePrefab.name + "(Clone)")
                {
                    animator.SetBool(IdleGrenadeHash, true);
                }
            }
        }
    }

    private void LookAtEnemy()
    {
        closestEnemy = null;
        var closestDistance = Mathf.Infinity;

        // Loop through each enemy
        foreach (var enemy in _enemyController)
        {
            // Calculate the distance between the player and the enemy
            if (enemy == null) continue;
            var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            // Check if the current enemy is closer than the previously closest enemy
            if (distanceToEnemy < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distanceToEnemy;
            }
        }
        if (closestEnemy != null)
        {
            Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;
            direction.y = 0;
            transform.LookAt(transform.position + direction);
            //transform.LookAt(closestEnemy.gameObject.transform);
        }
    }

    public void StandUp()
    {

    }

    public void AttackJump()
    {
        Jump();
    }

    public void EnableDarkness(Collider other)
    {

    }

    private void OnApplicationQuit()
    {
    }

    private void OnDestroy()
    {

    }
}