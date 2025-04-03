using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace SafeAndSound
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
         ** Public methods
         *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;
        }


        /*********
         ** Private methods
         *********/
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnTimeChanged(object? sender, TimeChangedEventArgs e)
        {
            if (e.NewTime == 2300 && Context.IsWorldReady)
            {
                // Stop any playing music
                Game1.changeMusicTrack("");

                // Play Safe and Sound
                Game1.soundBank?.GetCue("Melzr.SafeAndSound_Music")?.Play();
                
                AddFireworksToInventory();
            }
        }
        
        private void AddFireworksToInventory()
        {
            string redFireworkId = "893";
            string purpleFireworkId = "894";
            string greenFireworkId = "895";
            
            AddItemToInventory(redFireworkId, 5);
            AddItemToInventory(purpleFireworkId, 5);
            AddItemToInventory(greenFireworkId, 5);
        }
        
        private void AddItemToInventory(string itemId, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                Item item = new StardewValley.Object(itemId, 1);
                Game1.player.addItemToInventory(item);
            }
        }
    }
}