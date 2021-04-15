using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertSystem : MonoBehaviour
{
	//fill a list with Alert messages
	//when right clicked, close and delete.

	public Text alertDisplay1;
	public Text alertDisplay2;
	public Text alertDisplay3;

	public static List<string> alertList = new List<string>();

	private void Start()
	{
		alertList.Add("Hello");
		alertList.Add("World");
		alertList.Add("");
	}

	//static method to add alert message to queue
	public void AddAlert(string message)
	{
		alertList.Add(message);
	}

	//static method to remove alert message from list
	public void RemoveAlert(int alertSlot)
	{
		alertList.Remove(alertList[alertSlot]);
		//Debug.Log() //check to make sure indeces go down
		ShiftDisplayDown();
	}

	public void ShiftDisplayDown()
	{
		alertDisplay1.text = alertList[0];
		alertDisplay2.text = alertList[1];
		alertDisplay3.text = alertList[2];
	}
	
}
