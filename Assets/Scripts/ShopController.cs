using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            UIMaster.instance.ShowShopButton(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            UIMaster.instance.ShowShopButton(false);
        }
    }
}
