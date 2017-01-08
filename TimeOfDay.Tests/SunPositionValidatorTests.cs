namespace TimeOfDay.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SunPositionValidatorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_InvalidTimeZone_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: 100,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 39.742476,
                longitude: -105.1786,
                elevation: 1830.14,
                temperature: 11,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooLowDeltaUT1_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: -2,
                deltaT: 67,
                latitude: 39.742476,
                longitude: -105.1786,
                elevation: 1830.14,
                temperature: 11,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooHighDeltaUT1_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: 2,
                deltaT: 67,
                latitude: 39.742476,
                longitude: -105.1786,
                elevation: 1830.14,
                temperature: 11,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooLowDeltaT_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: 0,
                deltaT: -67000,
                latitude: 39.742476,
                longitude: -105.1786,
                elevation: 1830.14,
                temperature: 11,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooHighDeltaT_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: 0,
                deltaT: 67000,
                latitude: 39.742476,
                longitude: -105.1786,
                elevation: 1830.14,
                temperature: 11,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooLowLatitude_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: 0,
                deltaT: 67,
                latitude: -100,
                longitude: -105.1786,
                elevation: 1830.14,
                temperature: 11,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooHighLatitude_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 100,
                longitude: -105.1786,
                elevation: 1830.14,
                temperature: 11,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooLowLongitude_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 39.742476,
                longitude: -200,
                elevation: 1830.14,
                temperature: 11,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooHighLongitude_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 39.742476,
                longitude: 200,
                elevation: 1830.14,
                temperature: 11,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_InvalidElevation_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 39.742476,
                longitude: -105.1786,
                elevation: -7500000,
                temperature: 11,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooLowTemperature_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 39.742476,
                longitude: -105.1786,
                elevation: 1830.14,
                temperature: -273.15,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooHighTemperaturee_ShouldThrowException()
        {
            var data = new SunData(
                observationTime: new DateTime(2003, 10, 17, 12, 30, 30),
                timezone: -7,
                deltaUT1: 0,
                deltaT: 67,
                latitude: 39.742476,
                longitude: -105.1786,
                elevation: 1830.14,
                temperature: 10000,
                pressure: 820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_NegativePressure_ShouldThrowException()
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
                pressure: -820,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooHighPressure_ShouldThrowException()
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
                pressure: 8200,
                refraction: null);
            SunPositionCalculator.CalculateSunPosition(data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CalculateSolarPosition_TooHighRefraction_ShouldThrowException()
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
                refraction: 7);
            SunPositionCalculator.CalculateSunPosition(data);
        }
    }
}
