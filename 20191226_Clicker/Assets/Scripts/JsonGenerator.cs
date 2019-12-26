using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class JsonGenerator : MonoBehaviour
{
    [SerializeField]
    private ColleagueData[] mDataArr;

    [SerializeField]
    private PlayerInfo[] mPlayerInfos;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void LoadColleague()
    {
        string data = Resources.Load<TextAsset>("JsonFiles/Colleague").text;
        Debug.Log(data);
        // GemController에서 LoadAll을 할 때 자료형의 타입이 Sprite이고 받는 타입은 Sprite[]이었다.
        // 이유는 LoadAll자체가 []형태로 받아지기 때문이다.
        // 하지만 여기서는 ColleageData만 하면 1개만 나오기 때문에 ColleageData[]로 해주어야 한다.
        mDataArr = JsonConvert.DeserializeObject<ColleagueData[]>(data);
    }

    public void SaveColleague()
    {
        string data = JsonConvert.SerializeObject(mDataArr, Formatting.Indented);
        Debug.Log(data);

        StreamWriter writer = new StreamWriter(Application.dataPath + "/Colleague.json");
        writer.Write(data);
        writer.Close();
    }

    private void LoadPlayerInfo()
    {
        string data = Resources.Load<TextAsset>("JsonFiles/PlayerInfo").text;
        Debug.Log(data);

        mPlayerInfos = JsonConvert.DeserializeObject<PlayerInfo[]>(data);
    }

    private void SavePlayerInfo()
    {
        mPlayerInfos = PlayerInfoController.Instance.Infos;
        string data = JsonConvert.SerializeObject(mPlayerInfos, Formatting.Indented);
        Debug.Log(data);

        //StreamWriter writer = new StreamWriter(Application.dataPath + "/PlayerInfo.json");
        StreamWriter writer = new StreamWriter(Application.dataPath + "/Resources/JsonFiles/PlayerInfo.json");
        writer.Write(data);
        writer.Close();
    }

    [SerializeField]
    private Dummy[] mSampleArr; 
    private void LoadSample()
    {
        string data = Resources.Load<TextAsset>("JsonFiles/test").text;
        string[] dataArr = data.Split('\n');
        Debug.Log(dataArr.Length);
        mSampleArr = new Dummy[dataArr.Length - 2];
        for(int i = 0; i < mSampleArr.Length; i++)
        {
            string[] splited = dataArr[i + 1].Split(',');

            mSampleArr[i] = new Dummy();
            mSampleArr[i].id = int.Parse(splited[0]);
            mSampleArr[i].name = splited[1];
            mSampleArr[i].value = int.Parse(splited[2]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            LoadSample();
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SaveColleague();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadColleague();
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SavePlayerInfo();
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadPlayerInfo();
        }
    }
}

[System.Serializable]
public class Dummy
{
    public int id;
    public string name;
    public int value;
}
