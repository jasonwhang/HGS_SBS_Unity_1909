using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2019.12.06 - 클래스 추가

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float time;

    private void OnEnable()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
