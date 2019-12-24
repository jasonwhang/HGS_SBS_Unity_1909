using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoController : MonoBehaviour
{
    public static PlayerInfoController Instance;

    [SerializeField]
    private PlayerInfo[] mInfos;

    [SerializeField]
    private UIElement mElementPrefab;
    [SerializeField]
    private Transform mScrollTarget;
    private List<UIElement> mElementList;

    private void Awake()
    {
        if (Instance == null)
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
        mElementList = new List<UIElement>();
        for(int i = 0; i < mInfos.Length; i++)
        {
            UIElement element = Instantiate(mElementPrefab, mScrollTarget);
            element.Init(null, i, mInfos[i].Name, mInfos[i].Contents, "Level UP", 
                         mInfos[i].Level, mInfos[i].ValueCurrent, mInfos[i].CostCurrent,
                         mInfos[i].Duration, AddLevel);

            mElementList.Add(element);
        }
    }

    public void AddLevel(int id, int amount)
    {
        GameController.Instance.GoldConsumeCallback = () => { ApplyLevelUp(id, amount); };
        GameController.Instance.Gold -= mInfos[id].CostCurrent;
    }
    public void ApplyLevelUp(int id, int amount)
    {
        mInfos[id].Level += amount;
        // 플레이어가 활성화를 시키지 않은 경우 0레벨로 적용이 되어 있을 것인데
        // 1레벨부터 적용되는 계산공식이(level -1) 0레벨일 때 -1로 오류가 나타난다.
        // 1레벨일 떄는 Cost가 0이어야 한다.
        mInfos[id].CostCurrent = mInfos[id].CostBase * Math.Pow(mInfos[id].CostWeight, mInfos[id].Level);
        // Value값은 표시하는 방식이 퍼센트와 일반값 2가지이고 
        // 계산하는 방식도 그냥 더하는 방식과 지수방식으로 더하는 방식 2가지가 있다.
        // 이런 경우에는 switch로 구분을 하는 것이 좋다.
        switch(mInfos[id].ValueType)
        {
            case ePlayerValueType.Expo:
                mInfos[id].ValueCurrent = mInfos[id].ValueBase *
                    Math.Pow(mInfos[id].ValueWeight, mInfos[id].Level);
                break;
            case ePlayerValueType.Numeric:
            case ePlayerValueType.Percent:
                // 이렇게 break가 없이 case가 연결이 된 것이면
                // 조건이 2개가 or로 연결이 된 것과 같은 의미이다.
                // 즉, 조건이 Numeric이거나 Percent인 경우 case문 코드를 실행하라는 의미와 같다.
                mInfos[id].ValueCurrent = mInfos[id].ValueBase + mInfos[id].ValueWeight * mInfos[id].Level;
                break;
            default:
                // default는 우리가 쓰지 않아도 꼭 넣어주는 것이 좋다.
                // 규모가 큰 게임인 경우 기획자가 실수로 데이터를 잘못 넣을 수도 있기 때문에
                // 게임이 터지는 것보다는 default로 에러를 잡는 것이 수정하는 것이 더 편하다.
                Debug.LogError("wrong value type : " + mInfos[id].ValueType);
                break;
        }
        // 여기서 문제점이 넘겨주어야 하는 double value값이 현재는 2가지의 방식이 존재한다.
        // double값이면 이 2가지의 방식을 모두 담아낼 수는 없다.
        mElementList[id].Renew(mInfos[id].Contents, "Level UP", mInfos[id].Level,
                               mInfos[id].ValueCurrent, mInfos[id].CostCurrent, mInfos[id].Duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class PlayerInfo
{
    public string Name;
    public string Contents;

    public int ID;
    public int IconID;
    public int Level;

    public ePlayerValueType ValueType;

    public float CoolTime;
    public float CoolTImeCurrent;
    public float Duration;

    public double ValueCurrent;
    public double ValueWeight;
    public double ValueBase;

    public double CostCurrent;
    public double CostWeight;
    public double CostBase;
}

public enum ePlayerValueType
{
    // 기하급수로 증가하는 방식
    Expo,
    // 값을 더하는 방식
    Numeric,
    // 퍼센트로 증가하는 방식
    Percent
}

