using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteZombies
{
    public class AmmoBox : MonoBehaviour, IInteractable
    {
        private CharacterBehaviour playerCharacter;
        private InventoryBehaviour playerCharacterInventory;

        private void Awake()
        {
            playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
            playerCharacterInventory = playerCharacter.GetInventory();
        }

        public void Interact()
        {
            foreach (var weapon in playerCharacterInventory.GetWeapons())
            {
                weapon.PickUpFullAmmo();
            }
        }
    }
}