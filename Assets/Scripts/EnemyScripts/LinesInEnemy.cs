using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesInEnemy : MonoBehaviour
{
    [SerializeField] GameObject line_h;
    [SerializeField] GameObject line_v;
    [SerializeField] GameObject line_dh;
    [SerializeField] GameObject line_uh;
    [SerializeField] GameObject line_rv;
    [SerializeField] GameObject line_lv;
    [SerializeField] GameObject point;
    [SerializeField] GameObject lpoint;

    public int NumOfLines { get; private set; } 

    void Start()
    {
        // Gets a random number 1-3
        NumOfLines = Random.Range(1, 4);

        List<GameObject> lineArray = new List<GameObject>();

        lineArray.Add(line_h);
        lineArray.Add(line_v);
        lineArray.Add(line_dh); 
        lineArray.Add(line_uh);
        //lineArray.Add(line_rv);
        //lineArray.Add(line_lv);
        lineArray.Add(point);
        lineArray.Add(lpoint);

        for (int i = 1; i <= NumOfLines; i++)
        {
            int randomNext = Random.Range(0, lineArray.Count);
            GameObject currentLine;

            currentLine = Instantiate(lineArray[randomNext]);

            lineArray.RemoveAt(randomNext);

            Debug.Log(currentLine);        

            currentLine.transform.SetParent(transform);
            currentLine.transform.localPosition = new Vector3(0, 0, 0);
        }

    }
}
