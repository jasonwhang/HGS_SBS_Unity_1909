using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2019.12.13 금요일 - 클래스추가

public class ColleagueController : MonoBehaviour
{
    private ColleagueData[] mDataArr;
    [SerializeField]
    private Colleague[] mPrefabArr;
    [SerializeField]
    private Transform mSpawnPos;

    private void Awake()
    {
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

    // amount를 해주는 이유는 1레벨업씩만 하는 것이 아닌 10레벨업씩 할 수도 있기 때문이다.
    // 예를들어 함수를 호출할 때 0번데이터를 10레벨업을 할 것이다.
    public void AddLevel(int id, int amount)
    {

    }
}

public class ColleagueData
{
    public string Name;
    public int Level;
    public float JobTime;
    public double CostCurrent;
}
