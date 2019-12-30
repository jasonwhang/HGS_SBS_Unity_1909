using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimHash
{
    public static readonly int Move = Animator.StringToHash("IsMove");
    // 2019.12.30 월요일 - 변수 추가
    public static readonly int UIMove = Animator.StringToHash("Move");

    public delegate void TwoIntPramCallback(int a, int b);
    public delegate void VoidCallback();
}
