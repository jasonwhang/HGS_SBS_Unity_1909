using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    public const int MAX_GEM_COUNT = 3;

    [SerializeField]
    private int mSheetCount = 5;

    [SerializeField]
    private SpriteRenderer mGem;

    [SerializeField]
    private Sprite[] mGemSprite;
    // mPhaseBoundary = 2019.12.11 수요일 - 변수추가
    private double mCurrentHP, mMaxHP, mPhaseBoundary;
    private int mCurrentPhase, mStartIndex;

    // Start is called before the first frame update
    void Awake()
    {
        mGemSprite = Resources.LoadAll<Sprite>("Gem");
    }

    public void GetNewGem(int id)
    {
        mStartIndex = id * mSheetCount;
        mGem.sprite = mGemSprite[mStartIndex];
        mCurrentPhase = 0;
        // 2019.12.11 수요일 - 코드 추가
        // 새로운보석이 가져와지면 그 보석의 현재HP는 0으로 초기화를 해준다.
        mCurrentHP = 0;
        mMaxHP = 100;
        mPhaseBoundary = mMaxHP * 0.2f * (mCurrentPhase + 1);
    }

    public void AddProgress(double value)
    {
        mCurrentHP += value;
        Debug.LogFormat("{0} / {1}", mCurrentHP, mMaxHP);

        // 2019.12.11 수요일 - 코드 수정(if문 조건수정)
        if (mCurrentHP >= mPhaseBoundary)
        {
            //next phase
            mCurrentPhase++;

            if (mCurrentPhase > 4)
            {
                //Clear
                return;
            }
            mGem.sprite = mGemSprite[mStartIndex + mCurrentPhase];
            // 2019.12.11 수요일 - 코드 추가
            mPhaseBoundary = mMaxHP * 0.2f * (mCurrentPhase + 1);
        }
    }
}
