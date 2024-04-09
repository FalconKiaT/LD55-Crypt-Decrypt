using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    
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

    public bool CheckSceneUnlocked(Scenes scene)
    {
        if (!unlockedScenes.ContainsKey(scene))
        {
            Debug.LogError("Scene does not exist");
            return false;
        }

        if (unlockedScenes[scene])
        {
            if (debug) Debug.Log("Player has access to scene.");
            return true;
        }
        else
        {
            if (debug) Debug.Log("Player does not have access to this scene yet.");
            return false;
        }
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
        SceneManager.LoadScene(currentScene);
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

    public void LoadLevel(int level)
    {
        switch (level)
        {
            case 1:
                // TODO: load lvl 1
                // LoadScene(Scenes.Level_1)
                break;
            case 2:
                // TODO: load lvl 2
                break;
            case 3:
                // TODO: load lvl 3
                break;
            default:
                Debug.LogError("Error: Level does not exist");
                break;
        }
    }

    public bool CheckLevel(int level)
    {
        switch (level)
        {
            case 1:
                // TODO: check if lvl 1 is unlocked
                // return CheckScene(Scenes.Level_1)
            case 2:
                // TODO: check if lvl 2 is unlocked
            case 3:
                // TODO: check if lvl 3 is unlocked
            default:
                return false;
        }
    }
}

