using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct youregay
{
    float haha;
};


public enum Scenes
{
    MainMenu,           // 0
    Level_1,            // 1
    Level_1_Checkpoint, // 2
    Level_2,            // 3
    Level_2_Checkpoint  // 4
}

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance = null;     // Singleton instance

    [SerializeField] private bool debug = false;

    public static int currentScene { get; private set; }
    public static int currentLevel = 1;
    private Dictionary<Scenes, bool> unlockedScenes;

    private void Awake()
    {
        // Singleton: Checks if a scene object is currently in use and destroys it if true
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;

        InitializeDict();
    }

    private void InitializeDict()
    {
        unlockedScenes = new Dictionary<Scenes, bool>();

        // TODO: Initialize Dictionary here
        // { Scenes.MainMenu, true },
    }

    // Gets the name of the scene and loads it 
    public void LoadScene(Scenes scene)
    {
        if (!unlockedScenes.ContainsKey(scene))
        {
            Debug.LogError("Scene does not exist");
            return;
        }

        SceneManager.LoadScene((int)scene);
    }

    public void LoadNextScene()
    {
        currentScene++;

        Debug.Log(currentScene);
        if (currentScene > 4)
        {
            LoadScene(Scenes.MainMenu);
            currentScene = 0;
        }

        SceneManager.LoadScene(currentScene);
    }

    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(currentScene);
    }

    public void CheckpointReached()
    {
        if (currentScene % 2 == 1)
            currentScene++;
    }

    public void LoadNextLevel()
    {
        Debug.Log(currentScene);
        if (currentScene % 2 == 1)
            currentScene++;

        LoadNextScene();
    }

    public void UnlockScene(Scenes scene)
    {
        if (!unlockedScenes.ContainsKey(scene))
        {
            Debug.LogError("Error: Scene does not exist");
            return;
        }

        unlockedScenes[scene] = true;
    }
}

