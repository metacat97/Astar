using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TerrainMap : TileMapController
{
    private const string TERRAIN_TILEMAP_OBJ_NAME = "TerrainTilemap";

    private Vector2Int mapCellSize = default;
    private Vector2 mapCellGap = default;

    private List<TerrainController> allTerrains = default;

    //! Awake Ÿ�ӿ� �ʱ�ȭ �� ������ �������Ѵ�.
    public override void InitAwake(MapBoard mapController_)
    {
        tileMapObjName = TERRAIN_TILEMAP_OBJ_NAME;
        base.InitAwake(mapController_);

        //Debug.LogFormat("mapController_ empty: {0}", mapController_ == null);

        allTerrains = new List<TerrainController>();
        // {Ÿ���� x�� ������ ��ü Ÿ���� ���� ���� ����, ���� ����� �����Ѵ�.
        mapCellSize = Vector2Int.zero;
        float tempTileY = allTileObjs[0].transform.localPosition.y;
        for (int i = 0; i < allTileObjs.Count; i++)
        {
            if (tempTileY.IsEquals(allTileObjs[i].transform.localPosition.y)== false)
            {
                mapCellSize.x = i;
                break;
            }// if ù��° Ÿ���� y ��ǥ�� �޶����� ���� �������� ���� ���� �� ũ���̴�.
        }
        // ��ü Ÿ���� ���� ���� ���� �� ũ��� ���� ���� ���� ���� �� ũ���̴�.
        mapCellSize.y = Mathf.FloorToInt(allTileObjs.Count / mapCellSize.x);
        //} Ÿ���� x�� ������ ��ü Ÿ���� ���� ���� ����, ���� ����� �����Ѵ�.

        // {x �� ���� �� Ÿ�ϰ�, y�� ���� �� Ÿ�� ������ ���� ���������� Ÿ�� ���� �����Ѵ�.   
        mapCellGap = Vector2.zero;
        mapCellGap.x = allTileObjs[1].transform.localPosition.x - allTileObjs[0].transform.localPosition.x;
        mapCellGap.y = allTileObjs[mapCellSize.x].transform.localPosition.y - allTileObjs[0].transform.localPosition.y;

        // }x �� ���� �� Ÿ�ϰ�, y�� ���� �� Ÿ�� ������ ���� ���������� Ÿ�� ���� �����Ѵ�.   
    }
    //! �ʱ�ȭ�� Ÿ���� ������ ������ ���� ����, ���� ũ�⸦ �����ϴ� �Լ�

    private void Start()
    {
        //{Ÿ�ϸ��� �Ϻθ� ���� Ȯ���� �ٸ� Ÿ�Ϸ� ��ü�ϴ� ����
        GameObject changeTilePrefab = ResManager.Instance.terrainPrefabs
            [RDefine.TERRAIN_PREF_OCEAN];
        //}Ÿ�ϸ��� �Ϻθ� ���� Ȯ���� �ٸ� Ÿ�Ϸ� ��ü�ϴ� ����

        //Ÿ�ϸ� �߿� ��� ������ �ٴٷ� ��ü�� ������ �����Ѵ�.
        const float CHANGE_PERCENTAGE = 15.0F;
        float correctChangePercentage = allTileObjs.Count * (CHANGE_PERCENTAGE / 100.0f);

        List<int> changedTileResult = GFunc.CreateList(allTileObjs.Count, 1);
        changedTileResult.Shuffle();

        GameObject tempChangeTile = default;
        for(int i = 0; i < allTileObjs.Count; i ++)
        {
            if (correctChangePercentage <= changedTileResult[i]) { continue; }

            tempChangeTile = Instantiate(changeTilePrefab, tileMap.transform);
            tempChangeTile.name = changeTilePrefab.name;
            tempChangeTile.SetLocalScale(allTileObjs[i].transform.localScale);
            tempChangeTile.SetLocalPos(allTileObjs[i].transform.localPosition);

            allTileObjs.Swap(ref tempChangeTile, i);
            tempChangeTile.DestroyObj();
            
        }//loop : ������ ������ ������ ���� Ÿ�ϸʿ� �ٴٸ� �����ϴ� ����
        //{������ �����ϴ� Ÿ���� ������ �����ϰ�, ��Ʈ�ѷ��� ĳ���ϴ� ����
        TerrainController tempTerrain = default;
        TerrainType terrainType = TerrainType.NONE;

        int loopCnt = 0;
        foreach(GameObject tile_ in allTileObjs)
        {
            tempTerrain = tile_.GetComponent<TerrainController>();
            switch(tempTerrain.name)
            {
                case RDefine.TERRAIN_PREF_PLAIN:
                    terrainType = TerrainType.PLAIN_PASS;
                    break;
                case RDefine.TERRAIN_PREF_OCEAN:
                    terrainType = TerrainType.OCEAN_N_PASS;
                    break;
                default:
                    terrainType = TerrainType.NONE;
                    break;
            }//switch : �������� �ٸ� ������ �Ѵ�

            //TODO: tempTerrain Setup �Լ� �ʿ���.
            tempTerrain.SetupTerrain(mapController, terrainType, loopCnt);
            tempTerrain.transform.SetAsFirstSibling();
            allTerrains.Add(tempTerrain);
            loopCnt += 1;
        }
    }//} Ÿ�ϸ����Ϻθ����� Ȯ���� �ٸ� Ÿ�Ϸ� �����ϴ� ����

    public Vector2Int GetCellSize() { return mapCellSize; }
    
    public Vector2 GetCellGap() { return mapCellGap; }

    public TerrainController GetTile(int tileIdx1D)
    {
        if(allTerrains.IsValid(tileIdx1D))
        {
            return allTerrains[tileIdx1D];
        }
        return default;
    }//GetTile()




}
