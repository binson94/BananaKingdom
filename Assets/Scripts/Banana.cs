using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public int bananaCount;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Monkey"))
        {
            AudioManager.instance.PlaySound("GetItem");
            GameManager.instance.addCount = bananaCount;
            GameManager.instance.CreateBat();
            GameManager.instance.PointPlus(bananaCount);
            gameObject.SetActive(false);
        }
    }
}
