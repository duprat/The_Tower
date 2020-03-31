using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    public static UIManager current;

    public Canvas mainMenu;
    public Canvas overlay;
    public Canvas pauseMenu;
    public Canvas gameOver;
    public Canvas optionsMenu;

    public Button pauseButton;

    public TextMeshProUGUI musicName;

    public List<Toggle> arene;


    public static bool isMainMenuActive;
    public static bool isOverlayActive;
    public static bool isGameOverActive;
    public static bool isPauseMenuActive;
    public static bool isOptionsMenuActive;
    public static bool inGame;

    public TextMeshProUGUI Text_FallenBlocks;
    private int lastCanvas;
    private int nb_FallenBlocks;
    private string test = "testo";
    private int indexMusic;

    // Start is called before the first frame update
    void Start()
    {
        indexMusic = 0;
        current = this;
        isMainMenuActive = true;
        isOverlayActive = false;
        isGameOverActive = false;
        isPauseMenuActive = false;
        isOptionsMenuActive = false;
        inGame = false;
        mainMenu.gameObject.SetActive(isMainMenuActive);
        gameOver.gameObject.SetActive(isGameOverActive);
        overlay.gameObject.SetActive(isOverlayActive);
        pauseMenu.gameObject.SetActive(isPauseMenuActive);
        optionsMenu.gameObject.SetActive(isOptionsMenuActive);

        nb_FallenBlocks = 0;
        Text_FallenBlocks.text = "Blocks survived: " + nb_FallenBlocks;
        GameEvents.current.onPlayerDeath += GameOver;
        pauseButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOverlayActive)
        {
            Text_FallenBlocks.text = "Blocks survived: " + nb_FallenBlocks;
        }
    }

    public void addMusic()
    {
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Select a Song", "", "mp3");
        print(path);
        string extension = path.Substring(path.IndexOf('.') + 1);
        //print(extension);
        string song = path.Substring(path.LastIndexOf('/') + 1);
        print(song);
        if(!(path.Length == 0))
        {
            string target = Application.dataPath + "/Musics/" + song;
            print(target);
            FileUtil.CopyFileOrDirectory(path, target);
            Resources.Load(target);
        }
#else
        // TODO FILE MANAGEMENT FOR WINDOWS
#endif
    }

    

    public void onHover(TextMeshProUGUI resume)
    {
        resume.color = new Color(0f,0f,0f);
    }

    public void onHoverExit(TextMeshProUGUI resume)
    {
        resume.color = new Color(255f, 255f, 255f);
    }

    public void onClickedResume(TextMeshProUGUI resume)
    {
        resume.color = new Color(255f, 255f, 255f);
    }

    public void play()
    {
        AudioManager.current.playMusic();

        Time.timeScale = 1f;
        isOverlayActive = true;
        overlay.gameObject.SetActive(isOverlayActive);
        pauseButton.gameObject.SetActive(true);

        isPauseMenuActive = false;
        pauseMenu.gameObject.SetActive(isPauseMenuActive);
    }

    public void start()
    {
        inGame = true;

        isMainMenuActive = false;
        mainMenu.gameObject.SetActive(isMainMenuActive);

        isPauseMenuActive = false;
        pauseMenu.gameObject.SetActive(isPauseMenuActive);

        isGameOverActive = false;
        gameOver.gameObject.SetActive(isGameOverActive);

        isOptionsMenuActive = false;
        optionsMenu.gameObject.SetActive(isOptionsMenuActive);

        GameManager.current.startGame();

        isOverlayActive = true;
        overlay.gameObject.SetActive(isOverlayActive);
        pauseButton.gameObject.SetActive(true);

        AudioManager.current.playMusic();
    }

    public void GameOver()
    {
        inGame = false;
        isMainMenuActive = false;
        mainMenu.gameObject.SetActive(isMainMenuActive);

        isOverlayActive = false;
        overlay.gameObject.SetActive(isOverlayActive);

        isPauseMenuActive = false;
        pauseMenu.gameObject.SetActive(isPauseMenuActive);

        isGameOverActive = true;
        gameOver.gameObject.SetActive(isGameOverActive);

        isOptionsMenuActive = false;
        optionsMenu.gameObject.SetActive(isOptionsMenuActive);

        AudioManager.current.stopMusic();
    }

    public void displayOptions(int canvas)
    {
        if(canvas == 0) // mainMenu
        {
           // optionsMenu.gameObject.
        }
        else if (canvas == 1) // pauseMenu
        {

        }

        inGame = false;
        isMainMenuActive = false;
        mainMenu.gameObject.SetActive(isMainMenuActive);

        isOverlayActive = false;
        overlay.gameObject.SetActive(isOverlayActive);

        isPauseMenuActive = false;
        pauseMenu.gameObject.SetActive(isPauseMenuActive);

        isGameOverActive = false;
        gameOver.gameObject.SetActive(isGameOverActive);

        isOptionsMenuActive = true;
        optionsMenu.gameObject.SetActive(isOptionsMenuActive);
        lastCanvas = canvas;
    }

    public void returnFromOptions()
    {

        if(lastCanvas == 0)
        {
            inGame = false;
            isMainMenuActive = true;
            mainMenu.gameObject.SetActive(isMainMenuActive);

            isOverlayActive = false;
            overlay.gameObject.SetActive(isOverlayActive);

            isPauseMenuActive = false;
            pauseMenu.gameObject.SetActive(isPauseMenuActive);

            isGameOverActive = false;
            gameOver.gameObject.SetActive(isGameOverActive);

            isOptionsMenuActive = false;
            optionsMenu.gameObject.SetActive(isOptionsMenuActive);
        }
        else if(lastCanvas == 1)
        {
            inGame = false;
            isMainMenuActive = false;
            mainMenu.gameObject.SetActive(isMainMenuActive);

            isOverlayActive = false;
            overlay.gameObject.SetActive(isOverlayActive);

            isPauseMenuActive = true;
            pauseMenu.gameObject.SetActive(isPauseMenuActive);

            isGameOverActive = false;
            gameOver.gameObject.SetActive(isGameOverActive);

            isOptionsMenuActive = false;
            optionsMenu.gameObject.SetActive(isOptionsMenuActive);
        }
    }

    public void pause()
    {
        AudioManager.current.pauseMusic();
        inGame = false;
        pauseButton.gameObject.SetActive(inGame);

        isOverlayActive = false;
        overlay.gameObject.SetActive(isOverlayActive);

        isPauseMenuActive = true;
        pauseMenu.gameObject.SetActive(isPauseMenuActive);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        isGameOverActive = false;
        gameOver.gameObject.SetActive(isGameOverActive);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }


    public void getTest()
    {
        Debug.Log(test);
    }

    public void addFallenBlocks(int nbBlocks)
    {
        nb_FallenBlocks += nbBlocks;
    }

    public bool red()
    {
        return arene[0].isOn;
    }

    public bool green()
    {
        return arene[1].isOn;
    }

    public bool blue()
    {
        return arene[2].isOn;
    }

    public bool transition()
    {
        return arene[3].isOn;
    }

    public void nextMusic()
    {
        indexMusic = Mathf.Abs( (indexMusic + 1) % 11);
    }

    public void previousMusic()
    {
        indexMusic = indexMusic - 1;
        if(indexMusic < 0)
        {
            indexMusic = 10;
        }
    }

    public int getIndexMusic()
    {
        return indexMusic;
    }

    public void setIndexMusic(int index)
    {
        indexMusic = index;
    }
}
