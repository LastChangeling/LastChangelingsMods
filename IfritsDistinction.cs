using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using BepInEx;
using ItemLib;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace IfritsDistinction
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency(ItemLibPlugin.ModGuid)]
    [BepInPlugin("dev.LastChangeling.IfritsDistinction", "Ifrits Distinction", "0.0.1")]
    public class IfritsDistinction : BaseUnityPlugin
    {
        private const string ModVer = "0.0.1";
        private const string ModName = "Ifrits Distinction";
        public const string ModGuid = "dev.LastChangeling.IfritsDistinction";

        private static ItemDisplayRule[] _itemDisplayRules;

        public void Awake()
        {
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) =>
            {
                if (self.inventory.GetItemCount((ItemIndex)ItemLib.ItemLib.GetItemId("Ifrits Distinction")) > 0)
                {
                    self.AddBuff(BuffIndex.AffixRed);
                }
                else if (self.HasBuff(BuffIndex.AffixRed) && self.inventory.currentEquipmentIndex != EquipmentIndex.AffixRed)
                {
                    self.RemoveBuff(BuffIndex.AffixRed);
                }
            };

            On.RoR2.GlobalEventManager.OnHitEnemy += (orig, self, damageInfo, victim) =>
            {
                orig(self, damageInfo, victim);
                Debug.Log("Test");
                int count = damageInfo.attacker.GetComponent<CharacterBody>().inventory.GetItemCount((ItemIndex)ItemLib.ItemLib.GetItemId("Ifrits Distinction"));
                if (count > 1)
                {
                    bool flag5 = (damageInfo.damageType & DamageType.PercentIgniteOnHit) 
                    != DamageType.Generic || damageInfo.attacker.GetComponent<CharacterBody>().HasBuff(BuffIndex.AffixRed);
                    CharacterBody bleck = victim.GetComponent<CharacterBody>();
                    for (int i = count - 1; i > 0; i--)
                    {
                        DotController.InflictDot(victim, damageInfo.attacker, flag5 ? DotController.DotIndex.PercentBurn : DotController.DotIndex.Burn, 4f * damageInfo.procCoefficient, 1f);
                    }
                }
            };
        }

        [Item(ItemAttribute.ItemType.Item)]
        public static ItemLib.CustomItem ifritsDistinction()
        {
            ItemDef ifritsDistinction = new ItemDef
            {
                tier = ItemTier.Tier3,
                pickupModelPath = "Prefabs/PickupModels/PickupAffixRed",
                pickupIconPath = "Textures/ItemIcons/texAffixRedIcon",
                nameToken = "Ifrits Distinction",
                pickupToken = "Makes you an aspect of fire",
                descriptionToken = "Burn, baby burn, disco inferno!"
            };
            _itemDisplayRules = null;

            return new ItemLib.CustomItem(ifritsDistinction, Resources.Load<GameObject>(ifritsDistinction.pickupModelPath), Resources.Load<Texture>(ifritsDistinction.pickupIconPath), _itemDisplayRules);
        }
    }
}
