using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public GameObject ButtonCanvas;
    public GameObject GameOverCanvas;
    public GameObject MenuBtn;
    public GameObject CrownImage;
    public Text GameOverScore;
    public Text GameOverScoreShadow;
    bool IsPause;
    bool IsMute;
    bool IsGameOver;

    public GameObject MuteBtn;
    public GameObject UnMuteBtn;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Mute"))
        {
            PlayerPrefs.SetInt("Mute", 0);
            IsMute = false;

            MuteBtn.SetActive(true);
            UnMuteBtn.SetActive(false);
        }
        else
        {
            IsMute = PlayerPrefs.GetInt("Mute") == 1;

            if (IsMute)
                AudioListener.volume = 0;

            MuteBtn.SetActive(!IsMute);
            UnMuteBtn.SetActive(IsMute);
        }

        CrownImage.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        ButtonCanvas.SetActive(true);
        GameOverCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!(IsPause || IsGameOver))
                PauseBtnPress();
        }
    }

    public void PauseBtnPress()
    {
        AudioManager.instance.PlaySound("ButtonClk");
        pauseMenuCanvas.SetActive(true);
        MenuBtn.SetActive(false);

        if (!IsPause)
        {
            Time.timeScale = 0;
            IsPause = true;
            return;
        }
    }

    public void ResumeBtnPress()
    {
        AudioManager.instance.PlaySound("ButtonClk");
        pauseMenuCanvas.SetActive(false);
        MenuBtn.SetActive(true);

        if (IsPause)
        {
            Time.timeScale = 1;
            IsPause = false;
            return;
        }
    }

    public void GameOver()
    {
        IsGameOver = true;
        GameOverCanvas.SetActive(true);
        ButtonCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);

        GameOverScoreShadow.text = GameOverScore.text = GameManager.instance.Point.ToString();
    }

    public void HighScore()
    {
        CrownImage.SetActive(true);
        AudioManager.instance.PlaySound("Success");
        StartCoroutine(HighScoreAfter());
    }

    IEnumerator HighScoreAfter()
    {
        yield return new WaitForSeconds(2.5f);
        AudioManager.instance.PlaySound("Yeah");
    }

    public void SoundMuteToggle()
    {
        IsMute = !IsMute;
        PlayerPrefs.SetInt("Mute", IsMute ? 1 : 0);

        if (IsMute)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }

        UnMuteBtn.SetActive(IsMute);
        MuteBtn.SetActive(!IsMute);
    }

    public void RestartBtnPress()
    {
        AudioManager.instance.PlaySound("ButtonClk");
        AudioManager.instance.StopSound("BGM");
        AudioManager.instance.StopSound("Seagull");
        AudioManager.instance.StopSound("BGM8bit");
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void BackTitlePress()
    {
        AudioManager.instance.PlaySound("ButtonClk");
        AudioManager.instance.StopSound("BGM");
        AudioManager.instance.StopSound("Seagull");
        AudioManager.instance.StopSound("BGM8bit");
        AudioManager.instance.PlaySound("BGM");
        AudioManager.instance.PlaySound("Seagull");
        Time.timeScale = 1; //  Pause 에서 0으로 설정된 timescale을 되돌려 놓습니다.
        SceneManager.LoadScene(0);
    }
}
