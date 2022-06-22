using HarmonyLib;
using flanne;
using UnityEngine;
using UnityEngine.UI;

namespace MTDAssetLoading
{
    /*
        Some objects are always pooled within the Awake function and therefore have to be accessed in its prefix. See console output for the log generated in Line #20 
        If the pooled object is not listed here that means it's pooled using ObjectPooler.AddObject and can be accessed by hooking that function.
        Other objects aren't pooled at all and expose their SpriteRenderer by themselves. Such as Pickups.ChestPickup, PlayerController and PlayerState.
    */
    static class PoolHandler
    {
        [HarmonyPatch(typeof(ObjectPooler), "Awake")]
        [HarmonyPrefix]
        static bool AwakePrefix(ref ObjectPooler __instance)
        {
            foreach (ObjectPoolItem objectPoolItem in __instance.itemsToPool)
            {
                MTDAssetLoader.Log.LogInfo("Item to pool: " + objectPoolItem.tag + " " + objectPoolItem.amountToPool + " " + objectPoolItem.shouldExpand + " " + objectPoolItem.objectToPool.tag);
                //swap Enemy "BrainMonster" texture
                if (objectPoolItem.tag == "BrainMonster")
                {
                    MTDAssetLoader.Log.LogInfo("Cutifying BrainMonster...");
                    SpriteRenderer spriteRenderer = objectPoolItem.objectToPool.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = MTDAssetLoader.cuteBrainMonsterSprite;
                }
                //change XP orb color
                if (objectPoolItem.tag == "SmallXP" || objectPoolItem.tag == "LargeXP")
                {
                    MTDAssetLoader.Log.LogInfo("Changing XP orb color...");
                    GameObject xpObject = objectPoolItem.objectToPool;
                    GameObject bounce = xpObject.transform.GetChild(0).gameObject;
                    SpriteRenderer spriteRenderer = bounce.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = new Color(251 / 255.0f, 72 / 255.0f, 196 / 255.0f, 1.0f); //pink
                }
            }
            return true;
        }
    }
}