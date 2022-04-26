using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInteraction : MonoBehaviour
{
    JeopardyBoard jboard;
    CategoryLogic[] catLogs;
    public QuestionBanner qBanner; 
    public TMPro.TMP_InputField questionMoneyValue;

    void Awake()
    {
        jboard = JeopardyBoard.singleton;
        if(jboard == null)
        {
            Debug.LogWarning("No board found");
            return;
        }
        catLogs = GetComponentsInChildren<CategoryLogic>();
    }
    private void Start()
    {
        DrawBoard(false);
    }

    private void DrawBoard(bool isDoubleJeopardy)
    {
        var cats = isDoubleJeopardy ? jboard.jbd.dj : jboard.jbd.sj;

        System.Action<Question, int, RectTransform> onChooseQuestionEvent = qBanner.StartQuestion;
        onChooseQuestionEvent += (x, y, z) =>
        {
            questionMoneyValue.text = y.ToString();
        };

        for (int i = 0; i < 6; i++)
        {
            catLogs[i].SetUp(cats[i], isDoubleJeopardy, onChooseQuestionEvent);
        }
    }
}
