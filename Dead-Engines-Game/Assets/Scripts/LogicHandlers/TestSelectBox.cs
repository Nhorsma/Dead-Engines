using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class TestSelectBox : MonoBehaviour
{
    // straight from:
    // https://answers.unity.com/questions/1575087/how-can-i-draw-a-box-ingame.html

    public Texture drawBox;
    public SelectItems selectItems;

	bool dragging = false;
	Rect selectionBox;
	Vector2 initialMousePosition;
	Vector2 currentMousePosition;
	Event e;

    void OnGUI()
	{
		e = Event.current;
        if (e.type == EventType.MouseDrag)
		{
			if (!dragging && !selectItems.IsPointerOverUIElement())
			{
				dragging = true;
				initialMousePosition = new Vector2(e.mousePosition.x, Screen.height-selectItems.mouseFirst.y);
			}
		}
        if (e.type == EventType.MouseUp)
		{
			dragging = false;
		}
        if (dragging)
		{
            currentMousePosition = e.mousePosition;
			selectionBox = new Rect(initialMousePosition.x, initialMousePosition.y, currentMousePosition.x - initialMousePosition.x, currentMousePosition.y - initialMousePosition.y);
            GUI.Box(selectionBox, GUIContent.none);           
        }
	}
}
