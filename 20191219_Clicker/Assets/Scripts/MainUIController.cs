﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 2019.12.19 목요일 - 코드 추가
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public static MainUIController Instance;
    private static int mUIMoveHash = Animator.StringToHash("Move");
    [SerializeField]
    private Animator[] mWindowAnims;
    [SerializeField]
    private GaugeBar mProgressBar;
    // 2019.12.19 목요일 - 변수 추가
    [SerializeField]
    private Text mGoldText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 2019.12.19 목요일 - 함수 추가
    public void ShowGold(double value)
    {
        mGoldText.text = UnitBuilder.GetUnitStr(value);
    }

    public void ShowProgress(double current, double max)
    {
        //TODO calc Gauge progress float value
        float progress = (float)(current / max);
        ////hack build Gauge progress string

        // 2019.12.19 목요일 - 코드 수정
        //string progressString = progress.ToString("P0");
        // 유니티 텍스트는 자간조절이 안되서 어느정도 띄어져 있는 것이 좋다.
        string progressString = string.Format("{0} / {1}",
                                              UnitBuilder.GetUnitStr(current), 
                                              UnitBuilder.GetUnitStr(max));
        mProgressBar.ShowGaugeBar(progress, progressString);
    }

    public void MoveWindow(int id)
    {
        mWindowAnims[id].SetTrigger(mUIMoveHash);
    }
}
