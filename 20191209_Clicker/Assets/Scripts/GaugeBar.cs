using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
    [SerializeField]
    private Image mGaugeBar;

    // Start is called before the first frame update
    void Start()
    {
        mGaugeBar.GetComponent<Image>();
    }

    public void ShowGaugeBar(float value)
    {
        mGaugeBar.fillAmount = value;
    }
}
