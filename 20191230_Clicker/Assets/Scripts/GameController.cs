// 2019.12.30 월요일 - using 코드 추가
using System;
using System.Collections;
using System.Collections.Generic;
// 2019.12.30 월요일 - using 코드 추가
using System.IO;
// 2019.12.30 월요일 - using 코드 추가
// 시스템이 Runtime중에 직렬화된 binary형태로 변화시켜주기 위한 라이브러리
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameController : MonoBehaviour
{
#pragma warning disable 0649
    public static GameController Instance;

    [SerializeField]
    // 2019.12.30 월요일 - 변수 추가
    private PlayerSaveData mPlayer;

    public AnimHash.VoidCallback GoldConsumeCallback { get; set; }
#pragma warning restore

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

    [SerializeField]private GemController mGem;

    public double TouchPower
    {
        get { return mTouchPower; }
        set { mTouchPower = value; }
    }
    private double mTouchPower;

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
        MainUIController.Instance.ShowGold(0);
        int id = UnityEngine.Random.Range(0, GemController.MAX_GEM_COUNT);
        mGem.GetNewGem(mPlayer.GemID);
    }

    public void Touch()
    {
        if(mGem.AddProgress(mTouchPower))
        {
            mPlayer.Stage++;
            int id = UnityEngine.Random.Range(0, GemController.MAX_GEM_COUNT);
            mGem.GetNewGem(mPlayer.GemID);
        }
    }

    // 2019.12.30 월요일 - 함수 추가
    public void Save()
    {
        mPlayer.GemHP = mGem.CurrentHP;
        mPlayer.PlayerLevels = PlayerInfoController.Instance.LevelArr;
        mPlayer.ColleagueLevels = ColleagueController.Instance.LevelArr;

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();

        // mPlayer의 데이터를 stream에 메모리를 통채로 집어넣는 기능이다.(데이터가 복제가 된 것이다.)
        formatter.Serialize(stream, mPlayer);

        // Buffer는 어떠한 상황에서 데이터를 사용하기 위해 미리 들고 있는 메모리 데이터의 공간이다.
        // 버퍼링 : 인터넷에서 영상의 데이터를 미리 어느정도 들고 있게 하기 위해 로드를 하는 시간을 의미한다.
        string data = Convert.ToBase64String(stream.GetBuffer());
        Debug.Log(data);
        PlayerPrefs.SetString("Player", data);
        stream.Close();
    }

    // 2019.12.30 월요일 - 함수 추가
    public void Load()
    {
        string data = PlayerPrefs.GetString("Player");
        if(!string.IsNullOrEmpty(data))
        {
            // 정상적인 데이터가 data에 들어온 경우
            Debug.Log(data);

            BinaryFormatter formatter = new BinaryFormatter();
            // FromBase64String의 return값은 byte[]이다.
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(data));

            // formatter.Deserialize의 return값이 object이다.
            // 강제형변환을 해준다.
            mPlayer = (PlayerSaveData)formatter.Deserialize(stream);

            stream.Close();
        }
        else
        {
            // 잘못된 데이터가 data에 들어온 경우
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
        }
    }
}
