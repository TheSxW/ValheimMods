using HarmonyLib;

class EditMaterialProperties {
	[HarmonyPatch(typeof(WearNTear), "GetMaterialProperties")]
	[HarmonyPrefix]
	private static bool GetMaterialProperties_Prefix(out float maxSupport, out float minSupport, out float horizontalLoss, out float verticalLoss, WearNTear.MaterialType ___m_materialType, ref bool __runOriginal) {
		__runOriginal = false;
		// im using multiply cause its easier to change overall values just by editing those 4 below ;)
		// wood = 1, stone = 10 (in support) ~1/2 in loss, iron is 15 and between stone and wood in loss, hardwood is vertical as stone and orizontal as stone
		maxSupport = 100;
		minSupport = 10;
		verticalLoss = 0.1f;
		horizontalLoss = 0.125f;
		if (___m_materialType == WearNTear.MaterialType.Wood)
		{
			return false;
		}
		if (___m_materialType == WearNTear.MaterialType.Stone)
		{
			maxSupport *= 10f;
			minSupport *= 10f;
			verticalLoss *= 0.5f;
			horizontalLoss *= 0.8f;
			return false;
		}
		if (___m_materialType == WearNTear.MaterialType.Iron)
		{
			maxSupport *= 15f;
			minSupport *= 2f;
			verticalLoss *= 0.75f;
			horizontalLoss *= 0.5f;
			return false;
		}
		if (___m_materialType == WearNTear.MaterialType.HardWood)
		{
			maxSupport *= 2f;
			//minSupport *= 10f;
			verticalLoss *= 0.5f;
			horizontalLoss *= 0.8f;
			return false;
		}
		return false;
		// Original Setting Values
		//Wood = new MaterialSettings() { maxSupport = 100f, minSupport = 10f, verticalLoss = 0.125f, horizontalLoss = 0.2f };
		//Stone = new MaterialSettings() { maxSupport = 1000f, minSupport = 100f, verticalLoss = 0.125f, horizontalLoss = 0.1f };
		//Iron = new MaterialSettings() { maxSupport = 1500f, minSupport = 20f, verticalLoss = 0.07692308f, horizontalLoss = 0.07692308f };
		//HardWood = new MaterialSettings() { maxSupport = 140f, minSupport = 10f, verticalLoss = 0.1f, horizontalLoss = 0.166666672f };
		//Default = new MaterialSettings() { maxSupport = 0f, minSupport = 0f, verticalLoss = 0f, horizontalLoss = 0f };
	}
}
