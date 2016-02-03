﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;   // Always needed
using RimWorld;      // RimWorld specific functions are found here
using Verse;         // RimWorld universal objects are here
//using Verse.AI;    // Needed when you do something with the AI
//using Verse.Sound; // Needed when you do something with the Sound

namespace OutpostGenerator
{
    /// <summary>
    /// RectTriggerIntrusion class.
    /// </summary>
    /// <author>Rikiki</author>
    /// <permission>Use this code as you want, just remember to add a link to the corresponding Ludeon forum mod release thread.
    /// Remember learning is always better than just copy/paste...</permission>
    public class TriggerIntrusion : Thing
    {
        public Building_OutpostCommandConsole commandConsole = null;
        public List<IntVec3> watchedCells = new List<IntVec3>();
        
        public override void Tick()
        {
            if (this.IsHashIntervalTick(60))
            {
                if (commandConsole == null)
                {
                    Log.Warning("M&Co. Outpost generator: triggerIntrusion is ticking with null commandConsole. This should not happen...");
                    if (base.Destroyed == false)
                    {
                        this.Destroy(DestroyMode.Vanish);
                    }
                }
                if (commandConsole.Destroyed)
                {
                    if (base.Destroyed == false)
                    {
                        this.Destroy(DestroyMode.Vanish);
                    }
                }
                foreach (IntVec3 cell in this.watchedCells)
                {
                    List<Thing> thingList = cell.GetThingList();
                    for (int thingIndex = 0; thingIndex < thingList.Count; thingIndex++)
                    {
                        if (thingList[thingIndex].def.category == ThingCategory.Pawn && thingList[thingIndex].Faction == Faction.OfColony)
                        {
                            commandConsole.TreatIntrusion(cell);
                            if (base.Destroyed == false)
                            {
                                this.Destroy(DestroyMode.Vanish);
                                return;
                            }
                        }
                    }
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_References.LookReference<Building_OutpostCommandConsole>(ref commandConsole, "commandConsole");
            Scribe_Collections.LookList<IntVec3>(ref this.watchedCells, "watchedCells", LookMode.Value);
        }
    }
}
