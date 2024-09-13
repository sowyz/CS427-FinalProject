// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Globalization;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Current Ammunition Text.
    /// </summary>
    public class TextHPAmount : ElementText
    {
        #region FIELDS SERIALIZED
        
        [Header("Colors")]
        
        [Tooltip("Determines if the color of the text should changes as HP changed.")]
        [SerializeField]
        private bool updateColor = true;
        
        [Tooltip("Determines how fast the color changes as the HP changed.")]
        [SerializeField]
        private float emptySpeed = 1.5f;
        
        [Tooltip("Color used on this text when the player character has no HP.")]
        [SerializeField]
        private Color emptyColor = Color.red;
        
        #endregion
        
        #region METHODS
        
        /// <summary>
        /// Tick.
        /// </summary>
        protected override void Tick()
        {
            //Current HP.
            float current = playerCharacter.GetHP();
            //Total HP.
            float total = playerCharacter.GetMaxHP();
            
            //Update Text.
            textMesh.text = current.ToString(CultureInfo.InvariantCulture);

            //Determine if we should update the text's color.
            if (updateColor)
            {
                //Calculate Color Alpha. Helpful to make the text color change based on count.
                float colorAlpha = (current / total) * emptySpeed;
                //Lerp Color. This makes sure that the text color changes based on count.
                textMesh.color = Color.Lerp(emptyColor, Color.white, colorAlpha);   
            }
        }
        
        #endregion
    }
}