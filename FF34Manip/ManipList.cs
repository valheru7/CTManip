using System;
using System.Collections.Generic;

namespace FF34Manip
{
    public class ManipList
    {
        private Dictionary<ManipNames, Func<Manip>> manipNameToFunc;

        public Manip GetManipByValue(ManipNames name)
        {
            if (manipNameToFunc.TryGetValue(name, out var manipFunc))
            {
                return manipFunc();
            }
            throw new ArgumentException("Error, manip value not in list", nameof(name));
        }
        
        public ManipList()
        {
            manipNameToFunc = new Dictionary<ManipNames, Func<Manip>>
            {
                // FF3
                {ManipNames.AltarCave, AltarCave},
                {ManipNames.LandTurtle, LandTurtle},
                {ManipNames.SealedCave, SealedCave},
                {ManipNames.DragonsPeak, DragonsPeak},
                {ManipNames.TozusTunnel, TozusTunnel},
                {ManipNames.TowerOfOwen, TowerOfOwen},
                {ManipNames.TowenOfOwen2, TowenOfOwen2},
                {ManipNames.Medusa, Medusa},
                {ManipNames.SubterraneanLake, SubterraneanLake},
                {ManipNames.MoltenCave, MoltenCave},
                {ManipNames.HeinCastle, HeinCastle},
                {ManipNames.CaveOfTides, CaveOfTides},
                {ManipNames.Sewers, Sewers},
                {ManipNames.Amur, Amur},
                
                // FF4
                {ManipNames.NewGame, NewGame},
                {ManipNames.RainbowPudding, RainbowPudding},
                {ManipNames.Octomammoth, Octomammoth},
                {ManipNames.SafeTravel, SafeTravel},
                {ManipNames.PinkTail, PinkTail},
                {ManipNames.MysidiaOrdealsZot, MysidiaOrdealsZot},
                {ManipNames.LugaeBabilRubi, LugaeBabilRubi}
                
            };
        }
        
        public enum ManipNames 
        {
            // FF3
            AltarCave,
            LandTurtle,
            SealedCave,
            DragonsPeak,
            TozusTunnel,
            TowerOfOwen,
            TowenOfOwen2,
            Medusa,
            SubterraneanLake,
            MoltenCave,
            HeinCastle,
            CaveOfTides,
            Sewers,
            Amur,
            
            // FF4
            NewGame,
            RainbowPudding,
            Octomammoth,
            SafeTravel,
            PinkTail,
            MysidiaOrdealsZot,
            LugaeBabilRubi
        }
        
        // FF3
        private Manip AltarCave() { return new Manip(ManipController.TimeZones.GMT, 10, 04, 21, 19, 43, 17); }
        private Manip LandTurtle() { return new Manip(ManipController.TimeZones.ET, 16, 10, 22, 16, 37, 01); }
        private Manip SealedCave() { return new Manip(ManipController.TimeZones.CEST, 10, 04, 21, 20, 43, 21); }
        private Manip DragonsPeak() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 24); }
        private Manip TozusTunnel() { return new Manip(ManipController.TimeZones.GMT, 12, 09, 23, 20, 22, 57); }
        // My ToO
        private Manip TowerOfOwen() { return new Manip(ManipController.TimeZones.GMT, 12, 09, 23, 23, 41, 03); }
        // Desch manip
        //private Manip TowerOfOwen() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 29); }
        
        // My ToO Inside
        private Manip TowenOfOwen2() { return new Manip(ManipController.TimeZones.GMT, 14, 09, 23, 20, 06, 07); }
        // Desch manip
        //private Manip TowenOfOwen2() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 31); }
        
        private Manip Medusa() { return new Manip(ManipController.TimeZones.GMT, 16, 09, 23, 17, 23, 08); }
        
        // My SubLake
        private Manip SubterraneanLake() { return new Manip(ManipController.TimeZones.GMT, 16, 09, 23, 21, 08, 02); }
        // Desch Manip
        //private Manip SubterraneanLake() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 34); }
        private Manip MoltenCave() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 38); }
        private Manip HeinCastle() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 38); }
        private Manip CaveOfTides() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 43); }
        private Manip Sewers() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 43); }

        private Manip Amur() { return new Manip(ManipController.TimeZones.CEST, 14, 10, 23, 11, 04, 04); }
        // Discarded pre-emptives
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 13, 10, 23, 00, 34, 09); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 10, 10, 23, 22, 00, 10); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 10, 10, 23, 20, 00, 55); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 10, 10, 23, 01, 23, 47); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 09, 10, 23, 19, 47, 39); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 08, 10, 23, 19, 53, 02); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 08, 10, 23, 13, 56, 54); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 08, 10, 23, 13, 15, 50); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 02, 10, 23, 20, 30, 37); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 01, 10, 23, 22, 36, 46); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 01, 10, 23, 21, 52, 33); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 30, 09, 23, 18, 37, 07); } 
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 30, 09, 23, 17, 43, 42); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 30, 09, 23, 16, 10, 29); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 27, 09, 23, 21, 21, 58); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 25, 09, 23, 22, 05, 38); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 25, 09, 23, 19, 54, 37); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 25, 09, 23, 00, 38, 16); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 24, 09, 23, 21, 18, 29); }
        //private Manip Amur() { return new Manip(ManipController.TimeZones.GMT, 21, 09, 23, 18, 55, 07); }
        // Sphie Manip
        //private Manip Amur() { return new Manip(ManipController.TimeZones.JST, 26, 06, 21, 13, 28, 29); }

        // FF4
        private Manip NewGame() { return new Manip(ManipController.TimeZones.CEST, 24, 10, 2021, 16, 20, 00); }
        private Manip RainbowPudding() { return new Manip(ManipController.TimeZones.CEST, 24, 9, 2021, 16, 19, 55); }
        private Manip Octomammoth() { return new Manip(ManipController.TimeZones.CEST, 24, 04, 2021, 16, 20, 03); }
        private Manip SafeTravel() { return new Manip(ManipController.TimeZones.CEST, 11, 05, 2021, 16, 45, 00); }
        private Manip PinkTail() { return new Manip(ManipController.TimeZones.CEST, 24, 04, 2021, 16, 20, 27); }
        private Manip MysidiaOrdealsZot() { return new Manip(ManipController.TimeZones.CEST, 24, 04, 2021, 16, 20, 08); }
        private Manip LugaeBabilRubi() { return new Manip(ManipController.TimeZones.CEST, 24, 04, 2021, 16, 20, 17); }
        
    }
}