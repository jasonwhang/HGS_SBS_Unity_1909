using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColleagueController : MonoBehaviour
{
    public static ColleagueController Instance;
    private ColleagueData[] mDataArr;
    [SerializeField]
    private Colleague[] mPrefabArr;
    [SerializeField]
    private Transform mSpawnPos;

    // 2019.12.17 화요일 - 변수 추가
    [SerializeField]
    private UIElement mElementPrefab;
    // 2019.12.17 화요일 - 변수 추가
    [SerializeField]
    private Transform mScrollTarget;

    // 2019.12.17 화요일 - 변수 추가
    // List는 SerializeField로 해주면 0으로 자동으로 초기화가 된다.
    // [SerializeField]는 잘 들어가나 보고싶으면 쓰는 것이고 기본적으로는 쓰지 않는다.
    //[SerializeField]
    private List<UIElement> mElementList;

    // 2019.12.17 화요일 - 변수 추가 - 2교시 1번째
    private List<Colleague> mSpawnedList;
    [SerializeField]
    private Sprite[] mIconArr;

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
        // 텍스트 색깔을 바꾸는 방법
        // 띄어쓰기를 하면 안된다. 영문자는 대문자든 소문자든지 상관없다.
        mDataArr[0].Contents = "<color=ff0000ff>{1}</color>초 마다 <color=0000ffff>{0}</color>골드를 획득합니다.";
        mDataArr[0].JobTime = 1.1f;
        mDataArr[0].JobType = eJobType.Touch;
        // 2019.12.17 화요일 - 코드 추가
        // 동료캐릭터가 골드를 올려주는 기능을 할 때는 1로 설정
        mDataArr[0].ValueCurrent = 1;
        mDataArr[0].CostCurrent = 100;

        mDataArr[1] = new ColleagueData();
        mDataArr[1].Name = "No.2";
        mDataArr[1].Level = 0;
        mDataArr[1].Contents = "<color=#FF0000FF>{1}초 마다 한번씩 터치를 해줍니다.";
        mDataArr[1].JobTime = 1f;
        mDataArr[1].JobType = eJobType.Touch;
        // 2019.12.17 화요일 - 코드 추가
        // 동료캐릭터가 터치만 해주는 기능을 할 때는 0으로 설정
        mDataArr[1].ValueCurrent = 0;
        mDataArr[1].CostCurrent = 200;

        mDataArr[2] = new ColleagueData();
        mDataArr[2].Name = "No.3";
        mDataArr[2].Level = 0;
        mDataArr[2].Contents = "<color=ff0000ff>{1}</color>초 마다 <color=0000ffff>{0}</color>골드를 획득합니다.";
        mDataArr[2].JobTime = 1.5f;
        mDataArr[2].JobType = eJobType.Gold;
        // 2019.12.17 화요일 - 코드 추가
        // 동료캐릭터가 골드를 올려주는 기능을 할 때는 1로 설정
        mDataArr[2].ValueCurrent = 1;
        mDataArr[2].CostCurrent = 300;
    }
    // Start is called before the first frame update
    void Start()
    {
        // 2019.12.17 화요일 - 코드 위치 수정
        //UIElement a = new UIElement();

        // 2019.12.17 화요일 - 코드 추가
        mElementList = new List<UIElement>();

        // 2019.12.17 화요일 - 코드 수정
        //a.Init(AddLevel);
        //a.Init(mDataArr[0].Name, AddLevel);
        for (int i = 0; i < mDataArr.Length; i++)
        {
            // 2019.12.17 화요일 - 코드 추가
            UIElement elem = Instantiate(mElementPrefab, mScrollTarget);

            // 2019.12.17 화요일 - 코드 삭제
            // private UIElement[] mElementList; 변수 추가후 삭제
            //UIElement a = new UIElement();

            // 데이터클래스로 넘겨주면 되지만 일일히 데이터를 넘겨준 이유
            // mDataArr[i]는 데이터에 변수들이 일관성이 없을 가능성이 더 높다.
            // mDataArr[i]에는 현재는 ColleagueData클래스이지만 나중에는 SkillData클래스도 만들것이다.
            // 이것도 여러가지 자료형의 데이터를 대응하기 위하여 일일히 데이터를 넘겨준 이유이다.
            // 편의성면에서는 데이터클래스를 넘겨주게 하는 것이 더 좋은 방법이다.
            // 부모데이터클래스가 있고 자식데이터클래스로 부모데이터클래스를 상속받아서 만드는 방법으로 해야
            // 하지만 상황에 따라서는 그 방법이 안될 가능성도 있다.

            // 2019.12.17 화요일 - 코드 수정
            //a.Init(
            //    mDataArr[i].Name,
            //    mDataArr[i].Contents,
            //    "구매",
            //    mDataArr[i].Level,
            //    mDataArr[i].ValueCurrent,
            //    mDataArr[i].CostCurrent,
            //    mDataArr[i].JobTime,
            //    AddLevel
            //);

            // sprite가 null이면 하얀색으로 바뀔 것이다.
            // 지금은 icon이 준비가 되지 않았으므로 null로 해준다.
            elem.Init(null, i, mDataArr[i].Name, mDataArr[i].Contents, "구매",
                      mDataArr[i].Level, mDataArr[i].ValueCurrent,
                      mDataArr[i].CostCurrent, mDataArr[i].JobTime, AddLevel);

            // 데이터를 저장하고 로드하는 기능을 추가해줄 것이기 때문에 Start에서 해주었다.
            // 원래는 Awake에서 해주는 것이 좋다.
            mElementList.Add(elem);
        }
    }
    public void JobFinish(int id)
    {
        ColleagueData data = mDataArr[id];
        switch(data.JobType)
        {
            case eJobType.Gold:
                // 2019.12.17 화요일 - 코드 수정
                //GameController.Instance.Gold += 1;
                // 이렇게 하드코딩이 아닌 방식으로 하는 경우 음수가 더해지는 경우가 안생기도록
                // 코드 설계를 잘 해야 한다.
                GameController.Instance.Gold += data.ValueCurrent;
                break;
            case eJobType.Touch:
                // 조건에 맞으면 터치만 해주면 되므로 이 상황에서는 문제가 생기지 않는다.
                GameController.Instance.Touch();
                break;
            default:
                Debug.LogError("Wrong job type " + data.JobType);
                break;
        }
    }

    // 2019.12.17 화요일 - 함수 삭제
    //public void TempInstantiate(int id)
    //{
    //    AddLevel(id, 1);
    //}

    // AddLevel을 누르면 묻지도 따지지도 않고 동료캐릭터가 스폰이 되도록 설정
    public void AddLevel(int id, int amount)
    {
        // 2019.12.17 화요일 - if문 추가
        if (mDataArr[id].Level == 0)
        {
            Colleague newCol = Instantiate(mPrefabArr[id]);
            newCol.transform.position = mSpawnPos.position;

            // 2019.12.17 화요일 - 매개변수 삭제
            //newCol.Init(mDataArr[id].Name, id, mDataArr[id].JobTime);
            newCol.Init(id, mDataArr[id].JobTime);

            // 2019.12.17 화요일 - 코드 추가
            mSpawnedList.Add(newCol);
        }

        // 2019.12.17 화요일 - 코드 추가
        // 레벨을 실질적으로 올려준다.
        mDataArr[id].Level += amount;
        // 2019.12.17 화요일 - 코드 추가
        mDataArr[id].ValueCurrent += mDataArr[id].Level;
        // 2019.12.17 화요일 - 코드 추가
        mDataArr[id].CostCurrent += mDataArr[id].Level;

        // 2019.12.17 화요일 - 코드 추가
        mElementList[id].Renew(mDataArr[id].Contents, "구매", mDataArr[id].Level,
                               mDataArr[id].ValueCurrent, mDataArr[id].CostCurrent, mDataArr[id].JobTime);
    }
}
public class ColleagueData
{
    public string Name;
    public int Level;
    public string Contents;
    public float JobTime;
    public eJobType JobType;
    // 2019.12.17 화요일 - 데이터클래스 변수 추가
    public double ValueCurrent;
    public double CostCurrent;
}
public enum eJobType
{
    Gold, Touch
}