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
        private const int FireworkAmount = 10;
        private static readonly string[] FireworkIds = {"893", "894", "895"}; // Red, Purple, Green
        private static readonly string[] SongNames = {
            "CountingStars",
            "Firework",
            "IGottaFeeling",
            "ILoveIt",
            "SafeAndSound",
            "Summer",
            "Umbrella"
        };
        
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
            // 2300 is 11PM
            if (e.NewTime == 2300 && Context.IsWorldReady)
            {
                // Stop any playing music
                Game1.changeMusicTrack("");

                // Play custom song
                Game1.soundBank?.GetCue(GetRandomSongName())?.Play();
                
                AddFireworksToInventory();
            }
        }
        
        private void AddFireworksToInventory()
        {
            foreach (string fireworkId in FireworkIds)
            {
                AddItemToInventory(fireworkId, FireworkAmount);
            }
        }
        
        private void AddItemToInventory(string itemId, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                Item item = new StardewValley.Object(itemId, 1);
                Game1.player.addItemToInventory(item);
            }
        }

        private string GetRandomSongName()
        {
            Random random = new();
            int index = random.Next(SongNames.Length);
            return SongNames[index];
        }
    }
}