using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JeopardyBoard : MonoBehaviour
{
    public JeopardyBoardData jbd { get; private set; }
    [SerializeField] string sceneToLoad;
    public static JeopardyBoard singleton;
    [SerializeField] TextAsset backupBoard;

    private void Awake()
    {
        if (singleton != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            singleton = this;
            BackupLoad(JsonUtility.FromJson<JeopardyBoardData>(backupBoard.text));
        }
    }

    public void LoadGame(JeopardyBoardData jbd)
    {
        this.jbd = jbd;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);
    }

    public void BackupLoad(JeopardyBoardData jbd)
    {
        this.jbd = jbd;
    }

    public void OutputBlankBoard(string boardName)
    {
        var jbd = new JeopardyBoardData();

        jbd.sj = new Category[6];
        jbd.dj = new Category[6];
        for (int i = 0; i < 6; i++)
        {
            jbd.sj[i] = new Category();
            jbd.dj[i] = new Category();
            for (int j = 0; j < 5; j++)
            {
                jbd.sj[i].qs = new Question[5];
                jbd.dj[i].qs = new Question[5];
                for (int k = 0; k < 5; k++)
                {
                    jbd.sj[i].qs[j] = new Question();
                    jbd.dj[i].qs[j] = new Question();
                }
            }
        }
        StreamWriter sw = new StreamWriter(Path.Combine(Application.persistentDataPath, boardName+ ".json"));
        sw.Write(JsonUtility.ToJson(jbd));
        Debug.Log(JsonUtility.ToJson(jbd));
        sw.Flush();
        sw.Close();
    }

}

[System.Serializable]
public class JeopardyBoardData
{
    public Category[] sj, dj;
}

[System.Serializable]
public class Category
{
    public string catName;
    public Question[] qs;
}

[System.Serializable]
public class Question
{
    public bool dd = false;//Daily double
    public string question;
}