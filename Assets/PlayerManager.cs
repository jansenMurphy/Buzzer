using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.Interactions;
using System;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public Buzzer buzzer;
    public TMP_InputField nameInput, currentQuestionScore;
    public TextMeshProUGUI primaryAcionLabel, secondaryActionLabel, scoreLabel;
    InputActionRebindingExtensions.RebindingOperation primaryRebindingOp, secondaryRebindingOp;
    public InputActionReference primaryAction, secondaryAction;
    private PlayerInput playerInput;
    [HideInInspector]
    public bool isHost;

    public AudioClip buzzerNoise;

    public Button correct, incorrect;

    private int _score;
    public int score { get { return _score; } set { scoreLabel.text = value.ToString();  _score = value; } }

    public Color playerColor { get; private set; } = Color.red;
    public float buzzFrequencyMod { get; private set; } = 1f;

    private void Start()
    {
        for (int i = 0; i < primaryAction.action.bindings.Count; i++)
        {
            Debug.Log(primaryAction.action.bindings[i].effectivePath);
        }

        buzzer = Buzzer.singleton;
        if (buzzer == null)
            Debug.LogWarning("Null buzzer");
        playerInput = GetComponent<PlayerInput>();
        int bindingIndexPrimary = primaryAction.action.GetBindingIndex(playerInput.currentControlScheme);
        int bindingIndexSecondary = secondaryAction.action.GetBindingIndex(playerInput.currentControlScheme);
        primaryAcionLabel.text = isHost?"Clear Buzzer":"Buzz-in" + " bound to " + InputControlPath.ToHumanReadableString(primaryAction.action.bindings[bindingIndexPrimary].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        secondaryActionLabel.text = isHost ? "Clear Question" : "Nothing" + " bound to " + InputControlPath.ToHumanReadableString(secondaryAction.action.bindings[bindingIndexSecondary].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void PrimaryAction(InputAction.CallbackContext ctx)//Buzz in if contestant, clear buzzer if host
    {
        if (buzzer == null) return;
        if (ctx.performed)
        {
            if (isHost)
                buzzer.Reset();
            else
                buzzer.BuzzIn(this);
        }
    }

    public void SecondaryAction(InputAction.CallbackContext ctx)//Nothing if contestant, clear question if host
    {
        if (buzzer == null) return;
        if (ctx.interaction is HoldInteraction && ctx.performed)
        {
            if (isHost)
            {
                buzzer.Reset();
                buzzer.ClearQuestion();
            }
        }
    }

    public void QuitGame(InputAction.CallbackContext ctx)
    {
        if (ctx.interaction is HoldInteraction && ctx.performed)
        {
            Application.Quit();
        }
    }

    public void ControlsChanged(PlayerInput pi)
    {
        Debug.Log("DING CHANGE");
    }


    public void ShowOrHidePlayerInputs(InputAction.CallbackContext ctx)
    {
        buzzer.ShowOrHidePlayerInputs(ctx);
    }

    public void StartPrimaryRebind()
    {
        if (primaryRebindingOp != null) return;
        Debug.Log("starting rebinding on " + name);
        playerInput.SwitchCurrentActionMap("UI");
        primaryAction.action.Disable();

        primaryRebindingOp = primaryAction.action.PerformInteractiveRebinding(primaryAction.action.GetBindingIndex(playerInput.currentControlScheme))
            .WithControlsExcluding("Mouse")
            .WithControlsExcluding("<Keyboard>/return")
            .OnMatchWaitForAnother(.1f)
            .OnComplete(x => { PrimaryRebindComplete(); })
            .OnCancel(x => { PrimaryRebindCancel(); })
            .Start();
        primaryAcionLabel.text = "Binding Primary Button";
    }
    public void StartSecondaryRebind()
    {
        if (secondaryRebindingOp != null) return;

        playerInput.SwitchCurrentActionMap("UI");
        secondaryAction.action.Disable();
        secondaryRebindingOp = secondaryAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithControlsExcluding("<Keyboard>/return")
            .OnMatchWaitForAnother(.1f)
            .OnComplete(x => { SecondaryRebindComplete(); })
            .OnCancel(x => { SecondaryRebindCancel(); })
            .Start();
        secondaryActionLabel.text = "Binding Secondary Button";
    }

    public void PrimaryRebindComplete()
    {
        Debug.Log("ending rebinding on " + name);
        for (int i = 0; i < primaryAction.action.bindings.Count; i++)
        {
            Debug.Log(primaryAction.action.bindings[i].effectivePath);
        }
        primaryAction.action.Enable();
        int bindingIndexPrimary = primaryAction.action.GetBindingIndex(playerInput.currentControlScheme);
        primaryAcionLabel.text = isHost ? "Clear Buzzer" : "Buzz-in" + " bound to " + InputControlPath.ToHumanReadableString(primaryAction.action.bindings[bindingIndexPrimary].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        playerInput.SwitchCurrentActionMap("Player");
        primaryRebindingOp.Dispose();
        primaryRebindingOp = null;
    }

    public void PrimaryRebindCancel()
    {
        primaryAction.action.Enable();
        int bindingIndexPrimary = primaryAction.action.GetBindingIndex(playerInput.currentControlScheme);
        primaryAcionLabel.text = isHost ? "Clear Buzzer" : "Buzz-in" + " bound to " + InputControlPath.ToHumanReadableString(primaryAction.action.bindings[bindingIndexPrimary].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        primaryRebindingOp.Dispose();
        primaryRebindingOp = null;
    }
    public void SecondaryRebindComplete()
    {
        secondaryAction.action.Enable();
        int bindingIndexSecondary = secondaryAction.action.GetBindingIndex(playerInput.currentControlScheme);
        secondaryActionLabel.text = isHost ? "Clear Question" : "Nothing" + " bound to " + InputControlPath.ToHumanReadableString(secondaryAction.action.bindings[bindingIndexSecondary].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice); playerInput.SwitchCurrentActionMap("Player");
        secondaryRebindingOp.Dispose();
        secondaryRebindingOp = null;
    }

    public void SecondaryRebindCancel()
    {
        secondaryAction.action.Enable();
        int bindingIndexSecondary = secondaryAction.action.GetBindingIndex(playerInput.currentControlScheme);
        secondaryActionLabel.text = isHost ? "Clear Question" : "Nothing" + " bound to " + InputControlPath.ToHumanReadableString(secondaryAction.action.bindings[bindingIndexSecondary].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice); playerInput.SwitchCurrentActionMap("Player");
        secondaryRebindingOp.Dispose();
        secondaryRebindingOp = null;
    }

    public void DeviceLost(PlayerInput pi)
    {
        buzzer.StartTextAlert("Player " + name + " disconnected!");
    }

    public void ChangeColor(Color color)
    {
        playerColor = color;
    }

    public void ChangeFrequency(float freq)
    {
        buzzFrequencyMod = freq;
    }

    public void ChangeIsHost(bool isHost)
    {
        this.isHost = isHost;
    }

    public void SetScore(string endvalue)
    {
        score = int.Parse(endvalue);
    }

    public void AddScore()
    {
        score += int.Parse(currentQuestionScore.text);
    }

    public void SubtractScore()
    {
        score -= int.Parse(currentQuestionScore.text);
    }
}
