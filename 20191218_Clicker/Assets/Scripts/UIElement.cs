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

    public void Init(Sprite icon, int id, string name,
                     string contents, string purchaseText,
                     int level, double value, double cost, double time,
                     AnimHash.TwoIntPramCallback callback)
    {
        mIcon.sprite = icon;
        mID = id;
        mNameText.text = name;
        mPurchaseButton.onClick.AddListener(()=> { callback(mID, 1); });
        Renew(contents, purchaseText, level, value, cost, time);
    }
    public void Renew(string contents, string purchaseText, int level, double value, double cost,double time)
    {
        // 2019.12.18 수요일 - 코드 추가
        // string이 합쳐질 때 1개인 경우에는 이렇게 해도 상관없지만
        // 두개 이상일 때는 string.format을 사용하는 것이 좋다.
        mLevelText.text = "LV." + level.ToString();

        //mContentsText.text = string.Format(contents, value.ToString(),
        //                                             time.ToString("N1"));

        // 2019.12.18 수요일 - 코드 수정
        mContentsText.text = string.Format(contents, UnitBuilder.GetUnitStr(value),
                                             time.ToString("N1"));
        //mCostText.text = cost.ToString("N0");
        mCostText.text = UnitBuilder.GetUnitStr(cost);

        mPurchaseText.text = purchaseText;
    }
}
