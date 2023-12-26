using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public string[] TagsToKill;
    public string CurrentScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col) {
        foreach (string Tag in TagsToKill) {
            if (col.gameObject.CompareTag(Tag)) {
                LoadNextLevel();
            }
        }
    }
    void LoadNextLevel() {
        SceneLoading SceneLoader = GameObject.FindWithTag("SceneLoader").GetComponent<SceneLoading>();
        Debug.Log(CurrentScene);
        
        bool FoundScene = false;

        for (int i = 0; i < SceneLoader.LevelNames.Length; i++) {
            if (SceneLoader.LevelNames[i] == CurrentScene) {
                if (i+1 < SceneLoader.LevelNames.Length) {
                    SceneManager.LoadScene(SceneLoader.LevelNames[i+1]);
                    FoundScene = true;
                    break;
                }
                else {
                    Debug.LogWarning("FinalSceneInList");
                    FoundScene = true;
                    break;
                }
            }
        }
        if (!FoundScene) {
            Debug.LogWarning("SceneName Not Found");
        }
    }
}
