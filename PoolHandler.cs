using HarmonyLib;
using flanne;
using UnityEngine;

namespace MTDAssetLoading
{
    static class PoolHandler
    {
        [HarmonyPatch(typeof(ObjectPooler), "Awake")]
        [HarmonyPrefix]
        static bool AwakePrefix(ref ObjectPooler __instance)
        {
            foreach (ObjectPoolItem objectPoolItem in __instance.itemsToPool)
            {
                MTDAssetLoader.Log.LogInfo("Item to pool: " + objectPoolItem.tag + " " + objectPoolItem.amountToPool + " " + objectPoolItem.shouldExpand + " " + objectPoolItem.objectToPool.tag);
                if (objectPoolItem.tag == "BrainMonster")
                {
                    MTDAssetLoader.Log.LogInfo("Cutifying BrainMonster...");
                    SpriteRenderer spriteRenderer = objectPoolItem.objectToPool.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = MTDAssetLoader.cuteBrainMonsterSprite;
                }
            }
            return true;
        }
    }
}