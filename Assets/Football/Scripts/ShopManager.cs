using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject buyButton;
    public GameObject notEnough;
    public int cost;
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("Buy")>0)
        {
            buyButton.SetActive(false);
        }
    }
    public void Buy()
    {
        if (PlayerPrefs.GetInt("Money")>=cost)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - cost);
            PlayerPrefs.SetInt("Buy", 1);
            PlayerPrefs.Save();
            UIMaster.instance.UpdateMoney();
            buyButton.SetActive(false);
        }
        else
        {
            notEnough.SetActive(true);
            StartCoroutine(DisableObj());
        }
    }
    private IEnumerator DisableObj()
    {
        yield return new WaitForSeconds(1);
        notEnough.SetActive(false);
    }
}
