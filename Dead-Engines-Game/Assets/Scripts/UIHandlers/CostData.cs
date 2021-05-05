using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostData : MonoBehaviour
{
	////utility
	//public static List<float[]> changer_list = new List<float[]>() {};

	//public static float[] build_refinery = new float[4];
	//public static float[] build_storage = new float[4];
	//public static float[] build_shrine = new float[4];
	//public static float[] build_study = new float[4]; //0 & 1 : metal & electronics original

	//public static float[] upgrade_refinery, upgrade_storage, upgrade_shrine, upgrade_study = new float[3]; //3 levels of upgrades

	//public static int repair_generator, repair_controller;

	//public static int metal_bolt, metal_plate;

	//public static int electronics_wire, electronics_chip;

	//public static int[] special_part = new int[2];
	//public static int[] special_board = new int[2]; // [ingredient 1, ingredient 2] -> oh boy I wish I was better at parsing things

	public string refineryCode;
	public string storageCode;
	public string shrineCode;
	public string studyCode;
	public string dormitoryCode;
	public string infirmaryCode;
	public string barracksCode;
	public string cloneryCode;

	public string generatorCode;
	public string controllerCode;

	public string lasersCode;
	public string artilleryCode;
	public string wellOiledCode;
	public string reinforcedCode;
	public string overclockedCode;

	private void Start()
	{
		//build_refinery[0] = 10;
		//build_refinery[1] = 10;

		//build_storage[0] = 20;
		//build_storage[1] = 20;

		//build_shrine[0] = 25;
		//build_shrine[1] = 25;

		//build_study[0] = 25;
		//build_study[1] = 25;

		//changer_list.Add(build_refinery);
		//changer_list.Add(build_storage);
		//changer_list.Add(build_shrine);
		//changer_list.Add(build_study);

		//ChangeRoomCost();

		//repair_controller = 50;
		//repair_generator = 50;

		//metal_bolt = 1;
		//metal_plate = 3;

		//electronics_wire = 1;
		//electronics_chip = 3;

		//special_part[0] = 2;
		//special_part[1] = 2;

		//special_board[0] = 2;
		//special_board[1] = 2;
	}

	public void DecodeAndCheck(string whichCode)
	{
		string code = "";
		switch (whichCode)
		{
			case "refinery":
				code = refineryCode;
				break;
			case "storage":
				code = storageCode;
				break;
			case "shrine":
				code = shrineCode;
				break;
			case "study":
				code = studyCode;
				break;
			case "infirmary":
				code = infirmaryCode;
				break;
			case "clonery":
				code = cloneryCode;
				break;
			case "barracks":
				code = barracksCode;
				break;
			case "dormitory":
				code = dormitoryCode;
				break;
			case "controller":
				code = controllerCode;
				break;
			case "generator":
				code = generatorCode;
				break;
			case "lasers":
				code = lasersCode;
				break;
			case "artillery":
				code = artilleryCode;
				break;
			case "wellOiled":
				code = wellOiledCode;
				break;
			case "reinforced":
				code = reinforcedCode;
				break;
			case "overclocked":
				code = overclockedCode;
				break;
			default:
				break;
		}

		for (int i = 0; i < code.Length/2; i+=2)
		{
			switch (code[i + 1])
			{
				case 'm':
					if (ResourceHandling.metal >= code[i])
					{
						ResourceHandling.metal -= code[i];
					}
					else
					{
						//currently realizing that I need to check both resources before building...wow
					}
					break;
				case 'e':
					break;
				default:
					break;
			}
		}
	}

	//public static void ChangeRoomCost()
	//{
	//	foreach (float[] i in changer_list)
	//	{
	//		//change metal cost, 0 is original cost and 2 is new cost
	//		i[2] = i[0] * ((11 - EffectConnector.roomCost) / 10);
	//		i[2] = Mathf.RoundToInt(i[2]); // -> is this necessary???
	//		Debug.Log(i[2].ToString());

	//		//change electronics cost, 1 is original cost and 3 is new cost
	//		i[3] = i[1] * ((11 - EffectConnector.roomCost) / 10);
	//		i[3] = Mathf.RoundToInt(i[3]); // -> is this necessary???
	//		Debug.Log(i[3].ToString());
	//	}
	//}
}
