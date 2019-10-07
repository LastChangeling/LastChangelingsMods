using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using BepInEx;
using ItemLib;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace NkuhanasRetort
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency(ItemLibPlugin.ModGuid)]
    [BepInPlugin("dev.LastChangeling.NkuhanasRetort", "Nkuhanas Retort", "0.0.1")]
    public class NkuhanasRetort : BaseUnityPlugin
    {
        private const string ModVer = "0.0.1";
        private const string ModName = "Nkuhanas Retort";
        public const string ModGuid = "dev.LastChangeling.NkuhanasRetort";

        private static ItemDisplayRule[] _itemDisplayRules;

        public void Awake()
        {
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) =>
            {
                if (self.inventory.GetItemCount((ItemIndex)ItemLib.ItemLib.GetItemId("Nkuhanas Retort")) > 0)
                {
                    self.AddBuff(BuffIndex.AffixPoison);
                }
                else if (self.HasBuff(BuffIndex.AffixPoison) && self.inventory.currentEquipmentIndex != EquipmentIndex.AffixPoison)
                {
                    self.RemoveBuff(BuffIndex.AffixPoison);
                }
            };

            On.RoR2.CharacterBody.RecalculateStats += (orig, self) =>
            {
                orig(self);
                float newHP = self.maxHealth;
                int count = self.inventory.GetItemCount((ItemIndex)ItemLib.ItemLib.GetItemId("Nkuhanas Retort"));
                if (self.inventory && count > 0)
                {
                    newHP += 100 * count;
                }
                Debug.Log(newHP);
                self.SetPropertyValue<float>("maxHealth", newHP);
                Debug.Log(newHP);
            };
        }

        [Item(ItemAttribute.ItemType.Item)]
        public static ItemLib.CustomItem nkuhanasRetort()
        {
            ItemDef NkuhanasRetort = new ItemDef
            {
                tier = ItemTier.Tier3,
                pickupModelPath = "Prefabs/PickupModels/PickupAffixPoison",
                pickupIconPath = "Textures/ItemIcons/texAffixPoisonIcon",
                nameToken = "Nkuhanas Retort",
                pickupToken = "Makes you an aspect of poison",
                descriptionToken = "Nothin' But A Good Time"
            };
            _itemDisplayRules = null;

            return new ItemLib.CustomItem(NkuhanasRetort, Resources.Load<GameObject>(NkuhanasRetort.pickupModelPath), Resources.Load<Texture>(NkuhanasRetort.pickupIconPath), _itemDisplayRules);
        }
    }
}
