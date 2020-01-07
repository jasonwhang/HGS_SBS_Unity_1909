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
public enum eSkillID
{
    // 2020.01.07 화요일 - 코드 수정
    // CoolTimeIndex로 스킬을 접근하려고 바꾸었기 때문에 1부터 시작하는 것이 아니라 0부터 시작하는 것으로 바꾸었다.
    //Chain = 1,
    Chain,
    Overwork
}


[Serializable]
public class PlayerSaveData
{
    public int Stage;
    public int GemID;
    public double Gold;
    // 2020.01.07 화요일 - 변수 추가
    public double Soul;
    public double GemHP;
    public int[] PlayerLevels;
    // 2020.01.07 화요일 - 변수 추가
    public float[] Cooltimes;
    public int[] ColleagueLevels;
}