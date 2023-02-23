using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private string destination="";
    private string currLocation="";

    public Tilemap[] tilemaps;

    public Tilemap[] roadMaps;

    public Tilemap pointMap;

    public TileBase roadTile;

    public SpriteRenderer[] floors;

    public Vector3Int[,] spots0;
    public Vector3Int[,] spots1;
    public Vector3Int[,] spots2;
    public Vector3Int[,] spots3;

    Astar astar0;
    Astar astar1;
    Astar astar2;
    Astar astar3;

    List<Spot> roadPath0 = new List<Spot>();
    List<Spot> roadPath1 = new List<Spot>();
    List<Spot> roadPath2 = new List<Spot>();
    List<Spot> roadPath3 = new List<Spot>();
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

        spots0 = new Vector3Int[bounds0.size.x, bounds0.size.y];
        spots1 = new Vector3Int[bounds1.size.x, bounds1.size.y];
        spots2 = new Vector3Int[bounds2.size.x, bounds2.size.y];
        spots3 = new Vector3Int[bounds3.size.x, bounds3.size.y];
        CreateGrid(bounds0, tilemaps[0], spots0);
        CreateGrid(bounds1, tilemaps[1], spots1);
        CreateGrid(bounds2, tilemaps[2], spots2);
        CreateGrid(bounds3, tilemaps[3], spots3);
        astar0 = new Astar(spots0, bounds0.size.x, bounds0.size.y);
        astar1 = new Astar(spots1, bounds1.size.x, bounds1.size.y);
        astar2 = new Astar(spots2, bounds2.size.x, bounds2.size.y);
        astar3 = new Astar(spots3, bounds3.size.x, bounds3.size.y);
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
        for (int i = 0; i < roadPath.Count; i++)
        {
            roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile);
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
            ShowPoints(coordinates.RoomCoord2, pointMap);
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

    public void GetDestination(string s)
    {
        destination = s;
        Debug.Log(destination);
    }

    public void GetCurrLocation(string s)
    {
        currLocation = s;
        Debug.Log(currLocation);
    }


    // Button Code
    private Color btnColor = Color.white;
    private Color btnColorActive = Color.grey;
    private int currFloor = 0;

    public Button[] floorBtns;
    public SVGImage indicator;

    private readonly float[] indicatorPos = { -200, -34, 134, 300 };
    private float indicatorCurrPos = 300;

    private bool searched = false;
    private bool onSameFloor = true;

    public void Search()
    {
        roadMaps[0].ClearAllTiles();
        roadMaps[1].ClearAllTiles();
        roadMaps[2].ClearAllTiles();
        roadMaps[3].ClearAllTiles();
        try 
        {
            if (destination[2] == currLocation[2])
            {
                onSameFloor = true;
                if (destination[2] == '0')
                {
                    findPath(roadPath0, roadMaps[0], astar0, coordinates.RoomCoord0[currLocation], coordinates.RoomCoord0[destination], spots0);
                    MoveIndicator(0);
                }
                else if (destination[2] == '1')
                {
                    findPath(roadPath1, roadMaps[1], astar1, coordinates.RoomCoord1[currLocation], coordinates.RoomCoord1[destination], spots1);
                    MoveIndicator(1);
                }
                else if (destination[2] == '2')
                {
                    findPath(roadPath2, roadMaps[2], astar2, coordinates.RoomCoord2[currLocation], coordinates.RoomCoord2[destination], spots2);
                    MoveIndicator(2);
                }
                else if (destination[2] == '3')
                {
                    findPath(roadPath3, roadMaps[3], astar3, coordinates.RoomCoord3[currLocation], coordinates.RoomCoord3[destination], spots3);
                    MoveIndicator(3);
                }
                else
                {
                    throw new Exception("Invalid Input");
                }
            }
            else
            {
                Debug.Log("Diff floor");
                onSameFloor = false;
                MoveIndicator(currLocation[2] - '0');
                Debug.Log(currLocation[2] - '0'+"yea");
                // first stairs
                if (currLocation[2] == '1')
                {
                    Vector2Int currLocV = coordinates.RoomCoord1[currLocation];
                    Vector2Int destLocV = coordinates.RoomCoord2[destination];
                    Vector2Int stairLocV = new Vector2Int();
                    float minDist = 1000, currDist;
                    for (int i = 0; i < coordinates.stairs1.Count; i++)
                    {
                        currDist = Vector2.Distance(currLocV, coordinates.stairs1[i]) + Vector2.Distance(coordinates.stairs1[i], destLocV);
                        if (currDist < minDist)
                        {
                            minDist = currDist;
                            stairLocV = coordinates.stairs1[i];
                        }
                    }
                    Debug.Log(stairLocV + "stairs");

                    findPath(roadPath1, roadMaps[1], astar1, currLocV, stairLocV, spots1);
                    findPath(roadPath2, roadMaps[2], astar2, stairLocV, destLocV, spots2);

                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            searched = false;
            HideIndicator();
            return;
        }
        searched = true;
        if (indicatorCurrPos != currFloor) ShowIndicator();
    }

    private void ShowIndicator()
    {
        indicator.enabled = true;
    }

    private void HideIndicator()
    {
        indicator.enabled = false;
    }

    private void MoveIndicator(int Floor)
    {
        Vector2 tempPos = indicator.GetComponent<RectTransform>().anchoredPosition;
        tempPos.y = indicatorPos[Floor];
        indicator.GetComponent<RectTransform>().anchoredPosition = tempPos;
        indicatorCurrPos = Floor;
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
                if (currFloor == currLocation[2] - '0')
                {
                    MoveIndicator(destination[2] - '0');
                }
                else
                {
                    MoveIndicator(currLocation[2] - '0');
                }
            }
        }
    }
    public void ShowFloor0()
    {
        if (currFloor == 0) return;
        HideFloor();
        currFloor = 0;
        ShowFloor();
        UpdateIndicator();
        
    }
    public void ShowFloor1()
    {
        if (currFloor == 1) return;
        HideFloor();
        currFloor = 1;
        ShowFloor();
        UpdateIndicator();
    }
    public void ShowFloor2()
    {
        if (currFloor == 2) return;
        HideFloor();
        currFloor = 2;
        ShowFloor();
        UpdateIndicator();
    }
    public void ShowFloor3()
    {
        if (currFloor == 3) return;
        HideFloor();
        currFloor = 3;
        ShowFloor();
        UpdateIndicator();
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
            entryPoints.SetTile(new Vector3Int(keyValuePair.Value.x, keyValuePair.Value.y, 0), roadTile);
        }
    }
}
