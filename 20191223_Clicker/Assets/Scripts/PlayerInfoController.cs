using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoController : MonoBehaviour
{
    public static PlayerInfoController Instance;

    [SerializeField]
    private PlayerInfo[] mInfos;

    private void Awake()
    {
        if (Instance == null)
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class PlayerInfo
{
    public string Name;
    public string Contents;

    public int ID;
    public int IconID;
    public int Level;

    public ePlayerValueType ValueType;

    public float CoolTime;
    public float CoolTImeCurrent;
    public float Duration;

    public double ValueCurrent;
    public double ValueWeight;
    public double ValueBase;

    public double CostCurrent;
    public double CostWeight;
    public double CostBase;
}

public enum ePlayerValueType
{
    // 기하급수로 증가하는 방식
    Expo,
    // 값을 더하는 방식
    Numeric,
    // 퍼센트로 증가하는 방식
    Percent
}

