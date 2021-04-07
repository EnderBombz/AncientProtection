using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace AncientProtection
{
    [BepInPlugin("EnderBombz.AncientProtection", "AncientProtection", "1.0.0")]
    [BepInProcess("valheim.exe")]

    public class AncientProtection : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("EnderBombz.AncientProtection");
        public static List<BossLocation> bossList;

        void Awake()
        {
            bossList = new List<BossLocation>
            {
                new BossLocation { Name = "eikthyrnir"},
                new BossLocation { Name = "gdking"},
                new BossLocation { Name = "bonemass"},
                new BossLocation { Name = "dragonqueen"},
                new BossLocation { Name = "goblinking"},
            };

            harmony.PatchAll();
        }

     
        public class BossLocation
        {
            public string Name { get; set; }
        }

        [HarmonyPatch(typeof(ZNet), "Start")]

        class AncientProtection_Patch
        {

            public static bool BossLocation(string altarName)
            {
                foreach(var boss in bossList)
                {
                    if (altarName.Contains(boss.Name))
                    {
                        return true;
                    }
                }
                return false;
            }

            public static void Prefix()
            {

                Debug.Log("AncientProtection_Patch: Start debug");
                foreach (KeyValuePair<Vector2i, ZoneSystem.LocationInstance> keyValuePair in ZoneSystem.instance.m_locationInstances)
                {
                    if (keyValuePair.Value.m_location.m_prefabName.ToLower().Contains("stonecircle")|| BossLocation(keyValuePair.Value.m_location.m_prefabName.ToLower()))
                    {
                        Vector3 secure = new Vector3(keyValuePair.Value.m_position.x, keyValuePair.Value.m_position.y - 5, keyValuePair.Value.m_position.z);

                        GameObject prefab = ZNetScene.instance.GetPrefab("guard_stone");
                        GameObject prefP = Instantiate(prefab, secure, Quaternion.identity);
                        prefP.GetComponent<PrivateArea>().SetEnabled(true);
                        prefP.GetComponent<PrivateArea>().m_radius = 100f;
                    }
                }
                Debug.Log("AncientProtection_Patch: Finish debug");



            }
        }



    }
}