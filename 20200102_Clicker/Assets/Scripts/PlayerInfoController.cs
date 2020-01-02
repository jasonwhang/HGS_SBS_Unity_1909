using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoController : DataLoader
{
    public static PlayerInfoController Instance;
#pragma warning disable 0649
    [SerializeField]private PlayerInfo[] mInfos;
    public PlayerInfo[] Infos { get { return mInfos; } }
    [SerializeField]private UIElement mElementPrefab;
    [SerializeField]private Transform mScrollTarget;
    private List<UIElement> mElementList;

    // 2020.01.02 목요일 - 변수 추가
    private bool mIsLoaded;
    // 2020.01.02 목요일 - 코드 추가
    public bool bLoaded { get { return mIsLoaded; } }
#pragma warning restore

    public int[] LevelArr
    {
        get
        {
            int[] arr = new int[mInfos.Length];
            for(int i = 0; i < arr.Length; i++)
            {
                arr[i] = mInfos[i].Level;
            }
            return arr;
        }
    }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            // 2020.01.02 목요일 - 코드 추가
            mIsLoaded = false;
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log("sdfsdfs");
        LoadJsonData(out mInfos, StaticValues.PLAYER_DATA_PATH);
    }
    // Start is called before the first frame update
    void Start()
    {
        mElementList = new List<UIElement>();
        for(int i = 0; i < mInfos.Length; i++)
        {
            UIElement element = Instantiate(mElementPrefab, mScrollTarget);
            element.Init(null, i, mInfos[i].Name, mInfos[i].Contents, "Level UP", mInfos[i].Level, mInfos[i].ValueCurrent,
                        mInfos[i].CostCurrent, mInfos[i].Duration, AddLevel, mInfos[i].ValueType);
            mElementList.Add(element);
        }
        GameController.Instance.TouchPower = mInfos[0].ValueCurrent;

        // 2020.01.02 목요일 - 코드 추가
        mIsLoaded = true;
    }

    // 2020.01.02 목요일 - 함수 추가
    public void Load(int[] levelArr)
    {
        // levelArr은 구버전이었을 때의 세이브데이터이다.
        // mInfos는 최신버전일 때의 세이브데이터이다.
        // 구버전의 세이브데이터를 우선 불러오고 새롭게 추가되는 데이터들은 추가작업으로 데이터를 넣어준다.
        for(int i = 0; i < levelArr.Length; i++)
        {
            mInfos[i].Level = levelArr[i];
            // 아래의 경우 mInfos[0]의 경우 레벨이 1부터 시작하기 때문에 계산방식이 맞지 않게 된다.
            //ApplyLevelUP(i, levelArr[i]); 

            // 로드가 된 데이터를 가지고 현재 데이터의 정보를 계산해준다.
            CalcAndShowData(i);
        }
    }

    public void AddLevel(int id, int amount)
    {
        GameController.Instance.GoldConsumeCallback = () => { ApplyLevelUP(id, amount); };
        GameController.Instance.Gold -= mInfos[id].CostCurrent;
    }

    public void ApplyLevelUP(int id, int amount)
    {
        mInfos[id].Level += amount;
        CalcAndShowData(id);
    }

    // 20200102 목요일 - 함수 추가
    private void CalcAndShowData(int id)
    {
        mInfos[id].CostCurrent = mInfos[id].CostBase * Math.Pow(mInfos[id].CostWeight, mInfos[id].Level);
        switch (mInfos[id].ValueType)
        {
            case eValueType.Expo:
                mInfos[id].ValueCurrent = mInfos[id].ValueBase * Math.Pow(mInfos[id].ValueWeight, mInfos[id].Level);
                break;
            case eValueType.Numeric:
            case eValueType.Percent:
                mInfos[id].ValueCurrent = mInfos[id].ValueBase + mInfos[id].ValueWeight * mInfos[id].Level;
                break;
            default:
                Debug.LogError("wrong value type : " + mInfos[id].ValueType);
                break;
        }

        mElementList[id].Renew(mInfos[id].Contents, "Level UP", mInfos[id].Level, mInfos[id].ValueCurrent,
                       mInfos[id].CostCurrent, mInfos[id].Duration, mInfos[id].ValueType);

        if (id == 0)
        {
            GameController.Instance.TouchPower = mInfos[id].ValueCurrent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[Serializable]
public class PlayerInfo
{
    public int ID;
    public string Name;
    public string Contents;
    public int IconID;
    public int Level;

    public eValueType ValueType;
    public double ValueCurrent;
    public double ValueWeight;
    public double ValueBase;

    public float CoolTime;
    public float CoolTimeCurrent;
    public float Duration;

    public double CostCurrent;
    public double CostWeight;
    public double CostBase;
}
