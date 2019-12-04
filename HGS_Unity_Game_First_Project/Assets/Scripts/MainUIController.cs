using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2019.12.04 - 클래스 추가

public class MainUIController : MonoBehaviour
{
    // 변수가 한개여도 여러 군데에서 공유해서 사용하기 위하여 static으로 만듬
    private static int mUIMoveHash = Animator.StringToHash("Move");

    [SerializeField]
    Animator[] mWindowAnims;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 버튼에 연결하여 AddListener에 for문으로 index별로 넣는 것은 되지 않는다.
    public void MoveWindow(int id)
    {
        mWindowAnims[id].SetTrigger(mUIMoveHash);
    }
}
