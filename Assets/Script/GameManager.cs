using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    PlayerInput input;

    public static GameManager Instance {
        get {
            return instance != null ? instance : null;
        }
    }

    void Awake () {
        if(instance != null){
            Destroy(gameObject);
            return;
        }

        instance = this;

        input = new PlayerInput();
        
        MarkerManager.Init();
    }

    void Start ()
    {
        VuforiaARController vuforia = VuforiaARController.Instance;
        vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        vuforia.RegisterOnPauseCallback(OnPaused);

        input.Enable();
    }

    public void Retry () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit () {
        Application.Quit();
    }
     
    void OnVuforiaStarted()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }
     
    void OnPaused(bool paused)
    {
        if (!paused)
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }

    public PlayerInput Input {
        get {
            return input;
        }
    }
}
