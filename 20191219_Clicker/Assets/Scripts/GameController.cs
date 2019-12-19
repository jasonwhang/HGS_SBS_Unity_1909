using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    // 2019.12.19 목요일 - 코드 추가
    public AnimHash.VoidCallback goldConsumeCallback { get; set; }

    private double mGold;
    public double Gold {
        get { return mGold; }
        set
        {
            // 2019.12.19 목요일 - 코드 추가
            // 
            if(value >= 0)
            {
                if(mGold > value)
                {
                    // delegate가 비어있는지 확인시켜주는 키워드가 c# 7버전 이상에서부터 나왔다.
                    // ctrl + .을 누르면 코드가 알아서 바뀐다.
                    //if(goldConsumeCallback != null)
                    //{
                    //    goldConsumeCallback();
                    //}
                    goldConsumeCallback?.Invoke();
                    // 골드소모를 시켜주는 함수를 호출했으므로 delegate를 비워준다.
                    // 이중으로 호출이 되는 경우를 막기 위해서 이다.
                    goldConsumeCallback = null;
                    // 밑에 두개의 코드는 완전히 똑같고 퍼포먼스 상에서도 차이점이 별로 없다.
                    //goldConsumeCallback();
                    //goldConsumeCallback.Invoke();

                    // 이 경우가 아니면 UseGold함수를 따로 만들어서 매개변수에 delegate함수를
                    // 넣어주어 delegate의 기본을 null로 해주고 함수 안에서
                    // delegate를 호출해주면 간단해진다.
                    // 하지만 우리는 지금 get;set;을 사용하려고 하기 때문에 이렇게 약간 어려워진것이다.
                }
                mGold = value;
            }
            else
            {

            }
        }
    }
    private int mStage;
    [SerializeField]
    private GemController mGem;

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
        int id = Random.Range(0, GemController.MAX_GEM_COUNT);
        mGem.GetNewGem(id);
    }

    public void Touch()
    {
        if(mGem.AddProgress(1))
        {
            int id = Random.Range(0, GemController.MAX_GEM_COUNT);
            mGem.GetNewGem(id);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
