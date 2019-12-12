using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIController : MonoBehaviour
{
    public static MainUIController Instance;
    private static int mUIMoveHash = Animator.StringToHash("Move");
    [SerializeField]
    private Animator[] mWindowAnims;
    [SerializeField]
    private GaugeBar mProgressBar;

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
        
    }

    // 2019.12.12 - 함수 수정
    //public void ShowProgress(float progress)
    //{
    //    mProgressBar.ShowGaugeBar(progress);
    //}

    public void ShowProgress(double current, double max)
    {
        //TODO Calculate Gauge Progress float value
        // currnt / max를 먼저 double형태로 계산을 하고 그 계산된 값을 flaot으로 바꾸는 작업을 해준다.
        // float에서 오버플로가 나면 자료형에 상관없이 infinity 무한으로 나온다.
        float progress = (float)(current / max);
        //todo Build Gauge Progress string
        // 표준숫자형식 문자열
        string progressString = progress.ToString("P0");
        //mProgressBar.ShowGaugeBar(progress);
        mProgressBar.ShowGaugeBar(progress, progressString);
    }

    public void MoveWindow(int id)
    {
        mWindowAnims[id].SetTrigger(mUIMoveHash);
    }
}
