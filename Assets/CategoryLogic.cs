using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CategoryLogic : MonoBehaviour
{
    [SerializeField] QuestionLogic[] questions;
    [SerializeField] TextMeshProUGUI headerText;

    int qsAnswered = 0;

    public void SetUp(Category category, bool isDoubleJeopardy, System.Action<Question, int, RectTransform> triggerAnimation)
    {
        triggerAnimation += AnswerQ;
        qsAnswered = 0;
        if(questions.Length != 5)
        {
            Debug.LogWarning("Incorrect number of questions");
            return;
        }
        headerText.text = category.catName;
        int mult = isDoubleJeopardy ? 2 : 1;
        for (int i = 0; i < 5; i++)
        {
            questions[i].SetUp(category.qs[i], (i + 1) * 100 * mult, triggerAnimation);
        }
    }

    private void AnswerQ(Question q, int i, RectTransform r)
    {
        qsAnswered++;
        if(qsAnswered == 5)
        {
            headerText.text = "";
        }
    }
}
