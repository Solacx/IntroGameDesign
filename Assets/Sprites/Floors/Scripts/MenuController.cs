using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject stageSelectMenu;

    private string stagesFolder = "Scenes/Stages/";

    public void LoadStage(int stageNumber) {
        SceneManager.LoadSceneAsync(stagesFolder + stageNumber);
    }
    
    public void ShowMainMenu() {
        DeactivateMenus();
        mainMenu.SetActive(true);
    }

    public void ShowStageSelectMenu() {
        DeactivateMenus();
        stageSelectMenu.SetActive(true);
    }

    private void DeactivateMenus() {
        mainMenu.SetActive(false);
        stageSelectMenu.SetActive(false);
    }
}
