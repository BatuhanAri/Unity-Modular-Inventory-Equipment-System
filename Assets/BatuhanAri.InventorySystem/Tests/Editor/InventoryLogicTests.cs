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

        [Test]
        public void AddItem_WithNullItemData_ReturnsFalse()
        {
            bool result = inventoryManager.AddItem(null, 1);
            Assert.IsFalse(result, "Adding null item should return false");
        }

        [Test]
        public void AddItem_WithZeroQuantity_ReturnsFalse()
        {
            bool result = inventoryManager.AddItem(mockConsumable, 0);
            Assert.IsFalse(result, "Adding zero quantity should return false");
        }

        [Test]
        public void AddItem_WithNegativeQuantity_ReturnsFalse()
        {
            bool result = inventoryManager.AddItem(mockConsumable, -5);
            Assert.IsFalse(result, "Adding negative quantity should return false");
        }

        [Test]
        public void AddItem_InventoryFull_ReturnsFalse()
        {
            // Fill inventory with max items
            for (int i = 0; i < 10; i++)
            {
                inventoryManager.AddItem(mockConsumable, 1);
            }

            // Try to add more
            var newItem = ScriptableObject.CreateInstance<ItemData>();
            newItem.Name = "Extra Item";
            newItem.ItemType = ItemType.Consumable;
            newItem.MaxStack = 1;

            bool result = inventoryManager.AddItem(newItem, 1);
            Assert.IsFalse(result, "Inventory is full, cannot add more items");
            
            Object.DestroyImmediate(newItem);
        }

        [Test]
        public void RemoveItem_WithNullItemData_ReturnsFalse()
        {
            bool result = inventoryManager.RemoveItem(null, 1);
            Assert.IsFalse(result, "Removing null item should return false");
        }

        [Test]
        public void RemoveItem_WithZeroQuantity_ReturnsFalse()
        {
            inventoryManager.AddItem(mockConsumable, 3);
            bool result = inventoryManager.RemoveItem(mockConsumable, 0);
            Assert.IsFalse(result, "Removing zero quantity should return false");
        }

        [Test]
        public void GetItemCount_NonExistentItem_ReturnsZero()
        {
            var nonExistentItem = ScriptableObject.CreateInstance<ItemData>();
            nonExistentItem.Name = "Non Existent";
            nonExistentItem.ItemType = ItemType.Consumable;

            int count = inventoryManager.GetItemCount(nonExistentItem);
            Assert.AreEqual(0, count, "Non-existent item count should be zero");
            
            Object.DestroyImmediate(nonExistentItem);
        }

        [Test]
        public void AddItem_MultipleStacks_FillsCorrectly()
        {
            // Add 13 items when max stack is 5
            bool result = inventoryManager.AddItem(mockConsumable, 13);

            Assert.IsTrue(result);
            Assert.AreEqual(13, inventoryManager.GetItemCount(mockConsumable));

            var slots = inventoryManager.GetAllSlots();
            Assert.AreEqual(5, slots[0].Item.CurrentStack);
            Assert.AreEqual(5, slots[1].Item.CurrentStack);
            Assert.AreEqual(3, slots[2].Item.CurrentStack);
            Assert.IsTrue(slots[3].IsEmpty);
        }

        [Test]
        public void RemoveItem_FromMultipleSlots_RemovesCorrectly()
        {
            // Add 13 items (fills 3 slots: 5, 5, 3)
            inventoryManager.AddItem(mockConsumable, 13);

            // Remove 8 items (should empty first 2 slots partially)
            bool result = inventoryManager.RemoveItem(mockConsumable, 8);

            Assert.IsTrue(result);
            Assert.AreEqual(5, inventoryManager.GetItemCount(mockConsumable));

            var slots = inventoryManager.GetAllSlots();
            Assert.AreEqual(5, slots[2].Item.CurrentStack);
        }

        [Test]
        public void MixedItemTypes_InventoryHandlesCorrectly()
        {
            // Add consumable and equipment
            inventoryManager.AddItem(mockConsumable, 3);
            inventoryManager.AddItem(mockEquipment, 1);

            Assert.AreEqual(3, inventoryManager.GetItemCount(mockConsumable));
            Assert.AreEqual(1, inventoryManager.GetItemCount(mockEquipment));

            var slots = inventoryManager.GetAllSlots();
            Assert.IsNotNull(slots[0].Item);
            Assert.IsNotNull(slots[1].Item);
        }

        [Test]
        public void RemoveItem_AllQuantity_ClearsSlot()
        {
            inventoryManager.AddItem(mockConsumable, 5);
            bool result = inventoryManager.RemoveItem(mockConsumable, 5);

            Assert.IsTrue(result);
            Assert.AreEqual(0, inventoryManager.GetItemCount(mockConsumable));

            var slots = inventoryManager.GetAllSlots();
            Assert.IsTrue(slots[0].IsEmpty);
        }

        [Test]
        public void InventoryInitialization_CorrectSlotCount()
        {
            var newGo = new GameObject("TestInventory");
            var newManager = newGo.AddComponent<InventoryManager>();
            newManager.InitializeInventory(15);

            var slots = newManager.GetAllSlots();
            Assert.AreEqual(15, slots.Count, "Inventory should have 15 slots");

            Object.DestroyImmediate(newGo);
        }

        [Test]
        public void EquipItem_RemovesFromInventoryCorrectly()
        {
            equipGo = new GameObject("EquipmentManager");
            var equipmentManager = equipGo.AddComponent<EquipmentManager>();

            inventoryManager.AddItem(mockEquipment, 1);
            var inventoryItem = inventoryManager.GetAllSlots()[0].Item;

            equipmentManager.Equip(inventoryItem, inventoryManager);

            // Verify removed from inventory
            Assert.AreEqual(0, inventoryManager.GetItemCount(mockEquipment));
        }
    }
}

