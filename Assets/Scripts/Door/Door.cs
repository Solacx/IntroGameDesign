using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private int nextStage;

    private string stagesFolder = "Scenes/Stages/";

    public void GoToNextStage() {
        SceneManager.LoadSceneAsync(stagesFolder + nextStage);
    }
}
