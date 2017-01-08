namespace TimeOfDay.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JulianDayTests
    {
        [TestMethod]
        public void Test_1January2000_Noon()
        {
            double jd = SunPositionCalculator.CalculateJulianDay(new DateTime(2000, 1, 1, 12, 0, 0), 0);
            Assert.AreEqual(2451545.0, jd);
        }

        [TestMethod]
        public void Test_1January1999_Midnight()
        {
            double jd = SunPositionCalculator.CalculateJulianDay(new DateTime(1999, 1, 1, 0, 0, 0), 0);
            Assert.AreEqual(2451179.5, jd);
        }

        [TestMethod]
        public void Test_27January1987_Midnight()
        {
            double jd = SunPositionCalculator.CalculateJulianDay(new DateTime(1987, 1, 27, 0, 0, 0), 0);
            Assert.AreEqual(2446822.5, jd);
        }

        [TestMethod]
        public void Test_19June1987_Noon()
        {
            double jd = SunPositionCalculator.CalculateJulianDay(new DateTime(1987, 6, 19, 12, 0, 0), 0);
            Assert.AreEqual(2446966.0, jd);
        }

        [TestMethod]
        public void Test_27January1988_Midnight()
        {
            double jd = SunPositionCalculator.CalculateJulianDay(new DateTime(1988, 1, 27, 0, 0, 0), 0);
            Assert.AreEqual(2447187.5, jd);
        }

        [TestMethod]
        public void Test_19June1988_Noon()
        {
            double jd = SunPositionCalculator.CalculateJulianDay(new DateTime(1988, 6, 19, 12, 0, 0), 0);
            Assert.AreEqual(2447332.0, jd);
        }

        [TestMethod]
        public void Test_1January1900_Midnight()
        {
            double jd = SunPositionCalculator.CalculateJulianDay(new DateTime(1900, 1, 1, 0, 0, 0), 0);
            Assert.AreEqual(2415020.5, jd);
        }

        [TestMethod]
        public void Test_1January1600_Midnight()
        {
            double jd = SunPositionCalculator.CalculateJulianDay(new DateTime(1600, 1, 1, 0, 0, 0), 0);
            Assert.AreEqual(2305447.5, jd);
        }

        [TestMethod]
        public void Test_31December1600_Midnight()
        {
            double jd = SunPositionCalculator.CalculateJulianDay(new DateTime(1600, 12, 31, 0, 0, 0), 0);
            Assert.AreEqual(2305812.5, jd);
        }

        [TestMethod]
        public void Test_10April837_Morning()
        {
            double jd = SunPositionCalculator.CalculateJulianDay(new DateTime(837, 4, 10, 7, 12, 0), 0);
            Assert.AreEqual(2026871.8, jd);
        }
    }
}
