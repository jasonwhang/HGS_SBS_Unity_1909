using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 2019.12.03 - 코드 추가
using UnityEngine.UI;
// 2019.12.03 - 코드 추가
using UnityEngine.SceneManagement;

// 2019.12.03 - 클래스 추가

public class TitleController : MonoBehaviour
{
    // 2019.12.03 - 변수 추가
    [SerializeField]
    private Button mStartButton;
    // 2019.12.03 - 변수 추가
    [SerializeField]
    private Text mStatusText;

    // Start is called before the first frame update
    void Start()
    {
        // 2019.12.03 - 코드 추가
        mStartButton.onClick.AddListener(StartMainGame);
        // 2019.12.03 - 코드 추가
        mStatusText.text = "Touch to Start";
    }

    // 2019.12.03 - 함수 추가
    public void StartMainGame()
    {
        SceneManager.LoadScene(1);
    }
}
