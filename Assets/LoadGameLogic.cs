using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadGameLogic : MonoBehaviour
{
    private string filePath;
    [SerializeField] JeopardyBoard jb;

    public void TryLoadGame()
    {
        try
        {
            jb.LoadGame(JsonUtility.FromJson<JeopardyBoardData>(File.ReadAllText(filePath)));
        }
        catch (Exception e)
        {
            Debug.Log("Nope " +e);
        }
    }

    public void ChangeFilePath(string fp)
    {
        filePath = fp;
    }
}
