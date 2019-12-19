using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimHash
{
    public static readonly int Move = Animator.StringToHash("IsMove");
    public delegate void TwoIntPramCallback(int a, int b);
    //2019.12.19 목요일 - 코드 추가
    public delegate void VoidCallback();
}
