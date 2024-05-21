using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyText : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;
    [SerializeField] private float secondToDestroy = 1f;

    void Start()
    {
        StartCoroutine(DestroyAndShowRoutine());
    }

    IEnumerator DestroyAndShowRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondToDestroy);

            objectToDestroy.SetActive(false);

            yield return new WaitForSeconds(secondToDestroy);

            objectToDestroy.SetActive(true);
        }
    }

    public void DestroyObject()
    {
        StopCoroutine(DestroyAndShowRoutine());
        objectToDestroy.SetActive(false);
    }
}
