using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColleagueController : DataLoader
{
    public static ColleagueController Instance;
#pragma warning disable 0649
    [SerializeField]private ColleagueData[] mDataArr;
    [SerializeField]private Colleague[] mPrefabArr;
    private List<Colleague> mSpawnedList;
    [SerializeField]private Transform mSpawnPos;

    [SerializeField]private Sprite[] mIconArr;

    [SerializeField]private UIElement mElementPrefab;
    [SerializeField]private Transform mScrollTarget;
    private List<UIElement> mElementList;

    [SerializeField]private TextEffectPool mTextEffectPool;

    private int mCurrentId, mCurrentAmount;
    // 2020.01.02 목요일 - 변수 추가
    private bool mIsLoaded;
    // 2020.01.02 목요일 - 코드 추가
    public bool bLoaded { get { return mIsLoaded; } }
#pragma warning restore

    public int[] LevelArr
    {
        get
        {
            int[] arr = new int[mDataArr.Length];
            for(int i = 0;i <arr.Length; i++)
            {
                arr[i] = mDataArr[i].Level;
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
        LoadJsonData(out mDataArr, StaticValues.COLLEAGUE_DATA_PATH);
    }

    // Start is called before the first frame update
    void Start()
    {
        mElementList = new List<UIElement>();
        mSpawnedList = new List<Colleague>();
        for (int i =0; i < mDataArr.Length; i++)
        {
            UIElement elem = Instantiate(mElementPrefab, mScrollTarget);
            elem.Init(mIconArr[i], i, mDataArr[i].Name, mDataArr[i].Contents, "구매",
                      mDataArr[i].Level, mDataArr[i].ValueCurrent,
                      mDataArr[i].CostCurrent, mDataArr[i].JobTime,
                      AddLevel);
            mElementList.Add(elem);
        }

        // 2020.01.02 목요일 - 코드 추가
        mIsLoaded = true;
    }

    // 2020.01.02 목요일 - 함수 추가
    public void Load(int[] levelArr)
    {
        for(int i = 0; i < levelArr.Length; i++)
        {
            // PlayerInfoController.cs에서 mInfos가 여기에서 mDataArr과 동일한 기능을 하는 변수이다.
            // 이름을 통일시키는 것이 좋다.
            mDataArr[i].Level = levelArr[i];

            CalcAndShowData(i);
            if (mDataArr[i].Level >= 0)
            {
                Colleague newCol = Instantiate(mPrefabArr[i]);
                newCol.transform.position = mSpawnPos.position;
                newCol.Init(i, mDataArr[i].JobTime);
                mSpawnedList.Add(newCol);
            }
        }
    }

    public void JobFinish(int id, Vector3 pos)
    {
        ColleagueData data = mDataArr[id];
        switch(data.JobType)
        {
            case eJobType.Gold:
                GameController.Instance.Gold += data.ValueCurrent;
                TextEffect effect = mTextEffectPool.GetFromPool((int)eTextEffectType.ColleagueIncome);
                effect.ShowText(UnitBuilder.GetUnitStr(data.ValueCurrent));
                effect.transform.position = pos;
                break;
            case eJobType.Touch:
                GameController.Instance.Touch();
                break;
            default:
                Debug.LogError("Wrong job type " + data.JobType);
                break;
        }
    }

    public void AddLevel(int id, int amount)
    {
        GameController.Instance.GoldConsumeCallback = () => { ApplyLevel(id, amount); };
        GameController.Instance.Gold -= mDataArr[id].CostCurrent;      
    }

    public void ApplyLevel(int id, int amount)
    {
        if (mDataArr[id].Level == 0)
        {
            Colleague newCol = Instantiate(mPrefabArr[id]);
            newCol.transform.position = mSpawnPos.position;
            newCol.Init(id, mDataArr[id].JobTime);
            mSpawnedList.Add(newCol);
        }
        mDataArr[id].Level += amount;
        // 2020.01.02 목요일 - 코드 추가
        CalcAndShowData(id);
    }

    // 2020.01.02 목요일 - 함수 추가
    public void CalcAndShowData(int id)
    {
        mDataArr[id].ValueCurrent = mDataArr[id].ValueBase * 
                                    Math.Pow(mDataArr[id].ValueWeight, mDataArr[id].Level);
        mDataArr[id].CostCurrent = mDataArr[id].CostBase * 
                                    Math.Pow(mDataArr[id].CostWeight, mDataArr[id].Level);
        mElementList[id].Renew(mDataArr[id].Contents, "구매", mDataArr[id].Level,
                               mDataArr[id].ValueCurrent, mDataArr[id].CostCurrent,
                               mDataArr[id].JobTime);
    }
}
[Serializable]
public class ColleagueData
{
    public string Name;
    public int Level;
    public string Contents;
    public float JobTime;
    public eJobType JobType;

    public double ValueCurrent;
    public double ValueWeight;
    public double ValueBase;

    public double CostCurrent;
    public double CostWeight;
    public double CostBase;
}

public enum eJobType
{
    Gold, Touch
}