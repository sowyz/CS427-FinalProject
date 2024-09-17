using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteZombies
{
    interface IInteractable
    {
        void Interact();
    }

    public class Interactor : MonoBehaviour
    {
        #region FIELDS
        public float interactionDistance = 2.0f;
        public Transform interactorSource;
        private IInteractable currentInteractable; // Cached interactable
        private CharacterBehaviour playerCharacter;
        #endregion

        #region UNITY

        private void Awake()
        {
            playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        }
        private void Update()
        {
            CheckForInteractable();
            
            if (currentInteractable != null && playerCharacter.IsInteracting())
            {
                currentInteractable.Interact();
            }
        }

        private void CheckForInteractable()
        {
            Ray ray = new Ray(interactorSource.position, interactorSource.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                // Only update if the interactable is different
                if (interactable != currentInteractable)
                {
                    currentInteractable = interactable;
                }
            }
            else
            {
                currentInteractable = null; // No interactable in range
            }
        }
        #endregion
    }
}