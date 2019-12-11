using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIController : MonoBehaviour
{
    private static int mUIMoveHash = Animator.StringToHash("Move");
    [SerializeField]
    Animator[] mWindowAnims;

    [SerializeField]
    private GaugeBar mProgressBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowProgress(float progress)
    {
        mProgressBar.ShowGaugeBar(progress);
    }

    public void MoveWindow(int id)
    {
        mWindowAnims[id].SetTrigger(mUIMoveHash);
    }
}
