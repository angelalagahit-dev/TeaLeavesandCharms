using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public GameObject[] toolbarActivatorIcons;

    public TMP_Text timeText;

    public GameObject pauseScreen;
    public string mainMenuScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            PauseUnpause();
        }
    }

    public void SwitchTool(int selected)
    {
        foreach(GameObject icon in toolbarActivatorIcons)
        {
            icon.SetActive(false);
        }

        toolbarActivatorIcons[selected].SetActive(true);
    }

    public void UpdateTimeText(float currentTime)
    {
        if(currentTime < 12)
        {
            timeText.text = Mathf.FloorToInt(currentTime) + "am";
        } else if(currentTime < 13)
        {
            timeText.text = "12pm";
        } else if(currentTime < 24)
        {
            timeText.text = Mathf.FloorToInt(currentTime -12) + "pm";
        } else if(currentTime < 25)
        {
            timeText.text = "12am";
        } else
        {
            timeText.text = Mathf.FloorToInt(currentTime - 24) + "am";
        }
    }

    public void PauseUnpause()
    {
        if(pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);

            Time.timeScale= 1f;
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);

        Destroy(gameObject);
        Destroy(PlayerMovement.instance.gameObject);
        Destroy(GridInfo.instance.gameObject);
        Destroy(TimeController.instance.gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}