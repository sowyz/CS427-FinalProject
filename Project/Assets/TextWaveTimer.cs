// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Globalization;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Current Ammunition Text.
    /// </summary>
    public class TextWaveTimer : ElementText
    {
        #region METHODS
        
        /// <summary>
        /// Tick.
        /// </summary>
        protected override void Tick()
        {
            if(enemySpawner.isCoolingDown)
            {
                //Update Text.
                textMesh.text = "NEXT WAVE IN\n" + enemySpawner.coolDownTimer.ToString("n0");
            }
            else
            {
                textMesh.text = "Wave: " + enemySpawner.currentWave;
            }
            
        }
        
        #endregion
    }
}