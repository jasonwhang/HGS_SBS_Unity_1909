using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 2020.01.03 금요일 - 클래스 추가

public class SkillButton : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Image mCoolTimeImage;
    [SerializeField]
    private Text mCoolTimeText;
#pragma warning restore

    public void ShowCoolTime(float CoolTimeBase, float CoolTimeCurrent)
    {
        mCoolTimeImage.fillAmount = CoolTimeCurrent / CoolTimeBase;
        float min = Mathf.Floor(CoolTimeCurrent / 60);
        float sec = CoolTimeCurrent % 60;

        mCoolTimeText.text = string.Format("{0} : {1}", min.ToString("00"), sec.ToString("00"));
    }

    public void SetVisible(bool visible)
    {
        mCoolTimeImage.gameObject.SetActive(visible);
    }


}
