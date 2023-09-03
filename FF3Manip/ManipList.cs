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
                {ManipNames.AltarCave, AltarCave},
                {ManipNames.LandTurtle, LandTurtle},
                
            };
        }
        
        public enum ManipNames 
        {
            AltarCave,
            LandTurtle
            
        }

        public Manip AltarCave()
        {
            return new Manip(ManipController.TimeZones.GMT, 10, 04, 21, 19, 43, 17);
        }

        public Manip LandTurtle()
        {
            return new Manip(ManipController.TimeZones.ET, 16, 10, 22, 16, 37, 01);
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