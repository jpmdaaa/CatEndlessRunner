using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private Player player;
    public GameObject[] buttons;
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPause()
    {
        player.speed = 0;
        player.canMove = false;
        player.an.Rebind();
        buttons[0].SetActive(false);
        buttons[1].SetActive(true);
        
    }
    public void OfPause()
    {
        player.speed = player.currentSpeed;
        player.canMove = true;
        player.an.Play("runStart");
        buttons[0].SetActive(true);
        buttons[1].SetActive(false);
        
    }


}
