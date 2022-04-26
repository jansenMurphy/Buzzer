using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestionLogic : MonoBehaviour
{
    Question question;
    int value;

    Button btn;
    TextMeshProUGUI tmp;

    Action<Question, int,RectTransform> triggerAnimation;
    RectTransform rt;

    private void Awake()
    {
        btn = GetComponent<Button>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
    }

    public void SetUp(Question question, int value, Action<Question,int,RectTransform> triggerAnimation)
    {
        this.value = value;
        this.question = question;
        tmp.text = value.ToString();
        btn.interactable = true;
        tmp.enabled = true;
        this.triggerAnimation = triggerAnimation;
    }

    public void Clear()
    {
        btn.interactable = false;
        tmp.enabled = false;
    }

    public void OnChoose()
    {
        if (triggerAnimation != null)
        {
            triggerAnimation.Invoke(question, value,rt);
        }else
            Debug.LogWarning("Null animation");
    }

    private IEnumerator DelayAndDisappear()
    {
        yield return new WaitForSeconds(2f);
        Clear();
    }
}
