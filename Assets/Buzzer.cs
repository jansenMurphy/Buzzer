using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Buzzer : MonoBehaviour
{
    public static Buzzer singleton;
    private bool buzzerLockedIn = false;
    //private PlayerManager buzzedInPlayer;
    //public Light buzzerLight;
    public UnityEngine.UI.Image buzzerLight;
    public TextMeshProUGUI playerBuzzInText;
    public bool isEditingPlayerInfo = true;
    [SerializeField] private AudioClip clownHornBuzz, resetClick;
    [SerializeField] Canvas changeControls;//, mainGame;
    private bool isInMainGame = true, currentlyClicked=false;
    private AudioSource audioSource;

    [SerializeField] private Transform playerGridField;

    public Question currentQuestion;
    public int currentQuestionValue;

    public TMP_InputField currentQuestionValueField;

    public QuestionBanner questionBanner;


    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void BuzzIn(PlayerManager pm)
    {
        if (buzzerLockedIn || !isInMainGame) return;
        buzzerLockedIn = true;
        //buzzedInPlayer = pm;
        SetLightColor(pm.playerColor);
        playerBuzzInText.text = pm.name;
        if (Random.Range(0, 1000) == 0)
            audioSource.PlayOneShot(clownHornBuzz);
        else
        {
            audioSource.pitch = pm.buzzFrequencyMod;
            audioSource.PlayOneShot(pm.buzzerNoise);
        }
    }

    public void Reset()
    {
        if (!isInMainGame || !buzzerLockedIn) return;
        buzzerLockedIn = false;
        SetLightColor(Color.clear);
        SetBuzzInText("");
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(resetClick);
    }

    public void StartTimerLockout()
    {
        //TODO
    }

    public void SetLightColor(Color color)
    {
        buzzerLight.color = color;
    }

    public void SetBuzzInText(string txt)
    {
        playerBuzzInText.text = txt;
    }

    public void PlayerJoin(PlayerInput pi)
    {
        pi.gameObject.transform.SetParent(playerGridField);
        pi.GetComponent<PlayerManager>().currentQuestionScore = currentQuestionValueField;
    }

    public void ShowOrHidePlayerInputs(InputAction.CallbackContext ctx)
    {
        if(currentlyClicked)
        {
            if (ctx.ReadValueAsButton() == false)
                currentlyClicked = false;
        }
        else
        {
            currentlyClicked = true;
            if(isInMainGame = !isInMainGame)
            {
                changeControls.enabled = false;
                //mainGame.enabled = true;
            }
            else
            {
                changeControls.enabled = true;
                //mainGame.enabled = false;
            }
        }
    }

    public void ClearQuestion()
    {
        questionBanner.ClearQuestion();
    }

    #region largeAlert
    public TextMeshProUGUI wideAlertText;
    IEnumerator currentAlert;
    public void StartTextAlert(string alertMessage)
    {
        if(currentAlert != null)
        {
            StopCoroutine(currentAlert);
            currentAlert = null;
        }
        StartCoroutine(currentAlert = StartTextAlertCorutine(alertMessage));
    }
    IEnumerator StartTextAlertCorutine(string alertMessage)
    {
        wideAlertText.text = alertMessage;
        float currentTime, startTime, decayTime=.5f,strongTime=.5f;

        wideAlertText.color = Color.black;
        yield return new WaitForSeconds(strongTime);
        startTime = Time.time;
        while((currentTime = Time.time)-startTime < decayTime)
        {
            wideAlertText.color = Color.Lerp(Color.black, Color.clear, (currentTime - startTime) / decayTime);
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
