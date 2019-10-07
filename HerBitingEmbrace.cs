using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using BepInEx;
using ItemLib;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace HerBitingEmbrace
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency(ItemLibPlugin.ModGuid)]
    [BepInPlugin("dev.LastChangeling.HerBitingEmbrace", "Her Biting Embrace", "0.0.1")]
    public class HerBitingEmbrace : BaseUnityPlugin
    {
        private const string ModVer = "0.0.1";
        private const string ModName = "Her Biting Embrace";
        public const string ModGuid = "dev.LastChangeling.HerBitingEmbrace";

        private static ItemDisplayRule[] _itemDisplayRules;

        public void Awake()
        {
            On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) =>
            {
                if (self.inventory.GetItemCount((ItemIndex)ItemLib.ItemLib.GetItemId("Her Biting Embrace")) > 0)
                {
                    self.AddBuff(BuffIndex.AffixWhite);
                }
                else if (self.HasBuff(BuffIndex.AffixWhite) && self.inventory.currentEquipmentIndex != EquipmentIndex.AffixWhite)
                {
                    self.RemoveBuff(BuffIndex.AffixWhite);
                }
            };

            On.RoR2.CharacterBody.RecalculateStats += (orig, self) =>
            {
                orig(self);
                float newArmor = self.armor;
                int count = self.inventory.GetItemCount((ItemIndex)ItemLib.ItemLib.GetItemId("Her Biting Embrace"));
                if (self.inventory && count > 0)
                {
                    Debug.Log(newArmor);
                    newArmor += 45 * count;
                }
                self.SetPropertyValue<float>("armor", newArmor);
            };
        }

        [Item(ItemAttribute.ItemType.Item)]
        public static ItemLib.CustomItem herBitingEmbrace()
        {
            ItemDef herBitingEmbrace = new ItemDef
            {
                tier = ItemTier.Tier3,
                pickupModelPath = "Prefabs/PickupModels/PickupAffixWhite",
                pickupIconPath = "Textures/ItemIcons/texAffixWhiteIcon",
                nameToken = "Her Biting Embrace",
                pickupToken = "Makes you an aspect of ice",
                descriptionToken = "Ice ice baby"
            };
            _itemDisplayRules = null;

            return new ItemLib.CustomItem(herBitingEmbrace, Resources.Load<GameObject>(herBitingEmbrace.pickupModelPath), Resources.Load<Texture>(herBitingEmbrace.pickupIconPath), _itemDisplayRules);
        }
    }
}
