using System;
using System.Collections.Generic;

namespace FF3Manip
{
    public class ManipList
    {
        private Dictionary<ManipNames, Func<Manip>> manipNameToFunc;
        
        public enum ManipNames 
        {
            AltarCave,
            LandTurtle,
            SealedCave,
            DragonsPeak,
            TowerOfOwen,
            TowenOfOwen2,
            SubterraneanLake,
            MoltenCave,
            HeinCastle,
            CaveOfTides,
            Sewers,
            Amur,
        }
        public ManipList()
        {
            manipNameToFunc = new Dictionary<ManipNames, Func<Manip>>
            {
                {ManipNames.AltarCave, AltarCave},
                {ManipNames.LandTurtle, LandTurtle},
                {ManipNames.SealedCave, SealedCave},
                {ManipNames.DragonsPeak, DragonsPeak},
                {ManipNames.TowerOfOwen, TowerOfOwen},
                {ManipNames.TowenOfOwen2, TowenOfOwen2},
                {ManipNames.SubterraneanLake, SubterraneanLake},
                {ManipNames.MoltenCave, MoltenCave},
                {ManipNames.HeinCastle, HeinCastle},
                {ManipNames.CaveOfTides, CaveOfTides},
                {ManipNames.Sewers, Sewers},
                {ManipNames.Amur, Amur},
            };
        }
        
        private Manip AltarCave() { return new Manip(ManipController.TimeZones.GMT, 10, 04, 21, 19, 43, 17); }
        private Manip LandTurtle() { return new Manip(ManipController.TimeZones.ET, 16, 10, 22, 16, 37, 01); }
        private Manip SealedCave() { return new Manip(ManipController.TimeZones.CEST, 10, 04, 21, 20, 43, 21); }
        private Manip DragonsPeak() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 24); }
        private Manip TowerOfOwen() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 29); }
        private Manip TowenOfOwen2() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 31); }
        private Manip SubterraneanLake() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 34); }
        private Manip MoltenCave() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 38); }
        private Manip HeinCastle() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 38); }
        private Manip CaveOfTides() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 43); }
        private Manip Sewers() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 44, 43); }
        private Manip Amur() { return new Manip(ManipController.TimeZones.CEST, 22, 06, 22, 11, 45, 43); }
        
        public Manip GetManipByValue(ManipNames name)
        {
            if (manipNameToFunc.TryGetValue(name, out var manipFunc))
            {
                return manipFunc();
            }
            throw new ArgumentException("Error, manip value not in list", nameof(name));
        }
    }
}