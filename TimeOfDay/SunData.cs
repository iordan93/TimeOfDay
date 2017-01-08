namespace TimeOfDay
{
    using System;

    /// <summary>
    /// Contains all relevant data (input, intermediate and output values) to calculate
    /// the position of the Sun in the sky relative to an observer on Earth
    /// </summary>
    public class SunData
    {
        private const double DefaultRefraction = 0.5667;

        private double refraction;

        /// <summary>
        /// Initializes a new SunData object with all input values
        /// </summary>
        /// <param name="observationTime">The observation date and time</param>
        /// <param name="timezone">The observation time zone, in fractional hours</param>
        /// <param name="deltaUT1">The delta UT1 parameter, retrieved from observation, in seconds</param>
        /// <param name="deltaT">The delta T parameter, retrieved from observation, in seconds</param>
        /// <param name="latitude">The observer's latitude in degrees</param>
        /// <param name="longitude">The observer's longitude in degrees</param>
        /// <param name="elevation">The observer's elevation in meters</param>
        /// <param name="temperature">The annual average local temperature in degrees Celsius</param>
        /// <param name="pressure">The annual average local pressure in millibars</param>
        /// <param name="refraction">The atmospheric refraction at the horizon (at sunrise / sunset) in degrees</param>
        public SunData(
            DateTime observationTime,
            double timezone,
            double deltaUT1,
            double? deltaT,
            double latitude,
            double longitude,
            double elevation,
            double temperature,
            double pressure,
            double? refraction)
        {
            this.ObservationTime = observationTime;
            this.Timezone = timezone;
            this.DeltaUT1 = deltaUT1;
            this.DeltaT = deltaT;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Elevation = elevation;
            this.Temperature = temperature;
            this.Pressure = pressure;
            this.Refraction = refraction;
        }

        #region Input values
        /// <summary>
        /// The date and time of observation
        /// </summary>
        public DateTime ObservationTime { get; private set; }

        /// <summary>
        /// The observer's time zone
        /// </summary>
        public double Timezone { get; private set; }

        /// <summary>
        /// Fractional second difference between UTC and UT which is used to adjust UTC for Earth's 
        /// irregular rotation rate and is derived from observation only
        /// </summary>
        public double DeltaUT1 { get; private set; }

        /// <summary>
        /// The difference between Earth rotation time and terrestrial time.
        /// It is derived from observation only
        /// </summary>
        public double? DeltaT { get; private set; }

        /// <summary>
        /// The observer's latitude in degrees (negative south of the Equator)
        /// </summary>
        public double Latitude { get; private set; }

        /// <summary>
        /// The observer's longitude in degrees (negative west of Greenwich)
        /// </summary>
        public double Longitude { get; private set; }

        /// <summary>
        /// The observer's elevation in meters
        /// </summary>
        public double Elevation { get; private set; }

        /// <summary>
        /// The annual average local temperature in degrees Celsius
        /// </summary>
        public double Temperature { get; private set; }

        /// <summary>
        /// The annual average local pressure in millibars
        /// </summary>
        public double Pressure { get; private set; }

        /// <summary>
        /// The atmospheric refraction at the horizon (at sunrise / sunset) in degrees. The default value is 0.5667 degrees
        /// </summary>
        public double? Refraction
        {
            get
            {
                return this.refraction;
            }

            private set
            {
                if (value.HasValue)
                {
                    this.refraction = value.Value;
                }
                else
                {
                    this.refraction = DefaultRefraction;
                }
            }
        }
        #endregion

        #region Intermediate values
        /// <summary>
        /// The Julian day
        /// </summary>
        public double JulianDay { get; set; }

        /// <summary>
        /// The Julian century
        /// </summary>
        public double JulianCentury { get; set; }

        /// <summary>
        /// The Julian ephemeris day
        /// </summary>
        public double JulianEphemerisDay { get; set; }

        /// <summary>
        /// The Julian ephemeris century
        /// </summary>
        public double JulianEphemerisCentury { get; set; }

        /// <summary>
        /// The Julian ephemeris millennium
        /// </summary>
        public double JulianEphemerisMillennium { get; set; }

        /// <summary>
        /// The Earth's heliocentric latitude in degrees
        /// </summary>
        public double EarthHeliocentricLatitude { get; set; }

        /// <summary>
        /// The Earth's heliocentric longitude in degrees
        /// </summary>
        public double EarthHeliocentricLongitude { get; set; }

        /// <summary>
        /// The Earth's radius vector in Astronomical Units, AU
        /// </summary>
        public double EarthRadiusVector { get; set; }

        /// <summary>
        /// The Sun's geocentric latitude in degrees
        /// </summary>
        public double GeocentricLatitude { get; set; }

        /// <summary>
        /// The Sun's geocentric longitude in degrees
        /// </summary>
        public double GeocentricLongitude { get; set; }

        /// <summary>
        /// The mean elongation of the Moon from the Sun in degrees
        /// </summary>
        public double MeanElongationOfTheMoonFromTheSun { get; set; }

        /// <summary>
        /// The mean anomaly of the Sun in degrees
        /// </summary>
        public double MeanSunAnomaly { get; set; }

        /// <summary>
        /// The mean anomaly of the Moon in degrees
        /// </summary>
        public double MeanMoonAnomaly { get; set; }

        /// <summary>
        /// The argument of latitude of the Moon in degrees
        /// </summary>
        public double MoonArgumentOfLatitude { get; set; }

        /// <summary>
        /// The longitude of the ascending node of the Moon in degrees
        /// </summary>
        public double MoonAscendingNodeLongitude { get; set; }

        /// <summary>
        /// The nutation in longitude in degrees
        /// </summary>
        public double LongitudeNutation { get; set; }

        /// <summary>
        /// The nutation in obliquity in degrees
        /// </summary>
        public double ObliquityNutation { get; set; }

        /// <summary>
        /// The mean ecliptic obliquity in arcseconds
        /// </summary>
        public double MeanEclipticObliquity { get; set; }

        /// <summary>
        /// The true ecliptic obliquity in degrees
        /// </summary>
        public double TrueEclipticObliquity { get; set; }

        /// <summary>
        /// The aberration correction in degrees
        /// </summary>
        public double AberrationCorrection { get; set; }

        /// <summary>
        /// Apparent geocentric longitude of the Sun in degrees
        /// </summary>
        public double ApparentSunLongitude { get; set; }

        /// <summary>
        /// The mean sidereal time at Greenwich in degrees
        /// </summary>
        public double GreenwichMeanSiderealTime { get; set; }

        /// <summary>
        /// The Greenwich sidereal time in degrees
        /// </summary>
        public double GreenwichSiderealTime { get; set; }

        /// <summary>
        /// The geocentric Sun right ascension in degrees
        /// </summary>
        public double SunRightAscension { get; set; }

        /// <summary>
        /// The geocentric Sun declination in degrees
        /// </summary>
        public double SunDeclination { get; set; }

        /// <summary>
        /// The observer's local hour angle in degrees
        /// </summary>
        public double ObserverHourAngle { get; set; }

        /// <summary>
        /// The equatorial horizontal parallax of the Sun in degrees
        /// </summary>
        public double SunEquatorialHorizontalParallax { get; set; }

        /// <summary>
        /// The right ascension parallax of the Sun in degrees
        /// </summary>
        public double RightAscensionParallax { get; set; }

        /// <summary>
        /// The topocentric right ascension  of the Sun in degrees
        /// </summary>
        public double TopocentricSunRightAscension { get; set; }

        /// <summary>
        /// The topocentric declination of the Sun in degrees
        /// </summary>
        public double TopocentricSunDeclination { get; set; }

        /// <summary>
        /// The observer's topocentric local hour angle in degrees
        /// </summary>
        public double TopocentricLocalHourAngle { get; set; }

        /// <summary>
        /// The topocentric elevation angle (not corrected for refraction) in degrees
        /// </summary>
        public double TopocentricElevation { get; set; }

        /// <summary>
        /// The atmospheric refraction correction in degrees
        /// </summary>
        public double RefractionCorrection { get; set; }

        /// <summary>
        /// The topocentric elevation angle (corrected for refraction) in degrees
        /// </summary>
        public double TopocentricElevationCorrected { get; set; }
        #endregion

        #region Output values
        /// <summary>
        /// The topocentric zenith angle in degrees
        /// </summary>
        public double TopocentricZenithAngle { get; set; }

        /// <summary>
        /// The astronomical topocentric azimuth angle (measured westward from south)
        /// </summary>
        public double AstronomicalTopocentricAzimuth { get; set; }

        /// <summary>
        /// The geodesic topocentric azimuth angle (measured eastward from north)
        /// </summary>
        public double TopocentricAzimuth { get; set; }
        #endregion
    }
}