using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesInEnemy : MonoBehaviour
{
    public List<GameObject> lineArray;
    public int NumOfLines { get; private set; }
    public int maxLines;
    public bool hasNotLine;

    void OnEnable()
    {
        // Gets a random number 1-3
        if (hasNotLine)
            return;

        NumOfLines = Random.Range(1, maxLines + 1);

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
