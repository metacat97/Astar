using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderBtn : MonoBehaviour
{
   //! A star find ��ư�� ���� ���
   public void OnClickAstarFindBtn()
    {
        PathFinder.Instance.FindPath_Astar();
    }       // OnClickAstarFindBtn
}
