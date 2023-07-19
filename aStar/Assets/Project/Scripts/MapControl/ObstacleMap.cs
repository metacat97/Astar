using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMap : TileMapController
{
    private const string OBSTACLE_TILEMAP_OBJ_NAME = "ObstacleTilemap";
    private GameObject[] castleObjs = default; //!<��ã�� �˰����� �׽�Ʈ�� ������� �������� ĳ���� ������Ʈ �迭

    //! Awake Ÿ�ӿ� �ʱ�ȭ �� ������ ������ �Ѵ�.
    public override void InitAwake(MapBoard mapController_)
    {
        this.tileMapObjName = OBSTACLE_TILEMAP_OBJ_NAME;
        base.InitAwake(mapController_);
    }
    private void Start()
    {
        StartCoroutine(DelayStart(0f));
    }
    private IEnumerator DelayStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        DoStart();
    }

  
    private void DoStart()
    {
        // { ������� �������� �����ؼ� Ÿ���� ��ġ�Ѵ�.
        castleObjs = new GameObject[2];
        TerrainController[] passableTerrains = new TerrainController[2];

        List<TerrainController> searchTerrains = default;
        int searchIdx = 0;
        TerrainController foundTile = default;

        // ������� �������� �������� y���� ��ġ�ؼ� �� ������ �޾ƿ´�.
        searchIdx = 0;
        foundTile = default;
        while(foundTile == null || foundTile == default)
        {
            //������ �Ʒ��� ��ġ�Ѵ�.
            searchTerrains = mapController.GetTerrains_Colum(searchIdx, true);
            foreach(var searchTerrain in searchTerrains)
            {
                if ( searchTerrain.IsPassable == true)
                {
                    foundTile = searchTerrain; ;
                    break;
                }
                else { /*Do nothing*/}

                if(foundTile != null || foundTile != default) { break; }
                if(mapController.MapCellSize.x - 1 <= searchIdx) { break; }
                searchIdx++;
            }  //loop ������� ã�� ����
            passableTerrains[0] = foundTile;

            //�������� �������� �������� y���� ��ġ�ؼ� �� ������ �޾ƿ´�
            searchIdx = mapController.MapCellSize.x-1;
            foundTile = default;
            while(foundTile == null  || foundTile == default)
            {
                //�Ʒ����� ���� ��ġ�Ѵ�.
                searchTerrains = mapController.GetTerrains_Colum(searchIdx);
                foreach (var searchTerrain in searchTerrains)
                {
                    if (searchTerrain.IsPassable)
                    { 
                        foundTile = searchTerrain;
                        break;
                    }
                    else { /*Do nothing*/}
                    if (foundTile != null || foundTile != default) { break; }
                    if(searchIdx <= 0) { break; }
                    searchIdx--;
                }// loop : �������� ã�� ����
                passableTerrains[1] = foundTile;

                //������� �������� ������ �߰��Ѵ�.
                GameObject changeTilePrefab = ResManager.Instance.obstaclePrefabs[RDefine.OBSTACLE_PREF_PLAIN_CASTLE];
                GameObject tempChangeTile = default;
                for(int i = 0; i< 2; i ++)
                {
                    tempChangeTile = Instantiate(changeTilePrefab, tileMap.transform);
                    tempChangeTile.name = string.Format("{0}_{1}",
                        changeTilePrefab.name, passableTerrains[i].TileIdx1D);
                    tempChangeTile.SetLocalScale(passableTerrains[i].transform.localScale);
                    tempChangeTile.SetLocalPos(passableTerrains[i].transform.localPosition);

                    //������� �������� ĳ���Ѵ�.
                    castleObjs[i] = tempChangeTile;
                    Add_Obstacle(tempChangeTile);

                    tempChangeTile = default;
                }
            
            }
            

        }
        // } ������� �������� �����ؼ� Ÿ���� ��ġ�Ѵ�.
        Update_SourDestToPathFinder();
    } //DoStart()


    //! ������ �߰��Ѵ�.
    public void Add_Obstacle(GameObject obstacle_)
    {
        allTileObjs.Add(obstacle_);
    }// Add_Obstacle()

    //! �н����δ��� ������� �������� �����Ѵ�.
    public void Update_SourDestToPathFinder()
    {
        PathFinder.Instance.sourceObj = castleObjs[0];
        PathFinder.Instance.destinationObj = castleObjs[1];

    }       // Update_SourDestToPathFinder()
}
