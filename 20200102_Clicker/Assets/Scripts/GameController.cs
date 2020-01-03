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
    [SerializeField]
    private GemController mGem;
#pragma warning restore

    public double TouchPower
    {
        get { return mTouchPower; }
        set { mTouchPower = value; }
    }
    private double mTouchPower;

    // 2020.01.02 목요일 - 변수 & 프로퍼티 추가
    public float CriticalRate 
    {
        get { return mCriticalRate; }
        set { mCriticalRate = value; }
    }
    private float mCriticalRate;

    // 2020.01.02 목요일 - 변수 & 프로퍼티 추가
    public float CriticalVaule 
    {
        get { return mCriticalValue; }
        set { mCriticalValue = value; }
    }
    private float mCriticalValue;

    // 2020.01.03 금요일 - 프로퍼티 추가
    public double IncomeBonusWeight 
    {
        get { return mGem.IncomeBonusWeight; }
        set { mGem.IncomeBonusWeight = value; }
    }
    // 2020.01.03 금요일 - 프로퍼티 추가
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
        // 2020.01.02 목요일 - 코드 추가
        //PlayerPrefs.DeleteAll();

        Load();
        // 2020.01.02 목요일 - 코드 추가
        StartCoroutine(LoadGames());
    }

    // 2020.01.02 목요일 - 함수 추가
    private IEnumerator LoadGames()
    {
        WaitForSeconds pointOne = new WaitForSeconds(.1f);

        while(!PlayerInfoController.Instance.bLoaded || 
              !ColleagueController.Instance.bLoaded)
        {
            yield return pointOne;
        }
        // 2020.01.02 목요일 - 코드 추가
        if(mPlayer.GemID < 0)
        {
            mPlayer.GemID = UnityEngine.Random.Range(0, GemController.MAX_GEM_COUNT);
        }
        mGem.LoadGem(mPlayer.GemID, mPlayer.GemHP);
        PlayerInfoController.Instance.Load(mPlayer.PlayerLevels);
        ColleagueController.Instance.Load(mPlayer.ColleagueLevels);
    }

    public void Touch()
    {
        // 2020.01.02 목요일 - 코드 추가
        double touchPower = mTouchPower;
        // 2020.01.02 목요일 - 코드 추가
        //float randVar = UnityEngine.Random.Range(0, 100f);
        // 0 ~ 1사이의 값이 나오도록 하는 함수이다.
        float randVar = UnityEngine.Random.value; 

        if(randVar <= mCriticalRate)
        {
            // 크리티컬 확률이 들어왔으므로 크리티컬이 터져야 한다.
            touchPower *= (1 + mCriticalValue);
            Debug.Log("Critical" + "!!!!");
        }

        if (mGem.AddProgress(mTouchPower))
        {
            mPlayer.Stage++;
            mPlayer.GemID = UnityEngine.Random.Range(0, GemController.MAX_GEM_COUNT);
            mGem.GetNewGem(mPlayer.GemID);
        }
    }

    public void Save()
    {
        mPlayer.GemHP = mGem.CurrentHP;
        mPlayer.PlayerLevels = PlayerInfoController.Instance.LevelArr;
        mPlayer.ColleagueLevels = ColleagueController.Instance.LevelArr;

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
            // 2020.01.02 목요일 - 코드 추가
            mPlayer = new PlayerSaveData();
            // 2020.01.02 목요일 - 코드 추가
            // 새로운 데이터가 생성이 되었을 때 그 상태에 대한 안내에 대한 키워드를 알려주어야 한다.
            mPlayer.GemID = -1;
            // 2020.01.02 목요일 - 코드 추가
            mPlayer.PlayerLevels = new int[StaticValues.PLATER_INFOS_LENGTH];
            // 2020.01.02 목요일 - 코드 추가
            mPlayer.PlayerLevels[0] = 1;
            // 2020.01.02 목요일 - 코드 추가
            mPlayer.ColleagueLevels = new int[StaticValues.COLLEAGUE_INFOS_LENGTH];
            // 2020.01.02 목요일 - 코드 추가
            mPlayer.ColleagueLevels[0] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
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
