using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class DungeonGenerator : MonoBehaviour
{
    //変数宣言
    #region
    //分割エリアの初期化用
    private Area area1;
    private Area area2;

    private Area area1_F;
    private Area area2_F;

    [SerializeField]
    [Header("プレイヤーオブジェクト")]
    private GameObject playerObj;

    [SerializeField]
    [Header("ダンジョンのマップの幅")]
    private int mapWidth;

    [SerializeField]
    [Header("ダンジョンのマップの高さ")]
    private int mapHeight;

    private int blank = 2;

    [SerializeField]
    [Range(0, 100)]
    [Header("マップエリアの最小幅")]
    private int area_min_width;

    [SerializeField]
    [Range(0, 100)]
    [Header("マップエリアの最小高さ")]
    private int area_min_height;

    [SerializeField]
    [Range(0, 100)]
    [Header("部屋の最小幅")]
    private int room_min_width;

    [SerializeField]
    [Range(0, 100)]
    [Header("部屋の最小高さ")]
    private int room_min_height;

    //マップの二次元配列
    private int[,] map;

    //配列での区分け用マップ作成用
    const int divide = 0;
    const int divide_F = 4;
    const int wall = 1;
    const int room = 2;
    const int road = 3;

    const int up = 1;
    const int down = -1;
    const int left = 2;
    const int right = -2;
    const int none = 0;

    //分割されたエリアの初期化用
    private List<Area> areas = new List<Area>();
    private List<Area> areas1 = new List<Area>();

    private List<Room> room_list = new List<Room>();
    private List<Room> room_list1 = new List<Room>();

    [Header("壁用Prefabs")]
    public GameObject wallObject;

    [Header("道用Prefabs")]
    public GameObject roadObject;

    [Header("部屋用Prefabs")]
    public GameObject roomObject;

    [Header("ダンジョンの壁オブジェクトの親オブジェクト")]
    public GameObject walls;

    [Header("ダンジョンの道オブジェクトの親オブジェクト")]
    public GameObject roads;

    [Header("ダンジョンの仮の道オブジェクトの親オブジェクト")]
    public GameObject pre_roads;

    [Header("ダンジョンの部屋オブジェクトの親オブジェクト")]
    public GameObject rooms;

    //---ミニマップ用--
    [SerializeField]
    [Header("MiniMapManageスクリプト")]
    private MiniMapManage miniMapManage;

    [SerializeField]
    [Header("階段オブジェクト")]
    private GameObject stair;

    [SerializeField]
    [Header("DungeonManagerオブジェクト")]
    private DungeonManager dungeonManager;//DungeonManagerコンポネント

    private bool postStairChaeck; //階段が配置されているかどうか true:配置済み false:未配置

    private int randomX;//階段のx座標
    private int randomZ;//階段のz座標


    [SerializeField]
    [Header("壁用NavMeshSurface")]
    private NavMeshSurface wallSurface;

    [SerializeField]
    [Header("歩行可能オブジェクト用NavMeshSurface")]
    private NavMeshSurface walkAbleSurface;

    //----------------マテリアル----------------------
    [SerializeField]
    [Header("壁のマテリアル")]
    private Material wallMaterial;

    [SerializeField]
    [Header("部屋のマテリアル")]
    private Material roomMaterial;

    [SerializeField]
    [Header("道のマテリアル")]
    private Material roadMaterial;

    //----------------テクスチャ----------------------
    [SerializeField]
    [Header("壁のテクスチャ")]
    private Texture[] wallTextures;

    [SerializeField]
    [Header("部屋のテクスチャ")]
    private Texture[] roomTextures;

    [SerializeField]
    [Header("道のテクスチャ")]
    private Texture[] roadTextures;

    [SerializeField]
    [Header("FloorNumSaveスクリタブルオブジェクト")]
    private FloorNumSave floorNumSave;

    #endregion

    //クラスの定義
    #region
    class Room
    {
        public int x;//部屋の左上のx座標
        public int z;//部屋の左上のz座標
        public int width;//部屋の幅
        public int height;//部屋の高さ

        //部屋とアンカーとの接続点の初期化
        public int connectX;
        public int connectZ;

        public Area area_info;
        public Area next_area_info;
    }

    class Area
    {
        public int x;//エリアの左上のx座標
        public int z;//エリアの左上のz座標
        public int width;//エリアの幅
        public int height;//エリアの高さ
        public int cut_dir;
    }
    #endregion

    private void Start()
    {
        //マップを初期化
        ResetMapData();

        FirstDivideArea(mapWidth, mapHeight, 0, 0);

        //マップを分割
        DivideArea(area1_F.width, area1_F.height, area1_F.x, area1_F.z, none, areas);
        DivideArea(area2_F.width, area2_F.height, area2_F.x, area2_F.z, none, areas1);

        //分割されたエリアの中に部屋を1つ生成
        CreateRoom(areas, room_list);
        CreateRoom(areas1, room_list1);

        CreateRoad(room_list);
        CreateRoad(room_list1);

        ConnectArea();

        //ダンジョンの外周を壁にする
        CreatePerimeterWall();

        //テクスチャの変更
        SetDungeonTexture();

        //生成されたマップデータにオブジェクトを配置
        CreateDungeon();

        //ミニマップを生成
        miniMapManage.MiniMapGenerate(map);

        //階段のランダム配置
        PostStair();

        //NavMeshを生成する
        BakeNavMesh();

        //DungeonManagerにマップを渡して、敵を生成する
        //DungeonManagerから取得
        dungeonManager.StartInstantEnemy(map, playerObj);

        //プレイヤーの初期位置を設定
        SetPlayerPosition();
    }

    //関数宣言
    #region
    /// <summary>
    /// 分割エリアの初期設定
    /// </summary>
    /// <param name="area">初期化するAreaクラスのコンストラクタ</param>
    /// <param name="width">分割するエリアの幅</param>
    /// <param name="height">分割するエリアの高さ</param>
    /// <param name="baseX">分割するエリアの左上のx座標</param>
    /// <param name="baseZ">分割するエリアの左上のz座標</param>
    /// <returns></returns>
    private Area Area_Setting(Area area, int width, int height, int baseX, int baseZ, int dir)
    {
        area.x = baseX;
        area.z = baseZ;
        area.width = width;
        area.height = height;
        area.cut_dir = dir;
        return area;
    }

    /// <summary>
    /// Mapの二次元配列の初期化
    /// </summary>
    private void ResetMapData()
    {
        map = new int[mapHeight, mapWidth];

        for (int dz = 0; dz < mapHeight; dz++)
        {
            for (int dx = 0; dx < mapWidth; dx++)
            {
                map[dz, dx] = wall;
            }
        }
    }

    /// <summary>
    /// マップデータにオブジェクトを配置
    /// </summary>
    private void CreateDungeon()
    {
        for (int dz = 0; dz < mapHeight; dz++)
        {
            for (int dx = 0; dx < mapWidth; dx++)
            {
                if (map[dz, dx] == wall) Instantiate(wallObject, new Vector3(dx, 0, dz), Quaternion.identity, walls.transform);
                else if (map[dz, dx] == road)
                {
                    GameObject instantRoad = Instantiate(roadObject, new Vector3(dx, 0, dz), Quaternion.identity, roads.transform);
                }

                else if (map[dz, dx] == divide || map[dz, dx] == divide_F) Instantiate(wallObject, new Vector3(dx, 0, dz), Quaternion.identity, pre_roads.transform);
                else if (map[dz, dx] == room)
                {
                    GameObject instantRoom = Instantiate(roomObject, new Vector3(dx, 0, dz), Quaternion.identity, rooms.transform);
                }
            }
        }
    }

    /// <summary>
    /// マップの分割
    /// </summary>
    /// <param name="width">分割するマップの幅</param>
    /// <param name="height">分割するマップの高さ</param>
    /// <param name="baseX">分割するマップの左上のx座標</param>
    /// <param name="baseZ">分割するマップの左上のz座標</param>
    /// <returns></returns>
    private void DivideArea(int width, int height, int baseX, int baseZ, int dir, List<Area> areas)
    {
        int div;//分割の基準
        area1 = new Area();
        area2 = new Area();

        if (height <= width)
        {
            div = UnityEngine.Random.Range(width / 4, width * 3 / 4);
            int dz = 0;

            area1 = Area_Setting(area1, div, height, baseX, baseZ, right);
            area2 = Area_Setting(area2, width - (div + 1), height, baseX + div + 1, baseZ, dir);

            if (area1.width < area_min_width && area1.height < area_min_height || area2.width < area_min_width && area2.height < area_min_height)
            {
                while ((baseZ + dz) < mapHeight && map[baseZ + dz, baseX + div] == wall)
                {
                    map[baseZ + dz, baseX + div] = divide;
                    dz++;
                }
                //areas.Add(area2);
                //areas.Add(area1);
            }
            else
            {
                while ((baseZ + dz) < mapHeight && map[baseZ + dz, baseX + div] == wall)
                {
                    map[baseZ + dz, baseX + div] = divide;
                    dz++;
                }
                areas.Add(area2);
                DivideArea(area1.width, area1.height, area1.x, area1.z, area1.cut_dir, areas);
            }
        }

        //高さで分割
        else
        {
            div = UnityEngine.Random.Range(height / 3, height * 2 / 3);
            int dx = 0;

            area1 = Area_Setting(area1, width, div, baseX, baseZ, up);
            area2 = Area_Setting(area2, width, height - (div + 1), baseX, baseZ + div + 1, dir);

            if (area1.width < area_min_width && area1.height < area_min_height || area2.width < area_min_width && area2.height < area_min_height)
            {
                while ((baseX + dx) < mapWidth && map[baseZ + div, baseX + dx] == wall)
                {
                    map[baseZ + div, baseX + dx] = divide;
                    dx++;
                }
                //areas.Add(area2);
                //areas.Add(area1);
            }
            else
            {
                while ((baseX + dx) < mapWidth && map[baseZ + div, baseX + dx] == wall)
                {
                    map[baseZ + div, baseX + dx] = divide;
                    dx++;
                }
                areas.Add(area2);
                DivideArea(area1.width, area1.height, area1.x, area1.z, area1.cut_dir, areas);
            }
        }
    }

    private void FirstDivideArea(int width, int height, int baseX, int baseZ)
    {
        int div;//分割の基準
        area1_F = new Area();
        area2_F = new Area();

        if (height <= width)
        {
            div = width / 2;
            int dz = 0;

            area1_F = Area_Setting(area1_F, div, height, baseX, baseZ, none);
            area2_F = Area_Setting(area2_F, width - (div + 1), height, baseX + div + 1, baseZ, none);

            while ((baseZ + dz) < mapHeight && map[baseZ + dz, baseX + div] == wall)
            {
                map[baseZ + dz, baseX + div] = divide_F;
                dz++;
            }
        }

        //高さで分割
        else
        {
            div = height / 2;
            int dx = 0;

            area1_F = Area_Setting(area1_F, width, div, baseX, baseZ, none);
            area2_F = Area_Setting(area2_F, width, height - (div + 1), baseX, baseZ + div + 1, none);

            while ((baseX + dx) < mapWidth && map[baseZ + div, baseX + dx] == wall)
            {
                map[baseZ + div, baseX + dx] = divide_F;
                dx++;
            }
        }
    }

    private void CreateRoom(List<Area> areas, List<Room> room_list)
    {
        for (int index = 0; index < areas.Count; index++)
        {
            Room _room = new Room();

            _room.x = UnityEngine.Random.Range(areas[index].x + blank, areas[index].x + areas[index].width / 3);
            _room.z = UnityEngine.Random.Range(areas[index].z + blank, areas[index].z + areas[index].height / 3);


            _room.width = UnityEngine.Random.Range(room_min_width, areas[index].width - Mathf.Abs(areas[index].x - _room.x) - 2);
            _room.height = UnityEngine.Random.Range(room_min_height, areas[index].height - Mathf.Abs(areas[index].z - _room.z) - 2);
            _room.connectX = _room.x + (_room.width - 1) / 2;
            _room.connectZ = _room.z + (_room.height - 1) / 2;

            _room.area_info = areas[index];
            if (index + 1 < areas.Count) _room.next_area_info = areas[index + 1];

            for (int dz = 0; dz < _room.height; dz++)
            {
                for (int dx = 0; dx < _room.width; dx++)
                {
                    map[_room.z + dz, _room.x + dx] = room;
                }
            }
            room_list.Add(_room);
        }
    }
    private void CreateRoad(List<Room> room_list)
    {
        int anchor = 0;
        for(int index = 0 ; index < room_list.Count ; index++)
        {
            if (room_list[index].area_info.cut_dir == up)
            {
                for (int z = room_list[index].connectZ ; z < mapHeight; z++)
                {
                    anchor = z;
                    if (map[z, room_list[index].connectX] == wall) map[z, room_list[index].connectX] = road;
                    else if (map[z, room_list[index].connectX] == divide) break;
                }
                if (room_list[index].connectX < room_list[index - 1].connectX)
                {
                    for (int x = room_list[index].connectX; x <= room_list[index - 1].connectX; x++)
                    {
                        map[anchor, x] = road;
                    }
                }
                else if (room_list[index].connectX > room_list[index - 1].connectX)
                {
                    for (int x = room_list[index].connectX; x >= room_list[index - 1].connectX; x--)
                    {
                        map[anchor, x] = road;
                    }
                }
                else
                {
                    map[anchor, room_list[index].connectX] = road;
                }
            }
            else if (room_list[index].area_info.cut_dir == down)
            {
                for (int z = room_list[index].connectZ; z >= 0; z--)
                {
                    anchor = z;
                    if (map[z, room_list[index].connectX] == wall) map[z, room_list[index].connectX] = road;
                    else if (map[z, room_list[index].connectX] == divide) break;
                }

                if (room_list[index].connectX < room_list[index - 1].connectX)
                {
                    for (int x = room_list[index].connectX; x <= room_list[index - 1].connectX; x++)
                    {
                        map[anchor, x] = road;
                    }
                }
                else if (room_list[index].connectX > room_list[index - 1].connectX)
                {
                    for (int x = room_list[index].connectX; x >= room_list[index - 1].connectX; x--)
                    {
                        map[anchor, x] = road;
                    }
                }
                else
                {
                    map[anchor, room_list[index].connectX] = road;
                }
            }
            else if(room_list[index].area_info.cut_dir == left)
            {
                for (int x = room_list[index].connectX; x >= 0; x--)
                {
                    anchor = x;
                    if (map[room_list[index].connectZ, x] == wall) map[room_list[index].connectZ, x] = road;
                    else if (map[room_list[index].connectZ, x] == divide) break;
                }

                if (room_list[index].connectZ < room_list[index - 1].connectZ)
                {
                    for (int z = room_list[index].connectZ; z <= room_list[index - 1].connectZ; z++)
                    {
                        map[z, anchor] = road;
                    }
                }
                else if (room_list[index].connectZ > room_list[index - 1].connectZ)
                {
                    for (int z = room_list[index].connectZ; z >= room_list[index - 1].connectZ; z--)
                    {
                        map[z, anchor] = road;
                    }
                }
                else
                {
                    map[room_list[index].connectZ, anchor] = road;
                }
            }
            else if(room_list[index].area_info.cut_dir == right)
            {
                for (int x = room_list[index].connectX; x < mapWidth; x++)
                {
                    anchor = x;
                    if (map[room_list[index].connectZ, x] == wall) map[room_list[index].connectZ, x] = road;
                    else if (map[room_list[index].connectZ, x] == divide) break;
                }
                if (room_list[index].connectZ < room_list[index - 1].connectZ)
                {
                    for (int z = room_list[index].connectZ; z <= room_list[index - 1].connectZ; z++)
                    {
                        map[z, anchor] = road;
                    }
                }
                else if (room_list[index].connectZ > room_list[index - 1].connectZ)
                {
                    for (int z = room_list[index].connectZ; z >= room_list[index - 1].connectZ; z--)
                    {
                        map[z, anchor] = road;
                    }
                }
                else
                {
                    map[room_list[index].connectZ, anchor] = road;
                }
            }
            
            //--------------------------------------------------------------------------------------------------------------------------------------------------
            if(index < room_list.Count - 1)
            {
                if (room_list[index].next_area_info.cut_dir == down)
                {
                    for (int z = room_list[index].connectZ; z < mapHeight; z++)
                    {
                        if (map[z, room_list[index].connectX] == wall) map[z, room_list[index].connectX] = road;
                        else if(map[z, room_list[index].connectX] == divide) break;
                    }
                }
                else if (room_list[index].next_area_info.cut_dir == up)
                {
                    for (int z = room_list[index].connectZ; z >= 0; z--)
                    {
                        if (map[z, room_list[index].connectX] == wall) map[z, room_list[index].connectX] = road;
                        else if (map[z, room_list[index].connectX] == divide) break;
                    }
                }
                else if (room_list[index].next_area_info.cut_dir == left)
                {
                    for (int x = room_list[index].connectX; x < mapWidth; x++)
                    {
                        if (map[room_list[index].connectZ, x] == wall) map[room_list[index].connectZ, x] = road;
                        else if (map[room_list[index].connectZ, x] == divide) break;
                    }
                }
                else if (room_list[index].next_area_info.cut_dir == right)
                {
                    for (int x = room_list[index].connectX; x >= 0; x--)
                    {
                        if (map[room_list[index].connectZ, x] == wall) map[room_list[index].connectZ, x] = road;
                        else if (map[room_list[index].connectZ, x] == divide) break;
                    }
                }
            }
        }
    }

    private void ConnectArea()
    {
        int anchorX = 0, anchorZ = 0, anchorZ1 = 0, check_count = 0;
        bool check = false;

        for(int index = 1; index < room_list.Count;index++)
        {
            check = false;
            for(int dx = 0; room_list[index].connectX + dx < mapWidth;dx++)
            {
                if (map[room_list[index].connectZ, room_list[index].connectX + dx] == road) break;
                if(map[room_list[index].connectZ, room_list[index].connectX + dx] == divide_F)
                {
                    check = true;
                    check_count++;
                    anchorX = room_list[index].connectX + dx;
                    anchorZ = room_list[index].connectZ;
                    break;
                }
            }

            if (check == true)
            {            
                Horizontal(room_list[index].connectX, room_list[index].connectZ, anchorX - 1, right);
                break;
            }
        }


        for(int index = 1 ; index < room_list1.Count ; index++)
        {
            check = false;
            for(int dx = 0; room_list1[index].connectX - dx >= 0;dx++)
            {
                if (map[room_list1[index].connectZ, room_list1[index].connectX - dx] == road) break;
                if(map[room_list1[index].connectZ, room_list1[index].connectX - dx] == divide_F)
                {
                    check = true;
                    anchorZ1 = room_list1[index].connectZ;
                    break;
                }
            }

            if (check == true)
            {
                Horizontal(room_list1[index].connectX, room_list1[index].connectZ, anchorX + 1, left);
                break;
            }
        }

        if (check == false)
        {
            for (int dx = 0; room_list[0].connectX + dx < mapWidth; dx++)
            {
                if (map[room_list[0].connectZ, room_list[0].connectX + dx] == divide_F)
                {
                    anchorX = room_list[0].connectX + dx;
                    anchorZ = room_list[0].connectZ;
                    break;
                }
            }
            Horizontal(room_list[0].connectX, room_list[0].connectZ, anchorX - 1, right);

            for (int dx = 0; room_list1[0].connectX - dx >= 0; dx++)
            {
                if (map[room_list1[0].connectZ, room_list1[0].connectX - dx] == divide_F)
                {
                    anchorZ1 = room_list1[0].connectZ;
                    break;
                }
            }                
            Horizontal(room_list1[0].connectX, room_list1[0].connectZ, anchorX + 1, left);

        }

        if (anchorZ < anchorZ1) Vertical(anchorX, anchorZ, anchorZ1, up);

        else if (anchorZ > anchorZ1) Vertical(anchorX, anchorZ, anchorZ1, down);

        else map[anchorZ, anchorX] = road;

    }

    private void Horizontal(int startX, int startZ, int endX, int dir)
    {
        if(dir == right)
        {
            for (int dx = 0; startX + dx < mapWidth; dx++)
            {
                if (map[startZ, startX + dx] != room) map[startZ, startX + dx] = road;
                if (startX + dx == endX) break;
            }
        }
        else
        {
            for (int dx = 0; startX - dx >= 0; dx++)
            {
                if (map[startZ, startX - dx] != room) map[startZ, startX - dx] = road;
                if (startX - dx == endX) break;
            }
        }
    }

    private void Vertical(int startX, int startZ, int endZ, int dir)
    {
        if(dir == up)
        {
            for (int dz = 0; startZ + dz < mapHeight; dz++)
            {
                if (map[startZ + dz, startX] != room) map[startZ + dz, startX] = road;
                if (startZ + dz == endZ) break;
            }
        }
        else
        {
            for (int dz = 0; startZ - dz < mapHeight; dz++)
            {
                if (map[startZ - dz, startX] != room) map[startZ - dz, startX] = road;
                if (startZ - dz == endZ) break;
            }
        }
    }

    private void CreatePerimeterWall()
    {
        //左辺。0列目
        for(int i = 0; i < mapHeight; i++)
        {
            map[0, i] = wall;
        }

        //右辺。map最大値列目
        for(int j = 0; j < mapHeight; j++)
        {
            map[mapWidth - 1, j] = wall;
        }

        //下辺。0行目
        for(int k = 1; k < mapWidth -1; k++)
        {
            map[k, 0] = wall;
        }

        //上辺。map最大値行目
        for(int l = 1; l < mapWidth - 1; l++)
        {
            map[l, mapHeight - 1] = wall;
        }
    }

    /// <summary>
    /// 階段をランダムに配置
    /// </summary>
    private void PostStair()
    {
        while (!postStairChaeck)
        {
            randomX = UnityEngine.Random.Range(0, mapWidth);
            randomZ = UnityEngine.Random.Range(0, mapHeight);

            Debug.Log("ランダムに座標を取得 :" + randomX + "," + randomZ + "> mapの値:" + map[randomX, randomZ]);

            if (map[randomZ, randomX] == room)
            {
                stair.transform.position = new Vector3(randomX, 0.1f, randomZ);

                Debug.Log("階段を配置");

                postStairChaeck = true;
            }
        }

        //ミニマップに階段を配置
        miniMapManage.StairOnMiniMap(randomX, randomZ);
    }

    /// <summary>
    /// NavMeshをBakeする
    /// </summary>
    private void BakeNavMesh()
    {
        // 各 NavMeshSurface をベイク
        wallSurface.BuildNavMesh();
        walkAbleSurface.BuildNavMesh();
    }

    /// <summary>
    /// プレイヤーを置く
    /// </summary>
    private void SetPlayerPosition()
    {
        int x;
        int z;

        //ランダムに座標を取得し、部屋の中かどうかを確認する
        do
        {
            x = UnityEngine.Random.Range(0, map.GetLength(0));
            z = UnityEngine.Random.Range(0, map.GetLength(1));
        }
        while (map[z, x] != room || (x == randomX && z == randomZ));

        //ランダムに取得した座標にプレイヤーを配置する
        playerObj.transform.position = new Vector3(x, 0.1f, z);
    }

    /// <summary>
    /// ダンジョンテクスチャ変更
    /// </summary>
    private void SetDungeonTexture()
    {
        //スクリタブルオブジェクトから現在の階層を取得
        int floorNum = floorNumSave.GetCurrentFloorNum();

        //階層を5で割った値を、配列のインデックスとする
        int texIndex = floorNum / 5;

        if (texIndex >= 6)
        {
            texIndex = 5;
        }

        //壁のテクスチャの変更
        wallMaterial.SetTexture("_MainTex", wallTextures[texIndex]);

        //紅葉のため、テクスチャの色を変更する
        if (texIndex == 1)
        {
            wallMaterial.SetColor("_Color", new Color(1.0f, 0.12f, 0.01f, 1.0f));
        }
        else
        {
            wallMaterial.SetColor("_Color", Color.white);
        }

        //部屋のテクスチャの変更
        roomMaterial.SetTexture("_MainTex", roomTextures[texIndex]);

        //道のテクスチャの変更
        roadMaterial.SetTexture("_MainTex", roadTextures[texIndex]);
    }

    #endregion
}
