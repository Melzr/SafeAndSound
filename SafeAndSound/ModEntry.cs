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
        private const int FireworkAmount = 8;
        private static readonly string[] FireworkIds = {"893", "894", "895"}; // Red, Purple, Green
        private static readonly string[] SongNames = {
            "BadRomance",
            "CallMeMaybe",
            "CountingStars",
            "FeelThisMoment",
            "Firework",
            "HotNCold",
            "IGottaFeeling",
            "ILoveIt",
            "JustDance",
            "SafeAndSound",
            "Summer",
            "TeenageDream",
            "Umbrella"
        };
        
        private readonly PerScreen<string> _currentSongId = new PerScreen<string>();
        
        /*********
         ** Public methods
         *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;
            helper.Events.Multiplayer.ModMessageReceived += OnModMessageReceived;
        }


        /*********
         ** Private methods
         *********/
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnTimeChanged(object? sender, TimeChangedEventArgs e)
        {
            // 2250 is 10:50PM
            if (e.NewTime == 2250 && Context.IsWorldReady)
            {
                SetRandomSongId();
                return;
            }
            
            // 2300 is 11:00PM
            if (e.NewTime == 2300 && Context.IsWorldReady)
            {
                string songId = this._currentSongId.Value;
                if (string.IsNullOrEmpty(songId))
                {
                    this.Monitor.Log($"Song ID not received.", LogLevel.Warn);
                    return;
                }
                
                // Stop any playing music
                Game1.changeMusicTrack("");

                // Play custom song
                Game1.soundBank?.GetCue(songId)?.Play();
                
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

        /// <summary>
        /// Sets a random song ID for the main player and broadcasts it
        /// so that the song is synchronized for all players.
        /// </summary>
        private void SetRandomSongId()
        {
            if (!Context.IsMainPlayer)
                return;
            
            Random random = new();
            int index = random.Next(SongNames.Length);
            string selectedSong = SongNames[index];
            
            this._currentSongId.Value = selectedSong;

            this.Helper.Multiplayer.SendMessage(
                selectedSong,
                "SetSongId",
                modIDs: new[] { this.ModManifest.UniqueID }
            );
        }
        
        private void OnModMessageReceived(object? sender, ModMessageReceivedEventArgs e)
        {
            if (e.Type == "SetSongId" && e.FromModID == this.ModManifest.UniqueID)
            {
                string? song = e.ReadAs<string>();
                if (!string.IsNullOrEmpty(song))
                    this._currentSongId.Value = song;
            }
        }
    }
}