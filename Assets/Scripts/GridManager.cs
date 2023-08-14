using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Unity.Mathematics;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public Autocomplete autocomplete;

    public string destination="";
    public string currLocation="";
    private int dest = 0, currLoc = 0;

    public bool destSelected;
    public bool currSelected;

    public Tilemap[] tilemaps;

    public Tilemap[] roadMaps;

    public Tilemap pointMap;

    public TileBase[] roadTile;

    public GameObject[] floors;

    public Vector3Int[][,] spots = new Vector3Int[4][,];

    Astar[] astar = new Astar[4];

    List<Spot>[] roadPath = new List<Spot>[4];

    new Camera camera;

    BoundsInt bounds0;
    BoundsInt bounds1;
    BoundsInt bounds2;
    BoundsInt bounds3;

    public Coordinates coordinates;
    public Camera mainCamera;
    public GameObject errorMessage;

    private float transitionSpeed = 0.3f;
    private float elapsedTime;
    private Vector3 camTargetLocation;
    private Vector3 camStartLocation;
    private bool camTransitionActive = false;
    private float camStartZoom;
    private float camTargetZoom;
    private float percentageComplete;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;

        tilemaps[0].CompressBounds();
        tilemaps[1].CompressBounds();
        tilemaps[2].CompressBounds();
        tilemaps[3].CompressBounds();

        roadMaps[0].CompressBounds();
        roadMaps[1].CompressBounds();
        roadMaps[2].CompressBounds();
        roadMaps[3].CompressBounds();

        bounds0 = tilemaps[0].cellBounds;
        bounds1 = tilemaps[1].cellBounds;
        bounds2 = tilemaps[2].cellBounds;
        bounds3 = tilemaps[3].cellBounds;

        camera = Camera.main;
        camTargetLocation = mainCamera.transform.position;

        spots[0] = new Vector3Int[bounds0.size.x, bounds0.size.y];
        spots[1] = new Vector3Int[bounds1.size.x, bounds1.size.y];
        spots[2] = new Vector3Int[bounds2.size.x, bounds2.size.y];
        spots[3] = new Vector3Int[bounds3.size.x, bounds3.size.y];
        CreateGrid(bounds0, tilemaps[0], spots[0]);
        CreateGrid(bounds1, tilemaps[1], spots[1]);
        CreateGrid(bounds2, tilemaps[2], spots[2]);
        CreateGrid(bounds3, tilemaps[3], spots[3]);
        astar[0] = new Astar(spots[0], bounds0.size.x, bounds0.size.y);
        astar[1] = new Astar(spots[1], bounds1.size.x, bounds1.size.y);
        astar[2] = new Astar(spots[2], bounds2.size.x, bounds2.size.y);
        astar[3] = new Astar(spots[3], bounds3.size.x, bounds3.size.y);

        ShowFloor();
        destSelected = false;
        currSelected = false;
}
    public void CreateGrid(BoundsInt bounds, Tilemap tilemap, Vector3Int[,] spots)
    {
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    spots[i, j] = new Vector3Int(x, y, 0);
                }
                else
                {
                    spots[i, j] = new Vector3Int(x, y, 1);
                }
            }
        }
    }
    private void DrawRoad(List<Spot> roadPath, Tilemap roadMap)
    {
        for (int i = 0; i < roadPath.Count-1; i++)
        {
            if (roadPath[i + 1].Y == roadPath[i].Y + 1) { roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile[0]); }
            else if (roadPath[i + 1].Y == roadPath[i].Y - 1) { roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile[1]); }
            else if (roadPath[i + 1].X == roadPath[i].X + 1) { roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile[2]); }
            else if (roadPath[i + 1].X == roadPath[i].X - 1) { roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile[3]); }
        }
    }
    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(1))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(world);
            start = new Vector2Int(gridPos.x, gridPos.y);
        }*/
        /*if (Input.GetMouseButtonDown(2))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemaps[1].WorldToCell(world);
            roadMaps[1].SetTile(new Vector3Int(gridPos.x, gridPos.y, 0), null);
        }*/

        /*if (Input.GetKeyDown("q"))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemaps[0].WorldToCell(world);
            Vector2Int pos = new Vector2Int(gridPos.x, gridPos.y);
            Debug.Log(pos);

        }*/
        /*if (Input.GetKeyDown("w"))
        {
            ShowPoints(coordinates.RoomCoord, pointMap);
            Debug.Log(Physics2D.simulationMode);
        }*/

        /*if (Input.GetKeyDown("d"))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemaps[2].WorldToCell(world);
            Debug.Log("{ \"\", new Vector2Int("+gridPos.x+","+gridPos.y+")},");
        }*/
        if (camTransitionActive)
        {
            /*Debug.Log(mainCamera.transform.position + "****" + targetLocation);
            Debug.Log(mainCamera.transform.position == targetLocation);*/
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / transitionSpeed;
            mainCamera.transform.position = Vector3.Lerp(camStartLocation, camTargetLocation, Mathf.SmoothStep(0,1,percentageComplete));
            mainCamera.orthographicSize = Mathf.Lerp(camStartZoom, camTargetZoom, Mathf.SmoothStep(0, 1, percentageComplete));
            if (mainCamera.transform.position == camTargetLocation && mainCamera.orthographicSize == camTargetZoom)
            {
                camTransitionActive = false;
            }
            
        }
        
    }


    private void findPath(List<Spot> roadPath, Tilemap roadMap, Astar astar, Vector2Int currLoc, Vector2Int dest, Vector3Int[,] spots)
    {
        if (roadPath != null && roadPath.Count > 0)
            roadPath.Clear();

        roadPath = astar.CreatePath(spots, currLoc, dest, 1000);

        if (roadPath == null)
            return;
        DrawRoad(roadPath, roadMap);
    }

    // When getting input, suggestions get shown 
    public void GetDestination(string s)
    {
        destination = s.Replace("-","").ToUpper();
        autocomplete.UpdateSuggestions(destination);
        destSelected = true;
        currSelected = false;
    }

    public void GetCurrLocation(string s)
    { 
        currLocation = s.Replace("-","").ToUpper();
        autocomplete.UpdateSuggestions(currLocation);
        currSelected = true;
        destSelected = false;
    }

    /*public void DestDeselected() { destSelected = false; }
    public void CurrDeselected() { currSelected = false; }*/


    // UI Code
    private Color btnColor = Color.white;
    private Color btnColorActive = new Color(0.8f,0.8f,0.8f,1);
    public int currFloor = 0;

    public Button[] floorBtns;
    public SVGImage indicator;
    public Button goUpBtn, goDownBtn;

    private readonly float[] indicatorPos = { -200, -34, 134, 300 };
    private int indicatorCurrPos = -200;

    private bool searched = false;
    private bool onSameFloor = true;

    public void Search()
    {
        autocomplete.HideSuggestions();
        roadMaps[0].ClearAllTiles();
        roadMaps[1].ClearAllTiles();
        roadMaps[2].ClearAllTiles();
        roadMaps[3].ClearAllTiles();
        try
        {
            dest = coordinates.RoomCoord[destination].Item1;
            currLoc = coordinates.RoomCoord[currLocation].Item1;
            Vector2Int currLocV = coordinates.RoomCoord[currLocation].Item2;
            Vector2Int destV = coordinates.RoomCoord[destination].Item2;

            if (destination == "WASHROOM (MEN)")
            {
                float minDist = 1000, currDist;
                Vector2Int washRoomLocV = new Vector2Int();
                foreach (Vector2Int washRoomLocation in coordinates.washRoomLocVM)
                {
                    currDist = Vector2.Distance(currLocV, washRoomLocation);
                    if (currDist < minDist)
                    {
                        minDist = currDist;
                        washRoomLocV = washRoomLocation;
                    }
                }
                findPath(roadPath[currLoc], roadMaps[currLoc], astar[currLoc], washRoomLocV, currLocV, spots[currLoc]);
            }
            else if (destination == "WASHROOM (WOMEN)")
            {
                float minDist = 1000, currDist;
                Vector2Int washRoomLocV = new Vector2Int();
                foreach (Vector2Int washRoomLocation in coordinates.washRoomLocVF)
                {
                    currDist = Vector2.Distance(currLocV, washRoomLocation);
                    if (currDist < minDist)
                    {
                        minDist = currDist;
                        washRoomLocV = washRoomLocation;
                    }
                }
                findPath(roadPath[dest], roadMaps[dest], astar[dest], washRoomLocV, currLocV, spots[dest]);
            }
            else if (dest == currLoc)
            {
                onSameFloor = true;
                findPath(roadPath[dest], roadMaps[dest], astar[dest], destV, currLocV, spots[dest]);
            }
            else
            {
                onSameFloor = false;

                Vector2Int stairLocV = new Vector2Int();
                Vector2Int stairIconLocV = new Vector2Int();
                float minDist = 1000, currDist;

                foreach(Coordinates.stairsLocation stairs in coordinates.stairsLocations)
                {
                    currDist = Vector2.Distance(currLocV, stairs.LocV) + Vector2.Distance(stairs.LocV, destV);
                    if (currDist < minDist)
                    {
                        minDist = currDist;
                        stairLocV = stairs.LocV;
                        stairIconLocV = stairs.IconLocV;
                    }
                }

                findPath(roadPath[currLoc], roadMaps[currLoc], astar[currLoc], stairLocV, currLocV, spots[currLoc]);
                findPath(roadPath[dest], roadMaps[dest], astar[dest], destV, stairLocV, spots[dest]);
                MoveStairsBtn(stairIconLocV);
            }
            MoveIndicator(currLoc);
            elapsedTime = 0f;
            camStartLocation = mainCamera.transform.position;
            camTargetLocation = new Vector3(currLocV.x, currLocV.y, -1);
            camTransitionActive = true;
            camStartZoom = mainCamera.orthographicSize;
            camTargetZoom = 40f;
            //mainCamera.transform.position = new Vector3(currLocV.x, currLocV.y, -1);
            //mainCamera.orthographicSize = 40;
            ChangeFloor(currLoc);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            searched = false;
            HideIndicator();
            HideGoUpBtn();
            HideGoDownBtn();
            showErrorMessage();
            return;
        }
        searched = true;
        UpdateIndicator();
        UpdateStairsBtn();
    }

    private void showErrorMessage()
    {
        errorMessage.SetActive(true);
        StartCoroutine(ShowMessage());
    }

    IEnumerator ShowMessage()
    {
        yield return new WaitForSeconds(1.5f);
        errorMessage.SetActive(false);
    }

    private void ShowGoUpBtn() 
    { 
        goUpBtn.enabled = true;
        goUpBtn.GetComponent<RawImage>().enabled = true;
    }
    private void ShowGoDownBtn() 
    { 
        goDownBtn.enabled = true; 
        goDownBtn.GetComponent <RawImage>().enabled = true;
    }
    private void HideGoUpBtn() 
    {
        goUpBtn.enabled = false;
        goUpBtn.GetComponent<RawImage>().enabled = false;
    }
    private void HideGoDownBtn() 
    { 
        goDownBtn.enabled = false;
        goDownBtn.GetComponent<RawImage>().enabled = false;
    }

    private void MoveStairsBtn(Vector2 stairsIconLocV)
    {
        goUpBtn.GetComponent<RectTransform>().anchoredPosition = stairsIconLocV;
        goDownBtn.GetComponent<RectTransform>().anchoredPosition = stairsIconLocV;
    }

    private void UpdateStairsBtn()
    {
        if (searched)
        {
            HideGoUpBtn();
            HideGoDownBtn();
            if (!onSameFloor)
            {
                if (currFloor == currLoc)
                {
                    if (currLoc > dest) ShowGoDownBtn();
                    else ShowGoUpBtn();
                }
                else if (currFloor == dest)
                {
                    if (currLoc > dest) ShowGoUpBtn();
                    else ShowGoDownBtn();
                }
            }
        }
    }
    public void goUp() { ChangeFloor(Math.Max(currLoc, dest)); }
    public void goDown() { ChangeFloor(Math.Min(currLoc, dest)); }

    private void ShowIndicator() { indicator.enabled = true; }

    private void HideIndicator() { indicator.enabled = false; }

    private void MoveIndicator(int floor)
    {
        Vector2 tempPos = indicator.GetComponent<RectTransform>().anchoredPosition;
        tempPos.y = indicatorPos[floor];
        indicator.GetComponent<RectTransform>().anchoredPosition = tempPos;
        indicatorCurrPos = floor;
    }

    public void UpdateIndicator()
    {
        if (searched)
        {
            if (onSameFloor)
            {
                if (indicatorCurrPos == currFloor) HideIndicator();
                else ShowIndicator();
            }
            else
            {
                if (currFloor == currLoc)
                {
                    MoveIndicator(dest);
                }
                else
                {
                    MoveIndicator(currLoc);
                }
                ShowIndicator();
            }
        }
    }

    public void ChangeFloorX(Button btn)
    {
        int buttonClicked = btn.name[5] - '0';
        ChangeFloor(buttonClicked);
    }

    private void ChangeFloor(int toFloor)
    {
        if (currFloor == toFloor) return;
        HideFloor();
        currFloor = toFloor;
        ShowFloor();
        UpdateIndicator();
        UpdateStairsBtn();
    }
    private void HideFloor()
    {
        floorBtns[currFloor].GetComponent<SVGImage>().color = btnColor;
        roadMaps[currFloor].GetComponent<TilemapRenderer>().enabled = false;
        floors[currFloor].SetActive(false);
    }
    private void ShowFloor()
    {
        floorBtns[currFloor].GetComponent<SVGImage>().color = btnColorActive;
        roadMaps[currFloor].GetComponent<TilemapRenderer>().enabled = true;
        floors[currFloor].SetActive(true);
    }
    
    private void ShowPoints(Dictionary<string,Tuple<int,Vector2Int>> roomCoords, Tilemap entryPoints)
    {
        foreach(KeyValuePair<string, Tuple<int, Vector2Int>> keyValuePair in roomCoords) 
        {
            entryPoints.SetTile(new Vector3Int(keyValuePair.Value.Item2.x, keyValuePair.Value.Item2.y, 0), roadTile[0]);
        }
    }
}
