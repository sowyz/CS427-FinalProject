using UnityEngine;

namespace InfiniteZombies.Interface
{
    /// <summary>
    /// Player Interface.
    /// </summary>
    public class CanvasSpawner : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Settings")]
        
        [Tooltip("Canvas prefab spawned at start. Displays the player's user interface.")]
        [SerializeField]
        private GameObject canvasPrefab;

        #endregion

        #region UNITY FUNCTIONS

        /// <summary>
        /// Awake.
        /// </summary>
        private void Start()
        {
            //Spawn Interface.
            GameObject canvas = Instantiate(canvasPrefab);
            GlobalReference.Instance.playerUI = canvas;
        }

        #endregion
    }
}