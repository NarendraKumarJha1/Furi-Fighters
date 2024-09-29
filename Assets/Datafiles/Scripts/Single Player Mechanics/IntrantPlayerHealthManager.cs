using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntrantPlayerHealthManager : MonoBehaviour
{
    public static IntrantPlayerHealthManager instance;
    private IntrantThirdPersonController playerController;
    [SerializeField] public int maxHealth;
    [SerializeField] public float currentHealth;

    [SerializeField] public Image _healthFill;
    [SerializeField] public TMP_Text _healthTxt;

    [SerializeField] public CanvasGroup _damageEffect;
    [SerializeField] public Sprite []_damageEffects;

    [SerializeField] public float groundHieght;
    [SerializeField] public GameObject _bloodFx;
    public GameObject _damagePoint;
    GameObject _damagePointInstance;
    [SerializeField] public GameObject []_bloodSplatter;
    [SerializeField] public GameObject BloodAttach;
    GameObject _bloodFxInstance;


    public event Action PlayerOnDie;
    public static event Action OnHealthUpdated;
    

    private void Awake()
    {
        instance = this;
        
        currentHealth = maxHealth;

        playerController = GetComponent<IntrantThirdPersonController>();
        OnHealthUpdated += UpdateHealthStatus;
        PlayerOnDie += DisablePlayeBehaviour;
        OnHealthUpdated?.Invoke();
    }

    private void OnDestroy()
    {
        OnHealthUpdated -= UpdateHealthStatus; 
        
        PlayerOnDie -= DisablePlayeBehaviour;
    }

    public void DealDamage(float damageAmount)
    {
        if(playerController.pause)
            return;

        if(currentHealth == 0)
            return;

        currentHealth = (Mathf.Max(currentHealth - damageAmount, 0));

        playerController.animator.SetTrigger("TookHit");

        StartCoroutine(ShowBloodtoast(damageAmount));
        
        OnHealthUpdated?.Invoke();
        
        if(currentHealth != 0){return;}

        ActivateGameOver_Panel(true);

        Debug.Log("We Died");
    }

    IEnumerator ShowBloodtoast(float damageAmount)
    {
        if (_bloodFxInstance != null) yield return null;
     
        
        Vector3 _pos = new Vector3(transform.position.x + UnityEngine.Random.Range(UnityEngine.Random.Range(-1, -0.5f), UnityEngine.Random.Range(0.5f, 1)),
               transform.position.y + 2.5f, transform.position.z + UnityEngine.Random.Range(UnityEngine.Random.Range(-1, -0.5f), UnityEngine.Random.Range(0.5f, 1)));
        _damagePointInstance = Instantiate(_damagePoint, _pos, Quaternion.identity, transform);
        if (damageAmount < 1)
        {
            damageAmount = damageAmount * 10;
        }
        _damagePointInstance.GetComponent<TextMeshPro>().text = "-" + damageAmount.ToString("F0");
        Destroy(_damagePointInstance, 1f);


        float angle = Mathf.Atan2(transform.position.x, transform.position.z) * Mathf.Rad2Deg + 180;

        _bloodFxInstance = Instantiate(_bloodSplatter[UnityEngine.Random.Range(0, _bloodSplatter.Length)],
            new Vector3(transform.position.x, transform.position.y+1f, transform.position.z),
            Quaternion.Euler(0, angle + 90, 0));
        Destroy(_bloodFxInstance, 20f);

        var attachBloodInstance = Instantiate(BloodAttach);
        var bloodT = attachBloodInstance.transform;
        bloodT.position = new Vector3(transform.position.x, transform.position.y+0.4f, transform.position.z);
        bloodT.localRotation = Quaternion.identity;
        bloodT.localScale = Vector3.one * UnityEngine.Random.Range(0.75f, 1.2f);
        Destroy(attachBloodInstance, 20f);

        _damageEffect.alpha = 1;
        _damageEffect.GetComponent<Image>().sprite = _damageEffects[UnityEngine.Random.Range(0, _damageEffects.Length)];
        yield return new WaitForSeconds(1.5f);
    }

    private void UpdateHealthStatus()
    {
    }
    
    public void Increasehealth(int val)
    {
        if(val == maxHealth)
            ActivateGameOver_Panel(false);

        if(currentHealth == maxHealth)
        {
            return;
        }
        
        Debug.LogError("Increasing health....");
        currentHealth = currentHealth>maxHealth? maxHealth : Mathf.Max(currentHealth + val, currentHealth);
        OnHealthUpdated?.Invoke();
    }


    private void DisablePlayeBehaviour()
    {
        StartCoroutine(DisableBehaviour());
    }

   IEnumerator DisableBehaviour()
    {
        Debug.LogWarning("Disabling behaviour");
        //Dead_BloodSplatter.SetActive(true);
        GetComponent<IntrantThirdPersonController>().animator.SetTrigger("Death");
        GetComponent<IntrantThirdPersonController>().enabled = false;
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().enabled = false;
        if (playerController._gameOverCanvas != null)
            ActivateGameOver_Panel(true);
        Destroy(this.gameObject, 2f);
        this.enabled = false;
        Debug.LogWarning("Disabled behaviour");
    }
    void ActivateGameOver_Panel(bool val)
    {
        //playerController._gameOverCanvas.SetActive(val);
    }

    internal void ReduceHealth(int val)
    {
        if (currentHealth > val/2)
        {
            DealDamage(val/2);
        }
    }
}
