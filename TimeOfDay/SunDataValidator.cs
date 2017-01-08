namespace TimeOfDay
{
    using System;

    /// <summary>
    /// Validates a given SunData object
    /// </summary>
    public static class SunDataValidator
    {
        private const double MinDeltaUT1 = -1;
        private const double MaxDeltaUT1 = 1;

        private const double MaxAbsDeltaT = 8000;

        private const double MaxAbsTimeZone = 18;

        private const double MaxAbsLatitude = 90;
        private const double MaxAbsLongitude = 180;
        private const double MinElevation = -6500000;

        private const double MinTemperature = -273;
        private const double MaxTemperature = 6000;

        private const double MinAtmosphericPressure = 0;
        private const double MaxAtmosphericPressure = 5000;

        private const double MaxRefraction = 5;

        public static void ValidateSunData(SunData data)
        {
            if (Math.Abs(data.Timezone) > MaxAbsTimeZone)
            {
                throw new ArgumentException(string.Format(
                    "The timezone is invalid. It should be between -{0} and {0}.",
                    MaxAbsTimeZone));
            }

            if (data.DeltaUT1 <= MinDeltaUT1 || data.DeltaUT1 >= MaxDeltaUT1)
            {
                throw new ArgumentException(string.Format(
                    "The delta UT1 parameter is invalid. It should be between {0} and {1} seconds.",
                    MinDeltaUT1,
                    MaxDeltaUT1));
            }

            if (data.DeltaT.HasValue && Math.Abs(data.DeltaT.Value) > MaxAbsDeltaT)
            {
                throw new ArgumentException(string.Format(
                    "The delta T parameter is invalid. It should be between -{0} and {0} seconds.",
                    MaxAbsDeltaT));
            }

            if (Math.Abs(data.Latitude) > MaxAbsLatitude)
            {
                throw new ArgumentException(string.Format(
                    "The latitude is invalid. It should be between -{0} and {0} degrees.",
                    MaxAbsLatitude));
            }

            if (Math.Abs(data.Longitude) > MaxAbsLongitude)
            {
                throw new ArgumentException(string.Format(
                    "The longitude is invalid. It should be between -{0} and {0} degrees.",
                    MaxAbsLongitude));
            }

            if (data.Elevation < MinElevation)
            {
                throw new ArgumentException(string.Format(
                    "The elevation is invalid. It should be at least {0} meters.",
                    MinElevation));
            }

            if (data.Temperature <= MinTemperature || data.Temperature >= MaxTemperature)
            {
                throw new ArgumentException(string.Format(
                    "The temperature is invalid. It should be between {0} and {1} degrees Celsius.",
                    MinTemperature,
                    MaxTemperature));
            }

            if (data.Pressure < MinAtmosphericPressure || data.Pressure > MaxAtmosphericPressure)
            {
                throw new ArgumentException(string.Format(
                    "The atmospheric pressure is invalid. It should be between {0} and {1} millibars.",
                    MinAtmosphericPressure,
                    MaxAtmosphericPressure));
            }

            if (data.Refraction.HasValue && data.Refraction > MaxRefraction)
            {
                throw new ArgumentException(string.Format(
                    "The refraction at the horizon is invalid. It should be at most {0} degrees.",
                    MaxRefraction));
            }
        }
    }
}
