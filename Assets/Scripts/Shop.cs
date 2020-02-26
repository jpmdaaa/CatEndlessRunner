using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shop;
    public GameObject chars;
    public GameObject start;
    public GameObject btnMissions;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShopOn()
    {
        shop.SetActive(true);
        chars.SetActive(true);
        start.SetActive(false);
        btnMissions.SetActive(false);
      
    }

    public void ShopOf()
    {
        shop.SetActive(false);
        chars.SetActive(false);
        start.SetActive(true);
        btnMissions.SetActive(true);
    }




}
