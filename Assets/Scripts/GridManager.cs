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
    private string destination="";
    private string currLocation="";
    private int dest = 0, currLoc = 0;

    public Tilemap[] tilemaps;

    public Tilemap[] roadMaps;

    public Tilemap pointMap;

    public TileBase[] roadTile;

    public SpriteRenderer[] floors;

    public Vector3Int[][,] spots = new Vector3Int[4][,];

    Astar[] astar = new Astar[4];

    List<Spot>[] roadPath = new List<Spot>[4];

    new Camera camera;

    BoundsInt bounds0;
    BoundsInt bounds1;
    BoundsInt bounds2;
    BoundsInt bounds3;

    public Coordinates coordinates;

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
        // Debug.Log("Hello");

        /*if (Input.GetMouseButtonDown(1))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(world);
            start = new Vector2Int(gridPos.x, gridPos.y);
        }*/
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemaps[1].WorldToCell(world);
            roadMaps[1].SetTile(new Vector3Int(gridPos.x, gridPos.y, 0), null);
        }


        if (Input.GetKeyDown("q"))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemaps[1].WorldToCell(world);
            Vector2Int pos = new Vector2Int(gridPos.x, gridPos.y);
            Debug.Log(pos);

        }
        if (Input.GetKeyDown("w"))
        {
            ShowPoints(coordinates.RoomCoord[2], pointMap);
            Debug.Log(Physics2D.simulationMode);
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

    public void GetDestination(string s){ destination = s; }

    public void GetCurrLocation(string s){ currLocation = s; }


    // Button Code
    private Color btnColor = Color.white;
    private Color btnColorActive = new Color(0.8f,0.8f,0.8f,1);
    private int currFloor = 0;

    public Button[] floorBtns;
    public SVGImage indicator;
    public Button goUpBtn, goDownBtn;

    private readonly float[] indicatorPos = { -200, -34, 134, 300 };
    private int indicatorCurrPos = -200;

    private bool searched = false;
    private bool onSameFloor = true;

    public void Search()
    {
        roadMaps[0].ClearAllTiles();
        roadMaps[1].ClearAllTiles();
        roadMaps[2].ClearAllTiles();
        roadMaps[3].ClearAllTiles();
        dest = destination[2] - '0';
        currLoc = currLocation[2]-'0';
        try 
        {
            if (dest == currLoc)
            {
                onSameFloor = true;
                findPath(roadPath[dest], roadMaps[dest], astar[dest], coordinates.RoomCoord[dest][destination], coordinates.RoomCoord[dest][currLocation], spots[dest]);
                MoveIndicator(dest);
            }
            else
            {
                Debug.Log("Diff floor");
                onSameFloor = false;
                MoveIndicator(currLoc);

                Vector2Int currLocV = coordinates.RoomCoord[currLoc][currLocation];
                Vector2Int destV = coordinates.RoomCoord[dest][destination];
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
                Debug.Log(stairLocV + "stairs");

                findPath(roadPath[currLoc], roadMaps[currLoc], astar[currLoc], stairLocV, currLocV, spots[currLoc]);
                findPath(roadPath[dest], roadMaps[dest], astar[dest], destV, stairLocV, spots[dest]);
                MoveStairsBtn(stairIconLocV);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            searched = false;
            HideIndicator();
            HideGoUpBtn();
            HideGoDownBtn();
            return;
        }
        searched = true;
        UpdateIndicator();
        UpdateStairsBtn();
    }

    private void ShowGoUpBtn() 
    { 
        goUpBtn.enabled = true;
        goUpBtn.GetComponent<SVGImage>().enabled = true;
    }
    private void ShowGoDownBtn() 
    { 
        goDownBtn.enabled = true; 
        goDownBtn.GetComponent <SVGImage>().enabled = true;
    }
    private void HideGoUpBtn() 
    {
        goUpBtn.enabled = false;
        goUpBtn.GetComponent<SVGImage>().enabled = false;
    }
    private void HideGoDownBtn() 
    { 
        goDownBtn.enabled = false;
        goDownBtn.GetComponent<SVGImage>().enabled = false;
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
        roadMaps[currFloor].GetComponent<TilemapRenderer>().enabled = false;
        floors[currFloor].enabled = false;
        floorBtns[currFloor].GetComponent<SVGImage>().color = btnColor;
    }
    private void ShowFloor()
    {
        floorBtns[currFloor].GetComponent<SVGImage>().color = btnColorActive;
        roadMaps[currFloor].GetComponent<TilemapRenderer>().enabled = true;
        floors[currFloor].enabled = true;
    }

    private void ShowPoints(Dictionary<string,Vector2Int> roomCoords, Tilemap entryPoints)
    {
        foreach(KeyValuePair<string,Vector2Int> keyValuePair in roomCoords) 
        {
            entryPoints.SetTile(new Vector3Int(keyValuePair.Value.x, keyValuePair.Value.y, 0), roadTile[0]);
        }
    }
}
