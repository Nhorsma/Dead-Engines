using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostData : MonoBehaviour
{
	//utility
	public static List<float[]> changer_list = new List<float[]>() {};

	public static float[] build_refinery = new float[4];
	public static float[] build_storage = new float[4];
	public static float[] build_shrine = new float[4];
	public static float[] build_study = new float[4]; //0 & 1 : metal & electronics original

	public static float[] upgrade_refinery, upgrade_storage, upgrade_shrine, upgrade_study = new float[3]; //3 levels of upgrades

	public static int repair_generator, repair_controller;

	public static int metal_bolt, metal_plate;

	public static int electronics_wire, electronics_chip;

	public static int[] special_part, special_board = new int[2]; // [ingredient 1, ingredient 2] -> oh boy I wish I was better at parsing things

	private void Start()
	{
		build_refinery[0] = 15;
		build_refinery[1] = 15;

		build_storage[0] = 25;
		build_storage[1] = 25;

		build_shrine[0] = 50;
		build_shrine[1] = 50;

		build_study[0] = 50;
		build_study[1] = 50;

		changer_list.Add(build_refinery);
		changer_list.Add(build_storage);
		changer_list.Add(build_shrine);
		changer_list.Add(build_study);

		ChangeRoomCost();
	}

	public static void ChangeRoomCost()
	{
		foreach (float[] i in changer_list)
		{
			//change metal cost, 0 is original cost and 2 is new cost
			i[2] = i[0] * ((11 - EffectConnector.roomCost) / 10);
			i[2] = Mathf.RoundToInt(i[2]); // -> is this necessary???
			Debug.Log(i[2].ToString());

			//change electronics cost, 1 is original cost and 3 is new cost
			i[3] = i[1] * ((11 - EffectConnector.roomCost) / 10);
			i[3] = Mathf.RoundToInt(i[3]); // -> is this necessary???
			Debug.Log(i[3].ToString());
		}
	}
}
