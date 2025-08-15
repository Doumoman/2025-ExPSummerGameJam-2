using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private static StageManager instance = null;
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    string currentScene = "";
     
    public void LoadGameScene()
    {
        SceneManager.LoadScene("DasanTestScene");
        currentScene = "DasanTestScene";

        //ScoreManager.Instance.StartStage(ScoreManager.Instance.stage)
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("StartScene");
        currentScene = "StartScene";
    }

    public void ReLoadScene()
    {
        SceneManager.LoadScene(currentScene);
    }
}
