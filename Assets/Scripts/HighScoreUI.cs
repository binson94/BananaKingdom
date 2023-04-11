using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
    Text text;


    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();

        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }

        text.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
}
