using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
#pragma warning disable 0649
    public const int MAX_GEM_COUNT = 3;

    [SerializeField] private EffectPool mEffectPool;
    [SerializeField] private int mSheetCount = 5;
    [SerializeField] private SpriteRenderer mGem;
    [SerializeField] private Sprite[] mGemSprite;
    [SerializeField] private float mHPBase = 10, mHPWeight = 1.4f,
                                  mRewardBase = 10, mRewardWeight = 1.5f;

    private double mCurrentHP, mMaxHP, mPhaseBoundary;
    // 2019.12.30 월요일 - 코드 추가
    public double CurrentHP { get { return mCurrentHP; } }

    private int mCurrentPhase, mStartIndex;
#pragma warning restore

    // Start is called before the first frame update
    void Awake()
    {
        mGemSprite = Resources.LoadAll<Sprite>("Gem");
    }

    // 2019.12.30 월요일 - 함수 추가
    public void LoadGem(int lastGemID, double currentHP)
    {
        // 현재는 lastGemID와 currentHP 두가지의 정보만을 가지고 현재 보석의 정보를 다시 복원시켜야 한다.

        mStartIndex = lastGemID * mSheetCount;
        mCurrentHP = currentHP;
        mMaxHP = mHPBase * Math.Pow(mHPWeight, GameController.Instance.StageNumber);

        //mCurrentPhase에 대한 정보를 현재 저장하지 않았으므로 저장하지 않고 가져오는 방법으로 해결을 해보자.
        mPhaseBoundary = mMaxHP * 0.2f * (mCurrentPhase + 1);
        MainUIController.Instance.ShowProgress(mCurrentHP, mMaxHP);

        mCurrentPhase = 0;
        while(mCurrentHP >= mPhaseBoundary)
        {
            mCurrentPhase++;
            mPhaseBoundary = mMaxHP * 0.2f * (mCurrentPhase + 1);
        }

        // 보석의 이미지 페이즈 변환작업은 AddProgress함수에서 작업을 해준다.
        // 이미지를 변환시키기 위해서는 mCurrentPhase의 값을 받아와야 한다.
        mGem.sprite = mGemSprite[mStartIndex + mCurrentPhase];
    }

    public void GetNewGem(int id)
    {
        mStartIndex = id * mSheetCount;
        mGem.sprite = mGemSprite[mStartIndex];
        mCurrentPhase = 0;
        mCurrentHP = 0;
        mMaxHP = mHPBase * Math.Pow(mHPWeight, GameController.Instance.StageNumber);
        mPhaseBoundary = mMaxHP * 0.2f * (mCurrentPhase + 1);
        MainUIController.Instance.ShowProgress(mCurrentHP, mMaxHP);
    }

    public bool AddProgress(double value)
    {
        mCurrentHP += value;
        MainUIController.Instance.ShowProgress(mCurrentHP, mMaxHP);
        if (mCurrentHP >= mPhaseBoundary)
        {
            //next phase
            mCurrentPhase++;
            //GameController.Instance.NextImage();
            if (mCurrentPhase > 4)
            {
                //Clear
                //GameController.Instance.NextStage();
                GameController.Instance.Gold += mRewardBase *
                            Math.Pow(mRewardWeight, GameController.Instance.StageNumber);
                return true;
            }
            Timer effect = mEffectPool.GetFromPool((int)eEffectType.PhaseShift);
            effect.transform.position = mGem.transform.position;
            mGem.sprite = mGemSprite[mStartIndex + mCurrentPhase];
            mPhaseBoundary = mMaxHP * 0.2f * (mCurrentPhase + 1);
        }
        return false;
    }
}