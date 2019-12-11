using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private double mGold;
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
        // 2019.12.11 수요일 - 코드 추가
        // 조건을 두고서 새로운 보석을 가져와야 한다.
        int id = Random.Range(0, GemController.MAX_GEM_COUNT);
        mGem.GetNewGem(id);
    }

    // 2019.12.11 수요일 - 함수 추가
    // 터치기능을 만들어보자
    // 터치기능은 어떤 클래스가 호출을 할 것인지를 생각해보자.
    // 터치매니저에서 터치가 되는 시점을 인식 -> 게임컨틀롤러의 터치함수(게이트웨이기능) -> 잼컨트롤러
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
