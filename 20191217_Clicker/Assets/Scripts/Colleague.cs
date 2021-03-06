﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colleague : MonoBehaviour
{
    private Rigidbody2D mRB2D;
    [SerializeField]
    private float mSpeed;
    [SerializeField]
    private Transform mEffectPos;
    private Animator mAnim;

    // 2019.12.17 화요일 - 변수 삭제
    //private string mName;
    private int mID;

    private void Awake()
    {
        mRB2D = GetComponent<Rigidbody2D>();
        mAnim = GetComponent<Animator>();
    }

    // 2019.12.17 화요일 - 매개변수 삭제
    //public void Init(string Name, int id, float period)
    public void Init(int id, float period)
    {
        // 2019.12.17 화요일 - 코드 삭제
        //mName = Name;

        mID = id;
        StartCoroutine(Movement());
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
            // 스폰해야될 이펙트의 위치를 JobFinish함수에서 넘겨주는 것이 좋다.
            ColleagueController.Instance.JobFinish(mID);
        }
    }
}
