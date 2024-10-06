using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellFish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            UIMaster.instance.ShowSellFishButton(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            UIMaster.instance.ShowSellFishButton(false);
        }
    }
}
