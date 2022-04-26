using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwatchLogic : MonoBehaviour
{

    PlayerManager pm;
    Color c;
    [SerializeField] GameObject selector;
    public float freq = 1;
    public AudioClip buzzerNoise;


    void Start()
    {
        pm = GetComponentInParent<PlayerManager>();
        c = GetComponent<UnityEngine.UI.Image>().color;
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ChangeColor);
    }

    public void ChangeColor()
    {
        pm.ChangeColor(c);
        Debug.Log(c);
        selector.transform.position = transform.position;
        //pm.ChangeFrequency(freq);
        pm.buzzerNoise = buzzerNoise;
    }
}
