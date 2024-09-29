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
    [SerializeField] public GameObject [] externalCanvases;
    [SerializeField] public GameObject _virtualCamera;
    [SerializeField] public GameObject _kairRef;

    [Header("UI Ref")]
    public Button _signatureAttack;
    public Button _bomb;
    public Button _shoot;
    public TMP_Text _ObjectInfoContent;
    public TMP_Text _ObjectInfoTitle;
    public Image _otherInformationPic;
    public CanvasGroup _shoulder;
    public CanvasGroup []_forearms;
    public CanvasGroup _body;
    private readonly GameObject _shieldFxInstance = null;
    public GameObject _playerMinimapPointer;
    public GameObject _checkPointFx;
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
    private static readonly int BombHash = Animator.StringToHash("ThrowGrenade");
    private static readonly int SignatureHash = Animator.StringToHash("Kick");
    private static readonly int PowerUpHash = Animator.StringToHash("PowerUp");
    private static readonly int ShieldHash = Animator.StringToHash("Shield");
    private static readonly int DeadHash = Animator.StringToHash("Died");

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
    public int _switchedOnCount;
    public int _collectedTheraCode;
    public int _collectedTheraBox;
    public int _collectedMagicalScroll;
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
    public int _mapIndex;
    public int _missionNumber;
    public int _stepCount;
    public float _elapsedTime;
    public float _checkIntervel = 0.5f;
    public float distanceMoved = 0.0f;
    public Vector3 previousPosition = new Vector3();
    Vector3 pushDirection;
    public Vector3 positionBeforeTeleport;

    [Header("Panel")] 
    public GameObject _grimoirPanel;
    public GameObject _potionPanel;
    public GameObject _cutscenePanel;
    public GameObject _layoutPanel;
    public GameObject _weaponPowerUpPanel;
    public GameObject _mirasChallengePanel;
    public CanvasGroup _darkPanel;
    public GameObject _canvas;
    public GameObject _gameOverCanvas;
    public GameObject _settingPanel;
    public GameObject _revivePanel;
    public GameObject closestEnemy;
    public GameObject kairHealthCanvas;
    public Image _kairFill;

    public float movement_Speed = 3f;
    public string currentAreaName = "";
    public Stack<string> visitedAreas = new Stack<string>();
    public Dictionary<string, bool> playedCinematics = new Dictionary<string, bool>();
    public List<string> missions = new List<string>();
    public Stack<string> knownMonsters = new Stack<string>();
    public MAP _map;

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
        _bomb.onClick.AddListener(() =>
        {
            ThrowBomb();
        });

        _shoot.onClick.AddListener(() =>
        {
            Shoot();
        });
        #endregion

        #region GetBehaviourComponent

        _rb = GetComponent<Rigidbody>();
        _playerHealthManager = GetComponent<IntrantPlayerHealthManager>();
        #endregion
    }

    private void Shoot()
    {
        animator.SetBool(ShootHash,true);
    }

    private void ThrowBomb()
    {
        animator.SetBool(BombHash,true);
    }

    public void InstantiateAndThrow()
    {

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
        _grimoirPanel.SetActive(false);
    }

    public void Pause()
    {
        //Time.timeScale = 0f;
        pause = true;
        Debug.Log("Resuming game..");
        _grimoirPanel.SetActive(true);
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

    private IEnumerator InitiateAttacKOneSliceVfx(int index, float delay)
    {
        if (isAttacking) yield return null;

        isAttacking = true;
        yield return new WaitForSeconds(1f);
        _powerUpAura.SetActive(true);
        _attackOnefirstSlice.SetActive(true);
        yield return new WaitForSeconds(1f);
        _attackOnefirstSlice.SetActive(false);
        _attackOneSecondSlice.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        _attackOneSecondSlice.SetActive(false);
        _attackOneThirdSlice.SetActive(true);
        yield return new WaitForSeconds(0.10f);
        _attackOneThirdSlice.SetActive(false);
        _powerUpAura.SetActive(false);
        isAttacking = false;
    }

    private IEnumerator InitiateAttacKOneWeaponCollider(int index, float delay)
    {
        if (!isAttacking) yield return null;

        if (index == 1 && isAttacking)
        {
            yield return new WaitForSeconds(delay * 0.3f);
            _hammerCollider.enabled = true;
            yield return new WaitForSeconds(delay * 0.7f);
            _hammerCollider.enabled = false;
        }
        else if (index == 2 && isAttacking)
        {
            yield return new WaitForSeconds(delay * 0.4f);
            _hammerCollider.enabled = true;
            yield return new WaitForSeconds(delay * 0.6f);
            _hammerCollider.enabled = false;
        }
        else
        {
            yield return null;
        }
    }

    private IEnumerator InitiateSliceVfx(int index, float delay)
    {
        if (isAttacking) yield return null;
        isAttacking = true;
        yield return new WaitForSeconds(0.8f);
        _powerUpAura.SetActive(true);
        _attacktwoFirstSlice.SetActive(true);
        yield return new WaitForSeconds(1f);
        _attacktwoFirstSlice.SetActive(false);
        _attacktwoSecondSlice.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        _attacktwoSecondSlice.SetActive(false);
        _powerUpAura.SetActive(false);
        isAttacking = false;
    }

    IEnumerator WeaponCleave()
    {
        yield return new WaitForSecondsRealtime(1f);
        _rb.AddForce(transform.up * 50f, ForceMode.Acceleration);
        //_rigidbody.AddForce(transform.up * (extraGravity * 2 * Time.deltaTime), ForceMode.VelocityChange);
        yield return new WaitForSecondsRealtime(1.09f);
        GameObject _cleave = Instantiate(_hammerCleave,_cleaveSpts.position, _cleaveSpts.rotation);
        Destroy(_cleave, 1.5f);
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

        UpdateCheckInTime();

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<IntrantPlayerInput>().currentPlatform = PlatformType.PC;
        }

        if(!shallPause && pause) {
            pause = false;
        }

        if(Input.GetKeyDown(KeyCode.P))
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

        #region Map wise conditions

        #endregion
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

    private void UpdateCheckInTime()
    {
        _elapsedTime += Time.deltaTime;

        if(_elapsedTime > _checkIntervel)
        {
            CheckIfRunningInPlace();
            _elapsedTime = 0;
        }
    }

    private void CheckIfRunningInPlace()
    {
        distanceMoved = Vector3.Distance(transform.position, previousPosition);
        isActuallyMoving = distanceMoved > 0.7f;
        if(isActuallyMoving )
        {
            previousPosition = transform.position;
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