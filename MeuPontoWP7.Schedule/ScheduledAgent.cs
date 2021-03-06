﻿using System;
using System.Linq;
using System.Windows;
using MeuPonto.Common.Repositorios;
using Microsoft.Devices;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;

namespace MeuPontoWP7.Schedule
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: Add code to perform your task in background
            // Launch a toast to show that the agent is running.
            // The toast will not be shown if the foreground application is running.
            var cache = new CacheContext();
            var batidasHoje = cache.Batidas.Where(b => b.Horario.Date == DateTime.Now.Date);

            var shellTileData = new StandardTileData
            {
                BackContent = string.Join("\n", batidasHoje.Select(x => x.Horario.ToShortTimeString())), 
                BackTitle = "Batidas hoje"
            };


            ShellTile appTile = ShellTile.ActiveTiles.First();

            appTile.Update(shellTileData);

            // Call NotifyComplete to let the system know the agent is done working.

            NotifyComplete();
        }
    }
}