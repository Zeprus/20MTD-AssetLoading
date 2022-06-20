using System;
using System.Collections;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using HarmonyLib;

namespace MTDAssetLoading
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class MTDAssetLoader : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        internal static AssetBundle customAssetBundle;
        internal static Texture2D cuteBrainMonsterTexture;
        internal static Sprite cuteBrainMonsterSprite;

        private void Awake()
        {
            // Plugin startup logic
            Log = base.Logger;

            //sync assets loading
            //load asset bundle
            customAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "testAssetBundle"));
            foreach (String name in customAssetBundle.GetAllAssetNames())
            {
                Log.LogInfo("Found Asset: " + name);
            }
            //obtain our asset from the assetbundle
            UnityEngine.Object loadedAsset = customAssetBundle.LoadAsset("assets/cutebrainmonster.png");
            Log.LogInfo("Loaded Asset: " + loadedAsset.name + " with type " + loadedAsset.GetType());
            cuteBrainMonsterTexture = (Texture2D)loadedAsset;
            cuteBrainMonsterSprite = Sprite.Create(cuteBrainMonsterTexture, new Rect(0, 0, cuteBrainMonsterTexture.width, cuteBrainMonsterTexture.height), new Vector2(0.5f, 0.5f), 32);
            cuteBrainMonsterSprite.name = "CuteBrainMonster";

            try
            {
                Harmony.CreateAndPatchAll(typeof(SpawnHandler));
            }
            catch
            {
                Logger.LogError($"{PluginInfo.PLUGIN_GUID} failed to patch methods.");
            }

            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        //async asset loading example
        IEnumerator loadAssetBundleAsync(String fileName)
        {
            String filePath = Path.Combine(Application.streamingAssetsPath, fileName);
            AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(filePath);
            yield return assetBundleCreateRequest;

            AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;

            if (assetBundle == null)
            {
                Log.LogError("Failed to load assetbundle " + filePath + ". Please make sure the file exists and is a valid AssetBundle.");
                yield break;
            }
            else
            {
                customAssetBundle = assetBundle;

                foreach (String name in customAssetBundle.GetAllAssetNames())
                {
                    Log.LogInfo("Found Asset: " + name);
                }

                AssetBundleRequest assetBundleRequest = customAssetBundle.LoadAssetAsync("assets/brainmonster.png");
                yield return assetBundleRequest;
                UnityEngine.Object loadedAsset = assetBundleRequest.asset;
                Log.LogInfo("Loaded Asset: " + loadedAsset.name + " with type " + loadedAsset.GetType());
                cuteBrainMonsterTexture = (Texture2D)loadedAsset;
                cuteBrainMonsterSprite = Sprite.Create(cuteBrainMonsterTexture, new Rect(0, 0, cuteBrainMonsterTexture.width, cuteBrainMonsterTexture.height), new Vector2(0.5f, 0.5f), 32);
                cuteBrainMonsterSprite.name = "CuteBrainMonster";
            }

        }
        private void OnDestroy()
        {
        }
    }
}
