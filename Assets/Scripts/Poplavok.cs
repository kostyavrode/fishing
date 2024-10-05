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
    private void Awake()
    {
        if (rb==null)
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (isNeedToForce)
        {
            rb.AddForce(playerTransform.forward * 150);
            wastedTime += Time.deltaTime;
            if (wastedTime> timeToAddForce )
            {
                isNeedToForce = false;
                wastedTime = 0;
                rb.isKinematic = true;
                StartCoroutine(WaitToLyunylo(Random.Range(1, 3)));
            }
        }
    }
    public void AddForce()
    {
        isNeedToForce = true;
    }
    private void Klyunylo()
    {
        //rb.isKinematic = false;
        fish.SetActive(true);
        transform.DOMoveY(transform.position.y - 1, 0.5f);
        //transform.position=new Vector3(transform.position.x,transform.position.y-)
    }
    private IEnumerator WaitToLyunylo(float time)
    {
        yield return new WaitForSeconds(time);
        Klyunylo();
    }
}
