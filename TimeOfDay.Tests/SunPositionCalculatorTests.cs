namespace TimeOfDay.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SunPositionCalculatorTests
    {
        private const double DefaultAccuracy = 5e-10;

        [TestMethod]
        public void Test_CalculateSolarPosition_ShouldCalculateIntermediateValuesCorrectly()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 39.742476,
                longitude: -105.1786,
                elevation: 1830.14,
                temperature: 11,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);

            Assert.AreEqual(data.JulianDay, 2452930.312847, 5e-7);

            Assert.AreEqual(data.EarthHeliocentricLongitude, 24.0182616917, DefaultAccuracy);
            Assert.AreEqual(data.EarthHeliocentricLatitude, -0.0001011219, DefaultAccuracy);
            Assert.AreEqual(data.EarthRadiusVector, 0.9965422974, DefaultAccuracy);

            Assert.AreEqual(data.GeocentricLongitude, 204.0182616917, DefaultAccuracy);
            Assert.AreEqual(data.GeocentricLatitude, 0.0001011219, DefaultAccuracy);

            Assert.AreEqual(data.LongitudeNutation, -0.00399840, 5e-9);
            Assert.AreEqual(data.ObliquityNutation, 0.00166657, 5e-9);

            Assert.AreEqual(data.TrueEclipticObliquity, 23.440465, 5e-7);

            Assert.AreEqual(data.ApparentSunLongitude, 204.0085519281, DefaultAccuracy);

            Assert.AreEqual(data.SunRightAscension, 202.22741, 5e-6);
            Assert.AreEqual(data.SunDeclination, -9.31434, 5e-6);

            Assert.AreEqual(data.ObserverHourAngle, 11.105900, 5e-5);
            Assert.AreEqual(data.TopocentricLocalHourAngle, 11.10629, 5e-5);

            Assert.AreEqual(data.TopocentricSunRightAscension, 202.22704, 5e-6);
            Assert.AreEqual(data.TopocentricSunDeclination, -9.316179, 5e-7);

            Assert.AreEqual(data.TopocentricZenithAngle, 50.11162, 5e-6);
            Assert.AreEqual(data.TopocentricAzimuth, 194.34024, 5e-6);
        }

        [TestMethod]
        public void Test_CalculateSolarPosition_NEQuadrant()
        {
            // Moscow
            var data = new SunData(
                observationTime: new DateTime(2016, 6, 10, 12, 30, 0),
                timezone: 4,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 55.7558,
                longitude: 37.6173,
                elevation: 151,
                temperature: 5.8,
                pressure: 1013.25,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);

            Assert.AreEqual(data.TopocentricZenithAngle, 34.465079, 5e-6);
            Assert.AreEqual(data.TopocentricAzimuth, 155.536449, 5e-5);
        }

        [TestMethod]
        public void Test_CalculateSolarPosition_NEQuadrantDawn()
        {
            // Moscow
            var data = new SunData(
                observationTime: new DateTime(2016, 6, 10, 3, 0, 0),
                timezone: 4,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 55.7558,
                longitude: 37.6173,
                elevation: 151,
                temperature: 5.8,
                pressure: 1013.25,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);

            Assert.AreEqual(data.TopocentricZenithAngle, 98.879283, 5e-6);
            Assert.AreEqual(data.TopocentricAzimuth, 21.124674, 5e-5);
        }

        [TestMethod]
        public void Test_CalculateSolarPosition_NEQuadrantNearSunrise()
        {
            // Moscow
            var data = new SunData(
                observationTime: new DateTime(2016, 6, 10, 6, 0, 0),
                timezone: 4,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 55.7558,
                longitude: 37.6173,
                elevation: 151,
                temperature: 5.8,
                pressure: 1013.25,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);

            Assert.AreEqual(data.TopocentricZenithAngle, 82.565553, 5e-6);
            Assert.AreEqual(data.TopocentricAzimuth, 59.181964, 5e-5);
        }

        [TestMethod]
        public void Test_CalculateSolarPosition_NEQuadrantNearSunset()
        {
            // Moscow
            var data = new SunData(
                observationTime: new DateTime(2016, 6, 10, 22, 0, 0),
                timezone: 4,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 55.7558,
                longitude: 37.6173,
                elevation: 151,
                temperature: 5.8,
                pressure: 1013.25,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);

            Assert.AreEqual(data.TopocentricZenithAngle, 89.149519, 5e-6);
            Assert.AreEqual(data.TopocentricAzimuth, 313.304766, 5e-5);
        }

        [TestMethod]
        public void Test_CalculateSolarPosition_NEQuadrantDusk()
        {
            // Moscow
            var data = new SunData(
                observationTime: new DateTime(2016, 6, 10, 2, 20, 0),
                timezone: 4,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 55.7558,
                longitude: 37.6173,
                elevation: 151,
                temperature: 5.8,
                pressure: 1013.25,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);

            Assert.AreEqual(data.TopocentricZenithAngle, 100.481896, 5e-6);
            Assert.AreEqual(data.TopocentricAzimuth, 11.933247, 5e-5);
        }

        [TestMethod]
        public void Test_CalculateSolarPosition_NWQuadrant()
        {
            // Chicago
            var data = new SunData(
                observationTime: new DateTime(2016, 6, 10, 12, 30, 0),
                timezone: -5,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 41.8781,
                longitude: -87.6298,
                elevation: 181,
                temperature: 9.8,
                pressure: 1013.25,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);

            Assert.AreEqual(data.TopocentricZenithAngle, 19.259226, 5e-6);
            Assert.AreEqual(data.TopocentricAzimuth, 165.863616, 5e-5);
        }

        [TestMethod]
        public void Test_CalculateSolarPosition_SEQuadrant()
        {
            // Perth
            var data = new SunData(
                observationTime: new DateTime(2016, 6, 10, 12, 30, 0),
                timezone: 8,
                deltaUT1: 0,
                deltaT: 67,
                latitude: -31.9505,
                longitude: 115.8605,
                elevation: 31.5,
                temperature: 17.8,
                pressure: 1013.25,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);

            Assert.AreEqual(data.TopocentricZenithAngle, 55.067639, 5e-6);
            Assert.AreEqual(data.TopocentricAzimuth, 356.079664, 5e-5);
        }

        [TestMethod]
        public void Test_CalculateSolarPosition_SWQuadrant()
        {
            // Sao Paulo
            var data = new SunData(
                observationTime: new DateTime(2016, 6, 10, 12, 30, 0),
                timezone: -3,
                deltaUT1: 0,
                deltaT: 67,
                latitude: -23.5505,
                longitude: -46.6333,
                elevation: 760,
                temperature: 22.5,
                pressure: 1013.25,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);

            Assert.AreEqual(data.TopocentricZenithAngle, 46.965500, 5e-6);
            Assert.AreEqual(data.TopocentricAzimuth, 352.472097, 5e-5);
        }

        [TestMethod]
        public void Test_CalculateSolarPosition_NorthernCircumpolarRegion()
        {
            // Longyearbyen
            var data = new SunData(
                observationTime: new DateTime(2016, 6, 10, 12, 30, 0),
                timezone: 1,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 78.2232,
                longitude: 15.6267,
                elevation: 100,
                temperature: -5.8,
                pressure: 1013.25,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);

            Assert.AreEqual(data.TopocentricZenithAngle, 55.276166, 5e-6);
            Assert.AreEqual(data.TopocentricAzimuth, 189.234844, 5e-5);
        }

        [TestMethod]
        public void Test_CalculateSolarPosition_SouthernCircumpolarRegion()
        {
            // Antarctica
            var data = new SunData(
                observationTime: new DateTime(2016, 6, 10, 12, 30, 0),
                timezone: 8,
                deltaUT1: 0,
                deltaT: 67,
                latitude: -76.566667,
                longitude: 110.2,
                elevation: 2500,
                temperature: -45,
                pressure: 1013.25,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);

            Assert.AreEqual(data.TopocentricZenithAngle, 99.615244, 5e-6);
            Assert.AreEqual(data.TopocentricAzimuth, 2.023090, 5e-5);
        }
    }
}
