using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/**
 * TODO:
 *   - Fix issue where going back to Main Menu screen creates another
 *     event system object
 */
public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject stageSelectMenu;
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private UnityEngine.EventSystems.EventSystem eventSystem;

    private GameObject currentMenu;
    private int currentStage;

    void Start() {
        currentMenu = mainMenu;
        currentStage = -1;

        pauseMenuCanvas.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
    }

    public void LoadStage(int stage) {
        currentStage = stage;
        SceneManager.LoadSceneAsync(string.Format("Scenes/Stages/{0}", stage));
    }

    public void ShowMainMenu() {
        ShowMenu(mainMenu);
    }

    public void ShowStageSelectMenu() {
        ShowMenu(stageSelectMenu);
    }

    public void OpenMainMenu() {
        Resume();
        SceneManager.LoadSceneAsync(0);

        Destroy(pauseMenuCanvas);
        Destroy(eventSystem);
        Destroy(gameObject);
    }

    public void Pause() {
        Time.timeScale = 0;
        pauseMenuCanvas.SetActive(true);
    }

    public void Resume() {
        Time.timeScale = 1;
        pauseMenuCanvas.SetActive(false);
    }

    public void Restart() {
        Resume();
        LoadStage(currentStage);
    }

    public void Exit() {
        Application.Quit();
    }

    private void ShowMenu(GameObject menu) {
        currentMenu.SetActive(false);

        currentMenu = menu;
        currentMenu.SetActive(true);
    }
}
