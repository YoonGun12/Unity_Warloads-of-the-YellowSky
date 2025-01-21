using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite closedChest;
    public Sprite openedChest;
    public GameObject expOrb;
    public GameObject largeExpOrb;
    public GameObject healthItem;

    private bool isOpened = false;
        

    public void OpenChest()
    {
        if(isOpened) 
            return;

        isOpened = true;
        GetComponent<SpriteRenderer>().sprite = openedChest;

        if(largeExpOrb != null)
        {
            Instantiate(largeExpOrb, transform.position, Quaternion.identity);
        }
    }
}
