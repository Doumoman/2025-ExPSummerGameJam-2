using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    #region Singleton
    private static StageManager _instance;
    public static StageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<StageManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("InGameManager");
                    _instance = go.AddComponent<StageManager>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Initialize()
    {
        
    }
    #endregion

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
