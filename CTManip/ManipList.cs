﻿using System;
using System.Collections.Generic;

namespace CTManip
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
                {ManipNames.SealedCave, SealedCave},
                {ManipNames.DragonsPeak, DragonsPeak},
                {ManipNames.TozusTunnel, TozusTunnel},
                {ManipNames.ToTowerOfOwen, ToTowerOfOwen},
                {ManipNames.TowenOfOwen, TowenOfOwen},
                {ManipNames.SubterraneanLake, SubterraneanLake},
                {ManipNames.MoltenCave, MoltenCave},
                {ManipNames.HeinCastle, HeinCastle},
                {ManipNames.CaveOfTides, CaveOfTides},
                {ManipNames.Sewers, Sewers},
                {ManipNames.ChocoboWrath, ChocoboWrath},
                {ManipNames.GoldorManor, GoldorManor},
                {ManipNames.Garuda, Garuda},
                {ManipNames.CaveOfTheCircle, CaveOfTheCircle},
                {ManipNames.SaroniaCatacombs, SaroniaCatacombs},
                {ManipNames.AncientsMaze, AncientsMaze},
                {ManipNames.CaveOfShadows, CaveOfShadows},
                {ManipNames.ShiningCurtain, ShiningCurtain},
                {ManipNames.DogasGrotto, DogasGrotto},
                {ManipNames.ToXande, ToXande},
                {ManipNames.WorldOfDarkness, WorldOfDarkness},
                {ManipNames.CloudOfDarkness, CloudOfDarkness},
                
                
                // FF4
                {ManipNames.NewGame, NewGame},
                {ManipNames.Octomammoth, Octomammoth},
                {ManipNames.MysidiaOrdeals, MysidiaOrdeals},
                {ManipNames.RainbowPudding, RainbowPudding},
                {ManipNames.Underworld, Underworld },
                {ManipNames.Lugae, Lugae },
                {ManipNames.BabilRubi, BabilRubi },
                {ManipNames.SealedCaveFF4, SealedCaveFF4 }, 
                {ManipNames.SafeTravel, SafeTravel},
                {ManipNames.DragonOneCycle, DragonOneCycle },
                {ManipNames.PinkTail, PinkTail},
                
                
                
            };
        }
        
        public enum ManipNames 
        {
            // FF3
            AltarCave,
            SealedCave,
            DragonsPeak,
            TozusTunnel,
            ToTowerOfOwen,
            TowenOfOwen,
            SubterraneanLake,
            MoltenCave,
            HeinCastle,
            CaveOfTides,
            Sewers,
            ChocoboWrath,
            GoldorManor,
            Garuda,
            CaveOfTheCircle,
            SaroniaCatacombs,
            AncientsMaze,
            CaveOfShadows,
            ShiningCurtain,
            DogasGrotto,
            ToXande,
            WorldOfDarkness,
            CloudOfDarkness,
            
            // FF4
            NewGame,
            Octomammoth,
            MysidiaOrdeals,
            RainbowPudding,
            Underworld,
            Lugae,
            BabilRubi,
            SealedCaveFF4,
            SafeTravel,
            DragonOneCycle,
            PinkTail,

        }
        
        // FF3
        private Manip AltarCave() { return new Manip(ManipController.TimeZones.GMT, 10, 04, 21, 19, 43, 17); }
        private Manip SealedCave() { return new Manip(ManipController.TimeZones.CEST, 02, 05, 24, 09, 48, 08); }
        private Manip DragonsPeak() { return new Manip(ManipController.TimeZones.CEST, 02, 05, 24, 11, 42, 36); }
        private Manip TozusTunnel() { return new Manip(ManipController.TimeZones.CEST, 02, 05, 24, 11, 57, 50); }
        private Manip ToTowerOfOwen() { return new Manip(ManipController.TimeZones.CEST, 02, 05, 24, 13, 15, 41); }
        private Manip TowenOfOwen() { return new Manip(ManipController.TimeZones.CEST, 17, 02, 24, 10, 00, 07); }
        private Manip SubterraneanLake() { return new Manip(ManipController.TimeZones.CEST, 30, 04, 24, 13, 01, 56); }
        private Manip MoltenCave() { return new Manip(ManipController.TimeZones.CEST, 21, 02, 24, 12, 51, 32); }
        private Manip HeinCastle() { return new Manip(ManipController.TimeZones.CEST, 11, 02, 24, 13, 10, 42); }
        private Manip CaveOfTides() { return new Manip(ManipController.TimeZones.CEST, 02, 05, 24, 18, 14, 42); }
        private Manip Sewers() { return new Manip(ManipController.TimeZones.CEST, 03, 05, 24, 12, 02, 16); }
        private Manip ChocoboWrath() { return new Manip(ManipController.TimeZones.CEST, 14, 10, 23, 11, 04, 04); }
        private Manip GoldorManor() { return new Manip(ManipController.TimeZones.CEST, 05, 05, 24, 10, 21, 39); }
        private Manip Garuda() { return new Manip(ManipController.TimeZones.CEST, 15, 10, 23, 12, 37, 19); }
        private Manip CaveOfTheCircle() { return new Manip(ManipController.TimeZones.CEST, 15, 10, 23, 13, 07, 36); }
        private Manip SaroniaCatacombs() { return new Manip(ManipController.TimeZones.CEST, 05, 05, 24, 13, 08, 20); }
        private Manip AncientsMaze() { return new Manip(ManipController.TimeZones.CEST, 05, 04, 24, 19, 35, 15); }
        private Manip CaveOfShadows() { return new Manip(ManipController.TimeZones.CEST, 23, 01, 24, 12, 07, 14); }
        private Manip ShiningCurtain() { return new Manip(ManipController.TimeZones.CEST, 01, 04, 24, 18, 30, 13); }
        private Manip DogasGrotto() { return new Manip(ManipController.TimeZones.CEST, 16, 10, 23, 10, 50, 57); }
        private Manip ToXande() { return new Manip(ManipController.TimeZones.CEST, 21, 10, 23, 21, 35, 31); }
        private Manip WorldOfDarkness() { return new Manip(ManipController.TimeZones.CEST, 28, 10, 23, 14, 58, 25); }
        private Manip CloudOfDarkness() { return new Manip(ManipController.TimeZones.CEST, 04, 04, 24, 12, 36, 49); }

        // FF4
        private Manip NewGame() { return new Manip(ManipController.TimeZones.CEST, 24, 10, 2021, 16, 20, 00); }
        private Manip Octomammoth() { return new Manip(ManipController.TimeZones.CEST, 15, 03, 25, 14, 09, 00); }
        private Manip MysidiaOrdeals() { return new Manip(ManipController.TimeZones.CEST, 24, 04, 2021, 16, 20, 08); }
        private Manip RainbowPudding() { return new Manip(ManipController.TimeZones.CEST, 29, 03, 25, 12, 16, 31); }
        private Manip Underworld() { return new Manip(ManipController.TimeZones.CEST, 29, 05, 25, 19, 24, 00); }
        private Manip Lugae() { return new Manip(ManipController.TimeZones.CEST, 01, 03, 25, 23, 31, 31); }
        private Manip BabilRubi() { return new Manip(ManipController.TimeZones.CEST, 24, 04, 2021, 16, 20, 17); }
        private Manip SealedCaveFF4() {return new Manip(ManipController.TimeZones.CEST, 09, 06, 25, 13, 33, 26); }
        private Manip SafeTravel() { return new Manip(ManipController.TimeZones.CEST, 11, 05, 2021, 16, 45, 00); }
        private Manip DragonOneCycle() { return new Manip(ManipController.TimeZones.CEST, 25, 05, 25, 11, 55, 22); }
        private Manip PinkTail() { return new Manip(ManipController.TimeZones.CEST, 24, 04, 2021, 16, 20, 27); }
    }
}