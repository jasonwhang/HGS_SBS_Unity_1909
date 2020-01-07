using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField]private PlayerSaveData mPlayer;

    public StaticValues.VoidCallback GoldConsumeCallback
    { get; set; }
    public double Gold {
        get { return mPlayer.Gold; }
        set
        {
            if (value >= 0)
            {
                if(mPlayer.Gold > value)
                {
                    GoldConsumeCallback?.Invoke();
                    GoldConsumeCallback = null;
                }
                mPlayer.Gold = value;
                MainUIController.Instance.ShowGold(mPlayer.Gold);
                // UI show gold
            }
            else
            {
                //돈이 부족함
                Debug.Log("Not enough gold");
            }
        }
    }

    public int StageNumber
    {
        get { return mPlayer.Stage; }
    }
#pragma warning disable 0649
    [SerializeField]private GemController mGem;
#pragma warning restore

    public double TouchPower
    {
        get { return mTouchPower; }
        set { mTouchPower = value; }
    }
    private double mTouchPower;
    
    public float CriticalRate
    {
        get { return mCriticalRate; }
        set { mCriticalRate = value; }
    }
    private float mCriticalRate;

    public float CriticalValue
    {
        get { return mCriticalValue; }
        set { mCriticalValue = value; }
    }
    private float mCriticalValue;

    public double IncomeBonusWeight
    {
        get { return mGem.IncomeBonusWeight; }
        set { mGem.IncomeBonusWeight = value; }
    }

    public double MaxHPWeight
    {
        get { return mGem.MaxHPWeight; }
        set { mGem.MaxHPWeight = value; }
    }

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
    }
    // Start is called before the first frame update
    void Start()
    {
        MainUIController.Instance.ShowGold(mPlayer.Gold);
        //PlayerPrefs.DeleteAll();
        Load();
        StartCoroutine(LoadGames());
       
    }

    private IEnumerator LoadGames()
    {
        WaitForSeconds pointOne = new WaitForSeconds(.1f);
        while(!PlayerInfoController.Instance.bLoaded || 
              !ColleagueController.Instance.bLoaded)
        {
            yield return pointOne;
        }
        if(mPlayer.GemID < 0)
        {
            mPlayer.GemID = UnityEngine.Random.Range(0, GemController.MAX_GEM_COUNT);
        }
        mGem.LoadGem(mPlayer.GemID, mPlayer.GemHP);

        // 2020.01.07 화요일 - 코드 수정
        //PlayerInfoController.Instance.Load(mPlayer.PlayerLevels);
        PlayerInfoController.Instance.Load(mPlayer.PlayerLevels, mPlayer.Cooltimes);

        ColleagueController.Instance.Load(mPlayer.ColleagueLevels);
    }

    public void Touch()
    {
        double touchPower = mTouchPower;

        float randVal = UnityEngine.Random.value;

        if(randVal <= mCriticalRate)
        {
            touchPower *= 1 + mCriticalValue;
            Debug.Log("Cirtical" +
                "!!!!!");
        }

        if (mGem.AddProgress(touchPower))
        {
            mPlayer.Stage++;
            mPlayer.GemID = UnityEngine.Random.Range(0, GemController.MAX_GEM_COUNT);
            mGem.GetNewGem(mPlayer.GemID);
        }
    }

    // 2020.01.07 화요일 - 함수 추가
    public void Rebirth()
    {
        mPlayer.Soul += 10 * mPlayer.Stage;

        mPlayer.Stage = 0;
        mPlayer.GemID = -1;
        mPlayer.Gold = 0;
        mPlayer.GemHP = 0;
        mPlayer.PlayerLevels = new int[StaticValues.PLAYER_INFOS_LEGNTH];
        mPlayer.PlayerLevels[0] = 1;
        mPlayer.Cooltimes = new float[StaticValues.COOLTIME_LENGTH];
        mPlayer.ColleagueLevels = new int[StaticValues.COLLEAGUE_INFOS_LENGTH];

        PlayerInfoController.Instance.Load(mPlayer.PlayerLevels, mPlayer.Cooltimes);
        ColleagueController.Instance.Rebirth();
        ColleagueController.Instance.Load(mPlayer.ColleagueLevels);
    }

    public void Save()
    {
        mPlayer.GemHP = mGem.CurrentHP;
        //mPlayer.PlayerLevels = PlayerInfoController.Instance.LevelArr;

        // 2020.01.07 화요일 - 코드 삭제
        //mPlayer.ColleagueLevels = ColleagueController.Instance.LevelArr;

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();

        formatter.Serialize(stream, mPlayer);

        string data = Convert.ToBase64String(stream.GetBuffer());
        Debug.Log(data);
        PlayerPrefs.SetString("Player", data);
        stream.Close();
    }

    public void Load()
    {
        string data = PlayerPrefs.GetString("Player");
        if (!string.IsNullOrEmpty(data))
        {
            Debug.Log(data);
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(data));

            mPlayer = (PlayerSaveData)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            mPlayer = new PlayerSaveData();
            mPlayer.GemID = -1;
            mPlayer.PlayerLevels = new int[StaticValues.PLAYER_INFOS_LEGNTH];
            mPlayer.PlayerLevels[0] = 1;
            // 2020.01.07 화요일 - 코드 추가
            mPlayer.Cooltimes = new float[StaticValues.COOLTIME_LENGTH];
            mPlayer.ColleagueLevels = new int[StaticValues.COLLEAGUE_INFOS_LENGTH];
        }

        // 2020.01.07 화요일 - 코드 추가
        FixSavedData();
    }

    // 2020.01.07 화요일 - 함수 추가
    // 갱신하는 함수를 만들어준다.
    private void FixSavedData()
    {
        // 저장되어 있는 데이터가 없는 경우
        if(mPlayer.PlayerLevels == null)
        {
            mPlayer.PlayerLevels = new int[StaticValues.PLAYER_INFOS_LEGNTH];
        }
        // 이전에 저장되어 있는 데이터의 길이가 새롭게 추가된 플레이어 정보데이터의 길이보다 작은 경우
        // 즉 새롭게 추가된 변수가 있는 경우
        else if(mPlayer.PlayerLevels.Length < StaticValues.PLAYER_INFOS_LEGNTH)
        {
            // 새롭게 추가된 플레이어 정보데이터의 길이에 맞는 새로운 Arr을 만들어준다.
            int[] temp = new int[StaticValues.PLAYER_INFOS_LEGNTH];
            // 기존에 저장되어 있는 플레이어레벨의 데이터의 길이는 새롭게 만들어준 Arr에 그대로 복사를 해준다.
            for(int i = 0; i < mPlayer.PlayerLevels.Length; i++)
            {
                temp[i] = mPlayer.PlayerLevels[i];
            }
            mPlayer.PlayerLevels = temp;
        }
        // else는 일부러 넣지 않는 것이다.
        // 새롭게 만들어서 복사하는 작업을 else if로 하는 이유는 else로 하게 되면 
        // 정상적인 데이터가 들어오든지 비정상적인 데이터든지 간에 else가 들어와서 새롭게 할당하고 복사하는 작업을 하게 되어 
        // 퍼포먼스 상으로 문제가 생길 수도 있다.
        // 그러므로 else부분은 정상적인 데이터가 들어오는 경우이므로 그냥 아무것도 쓰지 않고 패스를 시켜버렸다.
        
        if(mPlayer.ColleagueLevels == null)
        {
            mPlayer.ColleagueLevels = new int[StaticValues.COLLEAGUE_INFOS_LENGTH];
        }
        else if(mPlayer.ColleagueLevels.Length < StaticValues.COLLEAGUE_INFOS_LENGTH)
        {
            int[] temp = new int[StaticValues.COLLEAGUE_INFOS_LENGTH];

            for (int i = 0; i < mPlayer.ColleagueLevels.Length; i++)
            {
                temp[i] = mPlayer.ColleagueLevels[i];
            }
            mPlayer.ColleagueLevels = temp;
        }

        if (mPlayer.Cooltimes == null)
        {
            mPlayer.Cooltimes = new float[StaticValues.COOLTIME_LENGTH];
        }
        else if (mPlayer.ColleagueLevels.Length < StaticValues.COOLTIME_LENGTH)
        {
            float[] temp = new float[StaticValues.COOLTIME_LENGTH];

            for (int i = 0; i < mPlayer.Cooltimes.Length; i++)
            {
                temp[i] = mPlayer.Cooltimes[i];
            }
            mPlayer.Cooltimes = temp;
        }
    }

    // 2020.01.07 화요일 - 함수 추가
    private void OnApplicationQuit()
    {
        Save();
    }

    // Update is called once per frame
    void Update()
    {
        // 2020.01.07 화요일 - 코드 추가
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Save();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            Load();
            mGem.LoadGem(mPlayer.GemID, mPlayer.GemHP);
        }
    }
}
