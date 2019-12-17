using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2019.12.13 금요일 - 클래스추가

public class ColleagueController : MonoBehaviour
{
    public static ColleagueController Instance;

    private ColleagueData[] mDataArr;
    [SerializeField]
    private Colleague[] mPrefabArr;
    [SerializeField]
    private Transform mSpawnPos;

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
        mDataArr[0].JobTime = 1.1f;
        mDataArr[0].CostCurrent = 100;

        mDataArr[1] = new ColleagueData();
        mDataArr[1].Name = "No.2";
        mDataArr[1].Level = 0;
        mDataArr[1].JobTime = 1f;
        mDataArr[1].CostCurrent = 200;

        mDataArr[2] = new ColleagueData();
        mDataArr[2].Name = "No.3";
        mDataArr[2].Level = 0;
        mDataArr[2].JobTime = 1.5f;
        mDataArr[2].CostCurrent = 300;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // 2019.12.13 금요일 - 함수 추가
    public void JobFinish(int id)
    {
        // 가지고 와야할 데이터는 JobData이다.
        // 복제본을 만들 것이면 이렇게 하면 안된다.
        // CurrentData를 따로 만들어서 작업을 해주어야 한다.
        ColleagueData data = mDataArr[id];
        switch (data.JobType)
        {
            case eJobType.Gold:
                GameController.Instance.Gold += 1;
                break;
            case eJobType.Touch:
                GameController.Instance.Touch();
                break;
            default:
                Debug.LogError("Wrong job type" + data.JobType);
                break;
        }

    }

    // 2019.12.13 금요일 - 함수 추가
    public void TempInstanciate(int id)
    {
        AddLevel(id, 1);
    }

    // amount를 해주는 이유는 1레벨업씩만 하는 것이 아닌 10레벨업씩 할 수도 있기 때문이다.
    // 예를들어 함수를 호출할 때 0번데이터를 10레벨업을 할 것이다.
    public void AddLevel(int id, int amount)
    {
        // 2019.12.13 금요일 - 코드 추가
        // 동료캐릭터 Prefab을 생성
        Colleague newCol = Instantiate(mPrefabArr[id]);
        // 생성된 동료캐릭터를 배치를 할 위치를 설정
        newCol.transform.position = mSpawnPos.position;
        newCol.Init(mDataArr[id].Name, id, mDataArr[id].JobTime);
    }
}

// 2019.12.13 금요일 - 클래스 추가
public class ColleagueData
{
    public string Name;
    public int Level;
    public float JobTime;
    public eJobType JobType;
    public double CostCurrent;
}

// 2019.12.13 금요일 - 열거형 추가
public enum eJobType
{
    Gold,
    Touch
}
