using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Autocomplete : MonoBehaviour
{
    public Coordinates coordinates;
    public GridManager gridManager;
    public GameObject suggestionScroll;
    public GameObject btnBack;
    public GameObject whiteScreen;
    public TMP_InputField tfCurr;
    public TMP_InputField tfDest;

    public GameObject btnTemplate;
    public GameObject suggestionPanel;

    List<GameObject> suggestions = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        LoadSuggestions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSuggestions()
    {
        GameObject g;
        foreach (KeyValuePair<string, Tuple<int,Vector2Int>> roomCoord in coordinates.RoomCoord)
        {
            g = Instantiate(btnTemplate, suggestionPanel.transform);
            g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = roomCoord.Key;
            g.GetComponent<Button>().onClick.AddListener(() => SelectSuggestion(roomCoord.Key));
            suggestions.Add(g);
        }
        Destroy(btnTemplate);
        foreach (GameObject suggestion in suggestions)
        {
            suggestion.SetActive(false);
        }
    }

    public void ShowSuggestions()
    {
        suggestionScroll.SetActive(true);
        whiteScreen.SetActive(true);
        btnBack.SetActive(true);
    }

    public void HideSuggestions()
    {
        suggestionScroll.SetActive(false);
        whiteScreen.SetActive(false);
        btnBack.SetActive(false);
        foreach (GameObject suggestion in suggestions)
        {
            suggestion.SetActive(false);
        }
    }

    public void UpdateSuggestions(string s)
    {
        int inputLen = s.Length;
        if (inputLen == 0)
        {
            foreach (GameObject suggestion in suggestions) suggestion.SetActive(false);
            return;
        }

        string temp;
        foreach (GameObject suggestion in suggestions)
        {
            //suggestion.SetActive(true);
            temp = suggestion.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            if (temp.Replace(" ","").Contains(s.Replace(" ","")))
            {
                suggestion.SetActive(true);
            }
            else
            {
                suggestion.SetActive(false);
            }
        }
    }

    public void SelectSuggestion(string s)
    {
        if (gridManager.currSelected)
        {
            tfCurr.text = s;
        }
        else if (gridManager.destSelected)
        {
            tfDest.text = s;
        }
        HideSuggestions();
    }

}
