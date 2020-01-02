using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticValues
{
    public static readonly int Move = Animator.StringToHash("IsMove");
    public static readonly int UIMove = Animator.StringToHash("Move");
    public delegate void TwoIntPramCallback(int a, int b);
    public delegate void VoidCallback();
    private const string JSON_PATH = "JsonFiles/";
    public const string COLLEAGUE_DATA_PATH = JSON_PATH +"Colleague";
    public const string PLAYER_DATA_PATH = JSON_PATH + "PlayerInfo";

    // 2020.01.02 목요일 - 상수 변수 추가
    public const int PLATER_INFOS_LENGTH = 7;
    public const int COLLEAGUE_INFOS_LENGTH = 3;
}
