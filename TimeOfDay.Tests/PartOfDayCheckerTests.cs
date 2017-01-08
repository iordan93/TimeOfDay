namespace TimeOfDay.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PartOfDayCheckerTests
    {
        [TestMethod]
        public void Test_PartOfDayChecker_ForFullDay_ShouldReturnCorrectResults()
        {
            for (DateTime observationTime = new DateTime(2016, 6, 28); observationTime <= new DateTime(2016, 6, 29); observationTime = observationTime.AddMinutes(10))
            {
                var data = new SunData(
                    observationTime: observationTime,
                    timezone: 3,
                    deltaUT1: 0,
                    deltaT: 67,
                    latitude: 42.72275253,
                    longitude: 23.2992956,
                    elevation: 540,
                    temperature: 10,
                    pressure: 940,
                    refraction: null);

                SunPositionCalculator.CalculateSunPosition(data);
                var zenithAngle = data.TopocentricZenithAngle;
                var azimuth = data.AstronomicalTopocentricAzimuth;
                var twilightZenithAngle = 90 + PartOfDayChecker.TwilightElevation;

                var partOfDay = PartOfDayChecker.GetPartOfDay(observationTime, new TimeSpan(3, 0, 0), 42.72275253, 23.2992956);

                // Sanity checks
                Assert.IsTrue(zenithAngle >= 0 && zenithAngle <= 180);
                Assert.IsTrue(azimuth >= 0 && azimuth <= 360);

                if (partOfDay == PartOfDay.Night)
                {
                    Assert.IsTrue(zenithAngle >= twilightZenithAngle);
                }
                else if (partOfDay == PartOfDay.Dawn)
                {
                    Assert.IsTrue(zenithAngle > 90 && zenithAngle < twilightZenithAngle);
                    Assert.IsTrue(azimuth > 180);
                }
                else if (partOfDay == PartOfDay.Day)
                {
                    Assert.IsTrue(zenithAngle < 90);
                }
                else if (partOfDay == PartOfDay.Dusk)
                {
                    Assert.IsTrue(zenithAngle > 90 && zenithAngle < twilightZenithAngle);
                    Assert.IsTrue(azimuth <= 180);
                }
            }
        }

        [TestMethod]
        public void Test_PartOfDayChecker_WithInvalidTimezone_ShouldReturnUnknown()
        {
            var partOfDay = PartOfDayChecker.GetPartOfDay(new DateTime(2016, 6, 10, 15, 0, 0), new TimeSpan(20, 0, 0), 42.72275253, 23.2992956);
            Assert.AreEqual(partOfDay, PartOfDay.Unknown);
        }

        [TestMethod]
        public void Test_PartOfDayChecker_WithInvalidLatitude_ShouldReturnUnknown()
        {
            var partOfDay = PartOfDayChecker.GetPartOfDay(new DateTime(2016, 6, 10, 15, 0, 0), new TimeSpan(3, 0, 0), 100, 23.2992956);
            Assert.AreEqual(partOfDay, PartOfDay.Unknown);
        }

        [TestMethod]
        public void Test_PartOfDayChecker_WithInvalidLongitude_ShouldReturnUnknown()
        {
            var partOfDay = PartOfDayChecker.GetPartOfDay(new DateTime(2016, 6, 10, 15, 0, 0), new TimeSpan(3, 0, 0), 42.72275253, 230);
            Assert.AreEqual(partOfDay, PartOfDay.Unknown);
        }
    }
}
