﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colleague : MonoBehaviour
{
    private Rigidbody2D mRB2D;
    [SerializeField]
    private float mSpeed;

    // 2019.12.13 금요일 - 변수 추가
    [SerializeField]
    private Transform mEffectPos;

    private Animator mAnim;

    // 2019.12.13 금요일 - 변수 추가
    private string mName;
    private int mID;


    private void Awake()
    {
        mRB2D = GetComponent<Rigidbody2D>();
        mAnim = GetComponent<Animator>();
    }
    // 2019.12.13 금요일 - 함수 수정(Start -> Init)
    // Start is called before the first frame update
    //void Start()
    //{
    //    // 2019.12.13 금요일 - 코드 추가
    //    StartCoroutine(Movement());
    //    StartCoroutine(Function(1));
    //}

    public void Init(string Name, int id, float period)
    {
        // 2019.12.13 금요일 - 코드 추가
        mName = Name;
        mID = id;

        // 2019.12.13 금요일 - 코드 추가
        StartCoroutine(Movement());

        // 2019.12.13 금요일 - 코드 수정
        //StartCoroutine(Function(1));
        StartCoroutine(Function(period));
    }

    private IEnumerator Movement()
    {
        WaitForSeconds moveTime = new WaitForSeconds(1);
        while(true)
        {
            int dir = Random.Range(0, 2);
            if(dir == 0) // see left side
            {
                transform.rotation = Quaternion.identity;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            int moveOrStay = Random.Range(0, 2);
            if(moveOrStay == 0)
            {
                mRB2D.velocity = Vector2.zero;
                mAnim.SetBool(AnimHash.Move, false);
            }
            else
            {
                mRB2D.velocity = transform.right * -mSpeed;
                mAnim.SetBool(AnimHash.Move, true);
            }

            yield return moveTime;
        }
    }

    private IEnumerator Function(float time)
    {
        WaitForSeconds term = new WaitForSeconds(time);
        while(true)
        {
            yield return term;
            // run special function
            // 2019.12.13 금요일 - 코드 추가
            ColleagueController.Instance.JobFinish(mID);

            // 2019.12.13 금요일 - 코드 추가
            // Time.time은 0초부터 올라가는 플레이 시간이다.
            // 만일 No.1동료가 행동을 했다면 No.1(0)
            Debug.LogFormat("{0}({1}) finish job current time is {2}", mName, mID, Time.time);
        }
    }
}
