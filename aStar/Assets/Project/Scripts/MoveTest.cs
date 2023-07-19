using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    string test = default;
    // Start is called before the first frame update
    void Start()
    {
        test = "Å×½ºÆ®";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.LogFormat("{0}", test);
        }
    }
}
