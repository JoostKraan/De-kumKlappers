using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDrag : MonoBehaviour
{
    
    
    public Camera myCam;
    public RectTransform boxVisual;
    public Rect selectionbox;
    Vector2 startPos;
    Vector2 endPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = Vector2.zero; 
        endPos = Vector2.zero;
        DrawVisual();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            selectionbox = new Rect();
        }   
        if(Input.GetMouseButton(0)) 
        {
            endPos = Input.mousePosition;
            DrawVisual();
        }
        if(Input.GetMouseButtonUp(0))
        {
            SelectedUnits();
            startPos = Vector2.zero;
            endPos = Vector2.zero;
            DrawVisual();
        }
    }
    void DrawVisual()
    {
        Vector2 boxStart = startPos;
        Vector2 boxEnd = endPos;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
        boxVisual.sizeDelta = boxSize;
    }
    void DrawSeletion()
    {
        if(Input.mousePosition.x < startPos.x)
        {
            selectionbox.xMin = Input.mousePosition.x;
            selectionbox.xMax = startPos.x;
        }
        else
        {
            selectionbox.xMin = startPos.x;
            selectionbox.xMax = Input.mousePosition.x;
        }
        if (Input.mousePosition.y < startPos.y)
        {
            selectionbox.xMin = Input.mousePosition.y;
            selectionbox.xMax = startPos.y;
        }
        else
        {
            selectionbox.xMin = startPos.y;
            selectionbox.xMax = Input.mousePosition.y;
        }
    }
    void SelectedUnits()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        foreach (Unit unit in allUnits)
        {
            Vector2 screenPos = myCam.WorldToScreenPoint(unit.transform.position);
            if (selectionbox.Contains(screenPos))
            {
                unit.Selected = true;
            }
            else
            {
                unit.Selected = false;
            }
        }
    }
}
