﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Game;
using Sandbox.Engine;
using Sandbox.Engine.Multiplayer;
using HarmonyLib;
using SEDB_LITE;
using Sandbox.Game.Gui;
using VRage.Utils;
using static SEDB_LITE.PatchController;
using VRage.GameServices;
using System.Text.RegularExpressions;

namespace SEDB_LITE.Patches {
    [PatchingClass]
    class PlayerLeftPatch {
        private static Plugin Plugin;
        public static MyLog Log = new MyLog();

        public PlayerLeftPatch(Plugin plugin) {
            Plugin = plugin;
        }

        [PrefixMethod]
        [TargetMethod(Type = typeof(MyDedicatedServerBase), Method = "OnDisconnectedClient")]
        public static void PlayerDisconnected(ref MyControlDisconnectedMsg data, ulong sender) {

            try {
                string playerName = Utilities.GetPlayerName(sender);
                if (!(playerName.StartsWith("[") && playerName.EndsWith("]") && playerName.Contains("...")))
                    Task.Run(async () => Plugin.ProcessStatusMessage(playerName, sender, Plugin.m_configuration.DisconnectedMessage));
            }
            catch (Exception e) {
                Log.WriteLineAndConsole(e.ToString());
            }
        }
    }
}
