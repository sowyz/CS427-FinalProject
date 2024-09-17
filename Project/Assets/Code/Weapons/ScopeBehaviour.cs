using UnityEngine;

namespace InfiniteZombies
{
    /// <summary>
    /// Scope Behaviour.
    /// </summary>
    public abstract class ScopeBehaviour : MonoBehaviour
    {
        #region GETTERS

        /// <summary>
        /// Returns the Sprite used on the Character's Interface.
        /// </summary>
        public abstract Sprite GetSprite();

        #endregion
    }
}