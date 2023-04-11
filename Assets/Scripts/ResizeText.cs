using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeText : MonoBehaviour
{
    Text t;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        t.fontSize = Screen.height / 125;
    }
}
