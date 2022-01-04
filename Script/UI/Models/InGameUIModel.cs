using Script.Event;
using Script.InGame;
using Script.Manager;
using Script.Manager.Util.Log;
using Script.Util;
using System;
using System.Collections.Generic;

namespace Script.UI.Models
{
    public class InGameUIModel : Model
    {
        public PuzzlePlayerRing LeftPlayerRing;
        public PuzzlePlayerRing RightPlayerRing;

        public override void Initialize()
        {
            UIData = new UIData(UIType.MAIN, AddressableID.INGAME_UI);

            LeftPlayerRing = new PuzzlePlayerRing();
            RightPlayerRing = new PuzzlePlayerRing();
        }
    }
}