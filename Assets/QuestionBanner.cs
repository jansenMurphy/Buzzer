using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionBanner : MonoBehaviour
{
    [SerializeField] RectTransform rt;
    [SerializeField] TextMeshProUGUI questionText;
    const float LERP_TIME = .25f;
    public Image img;
    public AudioClip dailyDoubleAudio;
    public AudioSource audioSource;

    private bool isShowingDD = false;
    string questionString = "";

    IEnumerator cor = null;
    public void StartQuestion(Question q, int money, RectTransform startRect)
    {
        img.enabled = true;
        questionText.enabled = true;
        if (cor != null)
        {
            StopCoroutine(cor);
        }
        if (q.dd)
        {
            audioSource.PlayOneShot(dailyDoubleAudio);
            questionText.text = "Daily Double";
            questionString = q.question;
            isShowingDD = true;
        }
        else
        {
            questionText.text = q.question;
        }
        StartCoroutine(cor = QuestionAnim(q, money, startRect));
    }

    public void ClearQuestion()
    {
        if (isShowingDD)
        {
            isShowingDD = false;
            questionText.text = questionString;
        }
        else
        {
            img.enabled = false;
            questionText.enabled = false;
        }
    }

    private IEnumerator QuestionAnim(Question q, int money, RectTransform startRect)
    {
        Vector3[] corners = new Vector3[4];
        startRect.GetWorldCorners(corners);
        Vector2 startMin = new Vector2(corners[0].x / Screen.width, corners[0].y / Screen.height);
        Vector2 startMax = new Vector2(corners[2].x / Screen.width, corners[2].y / Screen.height);

        if (q.dd)
        {
            //TODO
            yield return new WaitForSeconds(1);
        }
        else
        {
            float startTime = Time.time;
            float curTime;
            while((curTime = Time.time) < startTime + LERP_TIME)
            {
                rt.anchorMin = Vector2.Lerp(startMin, Vector2.zero, (curTime - startTime) / LERP_TIME);
                rt.anchorMax = Vector2.Lerp(startMax, Vector2.one, (curTime - startTime) / LERP_TIME);
                yield return new WaitForEndOfFrame();
            }
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
        }
    }
}
