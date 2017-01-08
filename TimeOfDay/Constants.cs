namespace TimeOfDay
{
    /// <summary>
    /// Contains constants for the sun position algorithm
    /// </summary>
    public static class Constants
    {
        public const double SunRadius = 0.26667;

        public const int LCount = 6;
        public const int BCount = 2;
        public const int RCount = 5;
        public const int YCount = 63;

        public const int LMaxSubcount = 64;
        public const int BMaxSubcount = 5;
        public const int RMaxSubcount = 40;

        public const int TermA = 0;
        public const int TermB = 1;
        public const int TermC = 2;
        public const int TermsCount = 3;

        public const int TermX0 = 0;
        public const int TermX1 = 1;
        public const int TermX2 = 2;
        public const int TermX3 = 3;
        public const int TermX4 = 4;
        public const int TermsXCount = 5;

        public const int TermsYCount = 5;

        public const int TermPsiA = 0;
        public const int TermPsiB = 1;
        public const int TermEpsilonC = 2;
        public const int TermEpsilonD = 3;

        public const int TermPsiEpsilonCount = 0;
        public const int JDMinus = 1;
        public const int JDZero = 2;
        public const int JDPlus = 3;
        public const int JDCount = 4;

        public const int SunTransit = 0;
        public const int SunRise = 1;
        public const int SunSet = 2;
        public const int SunTimesCount = 3;
    }
}
