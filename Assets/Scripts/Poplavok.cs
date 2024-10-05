using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Poplavok : MonoBehaviour
{
    public Rigidbody rb;

    public Transform playerTransform;

    public GameObject fish;

    public float timeToAddForce;
    private float wastedTime;

    private bool isKlyuet;

    private bool isNeedToForce;
    private bool isNeedToResist;
    private void Awake()
    {
        if (rb==null)
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if ((GameManager.instance.state == GameStates.PLAYING))
        {


            if (isNeedToForce)
            {
                rb.AddForce(playerTransform.forward * 150);
                wastedTime += Time.deltaTime;
                if (wastedTime > timeToAddForce)
                {
                    isNeedToForce = false;
                    wastedTime = 0;
                    rb.isKinematic = true;
                    StartCoroutine(WaitToLyunylo(Random.Range(1, 3)));
                }
            }
            else if ((isNeedToResist))
            {
                {
                    rb.AddForce(playerTransform.forward * 15);
                    wastedTime += Time.deltaTime;
                    if (wastedTime > timeToAddForce)
                    {
                        isNeedToResist = false;
                        wastedTime = 0;
                    }
                }
            }
        }
    }
    public void AddForce()
    {
        isNeedToForce = true;
    }
    public void UpPoplavok()
    {
        transform.DOMoveY(transform.position.y + 0.3f, 0.5f);
    }
    public void Resist()
    {
        isNeedToResist = true;
    }
    private void Klyunylo()
    {
        //rb.isKinematic = false;
        fish.SetActive(true);
        transform.DOMoveY(transform.position.y - 0.3f, 0.5f);
        //transform.position=new Vector3(transform.position.x,transform.position.y-)
    }
    private IEnumerator WaitToLyunylo(float time)
    {
        yield return new WaitForSeconds(time);
        Klyunylo();
    }
}
