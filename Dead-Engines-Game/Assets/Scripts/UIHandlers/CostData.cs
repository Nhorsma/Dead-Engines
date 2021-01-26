using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostData : MonoBehaviour
{
	//utility
	public static List<float[]> changer_list = new List<float[]>();

	[SerializeField]
	public static float[] build_refinery, build_storage, build_shrine, build_study = new float[4]; //0 & 1 : metal & electronics original

	[SerializeField]
	public static float[] upgrade_refinery, upgrade_storage, upgrade_shrine, upgrade_study = new float[3]; //3 levels of upgrades

	[SerializeField]
	public static int repair_generator, repair_controller;

	[SerializeField]
	public static int metal_bolt, metal_plate;

	[SerializeField]
	public static int electronics_wire, electronics_chip;

	[SerializeField]
	public static int[] special_part, special_board = new int[2]; // [ingredient 1, ingredient 2] -> oh boy I wish I was better at parsing things

	private void Start()
	{
		changer_list.Add(build_refinery);
		changer_list.Add(build_storage);
		changer_list.Add(build_shrine);
		changer_list.Add(build_study);

		ChangeRoomCost(); // initializes?
	}

	public static void ChangeRoomCost()
	{
		foreach (float[] i in changer_list)
		{
			//change metal cost, 0 is original cost and 2 is new cost
			i[2] = i[0] * ((11 - EffectConnector.roomCost) / 10);
			i[2] = Mathf.RoundToInt(i[2]); // -> is this necessary???

			//change electronics cost, 1 is original cost and 3 is new cost
			i[3] = i[1] * ((11 - EffectConnector.roomCost) / 10);
			i[3] = Mathf.RoundToInt(i[3]); // -> is this necessary???
		}
	}
}
