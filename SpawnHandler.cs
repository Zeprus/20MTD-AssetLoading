using HarmonyLib;

using flanne;
using UnityEngine;

namespace MTDAssetLoading
{
    static class SpawnHandler
    {
        [HarmonyPatch(typeof(HordeSpawner), "Spawn")]
        [HarmonyPrefix]
        static void SpawnPrefix(ref HordeSpawner __instance, string objectPoolTag, int HP, bool isElite, ObjectPooler ___OP)
        {
            //This doesn't work
            //TODO: Find a way to apply our sprite
            GameObject enemy = ___OP.GetPooledObject(objectPoolTag);
            SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
            Texture2D tex = MTDAssetLoader.cuteBrainMonsterTexture;
            spriteRenderer.sprite = MTDAssetLoader.cuteBrainMonsterSprite;
        }
    }
}