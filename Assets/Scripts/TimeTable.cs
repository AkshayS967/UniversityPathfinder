using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TimeTable : MonoBehaviour
{
    public TextMeshProUGUI CSEA;
    public TextMeshProUGUI CSEB;
    public TextMeshProUGUI CSEC;
    public TextMeshProUGUI CSED;
    public TextMeshProUGUI cloudAndEthicalT0;
    public TextMeshProUGUI cloudAndEthicalT1;

    public GameObject cloudAndEthical0;
    public GameObject cloudAndEthical1;

    public GameObject patternRec;
    public GameObject mobileAdhoc;
    public GameObject bigData;
    public GameObject neuro;

    public GameObject infoRet;
    public GameObject compVision;
    public GameObject sna;
    public GameObject env;


    public ShowTimeTable showTimeTable;

    private TimeSpan curTime;
    private string[] curClass = new string[4];
    public int dayOfWeek;
    public int curPeriod;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (showTimeTable.isActiveTT)
        {
            curTime = DateTime.Now.TimeOfDay;
            dayOfWeek = (int)DateTime.Now.DayOfWeek - 1;
            if (dayOfWeek == -1 || dayOfWeek == 5)
            {
                for (int i = 0; i < curClass.Length; i++)
                {
                    curClass[i] = "No Class";
                }
            }
            else
            {
                for (int i = 0; i < curClass.Length; i++)
                {
                    if (curTime > new TimeSpan(8, 50, 0) && curTime < new TimeSpan(9, 40, 0))
                    {
                        curClass[i] = timeTable.ElementAt(i).Value[dayOfWeek][0];
                        curPeriod = 0;
                    }
                    else if (curTime > new TimeSpan(9, 40, 0) && curTime < new TimeSpan(9, 45, 0))
                    {
                        UpdateTT(i, 0);
                        curPeriod = 0;
                    }
                    else if (curTime > new TimeSpan(9, 45, 0) && curTime < new TimeSpan(10, 35, 0))
                    {
                        curClass[i] = timeTable.ElementAt(i).Value[dayOfWeek][1];
                        curPeriod = 1;
                    }
                    else if (curTime > new TimeSpan(10, 35, 0) && curTime < new TimeSpan(10, 40, 0))
                    {
                        UpdateTT(i, 1);
                        curPeriod = 1;
                    }
                    else if (curTime > new TimeSpan(10, 40, 0) && curTime < new TimeSpan(11, 30, 0))
                    {
                        curClass[i] = timeTable.ElementAt(i).Value[dayOfWeek][2];
                        curPeriod = 2;
                    }
                    else if (curTime > new TimeSpan(11, 30, 0) && curTime < new TimeSpan(11, 35, 0))
                    {
                        UpdateTT(i, 2);
                        curPeriod = 2;
                    }
                    else if (curTime > new TimeSpan(11, 35, 0) && curTime < new TimeSpan(12, 25, 0))
                    {
                        curClass[i] = timeTable.ElementAt(i).Value[dayOfWeek][3];
                        curPeriod = 3;
                    }
                    else if (curTime > new TimeSpan(12, 25, 0) && curTime < new TimeSpan(12, 35, 0))
                    {
                        UpdateTT(i, 3);
                        curPeriod = 3;
                    }
                    else if (curTime > new TimeSpan(12, 35, 0) && curTime < new TimeSpan(13, 25, 0))
                    {
                        curClass[i] = timeTable.ElementAt(i).Value[dayOfWeek][4];
                        curPeriod = 4;
                    }
                    else if (curTime > new TimeSpan(13, 25, 0) && curTime < new TimeSpan(13, 30, 0))
                    {
                        UpdateTT(i, 4);
                        curPeriod = 4;
                    }
                    else if (curTime > new TimeSpan(13, 30, 0) && curTime < new TimeSpan(14, 20, 0))
                    {
                        curClass[i] = timeTable.ElementAt(i).Value[dayOfWeek][5];
                        curPeriod = 5;
                    }
                    else if (curTime > new TimeSpan(14, 20, 0) && curTime < new TimeSpan(14, 25, 0))
                    {
                        UpdateTT(i, 5);
                        curPeriod = 5;
                    }
                    else if (curTime > new TimeSpan(14, 25, 0) && curTime < new TimeSpan(15, 15, 0))
                    {
                        curClass[i] = timeTable.ElementAt(i).Value[dayOfWeek][6];
                        curPeriod = 6;
                    }
                    else if (curTime > new TimeSpan(15, 15, 0) && curTime < new TimeSpan(15, 20, 0))
                    {
                        UpdateTT(i, 6);
                        curPeriod = 6;
                    }
                    else if (curTime > new TimeSpan(15, 20, 0) && curTime < new TimeSpan(16, 10, 0))
                    {
                        curClass[i] = timeTable.ElementAt(i).Value[dayOfWeek][7];
                        curPeriod = 7;
                    }
                    else
                    {
                        curClass[i] = "No Class";
                        curPeriod = 8;
                    }
                }
            }
            for(int i = 0; i < curClass.Length; i++) UpdateClass(curClass);
            showElective();
        }
    }

    private void UpdateTT(int classID, int period)
    {
        if (timeTable.ElementAt(classID).Value[dayOfWeek][period + 1] == "No Class") curClass[classID] = "No Class";
        else if (timeTable.ElementAt(classID).Value[dayOfWeek][period] == timeTable.ElementAt(classID).Value[dayOfWeek][period + 1])
            curClass[classID] = timeTable.ElementAt(classID).Value[dayOfWeek][period];
        else curClass[classID] = "Break";
    }

    public void UpdateClass(string[] curClass)
    {
        CSEA.text = curClass[0];
        CSEB.text = curClass[1];
        CSEC.text = curClass[2];
        CSED.text = curClass[3];
    }

    private void showElective()
    {
        if (curClass[0] == "Elective 2")
        {
            compVision.SetActive(true);
            infoRet.SetActive(true);
            sna.SetActive(true);

            cloudAndEthical0.SetActive(true);
            cloudAndEthical1.SetActive(true);
            cloudAndEthicalT0.text = "Cloud Computing";
            cloudAndEthicalT1.text = "Cloud Computing";
        }
        else if (curClass[0] == "Elective 3")
        {
            patternRec.SetActive(true);
            mobileAdhoc.SetActive(true);
            bigData.SetActive(true);
            neuro.SetActive(true);


            cloudAndEthical0.SetActive(true);
            cloudAndEthical1.SetActive(true);
            cloudAndEthicalT0.text = "Ethical Hacking";
            cloudAndEthicalT1.text = "Ethical Hacking";
        }
        else
        {
            infoRet.SetActive(false);
            compVision.SetActive(false);
            sna.SetActive(false);
            neuro.SetActive(false);

            patternRec.SetActive(false);
            mobileAdhoc.SetActive(false);
            bigData.SetActive(false);

            cloudAndEthical0.SetActive(false);
            cloudAndEthical1.SetActive(false);
        }
    }

    public Dictionary<string,List<List<string>>> timeTable = new Dictionary<string, List<List<string>>>
    {
        {
            "CSEA",
            new List<List<string>>
            {
                new List<string>
                {
                    "Principles of Programming Languages",
                    "Elective 2",
                    "Software Engineering",
                    "Lunch Break",
                    "Distributed Systems Lab Eval",
                    "Distributed Systems Lab",
                    "Distributed Systems Lab",
                    "No Class",
                },
                new List<string>
                {
                    "Distributed Systems",
                    "Computer Security",
                    "Elective 2",
                    "Lunch Break",
                    "Software Engineering Lab",
                    "Software Engineering Lab",
                    "Software Engineering Lab",
                    "No Class",
                },
                new List<string>
                {
                    "Elective 3",
                    "Software Engineering",
                    "Distributed Systems",
                    "Lunch Break",
                    "Code Hour",
                    "Elective 2",
                    "Computer Security",
                    "No Class"
                },
                new List<string>
                {
                    "Computer Security",
                    "Elective 3",
                    "MA OM",
                    "Soft Skills",
                    "Lunch Break",
                    "Principles of Programming Languages Lab",
                    "Principles of Programming Languages Lab",
                    "Principles of Programming Languages Lab"
                },
                new List<string>
                {
                    "Quantitative Aptitude",
                    "Verbal",
                    "Free Period",
                    "Lunch Break",
                    "Principles of Programming Languages",
                    "Elective 3",
                    "Distributed Systems",
                    "No Class"
                }
            }
        },
        {
            "CSEB",
            new List<List<string>>
            {
                new List<string>
                {
                    "Computer Security",
                    "Elective 2",
                    "Distributed Systems",
                    "Soft Skills",
                    "Lunch Break",
                    "Computer Security",
                    "No Class",
                    "No Class",
                },
                new List<string>
                {
                    "Principles of Programming Languages",
                    "Distributed Systems",
                    "Elective 2",
                    "Lunch Break",
                    "Quantitative Aptitude",
                    "Principles of Programming Languages Lab",
                    "Principles of Programming Languages Lab",
                    "Principles of Programming Languages Lab",
                },
                new List<string>
                {
                    "Elective 3",
                    "Distributed Systems",
                    "Software Engineering",
                    "Lunch Break",
                    "Principles of Programming Languages",
                    "Elective 2",
                    "Verbal",
                    "Distributed Systems Lab Eval"
                },
                new List<string>
                {
                    "Software Engineering",
                    "Elective 3",
                    "Computer Security",
                    "Lunch Break",
                    "Computer Security Lab",
                    "Computer Security Lab",
                    "Computer Security Lab",
                    "No Class"
                },
                new List<string>
                {
                    "MA OM",
                    "Code Hour",
                    "Distributed Systems Lab",
                    "Distributed Systems Lab",
                    "Lunch Break",
                    "Elective 3",
                    "No Class",
                    "No Class"
                }
            }
        },
        {
            "CSEC",
            new List<List<string>>
            {
                new List<string>
                {
                    "Software Engineering",
                    "Elective 2",
                    "Computer Security",
                    "Lunch Break",
                    "Principles of Programming Languages",
                    "Software Engineering Lab",
                    "Software Engineering Lab",
                    "Software Engineering Lab",
                },
                new List<string>
                {
                    "Soft Skills",
                    "Software Engineering",
                    "Elective 2",
                    "Code Hour",
                    "Lunch Break",
                    "Distributed Systems Lab Eval",
                    "Distributed Systems Lab",
                    "Distributed Systems Lab",
                },
                new List<string>
                {
                    "Elective 3",
                    "Distributed Systems",
                    "MA OM",
                    "Computer Security",
                    "Lunch Break",
                    "Elective 2",
                    "No Class",
                    "No Class"
                },
                new List<string>
                {
                    "Distributed Systems",
                    "Elective 3",
                    "Quantitative Aptitude",
                    "Verbal",
                    "Lunch Break",
                    "Distributed Systems",
                    "No Class",
                    "No Class"
                },
                new List<string>
                {
                    "Principles of Programming Languages Lab",
                    "Principles of Programming Languages Lab",
                    "Principles of Programming Languages Lab",
                    "Computer Security",
                    "Lunch Break",
                    "Elective 3",
                    "Principles of Programming Languages",
                    "No Class"
                }
            }
        },
        {
            "CSED",
            new List<List<string>>
            {
                new List<string>
                {
                    "Free Period",
                    "Elective 2",
                    "Software Engineering",
                    "Lunch Break",
                    "Computer Security",
                    "Quantitative Aptitude",
                    "Verbal",
                    "No Class",
                },
                new List<string>
                {
                    "Computer Security",
                    "Distributed Systems",
                    "Elective 2",
                    "Soft Skills",
                    "Lunch Break",
                    "Principles of Programming Languages Lab",
                    "Principles of Programming Languages Lab",
                    "Principles of Programming Languages Lab",
                },
                new List<string>
                {
                    "Elective 3",
                    "Distributed Systems",
                    "Principles of Programming Languages",
                    "Computer Security",
                    "Lunch Break",
                    "Elective 2",
                    "Code Hour",
                    "Distributed Systems"
                },
                new List<string>
                {
                    "Principles of Programming Languages",
                    "Elective 3",
                    "MA OM",
                    "Software Engineering",
                    "Lunch Break",
                    "Software Engineering Lab",
                    "Software Engineering Lab",
                    "Software Engineering Lab"
                },
                new List<string>
                {
                    "Distributed Systems Lab",
                    "Distributed Systems Lab",
                    "Distributed Systems Lab Eval",
                    "Lunch Break",
                    "Free Period",
                    "Elective 3",
                    "No Class",
                    "No Class"
                }
            }
        }
    };

}
