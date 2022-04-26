using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSecondDisplay : MonoBehaviour
{
    public Buzzer buzzer;
    IEnumerator Start()
    {
        while(Display.displays.Length < 2)
        {
            buzzer.StartTextAlert("Please connect second display");
            yield return new WaitForSeconds(3);
        }
        Display.displays[1].Activate();
    }
}
