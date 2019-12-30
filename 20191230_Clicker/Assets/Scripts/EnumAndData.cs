using System;
using System.Collections;
using System.Collections.Generic;

public enum eEffectType
{
    Touch,
    PhaseShift
}
public enum eTextEffectType
{
    ColleagueIncome
}
public enum eValueType
{
    Expo,
    Numeric,
    Percent
}

[Serializable]
// 2019.12.30 금요일 - 클래스 추가
public class PlayerSaveData
{
    public int Stage;
    public double Gold;
    public double GemHP;
    public int[] PlayerLevels;
    public int[] ColleagueLevels;
}
