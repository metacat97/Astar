using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileMapController : MonoBehaviour
{
    protected string tileMapObjName = default;
    protected MapBoard mapController = null;
    protected Tilemap tileMap = default;
    protected List<GameObject> allTileObjs = default;

    //! Awake Ÿ�ӿ� �ʱ�ȭ �� ������ ��ӹ��� Ŭ�������� �������Ѵ�.
    public virtual void InitAwake(MapBoard mapController_)
    {
        tileMap = gameObject.FindChildComponent<Tilemap>(tileMapObjName);

        //���簢�� ���·� �ʱ�ȭ ��Ÿ���� ĳ���ؼ� ������ �ִ´�.
        allTileObjs = tileMap.gameObject.GetChildrenObjs();
        if (allTileObjs.IsValid())
        {
            allTileObjs.Sort(GFunc.CompareTileObjToLocalPos2D);
        }
        else { allTileObjs = new List<GameObject>(); }
    } //InitAwake()

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}