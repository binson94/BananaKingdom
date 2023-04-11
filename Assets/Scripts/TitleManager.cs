using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject EasyBtn;
    public GameObject HardBtn;
    public GameObject NormalBtn;

    public GameObject TitleCanvas;
    public GameObject OptionCanvas;
    public GameObject CreditCanvas;

    public GameObject TutorialCanvas;
    public List<GameObject> TutorialImages;

    public GameObject HelpImage;
    public Sprite[] HelpSprites;
    int HelpIdx = 0;

    public GameObject MuteImage;
    public GameObject UnMuteImage;

    Difficulty nowDifficulty;
    bool OptionOn;
    bool isTutorial;
    bool isMute;
    int nowTutorialImage = 0;

    void Start()
    {
        Screen.SetResolution(Screen.width, Screen.width * 1920 / 1080, true);
        EasyBtn.SetActive(true);
        NormalBtn.SetActive(false);
        HardBtn.SetActive(false);

        if (!PlayerPrefs.HasKey("Mute"))
        {
            PlayerPrefs.SetInt("Mute", 0);
            isMute = false;

            MuteImage.SetActive(true);
            UnMuteImage.SetActive(false);
        }
        else
        {
            isMute = PlayerPrefs.GetInt("Mute") == 1;

            if (isMute)
                AudioListener.volume = 0;

            MuteImage.SetActive(!isMute);
            UnMuteImage.SetActive(isMute);
        }

        foreach (GameObject tmp in TutorialImages)
            tmp.SetActive(false);

        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            StartTutorial();
        }
        else
        {
            TitleCanvas.SetActive(true);
            OptionCanvas.SetActive(false);
            TutorialCanvas.SetActive(false);
        }

        nowDifficulty = Difficulty.Easy;
        PlayerPrefs.SetInt("Difficulty", (int)nowDifficulty);
    }

    //난이도 설정 버튼
    public void DifficultyToggle()
    {
        AudioManager.instance.PlaySound("ButtonClk");
        switch(nowDifficulty)
        {
            case Difficulty.Easy:
                nowDifficulty = Difficulty.Normal;
                break;
            case Difficulty.Normal:
                nowDifficulty = Difficulty.Hard;
                break;
            case Difficulty.Hard:
                nowDifficulty = Difficulty.Easy;
                break;
        }

        EasyBtn.SetActive(nowDifficulty == Difficulty.Easy);
        NormalBtn.SetActive(nowDifficulty == Difficulty.Normal);
        HardBtn.SetActive(nowDifficulty == Difficulty.Hard);

        PlayerPrefs.SetInt("Difficulty", (int)nowDifficulty);
    }
    
    private void StartTutorial()
    {
        isTutorial = true;
        TitleCanvas.SetActive(false);
        OptionCanvas.SetActive(false);
        TutorialCanvas.SetActive(true);
        TutorialImages[0].SetActive(true);
    }

    //튜토리얼에서 다음 이미지로 넘어가기
    public void NextImage()
    {
        if (nowTutorialImage < 6)
        {
            AudioManager.instance.PlaySound("ButtonClk");
            TutorialImages[nowTutorialImage++].SetActive(false);
            TutorialImages[nowTutorialImage].SetActive(true);
        }
        else
        {
            TutorialImages[6].SetActive(false);
            EndTutorial();
        }
    }

    private void EndTutorial()
    {
        isTutorial = false;
        TitleCanvas.SetActive(true);
        OptionCanvas.SetActive(false);
        TutorialCanvas.SetActive(false);
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    //옵션 바나나를 눌렀을 때
    public void OptionToggle()
    {
        AudioManager.instance.PlaySound("ButtonClk");

        OptionOn = !OptionOn;

        TitleCanvas.SetActive(!OptionOn);
        OptionCanvas.SetActive(OptionOn);
    }

    public void MuteToggle()
    {
        isMute = !isMute;
        PlayerPrefs.SetInt("Mute", isMute ? 1 : 0);

        if (isMute)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }

        UnMuteImage.SetActive(isMute);
        MuteImage.SetActive(!isMute);
    }

    public void HelpButton()
    {
        AudioManager.instance.PlaySound("ButtonClk");
        HelpImage.GetComponent<Image>().sprite = HelpSprites[0];
        HelpIdx = 1;
        HelpImage.SetActive(true);
    }

    public void HelpNext()
    {
        AudioManager.instance.PlaySound("ButtonClk");

        if (HelpIdx < 3)
        {
            HelpImage.GetComponent<Image>().sprite = HelpSprites[HelpIdx++];
        }
        else
        {
            HelpImage.SetActive(false);
        }
    }

    public void CreditButton()
    {
        AudioManager.instance.PlaySound("ButtonClk");
        CreditCanvas.SetActive(true);
    }

    public void CancleCredit()
    {
        CreditCanvas.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    //시작 버튼을 눌렀을 때, 시작
    public void StartGame()
    {
        AudioManager.instance.PlaySound("ButtonClk");
        SceneManager.LoadScene(1);
    }

    //뒤로가기 버튼 대응, 뒤로가기 누르면 옵션 뜸
    void Update()
    {
        if (!isTutorial)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!OptionOn)
                {
                    OptionToggle();
                }
            }
        }
    }
}