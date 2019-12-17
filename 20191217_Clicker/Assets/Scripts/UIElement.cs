using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElement : MonoBehaviour
{
    [SerializeField]
    private Image mIcon;
    [SerializeField]
    private Text mNameText, mLevelText, mContentsText, mPurchaseText, mCostText;
    [SerializeField]
    private Button mPurchaseButton;
    private int mID;

    // 2019.12.17 - 함수 매개변수 추가
    //public void Init(string name, string contents, string purchaseText,
    //                 int level, double value, double cost, double time,
    //                 AnimHash.TwoIntPramCallback callback)
    //{
    //    mPurchaseButton.onClick.AddListener(()=> { callback(mID, 1); });
    //    //m10UP.onClick.AddListener(() => { callback(mID, 10); });
    //}

    // Icon을 받을 때는 자료형이 Image가 아닌 Sprite이다.
    public void Init(Sprite icon, int id, string name, string contents,
                    string purchaseText, int level, double value,
                    double cost, double time, AnimHash.TwoIntPramCallback callback)
    {
        // 2019.12.17 - 코드 추가
        mIcon.sprite = icon;
        // 2019.12.17 - 코드 추가
        mID = id;
        // 2019.12.17 - 코드 추가
        mNameText.text = name;

        mPurchaseButton.onClick.AddListener(() => { callback(mID, 1); });
        // 2019.12.17 - 코드 추가
        Renew(contents, purchaseText, level, value, cost, time);
    }

    public void Renew(string contents, string purchaseText, int level,
                      double value, double cost,double time)
    {
        //mContentsText.text = string.Format(contents, time.ToString("N1"),
        //                                   value.ToString());
        mContentsText.text = string.Format(contents, value.ToString(), time.ToString("N1"));

        // 2019.12.17 화요일 - 코드 추가
        // N0 = 소수점 단위 밑으로는 버리도록 텍스트를 처리
        mCostText.text = cost.ToString("N0");
        // 2019.12.17 화요일 - 코드 추가
        mPurchaseText.text = purchaseText;
    }
}
