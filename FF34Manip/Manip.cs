namespace FF34Manip
{
    public struct Manip
    {
        public string TimeZone;
        public short Day;
        public short Month;
        public short Year;
        public short Hour;
        public short Minute;
        public short Second;

        public Manip(string timeZone, short dd, short MM, short yyyy, short HH, short mm, short ss)
        {
            TimeZone = timeZone;
            Day = dd;
            Month = MM;
            Year = yyyy;
            Hour = HH;
            Minute = mm;
            Second = ss;
        }
    }
}