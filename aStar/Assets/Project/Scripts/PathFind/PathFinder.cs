using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : GSingleton<PathFinder> 
{
    #region ���� Ž���� ���� ����
    public GameObject sourceObj = default;
    public GameObject destinationObj = default;
    public MapBoard mapBoard = default;
    #endregion // ���� Ž���� ���� ����

    #region A star �˰������� �ִ� �Ÿ��� ã�� ���� ����
    private List<AStarNode> aStarResultPath = default;
    private List<AStarNode> aStarOpenPath = default;
    private List<AStarNode> aStarClosePath = default;
    #endregion //A star �˰������� �ִܰŸ� ã�� ���� ����

    //! ������� ������ ������ ���� ã�� �Լ� 
    public void FindPath_Astar()
    {
        StartCoroutine(DelayFindPath_Astar(0.1f));
        
    }

    //! Ž�� �˰��� �����̸� �ش�
    private IEnumerator DelayFindPath_Astar(float delay_)
    {
        //A star  �˰����� ����ϱ� ���ؼ� �н� ����Ʈ�� �ʱ�ȭ �Ѵ�.
        aStarOpenPath = new List<AStarNode>();
        aStarClosePath = new List<AStarNode>();
        aStarResultPath = new List<AStarNode>();

        TerrainController targetTerrain = default;

        //������� �ε����� ���ؼ� ����� ��带 ã�ƿ´�
        string[] sourceObjNameParts = sourceObj.name.Split('_');

        int sourceIdx1D = -1;
        int.TryParse(
            sourceObjNameParts[sourceObjNameParts.Length - 1], out sourceIdx1D);
        targetTerrain = mapBoard.GetTerrain(sourceIdx1D);
        //ã�ƿ� ����� ��带 Open ����Ʈ�� �߰��Ѵ�.
        AStarNode targetNode = new AStarNode(targetTerrain, destinationObj);
        Add_AstarOpenList(targetNode);

        int loopIdx = 0;
        bool isFoundDestination = false;
        bool isNowayToGo = false;
        //�� �۵��Ǹ� �ٲܰ��� TODO: �˰��� ���� �۵� Ȯ�� �� ���ǹ� ������ ����
        //while (loopIdx < 10)
        while(isFoundDestination == false && isNowayToGo == false)
        {
            //{ Open ����Ʈ�� ��ȸ�ؼ� ���� �ڽ�Ʈ�� ���� ��带 �����Ѵ�.
            AStarNode minCostNode = default; 
            foreach(var terrainNode in aStarOpenPath)
            {
                if(minCostNode == default)
                {
                    minCostNode = terrainNode;
                }       // if : ���� ���� �ڽ�Ʈ�� ��尡 ����ִ� ���
                else
                {
                    //terrainNode �� �� ���� �ڽ�Ʈ�� ������ ���
                    //minCostNode �� ������Ʈ �Ѵ�.
                    if(terrainNode.AstarF < minCostNode.AstarF)
                    {
                        minCostNode = terrainNode;
                    }
                    else { continue; }
                }//else: ���� ���� �ڽ�Ʈ�� ��尡 ĳ�̵Ǿ� �ִ� ���
            }
            //} Open ����Ʈ�� ��ȸ�ؼ� ���� �ڽ�Ʈ�� ���� ��带 �����Ѵ�.

            minCostNode.ShowCost_Astar();
            minCostNode.Terrain.SetTileActiveColor(RDefine.TileStatusColor.SEARCH);

            //������ ��尡 �������� �����ߴ��� Ȯ���Ѵ�
            bool isArriveDest = mapBoard.GetDistance2D(
                minCostNode.Terrain.gameObject, destinationObj).
                Equals(Vector2Int.zero);

            if (isArriveDest) 
            {


                // {�������� �����ߴٸ� aStarResultPath ����Ʈ�� �����Ѵ�
                AStarNode resultNode = minCostNode;
                bool isSet_aStarResultPahtok = false;
                while(isSet_aStarResultPahtok==false)
                {
                    aStarResultPath.Add(resultNode);
                    if (resultNode.AstarPrevNode == default || resultNode.AstarPrevNode == null)
                    {
                        isSet_aStarResultPahtok  = true;
                        break;
                    }
                    else {  /*Do nothing*/}

                    resultNode = resultNode.AstarPrevNode;
                }
                // }�������� �����ߴٸ� aStarResultPath ����Ʈ�� �����Ѵ�

                //Open list �� Close list �� �����Ѵ�.
                aStarOpenPath.Clear();
                aStarClosePath.Clear();
                isFoundDestination = true;
                break;
            }       //if: ������ ��尡 �������� ������ ���
            else
            {
                //{�������� �ʾҴٸ� ���� Ÿ���� �������� 4���� ��带 ã�ƿ´�.
                List<int> nextSearchIdx1Ds = mapBoard.
                    GetTileIdx2D_Around4ways(minCostNode.Terrain.TileIdx2D);

                //ã�ƿ� ��� �߿��� �̵� ������ ���� Open list�� �߰��Ѵ�.
                AStarNode nextNode = default;
                foreach(var nextIdx1D in nextSearchIdx1Ds)
                {
                    nextNode = new AStarNode(
                        mapBoard.GetTerrain(nextIdx1D), destinationObj);

                    if (nextNode.Terrain.IsPassable == false) { continue; }
                    Add_AstarOpenList(nextNode, minCostNode);
                }
                //} �������� �ʾҴٸ� ���� Ÿ���� �������� 4���� ��带 ã�ƿ´�.

                //Ž���� ���� ���� Close list�� �߰��ϰ�, OPen list���� �����Ѵ�.
                //�� �� , Openlist �� ����ִٸ� �� �̻� Ž���� �� �ִ� ���� �������� �ʴ� ���̴�.
                aStarClosePath.Add(minCostNode);
                aStarOpenPath.Remove(minCostNode);
                if (aStarOpenPath.IsValid() == false)
                {
                    GFunc.LogWarning("[Warning] There are no more tiles to explore.");
                    isNowayToGo = true;
                } // if: �������� �������� ���ߴµ�, �� �̻� Ž���� �� �ִ� ���� ���� ���

                foreach(var tempNode in aStarOpenPath)
                {
                    GFunc.Log($"Idx: {tempNode.Terrain.TileIdx1D}," +
                        $"Cost: {tempNode.AstarF}");
                }
            }       //else : ������ ��尡 �������� �������� ���� ���
            loopIdx++;
            yield return new WaitForSeconds(delay_);
        }       // loop:A star �˰������� ���� ã�� ���� ����
    }

    //! ����� ������ ��带 Open ����Ʈ�� �߰��Ѵ�.
    private void Add_AstarOpenList(
        AStarNode targetTerrain_, AStarNode prevNode = default)
    {
        //Open ����Ʈ�� �߰��ϱ� ���� �˰��� ����� �����Ѵ�.
        Update_AstarCostToTerrain(targetTerrain_, prevNode);

        AStarNode closeNode = aStarClosePath.FindNode(targetTerrain_);
        if (closeNode != default && closeNode != null) 
        {
            //�̹� Ž���� ���� ��ǥ�� ��尡 �����ϴ� ��쿡�� 
            // Open list �� �߰����� �ʴ´�.
            /*Do Nothing*/ 
        }
        else
        {
            AStarNode openedNode = aStarOpenPath.FindNode(targetTerrain_);
            if (openedNode != default && openedNode != null)
            {
                //Ÿ�� ����� �ڽ�Ʈ�� �� ���� ��쿡�� Open list���� ��带 ��ü�Ѵ�.
                //Ÿ�� ����� �ڽ�Ʈ�� �� ū ��쿡�� Open list �� �߰����� �ʴ´�.
                if(targetTerrain_.AstarF < openedNode.AstarF) 
                {
                    aStarOpenPath.Remove(openedNode);
                    aStarOpenPath.Add(targetTerrain_);
                }
                else { /*Do nothing*/}

            }
            else
            {
                aStarOpenPath.Add(targetTerrain_);
            }       //else : Open list �� ���� �߰��� ���� ���� ��ǥ�� ��尡 ���� ���
        }       //else : ���� Ž���� ������ ���� ����� ���
    } //Add_AstarOpenList()

    //! Target ���� ������ Destination ���� ������ Distance �� Heuristic �� �����ϴ� �Լ�
    private void Update_AstarCostToTerrain(
        AStarNode targetNode, AStarNode prevNode)
    {
        //{Target �������� Destination ������ 2D Ÿ�� �Ÿ��� ����ϴ� ����
        Vector2Int distance2D = mapBoard.GetDistance2D(
            targetNode.Terrain.gameObject, destinationObj);
        int totalDistance2D = distance2D.x + distance2D.y;

        //Heuristic �� ���� �Ÿ��� �����Ѵ�
        Vector2 localDistance = destinationObj.transform.localPosition -
            targetNode.Terrain.transform.localPosition;

        //#######################################################################################
        //#########                    �޸���ƽ ��������                                       #####
        //#######################################################################################
        float heuristic = Mathf.Abs(localDistance.magnitude);
        //float heuristic = 10.0f;

        //}Target �������� Destination ������ 2D Ÿ�� �Ÿ��� ����ϴ� ����

        //{ ���� ��尡 �����ϴ� ��� , ���� ����� �ڽ�Ʈ�� �߰��ؼ� �����Ѵ�.
        if (prevNode == default || prevNode == null) { /*Do nothing*/}
        else
        {
            totalDistance2D = Mathf.RoundToInt(prevNode.AstarG + 1.0f);
        }
        targetNode.UpdateCost_Astar(totalDistance2D, heuristic, prevNode);
        //} ���� ��尡 �����ϴ� ��� , ���� ����� �ڽ�Ʈ�� �߰��ؼ� �����Ѵ�.



    }// Update_AstarCostToTerrain()
}
