using System;
using System.Collections.Generic;

namespace FF3Manip
{
    public class ManipList
    {
        private Dictionary<ManipNames, Func<Manip>> manipNameToFunc;
        public ManipList()
        {
            manipNameToFunc = new Dictionary<ManipNames, Func<Manip>>
            {
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
            NewGame,
            RainbowPudding,
            Octomammoth,
            SafeTravel,
            PinkTail,
            MysidiaOrdealsZot,
            LugaeBabilRubi,
        }

        public Manip NewGame()
        {
            return new Manip(ManipController.TimeZones.CEST, 24, 10, 2021, 16, 20, 00);
        }

        public Manip RainbowPudding()
        {
            return new Manip(ManipController.TimeZones.CEST, 24, 9, 2021, 16, 19, 55);
        }
        
        public Manip Octomammoth()
        {
            return new Manip(ManipController.TimeZones.CEST, 24, 04, 2021, 16, 20, 03);
        }
        
        public Manip SafeTravel()
        {
            return new Manip(ManipController.TimeZones.CEST, 11, 05, 2021, 16, 45, 00);
        }
        
        public Manip PinkTail()
        {
            return new Manip(ManipController.TimeZones.CEST, 24, 04, 2021, 16, 20, 27);
        }
        
        public Manip MysidiaOrdealsZot()
        {
            return new Manip(ManipController.TimeZones.CEST, 24, 04, 2021, 16, 20, 08);
        }
        
        public Manip LugaeBabilRubi()
        {
            return new Manip(ManipController.TimeZones.CEST, 24, 04, 2021, 16, 20, 17);
        }
        
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