namespace TimeOfDay
{
    using System;

    /// <summary>
    /// Contains a method to check the part of day, calculating the position of the Sun in the sky
    /// </summary>
    public class PartOfDayChecker
    {
        private const double AnnualEarthTemperature = 15;
        private const double AnnualEarthPressure = 1013.25;

        // 6 degrees - civil twilight, 12 degrees - nautical twilight, 18 degrees - astronomical twilight
        public const int TwilightElevation = 12;

        /// <summary>
        /// Calculates the part of the day, using the given parameters
        /// </summary>
        /// <param name="observationTime">The date and time of observation</param>
        /// <param name="timezone">The observer's time zone</param>
        /// <param name="latitude">The observer's latitude in degrees (negative south of the Equator)</param>
        /// <param name="longitude">The observer's longitude in degrees (negative west of Greenwich)</param>
        /// <returns>Returns the part of the day. In case of invalid input data, returns Unknown</returns>
        public static PartOfDay GetPartOfDay(
            DateTime observationTime,
            TimeSpan timezone,
            double latitude,
            double longitude)
        {
            return GetPartOfDay(observationTime, timezone, 0, null, latitude, longitude, 0, AnnualEarthTemperature, AnnualEarthPressure, null);
        }

        /// <summary>
        /// Calculates the part of the day, using the given parameters
        /// </summary>
        /// <param name="observationTime">The date and time of observation</param>
        /// <param name="timezone">The observer's time zone</param>
        /// <param name="deltaUT1">The "delta UT1" parameter, retrieved from observation in seconds</param>
        /// <param name="deltaT">The "delta T" parameter, retrieved from observation in seconds</param>
        /// <param name="latitude">The observer's latitude in degrees (negative south of the Equator)</param>
        /// <param name="longitude">The observer's longitude in degrees (negative west of Greenwich)</param>
        /// <param name="elevation">The observer's elevation in meters</param>
        /// <param name="temperature">The annual average local temperature in degrees Celsius</param>
        /// <param name="pressure">The annual average local pressure in millibars</param>
        /// <param name="refraction">The atmospheric refraction at the horizon (at sunrise / sunset) in degrees</param>
        /// <returns>Returns the part of the day. In case of invalid input data, returns Unknown</returns>
        public static PartOfDay GetPartOfDay(
            DateTime observationTime,
            TimeSpan timezone,
            double deltaUT1,
            double? deltaT,
            double latitude,
            double longitude,
            double elevation,
            double temperature,
            double pressure,
            double? refraction)
        {
            var sunData = new SunData(observationTime, timezone.TotalHours, deltaUT1, deltaT, latitude, longitude, elevation, temperature, pressure, refraction);
            try
            {
                SunPositionCalculator.CalculateSunPosition(sunData);
            }
            catch (ArgumentException)
            {
                // Invalid input data - should return an Unknown value
                return PartOfDay.Unknown;
            }

            var azimuth = sunData.AstronomicalTopocentricAzimuth;
            var zenithAngle = sunData.TopocentricZenithAngle;

            if (zenithAngle < 90)
            {
                return PartOfDay.Day;
            }
            else if (zenithAngle >= 90 + TwilightElevation)
            {
                return PartOfDay.Night;
            }
            else
            {
                if (azimuth > 180)
                {
                    return PartOfDay.Dawn;
                }
                else
                {
                    return PartOfDay.Dusk;
                }
            }
        }
    }
}
