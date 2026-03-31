using NUnit.Framework;
using UnityEngine;
using BatuhanAri.InventorySystem.Core;
using BatuhanAri.InventorySystem.Data;
using BatuhanAri.InventorySystem.Inventory;
using BatuhanAri.InventorySystem.Equipment;

namespace BatuhanAri.InventorySystem.Tests
{
    public class InventoryLogicTests
    {
        private InventoryManager inventoryManager;
        private ItemData mockConsumable;
        private ItemData mockEquipment;
        private GameObject equipGo;

        [SetUp]
        public void Setup()
        {
            // Create a fake GameObject and attach the manager
            var go = new GameObject("InventorySystem");
            inventoryManager = go.AddComponent<InventoryManager>();
            inventoryManager.InitializeInventory(10); // 10 slots

            // Create mock ScriptableObjects
            mockConsumable = ScriptableObject.CreateInstance<ItemData>();
            mockConsumable.Name = "Health Potion";
            mockConsumable.ItemType = ItemType.Consumable;
            mockConsumable.MaxStack = 5;

            mockEquipment = ScriptableObject.CreateInstance<ItemData>();
            mockEquipment.Name = "Iron Sword";
            mockEquipment.ItemType = ItemType.Equipment;
            mockEquipment.EquipmentType = EquipmentType.Weapon;
            mockEquipment.MaxStack = 1;
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(inventoryManager.gameObject);
            Object.DestroyImmediate(mockConsumable);
            Object.DestroyImmediate(mockEquipment);
            if (equipGo != null) Object.DestroyImmediate(equipGo);
        }

        [Test]
        public void AddItem_WhenEmpty_ReturnsTrue()
        {
            bool result = inventoryManager.AddItem(mockConsumable, 1);
            Assert.IsTrue(result);
            Assert.AreEqual(1, inventoryManager.GetItemCount(mockConsumable));
        }

        [Test]
        public void AddItem_ExceedsMaxStack_SplitsAcrossMultipleSlots()
        {
            // Adding 7 items, MaxStack is 5. Should take 2 slots (5 + 2)
            bool result = inventoryManager.AddItem(mockConsumable, 7);
            
            Assert.IsTrue(result);
            Assert.AreEqual(7, inventoryManager.GetItemCount(mockConsumable));

            var slots = inventoryManager.GetAllSlots();
            Assert.IsNotNull(slots[0].Item, "Slot 0'da item olmalı");
            Assert.AreEqual(5, slots[0].Item.CurrentStack);
            Assert.IsNotNull(slots[1].Item, "Slot 1'de item olmalı");
            Assert.AreEqual(2, slots[1].Item.CurrentStack);
            Assert.IsTrue(slots[2].IsEmpty);
        }

        [Test]
        public void RemoveItem_WhenAvailable_DecreasesCountCorrectly()
        {
            inventoryManager.AddItem(mockConsumable, 3);
            bool result = inventoryManager.RemoveItem(mockConsumable, 2);

            Assert.IsTrue(result);
            Assert.AreEqual(1, inventoryManager.GetItemCount(mockConsumable));
        }

        [Test]
        public void RemoveItem_WhenNotEnoughItems_ReturnsFalseAndKeepsState()
        {
            inventoryManager.AddItem(mockConsumable, 2);
            bool result = inventoryManager.RemoveItem(mockConsumable, 5);

            Assert.IsFalse(result); // Cannot remove 5 if we only have 2 in the current design
            Assert.AreEqual(2, inventoryManager.GetItemCount(mockConsumable));
        }

        [Test]
        public void EquipItem_ValidItem_EquipsSuccessfully()
        {
            equipGo = new GameObject("EquipmentManager");
            var equipmentManager = equipGo.AddComponent<EquipmentManager>();

            inventoryManager.AddItem(mockEquipment, 1);
            var inventoryItem = inventoryManager.GetAllSlots()[0].Item;

            bool result = equipmentManager.Equip(inventoryItem, inventoryManager);

            Assert.IsTrue(result);
            Assert.AreEqual(0, inventoryManager.GetItemCount(mockEquipment)); // Removed from inventory
            Assert.AreEqual(mockEquipment, equipmentManager.GetSlot(EquipmentType.Weapon).CurrentItem.Data); // Inside equipment slot
        }
    }
}

