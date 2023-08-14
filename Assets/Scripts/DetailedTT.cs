using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailedTT : MonoBehaviour
{
    public TimeTable timeTableObj;
    public GameObject TTPopUP;
    public Button CloseBtn;
    public TextMeshProUGUI[] period;

    private bool isActiveDetailedTT=false;
    private string activeClass;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveDetailedTT)
        {
            for (int i = 0; i < 8; i++)
            {
                period[i].text = timeTableObj.timeTable[activeClass][timeTableObj.dayOfWeek][i];
                if (timeTableObj.curPeriod == i)
                {
                    period[i].color = new Color32(0, 143, 175, 255);
                    period[i].fontStyle = FontStyles.Bold;
                }
                else
                {
                    period[i].color = Color.black;
                    period[i].fontStyle = FontStyles.Normal;
                }
            }
        }
    }

    public void displayTT(Button classBtn)
    {
        if (timeTableObj.dayOfWeek == -1 || timeTableObj.dayOfWeek == 5) return;
        TTPopUP.SetActive(true);
        isActiveDetailedTT = true;
        activeClass = classBtn.gameObject.name;
    }

    public void hideTT()
    {
        TTPopUP.SetActive(false);
        isActiveDetailedTT = false;
    }
}
