using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[HarmonyPatch]
class EditMaterialProperties
{
	internal static string MESSAGE = "Changing Material Properties from config file";
	[HarmonyPatch(typeof(WearNTear), "GetMaterialProperties")]
	[HarmonyPrefix]
	private static bool GetMaterialProperties_Prefix(out float maxSupport, out float minSupport, out float horizontalLoss, out float verticalLoss, WearNTear.MaterialType ___m_materialType, ref bool __runOriginal)
	{
		__runOriginal = false;
		// Original Setting Values
		//Wood = new MaterialSettings() { maxSupport = 100f, minSupport = 10f, verticalLoss = 0.125f, horizontalLoss = 0.2f };
		//Stone = new MaterialSettings() { maxSupport = 1000f, minSupport = 100f, verticalLoss = 0.125f, horizontalLoss = 0.1f };
		//Iron = new MaterialSettings() { maxSupport = 1500f, minSupport = 20f, verticalLoss = 0.07692308f, horizontalLoss = 0.07692308f };
		//HardWood = new MaterialSettings() { maxSupport = 140f, minSupport = 10f, verticalLoss = 0.1f, horizontalLoss = 0.166666672f };
		//Default = new MaterialSettings() { maxSupport = 0f, minSupport = 0f, verticalLoss = 0f, horizontalLoss = 0f };

		if (Configuration.enable_old_calculation.Value)
		{

			switch (___m_materialType)
			{
				case WearNTear.MaterialType.Wood:
					maxSupport = Configuration.Wood_maxSupport.Value;
					minSupport = Configuration.Wood_minSupport.Value;
					verticalLoss = Configuration.Wood_verticalLoss.Value;
					horizontalLoss = Configuration.Wood_horizontalLoss.Value;
					return false;
				case WearNTear.MaterialType.Stone:
					maxSupport = Configuration.Stone_maxSupport.Value;
					minSupport = Configuration.Stone_minSupport.Value;
					verticalLoss = Configuration.Stone_verticalLoss.Value;
					horizontalLoss = Configuration.Stone_horizontalLoss.Value;
					return false;
				case WearNTear.MaterialType.Iron:
					maxSupport = Configuration.Iron_maxSupport.Value;
					minSupport = Configuration.Iron_minSupport.Value;
					verticalLoss = Configuration.Iron_verticalLoss.Value;
					horizontalLoss = Configuration.Iron_horizontalLoss.Value;
					return false;
				case WearNTear.MaterialType.HardWood:
					maxSupport = Configuration.HardWood_maxSupport.Value;
					minSupport = Configuration.HardWood_minSupport.Value;
					verticalLoss = Configuration.HardWood_verticalLoss.Value;
					horizontalLoss = Configuration.HardWood_horizontalLoss.Value;
					return false;
				default:
					maxSupport = 0;
					minSupport = 0;
					verticalLoss = 0;
					horizontalLoss = 0;
					return false;
			}
		}
		else
		{ 
			maxSupport = Configuration.Wood_maxSupport_New.Value;
			minSupport = Configuration.Wood_minSupport_New.Value;
			verticalLoss = Configuration.Wood_verticalLoss_New.Value;
			horizontalLoss = Configuration.Wood_horizontalLoss_New.Value;
			if (___m_materialType == WearNTear.MaterialType.Wood)
			{
				return false;
			}
			if (___m_materialType == WearNTear.MaterialType.Stone)
			{
				maxSupport *= Configuration.Stone_maxSupport_New.Value;
				minSupport *= Configuration.Stone_minSupport_New.Value;
				verticalLoss *= Configuration.Stone_verticalLoss_New.Value;
				horizontalLoss *= Configuration.Stone_horizontalLoss_New.Value;
				return false;
			}
			if (___m_materialType == WearNTear.MaterialType.Iron)
			{
				maxSupport *= Configuration.Iron_maxSupport_New.Value;
				minSupport *= Configuration.Iron_minSupport_New.Value;
				verticalLoss *= Configuration.Iron_verticalLoss_New.Value;
				horizontalLoss *= Configuration.Iron_horizontalLoss_New.Value;
				return false;
			}
			if (___m_materialType == WearNTear.MaterialType.HardWood)
			{
				maxSupport *= Configuration.HardWood_maxSupport_New.Value;
				minSupport *= Configuration.HardWood_minSupport_New.Value;
				verticalLoss *= Configuration.HardWood_verticalLoss_New.Value;
				horizontalLoss *= Configuration.HardWood_horizontalLoss_New.Value;
				return false;
			}
			return false;
		}
	}
}