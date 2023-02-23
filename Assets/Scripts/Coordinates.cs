using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    public Dictionary<string, Vector2Int> RoomCoord0 = new Dictionary<string, Vector2Int>
    {

    };
    public Dictionary<string, Vector2Int> RoomCoord1 = new Dictionary<string, Vector2Int>{
        {"A-109", new Vector2Int(68,-195)},
        {"N-117", new Vector2Int(68,-95)},
        {"N-116A", new Vector2Int(68,-109)},
        {"N-118", new Vector2Int(90,-91)},
        {"S-116", new Vector2Int(113,-51) },
        {"st1", new Vector2Int(104,-84)}

    };
    public Dictionary<string, Vector2Int> RoomCoord2 = new Dictionary<string, Vector2Int>{
        {"A-201", new Vector2Int(136,-195)},
        {"A-202", new Vector2Int(135,-205)},
        {"A-203", new Vector2Int(135,-213)},
        {"A-204", new Vector2Int(125,-215)},
        {"A-205", new Vector2Int(116,-215)},
        {"A-206", new Vector2Int(107,-215)},
        {"A-207", new Vector2Int(87,-215)},
        {"A-208", new Vector2Int(87,-215)},
        {"A-209", new Vector2Int(78,-215)},
        {"A-210", new Vector2Int(68,-213)},
        {"A-211", new Vector2Int(68,-205)},
        {"N-201", new Vector2Int(90,-189)},
        {"N-202", new Vector2Int(90,-178)},
        {"N-203", new Vector2Int(90,-167)},
        {"N-204", new Vector2Int(69,-170)},
        {"N-205", new Vector2Int(62,-170)},
        {"N-206", new Vector2Int(55,-170)},
        {"N-207", new Vector2Int(37,-170)},
        {"N-208", new Vector2Int(37,-157)},
        {"N-209", new Vector2Int(55,-157)},
        {"N-210", new Vector2Int(62,-157)},
        {"N-211", new Vector2Int(69,-157)},
        {"N-212", new Vector2Int(90,-161)},
        {"N-213", new Vector2Int(90,-155)},
        {"N-214", new Vector2Int(90,-149)},
        {"N-215", new Vector2Int(90,-135)},
        {"N-216", new Vector2Int(90,-131)},
        {"N-217", new Vector2Int(90,-120)},
        {"N-218", new Vector2Int(68,-125)},
        {"N-219", new Vector2Int(68,-115)},
        {"N-220", new Vector2Int(68,-109)},
        {"N-221", new Vector2Int(68,-99)},
        {"N-222", new Vector2Int(90,-102)},
        {"N-223", new Vector2Int(84,-86)},
        {"N-224", new Vector2Int(90,-66)},
        {"N-225", new Vector2Int(90,-51)},
        {"S-205", new Vector2Int(136,-163)},
        {"S-206", new Vector2Int(137,-148)},
        {"S-207", new Vector2Int(113,-161)},
        {"S-208", new Vector2Int(113,-154)},
        {"S-209", new Vector2Int(113,-149)},
        {"S-210", new Vector2Int(113,-135)},
        {"S-211", new Vector2Int(113,-131)},
        {"S-212", new Vector2Int(113,-120)},
        {"S-219", new Vector2Int(113,-62)}
    };
    public Dictionary<string, Vector2Int> RoomCoord3 = new Dictionary<string, Vector2Int>
    {

    };
    public List<Vector2Int> stairs1 = new List<Vector2Int>{
        new Vector2Int(102,-85),
        new Vector2Int(102,-161),
        new Vector2Int(87,-210),
        new Vector2Int(116,-210)
    };

}

