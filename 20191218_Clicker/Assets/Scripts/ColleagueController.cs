using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 2019.12.18 수요일 - 코드 추가
using System;

public class ColleagueController : MonoBehaviour
{
    public static ColleagueController Instance;
    private ColleagueData[] mDataArr;
    [SerializeField]
    private Colleague[] mPrefabArr;
    private List<Colleague> mSpawnedList;
    [SerializeField]
    private Transform mSpawnPos;

    [SerializeField]
    private Sprite[] mIconArr;

    [SerializeField]
    private UIElement mElementPrefab;
    [SerializeField]
    private Transform mScrollTarget;
    
    private List<UIElement> mElementList;
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
        mDataArr = new ColleagueData[3];
        mDataArr[0] = new ColleagueData();
        mDataArr[0].Name = "No.1";
        mDataArr[0].Level = 0;
        mDataArr[0].Contents = "<color=#ff0000ff>{1}초</color> 마다 <color=#0000ffff>{0}골드</color>를 획득합니다.";
        mDataArr[0].JobTime = 1.1f;
        mDataArr[0].JobType = eJobType.Gold;
        mDataArr[0].ValueCurrent = 1;
        // 2019.12.18 수요일 - 코드 추가
        // 소수점의 기본 자료형은 double이기 때문에 뒤에 d를 붙이지 않아도 된다.
        mDataArr[0].ValueBase = 1d;
        // 2019.12.18 수요일 - 코드 추가
        mDataArr[0].ValueWeight = 1.08d;
        mDataArr[0].CostCurrent = 100;
        // 2019.12.18 수요일 - 코드 추가
        mDataArr[0].CostBase = 100;
        // 2019.12.18 수요일 - 코드 추가
        mDataArr[0].CostWeight = 1.2d;

        mDataArr[1] = new ColleagueData();
        mDataArr[1].Name = "No.2";
        mDataArr[1].Level = 0;
        mDataArr[1].Contents = "<color=#ff0000ff>{1}초</color> 마다 한번씩 터치를 해줍니다.";
        mDataArr[1].JobTime = 1f;
        mDataArr[1].JobType = eJobType.Touch;
        mDataArr[1].ValueCurrent = 0;
        // 2019.12.18 수요일 - 코드 추가
        mDataArr[1].ValueBase = 1d;
        // 2019.12.18 수요일 - 코드 추가
        mDataArr[1].ValueWeight = 1.08d;
        mDataArr[1].CostCurrent = 200;
        // 2019.12.18 수요일 - 코드 추가
        mDataArr[1].CostBase = 1.2d;
        // 2019.12.18 수요일 - 코드 추가
        mDataArr[1].CostWeight = 200d;

        mDataArr[2] = new ColleagueData();
        mDataArr[2].Name = "No.3";
        mDataArr[2].Level = 0;
        mDataArr[2].Contents = "<color=#ff0000ff>{1}초</color> 마다 <color=#0000ffff>{0}골드</color>를 획득합니다.";
        mDataArr[2].JobTime = 1.5f;
        mDataArr[2].JobType = eJobType.Gold;
        // 2019.12.18 수요일 - 코드 수정
        //mDataArr[2].ValueCurrent = 1;
        mDataArr[2].ValueCurrent = 2;
        // 2019.12.18 수요일 - 코드 추가
        mDataArr[2].ValueBase = 1.1d;
        // 2019.12.18 수요일 - 코드 추가
        mDataArr[2].ValueWeight = 1.08d;
        mDataArr[2].CostCurrent = 300;
        // 2019.12.18 수요일 - 코드 추가
        mDataArr[2].CostBase = 300d;
        // 2019.12.18 수요일 - 코드 추가
        mDataArr[2].CostWeight = 1.2d;
    }
    // Start is called before the first frame update
    void Start()
    {
        mElementList = new List<UIElement>();
        mSpawnedList = new List<Colleague>();
        for (int i =0; i < mDataArr.Length; i++)
        {
            UIElement elem = Instantiate(mElementPrefab, mScrollTarget);
            elem.Init(null, i, mDataArr[i].Name, mDataArr[i].Contents, "구매",
                      mDataArr[i].Level, mDataArr[i].ValueCurrent,
                      mDataArr[i].CostCurrent, mDataArr[i].JobTime,
                      AddLevel);
            mElementList.Add(elem);
        }
        
    }
    public void JobFinish(int id)
    {
        ColleagueData data = mDataArr[id];
        switch(data.JobType)
        {
            case eJobType.Gold:
                GameController.Instance.Gold += data.ValueCurrent;
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
        if(mDataArr[id].Level == 0)
        {
            Colleague newCol = Instantiate(mPrefabArr[id]);
            newCol.transform.position = mSpawnPos.position;
            newCol.Init(id, mDataArr[id].JobTime);
            mSpawnedList.Add(newCol);
        }

        // 2019.12.18 수요일 - 코드 수정
        //mDataArr[id].Level += amount;
        //mDataArr[id].ValueCurrent += mDataArr[id].Level;
        //mDataArr[id].CostCurrent += mDataArr[id].Level;
        //mElementList[id].Renew(mDataArr[id].Contents, "구매", mDataArr[id].Level,
        //                       mDataArr[id].ValueCurrent, mDataArr[id].CostCurrent, 
        //                       mDataArr[id].JobTime);

        mDataArr[id].Level += amount;

        // float로 하면 double형태의 숫자를 감당해낼 수 없기 때문에 오버플로우가 나서 infinity가 뜨게 될 것이다.
        //mDataArr[id].ValueCurrent = mDataArr[id].ValueBase *
        //                            Mathf.Pow((float)mDataArr[id].ValueWeight, mDataArr[id].Level);

        mDataArr[id].ValueCurrent = mDataArr[id].ValueBase *
                                    Math.Pow(mDataArr[id].ValueWeight, mDataArr[id].Level);

        mDataArr[id].CostCurrent = mDataArr[id].CostBase *
                                    Math.Pow(mDataArr[id].CostWeight, mDataArr[id].Level);

        mElementList[id].Renew(mDataArr[id].Contents, "구매", mDataArr[id].Level,
                               mDataArr[id].ValueCurrent, mDataArr[id].CostCurrent, mDataArr[id].JobTime);

        // Random이 UntyEngine.Random인지 System.Random인지 모호하기 때문에 에러가 발생한다.
        UnityEngine.Random.Range(0, 1);
    }
}
public class ColleagueData
{
    public string Name;
    public int Level;
    public string Contents;
    public float JobTime;
    public eJobType JobType;
    public double ValueCurrent;
    // 2019.12.18 수요일 - 코드 추가
    public double ValueBase;
    public double ValueWeight;

    public double CostCurrent;
    // 2019.12.18 수요일 - 코드 추가
    public double CostBase;
    public double CostWeight;
}
public enum eJobType
{
    Gold, Touch
}