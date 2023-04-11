using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditUI : MonoBehaviour
{

    Animator anim;

    private void OnEnable()
    {
        AudioManager.instance.StopSound("Seagull");
        AudioManager.instance.StopSound("BGM");
        AudioManager.instance.PlaySound("BGM");
        anim = GetComponent<Animator>();
        anim.SetBool("isPlaying", true);
        anim.Play("credit");
    }

    private void OnDisable()
    {
        AudioManager.instance.PlaySound("Seagull");
        anim.SetBool("isPlaying", false);
    }

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            gameObject.SetActive(false);
        }
    }
}
