using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTimeTable : MonoBehaviour
{
    public GameObject[] classes;
    public Button tTBtn;
    public bool isActiveTT=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleTT()
    {
        if(isActiveTT)
        {
            isActiveTT = false;
            for(int i = 0; i<4; i++) classes[i].SetActive(false);
            tTBtn.GetComponent<Image>().color = Color.white;
        }
        else
        {
            isActiveTT = true;
            for (int i = 0; i < 4; i++) classes[i].SetActive(true);
            tTBtn.GetComponent<Image>().color = new Color32(208, 247, 255, 255);
        }
    }

}
