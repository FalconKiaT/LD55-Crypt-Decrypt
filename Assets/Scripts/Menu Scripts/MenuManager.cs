using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Levels to Load")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noGameDialogue;

    [Header("Pause Settings")]
    public static bool isPaused;
    [SerializeField] private GameObject pauseMenu;

    [Header("Death Controller")]
    public bool isDead;
    [SerializeField] private GameObject deathObject;

    private void Update()
    {
        // Pauses the game on escape if not dead
        if (Input.GetKeyDown(KeyCode.Escape) && !isDead) { ChangePause(); }

        // Pauses the game
        if (isPaused) { Time.timeScale = 0; }
        else { Time.timeScale = 1; }

        // Displays death screen if the player dies
        if (isDead) { DeathScreen(); }
    }

    public void ChangePause() // displays pause menu and pauses game
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
    }

    public void SaveGame() // writes new save file based on current level
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("SavedLevel", currentLevel);
    }

    public void ExitLevel() // Goes to Main Menu
    {
        SceneManager.LoadScene("Menus");
    }

    public void LoadNewGame() // creates a new save file and starts player there
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadSavedGame() // Loads the last saved level
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            isDead = false;
            deathObject.SetActive(false);
            isPaused = false;

            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else if (!isDead)
        {
            noGameDialogue.SetActive(true);
        }
        else
        {
            isDead = false;
            deathObject.SetActive(false);
            isPaused = false;

            ExitLevel();
        }

    }

    public void DeathScreen() // Creates death scenario
    {
        deathObject.SetActive(true);
        isPaused = true;
    }

    public void ExitGame() // Exits the whole game
    {
        Application.Quit();
    }

    public void EndGame()
    {
        SceneManager.LoadScene("EndScene");
    }
}
