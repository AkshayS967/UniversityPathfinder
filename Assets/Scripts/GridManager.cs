using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private string destination;
    private string currLocation;

    public Tilemap tilemap0;
    public Tilemap tilemap1;
    public Tilemap tilemap2;
    public Tilemap tilemap3;

    public Tilemap roadMap0;
    public Tilemap roadMap1;
    public Tilemap roadMap2;
    public Tilemap roadMap3;

    public Tilemap pointMap;

    public TileBase roadTile;

    public SpriteRenderer floor0;
    public SpriteRenderer floor1;
    public SpriteRenderer floor2;
    public SpriteRenderer floor3;

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
        tilemap0.CompressBounds();
        tilemap1.CompressBounds();
        tilemap2.CompressBounds();
        tilemap3.CompressBounds();

        roadMap0.CompressBounds();
        roadMap1.CompressBounds();
        roadMap2.CompressBounds();
        roadMap3.CompressBounds();

        bounds0 = tilemap0.cellBounds;
        bounds1 = tilemap1.cellBounds;
        bounds2 = tilemap2.cellBounds;
        bounds3 = tilemap3.cellBounds;
        camera = Camera.main;

        spots0 = new Vector3Int[bounds0.size.x, bounds0.size.y];
        spots1 = new Vector3Int[bounds1.size.x, bounds1.size.y];
        spots2 = new Vector3Int[bounds2.size.x, bounds2.size.y];
        spots3 = new Vector3Int[bounds3.size.x, bounds3.size.y];
        CreateGrid(bounds0, tilemap0, spots0);
        CreateGrid(bounds1, tilemap1, spots1);
        CreateGrid(bounds2, tilemap2, spots2);
        CreateGrid(bounds3, tilemap3, spots3);
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
            Vector3Int gridPos = tilemap1.WorldToCell(world);
            roadMap1.SetTile(new Vector3Int(gridPos.x, gridPos.y, 0), null);
        }

        if (Input.GetKeyDown("space"))
        {
            roadMap0.ClearAllTiles();
            roadMap1.ClearAllTiles();
            roadMap2.ClearAllTiles();
            roadMap3.ClearAllTiles();
            if (destination[2] == currLocation[2])
            {
                if (destination[2] == '0')
                {
                    findPath(roadPath0, roadMap0, astar0, coordinates.RoomCoord0[currLocation], coordinates.RoomCoord0[destination], spots0);
                }
                else if (destination[2] == '1')
                {
                    findPath(roadPath1, roadMap1, astar1, coordinates.RoomCoord1[currLocation], coordinates.RoomCoord1[destination], spots1);
                }
                else if (destination[2] == '2')
                {
                    findPath(roadPath2, roadMap2, astar2, coordinates.RoomCoord2[currLocation], coordinates.RoomCoord2[destination], spots2);
                }
                else if (destination[2] == '3')
                {
                    findPath(roadPath3, roadMap3, astar3, coordinates.RoomCoord3[currLocation], coordinates.RoomCoord3[destination], spots3);
                }


            }
            else
            {
                Debug.Log("Diff floor");
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

                    findPath(roadPath1, roadMap1, astar1, currLocV, stairLocV, spots1);
                    findPath(roadPath2, roadMap2, astar2, stairLocV, destLocV, spots2);

                }
            }

        }


        if (Input.GetKeyDown("q"))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap1.WorldToCell(world);
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
    public Button floorBtn0;
    public Button floorBtn1;
    public Button floorBtn2;
    public Button floorBtn3;
    public void ShowFloor0()
    {
        if (currFloor == 0) return;
        if (currFloor == 1)
        {
            roadMap1.GetComponent<TilemapRenderer>().enabled = false;
            floor1.enabled = false;
            floorBtn1.GetComponent<Image>().color = btnColor;
        }
        else if (currFloor == 2)
        {
            roadMap2.GetComponent<TilemapRenderer>().enabled = false;
            floor2.enabled = false;
            floorBtn2.GetComponent<Image>().color = btnColor;
        }
        else if (currFloor == 3)
        {
            roadMap3.GetComponent<TilemapRenderer>().enabled = false;
            floor3.enabled = false;
            floorBtn3.GetComponent<Image>().color = btnColor;
        }
        currFloor = 0;
        floorBtn0.GetComponent<Image>().color = btnColorActive;
        roadMap0.GetComponent<TilemapRenderer>().enabled = true;
        floor0.enabled = true;
    }
    public void ShowFloor1()
    {
        if (currFloor == 1) return;
        if (currFloor == 0)
        {
            roadMap0.GetComponent<TilemapRenderer>().enabled = false;
            floor0.enabled = false;
            floorBtn0.GetComponent<Image>().color = btnColor;
        }
        else if (currFloor == 2)
        {
            roadMap2.GetComponent<TilemapRenderer>().enabled = false;
            floor2.enabled = false;
            floorBtn2.GetComponent<Image>().color = btnColor;
        }
        else if (currFloor == 3)
        {
            roadMap3.GetComponent<TilemapRenderer>().enabled = false;
            floor3.enabled = false;
            floorBtn3.GetComponent<Image>().color = btnColor;
        }
        currFloor = 1;
        floorBtn1.GetComponent<Image>().color = btnColorActive;
        roadMap1.GetComponent<TilemapRenderer>().enabled = true;
        floor1.enabled = true;
    }
    public void ShowFloor2()
    {
        if (currFloor == 2) return;
        if (currFloor == 0)
        {
            roadMap0.GetComponent<TilemapRenderer>().enabled = false;
            floor0.enabled = false;
            floorBtn0.GetComponent<Image>().color = btnColor;
        }
        else if (currFloor == 1)
        {
            roadMap1.GetComponent<TilemapRenderer>().enabled = false;
            floor1.enabled = false;
            floorBtn1.GetComponent<Image>().color = btnColor;
        }
        else if (currFloor == 3)
        {
            roadMap3.GetComponent<TilemapRenderer>().enabled = false;
            floor3.enabled = false;
            floorBtn3.GetComponent<Image>().color = btnColor;
        }
        currFloor = 2;
        floorBtn2.GetComponent<Image>().color = btnColorActive;
        roadMap2.GetComponent<TilemapRenderer>().enabled = true;
        floor2.enabled = true;
    }
    public void ShowFloor3()
    {
        if (currFloor == 3) return;
        if (currFloor == 0)
        {
            roadMap0.GetComponent<TilemapRenderer>().enabled = false;
            floor0.enabled = false;
            floorBtn0.GetComponent<Image>().color = btnColor;
        }
        else if (currFloor == 1)
        {
            roadMap1.GetComponent<TilemapRenderer>().enabled = false;
            floor1.enabled = false;
            floorBtn1.GetComponent<Image>().color = btnColor;
        }
        else if (currFloor == 2)
        {
            roadMap2.GetComponent<TilemapRenderer>().enabled = false;
            floor2.enabled = false;
            floorBtn2.GetComponent<Image>().color = btnColor;
        }
        currFloor = 3;
        floorBtn3.GetComponent<Image>().color = btnColorActive;
        roadMap3.GetComponent<TilemapRenderer>().enabled = true;
        floor3.enabled = true;
    }

    private void ShowPoints(Dictionary<string,Vector2Int> roomCoords, Tilemap entryPoints)
    {
        foreach(KeyValuePair<string,Vector2Int> keyValuePair in roomCoords) 
        {
            entryPoints.SetTile(new Vector3Int(keyValuePair.Value.x, keyValuePair.Value.y, 0), roadTile);
        }
    }
}
