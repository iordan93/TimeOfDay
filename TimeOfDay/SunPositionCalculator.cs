namespace TimeOfDay
{
    using System;

    /// <summary>
    /// Contains methods to calculate the position of the Sun, relative to an observer on Earth
    /// </summary>
    public static class SunPositionCalculator
    {
        /// <summary>
        /// Calculates the position of the Sun (zenith angle and azimuth) for a given SunData object
        /// </summary>
        /// <param name="data">An object holding information about the place and time of observation</param>
        public static void CalculateSunPosition(SunData data)
        {
            SunDataValidator.ValidateSunData(data);

            data.JulianDay = CalculateJulianDay(data.ObservationTime, data.Timezone, data.DeltaUT1);

            CalculateSunGeocentricEquatorialCoordinates(data);

            data.ObserverHourAngle = CalculateObserverHourAngle(data.GreenwichSiderealTime, data.Longitude, data.SunRightAscension);
            data.SunEquatorialHorizontalParallax = CalculateTheSunEquatorialHorizontalParallax(data.EarthRadiusVector);

            double rightAscensionParallax;
            double topocentricSunDeclination;
            CalculateRightAscensionParallaxAndTopocentricDeclination(
                data.Latitude,
                data.Elevation,
                data.SunEquatorialHorizontalParallax,
                data.ObserverHourAngle,
                data.SunDeclination,
                out rightAscensionParallax,
                out topocentricSunDeclination);
            data.RightAscensionParallax = rightAscensionParallax;
            data.TopocentricSunDeclination = topocentricSunDeclination;

            data.TopocentricSunRightAscension = CalculateTopocentricRightAscension(data.SunRightAscension, data.RightAscensionParallax);
            data.TopocentricLocalHourAngle = CalculateTopocentricHourAngle(data.ObserverHourAngle, data.RightAscensionParallax);

            data.TopocentricElevation = CalculateTopocentricElevationAngle(data.Latitude, data.TopocentricSunDeclination, data.TopocentricLocalHourAngle);
            data.RefractionCorrection = CalculateAtmosphericRefraction(data.Pressure, data.Temperature, data.Refraction.Value, data.TopocentricElevation);
            data.TopocentricElevationCorrected = CalculateTopocentricElevationAngleWithRefractionCorrection(data.TopocentricElevation, data.RefractionCorrection);

            data.TopocentricZenithAngle = CalculateTopocentricZenithAngle(data.TopocentricElevationCorrected);
            data.AstronomicalTopocentricAzimuth = CalculateAstronomicalTopocentricAzimuth(data.TopocentricLocalHourAngle, data.Latitude, data.TopocentricSunDeclination);
            data.TopocentricAzimuth = CalculateTopocentricAzimuth(data.AstronomicalTopocentricAzimuth);
        }

        private static void CalculateSunGeocentricEquatorialCoordinates(SunData data)
        {
            double[] correctionTerms = new double[Constants.TermsXCount];

            data.JulianCentury = CalculateJulianCentury(data.JulianDay);

            data.JulianEphemerisDay = CalculateJulianEphemerisDay(data.JulianDay, data.DeltaT);
            data.JulianEphemerisCentury = CalculateJulianEphemerisCentury(data.JulianEphemerisDay);
            data.JulianEphemerisMillennium = CalculateJulianEphemerisMillennium(data.JulianEphemerisCentury);

            data.EarthHeliocentricLongitude = CalculateEarthHeliocentricLongitude(data.JulianEphemerisMillennium);
            data.EarthHeliocentricLatitude = CalculateEarthHeliocentricLatitude(data.JulianEphemerisMillennium);
            data.EarthRadiusVector = CalculateEarthRadiusVector(data.JulianEphemerisMillennium);

            data.GeocentricLongitude = CalculateGeocentricLongitude(data.EarthHeliocentricLongitude);
            data.GeocentricLatitude = CalculateGeocentricLatitude(data.EarthHeliocentricLatitude);

            correctionTerms[Constants.TermX0] = data.MeanElongationOfTheMoonFromTheSun = CalculateTheMeanElongationOfTheMoonFromTheSun(data.JulianEphemerisCentury);
            correctionTerms[Constants.TermX1] = data.MeanSunAnomaly = CalculateTheMeanSunAnomaly(data.JulianEphemerisCentury);
            correctionTerms[Constants.TermX2] = data.MeanMoonAnomaly = CalculateTheMeanMoonAnomaly(data.JulianEphemerisCentury);
            correctionTerms[Constants.TermX3] = data.MoonArgumentOfLatitude = CalculateTheMoonArgumentOfLatitude(data.JulianEphemerisCentury);
            correctionTerms[Constants.TermX4] = data.MoonAscendingNodeLongitude = CalculateTheMoonAscendingNodeLongitude(data.JulianEphemerisCentury);

            double longitudeNutation;
            double obliquityNutation;
            CalculateTheNutationInLongitudeAndObliquity(data.JulianEphemerisCentury, correctionTerms, out longitudeNutation, out obliquityNutation);
            data.LongitudeNutation = longitudeNutation;
            data.ObliquityNutation = obliquityNutation;

            data.MeanEclipticObliquity = CalculateTheMeanEclipticObliquity(data.JulianEphemerisMillennium);
            data.TrueEclipticObliquity = CalculateTheTrueEclipticObliquity(data.ObliquityNutation, data.MeanEclipticObliquity);

            data.AberrationCorrection = CalculateAberrationCorrection(data.EarthRadiusVector);
            data.ApparentSunLongitude = CalculateApparentSunLongitude(data.GeocentricLongitude, data.LongitudeNutation, data.AberrationCorrection);
            data.GreenwichMeanSiderealTime = CalculateTheGreenwichMeanSiderealTime(data.JulianDay, data.JulianCentury);
            data.GreenwichSiderealTime = CalculateTheGreenwichSiderealTime(data.GreenwichMeanSiderealTime, data.LongitudeNutation, data.TrueEclipticObliquity);

            data.SunRightAscension = CalculateTheSunGeocentricRightAscension(data.ApparentSunLongitude, data.TrueEclipticObliquity, data.GeocentricLatitude);
            data.SunDeclination = CalculateTheSunGeocentricDeclination(data.GeocentricLatitude, data.TrueEclipticObliquity, data.ApparentSunLongitude);
        }

        #region Julian and Julian ephemeris day, century and millennium
        /// <summary>
        /// Calculates the Julian day from a given DateTime
        /// </summary>
        /// <param name="observationTime">The DateTime object representing the local date and time, zero by default</param>
        /// <param name="timezone">The time zone offset</param>
        /// <param name="deltaUt1">Fraction of a second, added to UTC to adjust for the Earth's rotation</param>
        /// <returns>Returns the Julian day</returns>
        public static double CalculateJulianDay(DateTime observationTime, double timezone, double deltaUt1 = 0)
        {
            double decimalDay = observationTime.Day + (observationTime.Hour - timezone + (observationTime.Minute + (observationTime.Second + deltaUt1) / 60.0) / 60.0) / 24.0;

            int month = observationTime.Month;
            int year = observationTime.Year;
            if (observationTime.Month < 3)
            {
                month += 12;
                year--;
            }

            double julianDay = (int)(365.25 * (year + 4716.0)) + (int)(30.6001 * (month + 1)) + decimalDay - 1524.5;

            if (julianDay > 2299160)
            {
                int correction = year / 100;
                julianDay += 2 - correction + correction / 4;
            }

            return julianDay;
        }

        /// <summary>
        /// Calculates the Julian ephemeris day from a given Julian day
        /// </summary>
        /// <param name="julianDay">The Julian day</param>
        /// <param name="deltaT">The "delta T" parameter, retrieved from observation. 
        /// If none is provided, a close approximation is used instead</param>
        /// <returns>Returns the Julian ephemeris day</returns>
        private static double CalculateJulianEphemerisDay(double julianDay, double? deltaT = null)
        {
            if (!deltaT.HasValue)
            {
                deltaT = EstimateDeltaT(julianDay);
            }

            return julianDay + deltaT.Value / 86400.0;
        }

        /// <summary>
        /// Calculates the Julian century from a given Julian day
        /// </summary>
        /// <param name="julianDay">The Julian day</param>
        /// <returns>Returns the Julian century</returns>
        private static double CalculateJulianCentury(double julianDay)
        {
            return (julianDay - 2451545.0) / 36525.0;
        }

        /// <summary>
        /// Calculates the Julian ephemeris century from a given Julian ephemeris day
        /// </summary>
        /// <param name="julianEphemerisDay">The Julian ephemeris day</param>
        /// <returns>Returns the Julian ephemeris century</returns>
        private static double CalculateJulianEphemerisCentury(double julianEphemerisDay)
        {
            return (julianEphemerisDay - 2451545.0) / 36525.0;
        }

        /// <summary>
        /// Calculates the Julian ephemeris millennium from a given Julian ephemeris century
        /// </summary>
        /// <param name="julianEphemerisCentury">The Julian ephemeris century</param>
        /// <returns>Returns the Julian ephemeris millennium</returns>
        private static double CalculateJulianEphemerisMillennium(double julianEphemerisCentury)
        {
            return julianEphemerisCentury / 10.0;
        }

        private static double EstimateDeltaT(double julianDay)
        {
            // The coefficients have been obtained using a second-degree polynomial fitting
            // on historical data for the years 1973-2016
            return -0.00000007865 * julianDay * julianDay + 0.38682 * julianDay - 475550;
        }
        #endregion

        #region Earth heliocentric longitude, latitude and radius vector
        /// <summary>
        /// Calculates the Earth heliocentric longitude for a given Julian ephemeris millennium
        /// </summary>
        /// <param name="julianEphemerisMillennium">The Julian ephemeris millennium</param>
        /// <returns>Returns the Earth heliocentric longitude in degrees</returns>
        private static double CalculateEarthHeliocentricLongitude(double julianEphemerisMillennium)
        {
            double[] termSums = new double[Constants.LCount];
            for (int i = 0; i < Constants.LCount; i++)
            {
                termSums[i] = SumPeriodicTerms(Tables.LTerms[i], Tables.LSubcounts[i], julianEphemerisMillennium);
            }

            double heliocentricLongitude = CalculateEarthHeliocentricValues(termSums, Constants.LCount, julianEphemerisMillennium);
            return MathUtilities.LimitDegrees0To360(MathUtilities.ConvertRadiansToDegrees(heliocentricLongitude));
        }

        /// <summary>
        /// Calculates the Earth heliocentric latitude for a given Julian ephemeris millennium
        /// </summary>
        /// <param name="julianEphemerisMillennium">The Julian ephemeris millennium</param>
        /// <returns>Returns the Earth heliocentric latitude in degrees</returns>
        private static double CalculateEarthHeliocentricLatitude(double julianEphemerisMillennium)
        {
            double[] termSums = new double[Constants.BCount];
            for (int i = 0; i < Constants.BCount; i++)
            {
                termSums[i] = SumPeriodicTerms(Tables.BTerms[i], Tables.BSubcounts[i], julianEphemerisMillennium);
            }

            double heliocentricLatitude = CalculateEarthHeliocentricValues(termSums, Constants.BCount, julianEphemerisMillennium);
            return MathUtilities.ConvertRadiansToDegrees(heliocentricLatitude);
        }

        /// <summary>
        /// Calculates the Earth radius vector for a given Julian ephemeris millennium
        /// </summary>
        /// <param name="julianEphemerisMillennium">The Julian ephemeris millennium</param>
        /// <returns>Returns the Earth radius vector in astronomical units</returns>
        private static double CalculateEarthRadiusVector(double julianEphemerisMillennium)
        {
            double[] termSums = new double[Constants.RCount];
            for (int i = 0; i < Constants.RCount; i++)
            {
                termSums[i] = SumPeriodicTerms(Tables.RTerms[i], Tables.RSubcounts[i], julianEphemerisMillennium);
            }

            double earthRadiusVector = CalculateEarthHeliocentricValues(termSums, Constants.RCount, julianEphemerisMillennium);
            return earthRadiusVector;
        }

        private static double CalculateEarthHeliocentricValues(double[] termSums, int termsCount, double julianEphemerisMillennium)
        {
            double sum = 0;
            for (int i = 0; i < termsCount; i++)
            {
                sum += termSums[i] * Math.Pow(julianEphemerisMillennium, i);
            }

            sum /= 1.0e8;
            return sum;
        }

        private static double SumPeriodicTerms(double[][] terms, int termsCount, double julianEphemerisMillennium)
        {
            double sum = 0;
            for (int i = 0; i < termsCount; i++)
            {
                sum += terms[i][Constants.TermA] * Math.Cos(terms[i][Constants.TermB] + terms[i][Constants.TermC] * julianEphemerisMillennium);
            }

            return sum;
        }
        #endregion

        #region Geocentric longitude and latitude
        /// <summary>
        /// Calculates the geocentric longitude from a given heliocentric longitude
        /// </summary>
        /// <param name="heliocentricLongitude">The heliocentric longitude (in degrees)</param>
        /// <returns>Returns the geocentric longitude in degrees</returns>
        private static double CalculateGeocentricLongitude(double heliocentricLongitude)
        {
            double geocentricLongitude = heliocentricLongitude + 180.0;
            if (geocentricLongitude >= 360.0)
            {
                geocentricLongitude -= 360.0;
            }

            return geocentricLongitude;
        }

        /// <summary>
        /// Calculates the geocentric latitude from a given heliocentric latitude
        /// </summary>
        /// <param name="heliocentricLatitude">The heliocentric latitude (in degrees)</param>
        /// <returns>Returns the geocentric latitude in degrees</returns>
        private static double CalculateGeocentricLatitude(double heliocentricLatitude)
        {
            return -heliocentricLatitude;
        }
        #endregion

        #region Nutation in longitude and obliquity
        /// <summary>
        /// Calculates the mean elongation of the Moon from the Sun (X_0) for a given Julian ephemeris century
        /// </summary>
        /// <param name="julianEphemerisCentury">The Julian ephemeris century</param>
        /// <returns>Returns the mean elongation of the Moon from the Sun (in degrees)</returns>
        private static double CalculateTheMeanElongationOfTheMoonFromTheSun(double julianEphemerisCentury)
        {
            return MathUtilities.CalculateThirdDegreePolynomialValue(1.0 / 189474.0, -0.0019142, 445267.11148, 297.85036, julianEphemerisCentury);
        }

        /// <summary>
        /// Calculates the mean anomaly of the Sun (Earth) (X_1) for a given Julian ephemeris century
        /// </summary>
        /// <param name="julianEphemerisCentury">The Julian ephemeris century</param>
        /// <returns>Returns the mean anomaly of the Sun (Earth) (in degrees)</returns>
        private static double CalculateTheMeanSunAnomaly(double julianEphemerisCentury)
        {
            return MathUtilities.CalculateThirdDegreePolynomialValue(-1.0 / 300000.0, -0.0001603, 35999.05034, 357.52772, julianEphemerisCentury);
        }

        /// <summary>
        /// Calculates the mean anomaly of the Moon (X_2) for a given Julian ephemeris century
        /// </summary>
        /// <param name="julianEphemerisCentury">The Julian ephemeris century</param>
        /// <returns>Returns the mean anomaly of the Moon (in degrees)</returns>
        private static double CalculateTheMeanMoonAnomaly(double julianEphemerisCentury)
        {
            return MathUtilities.CalculateThirdDegreePolynomialValue(1.0 / 56250.0, 0.0086972, 477198.867398, 134.96298, julianEphemerisCentury);
        }

        /// <summary>
        /// Calculates the argument of latitude of the Moon (X_3) for a given Julian ephemeris century
        /// </summary>
        /// <param name="julianEphemerisCentury">The Julian ephemeris century</param>
        /// <returns>Returns the argument of latitude of the Moon (in degrees)</returns>
        private static double CalculateTheMoonArgumentOfLatitude(double julianEphemerisCentury)
        {
            return MathUtilities.CalculateThirdDegreePolynomialValue(1.0 / 327270.0, -0.0036825, 483202.017538, 93.27191, julianEphemerisCentury);
        }

        /// <summary>
        /// Calculates the longitude of the ascending node of the Moon's mean orbit on the Ecliptic,
        /// measured from the mean equinox of the date, (X_4), for a given Julian ephemeris century
        /// </summary>
        /// <param name="julianEphemerisCentury">The Julian ephemeris century</param>
        /// <returns>Returns the longitude of the ascending node of the Moon (in degrees)</returns>
        private static double CalculateTheMoonAscendingNodeLongitude(double julianEphemerisCentury)
        {
            return MathUtilities.CalculateThirdDegreePolynomialValue(1.0 / 450000.0, 0.0020708, -1934.136261, 125.04452, julianEphemerisCentury);
        }

        /// <summary>
        /// Calculates the nutation in longitude and the nutation in obliquity 
        /// from the given nutation correction terms at the given Julian ephemeris century.
        /// Returns the results in the variables longitudeNutation and obliquityNutation
        /// </summary>
        /// <param name="julianEphemerisCentury">The Julian ephemeris century</param>
        /// <param name="correctionTerms">The nutation correction terms</param>
        /// <param name="longitudeNutation">Contains the nutation in longitude after the method has finished</param>
        /// <param name="obliquityNutation">Contains the nutation in obliquity after the method has finished</param>
        private static void CalculateTheNutationInLongitudeAndObliquity(
            double julianEphemerisCentury,
            double[] correctionTerms,
            out double longitudeNutation,
            out double obliquityNutation)
        {
            double longitudeSum = 0;
            double obliquitySum = 0;
            for (int i = 0; i < Constants.YCount; i++)
            {
                double sum = MathUtilities.ConvertDegreesToRadians(CalculateXYSum(i, correctionTerms));
                longitudeSum += (Tables.PsiEpsilonTerms[i][Constants.TermPsiA] +
                    julianEphemerisCentury * Tables.PsiEpsilonTerms[i][Constants.TermPsiB]) *
                    Math.Sin(sum);
                obliquitySum += (Tables.PsiEpsilonTerms[i][Constants.TermEpsilonC] +
                    julianEphemerisCentury * Tables.PsiEpsilonTerms[i][Constants.TermEpsilonD]) *
                    Math.Cos(sum);
            }

            longitudeNutation = longitudeSum / 36000000.0;
            obliquityNutation = obliquitySum / 36000000.0;
        }

        private static double CalculateXYSum(int row, double[] correctionTerms)
        {
            double sum = 0;
            for (int col = 0; col < Constants.TermsYCount; col++)
            {
                sum += correctionTerms[col] * Tables.YTerms[row][col];
            }

            return sum;
        }
        #endregion

        #region True obliquity of the Ecliptic
        /// <summary>
        /// Calculates the mean obliquity of the Ecliptic for a given Julian ephemeris millennium
        /// </summary>
        /// <param name="julianEphemerisMillennium">The Julian ephemeris millennium</param>
        /// <returns>Returns the mean obliquity of the Ecliptic in arcseconds</returns>
        private static double CalculateTheMeanEclipticObliquity(double julianEphemerisMillennium)
        {
            double u = julianEphemerisMillennium / 10.0;

            return 84381.448 + u * (-4680.93 + u * (-1.55 + u * (1999.25 + u * (-51.38 + u * (-249.67 +
                               u * (-39.05 + u * (7.12 + u * (27.87 + u * (5.79 + u * 2.45)))))))));
        }

        /// <summary>
        /// Calculates the true obliquity of the Ecliptic, 
        /// given the mean obliquity and the nutation in obliquity for a given Julian century
        /// </summary>
        /// <param name="obliquityNutation">The nutation in obliquity</param>
        /// <param name="meanObliquity">The mean obliquity</param>
        /// <returns>The true obliquity of the Ecliptic in degrees</returns>
        private static double CalculateTheTrueEclipticObliquity(double obliquityNutation, double meanObliquity)
        {
            return obliquityNutation + meanObliquity / 3600.0;
        }
        #endregion

        #region Aberration correction
        /// <summary>
        /// Calculates the aberration correction for a given Earth radius vector
        /// </summary>
        /// <param name="earthRadiusVector">The Earth radius vector (in astronomical units)</param>
        /// <returns>Returns the aberration correction in degrees</returns>
        private static double CalculateAberrationCorrection(double earthRadiusVector)
        {
            return -20.4898 / (3600.0 * earthRadiusVector);
        }
        #endregion

        #region Apparent Sun longitude
        /// <summary>
        /// Calculates the apparent Sun longitude given the geocentric longitude, the nutation in longitude and the aberration correction
        /// </summary>
        /// <param name="geocentricLongitude">The geocentric longitude (in degrees)</param>
        /// <param name="longitudeNutation">The nutation in longitude (in degrees)</param>
        /// <param name="aberrationCorrection">The aberration correction (in degrees)</param>
        /// <returns>Returns the apparent Sun longitude in degrees</returns>
        private static double CalculateApparentSunLongitude(double geocentricLongitude, double longitudeNutation, double aberrationCorrection)
        {
            return geocentricLongitude + longitudeNutation + aberrationCorrection;
        }
        #endregion

        #region Apparent sidereal time at Greenwich
        /// <summary>
        /// Calculates the mean sidereal time at Greenwich, given the Julian day and century
        /// </summary>
        /// <param name="julianDay">The Julian day</param>
        /// <param name="julianCentury">The Julian century</param>
        /// <returns>Returns the mean sidereal time at Greenwich in degrees</returns>
        private static double CalculateTheGreenwichMeanSiderealTime(double julianDay, double julianCentury)
        {
            double meanSiderealTime = 280.46061837 + 360.98564736629 * (julianDay - 2451545.0) +
                julianCentury * julianCentury * (0.000387933 - julianCentury / 38710000.0);
            return MathUtilities.LimitDegrees0To360(meanSiderealTime);
        }

        /// <summary>
        /// Calculates the apparent sidereal time at Greenwich, 
        /// given the mean sidereal time, nutation in longitude and true obliquity of the Ecliptic
        /// </summary>
        /// <param name="meanSiderealTime">The mean sidereal time</param>
        /// <param name="longitudeNutation">The nutation in longitude</param>
        /// <param name="trueObliquity">The true obliquity of the Ecliptic</param>
        /// <returns>Returns the apparent sidereal time at Greenwich in degrees</returns>
        private static double CalculateTheGreenwichSiderealTime(double meanSiderealTime, double longitudeNutation, double trueObliquity)
        {
            return meanSiderealTime + longitudeNutation * Math.Cos(MathUtilities.ConvertDegreesToRadians(trueObliquity));
        }
        #endregion

        #region Geocentric Sun right ascension
        /// <summary>
        /// Calculates the geocentric right ascension of the Sun
        /// </summary>
        /// <param name="apparentSunLongitude">The apparent Sun longitude</param>
        /// <param name="trueObliquity">The true obliquity of the Ecliptic</param>
        /// <param name="geocentricLatitude">The geocentric latitude</param>
        /// <returns>Returns the geocentric right ascension of the Sun in degrees</returns>
        private static double CalculateTheSunGeocentricRightAscension(double apparentSunLongitude, double trueObliquity, double geocentricLatitude)
        {
            double apparentSunLongitudeRadians = MathUtilities.ConvertDegreesToRadians(apparentSunLongitude);
            double trueObliquityRadians = MathUtilities.ConvertDegreesToRadians(trueObliquity);
            double geocentricLatitudeRadians = MathUtilities.ConvertDegreesToRadians(geocentricLatitude);

            double geocentricRightAscension = Math.Atan2(
                Math.Sin(apparentSunLongitudeRadians) * Math.Cos(trueObliquityRadians) -
                    Math.Tan(geocentricLatitudeRadians) * Math.Sin(trueObliquityRadians),
                Math.Cos(apparentSunLongitudeRadians));

            return MathUtilities.LimitDegrees0To360(MathUtilities.ConvertRadiansToDegrees(geocentricRightAscension));
        }
        #endregion

        #region Geocentric Sun declination
        /// <summary>
        /// Calculates the geocentric declination of the Sun
        /// </summary>
        /// <param name="geocentricLatitude">The geocentric latitude</param>
        /// <param name="trueObliquity">The true obliquity of the Ecliptic</param>
        /// <param name="apparentSunLongitude">The apparent Sun longitude</param>
        /// <returns>Returns the geocentric declination of the Sun in degrees</returns>
        private static double CalculateTheSunGeocentricDeclination(double geocentricLatitude, double trueObliquity, double apparentSunLongitude)
        {
            double geocentricLatitudeRadians = MathUtilities.ConvertDegreesToRadians(geocentricLatitude);
            double trueObliquityRadians = MathUtilities.ConvertDegreesToRadians(trueObliquity);
            double apparentSunLongitudeRadians = MathUtilities.ConvertDegreesToRadians(apparentSunLongitude);

            double geocentricDeclination = Math.Asin(Math.Sin(geocentricLatitudeRadians) * Math.Cos(trueObliquityRadians) +
                                Math.Cos(geocentricLatitudeRadians) * Math.Sin(trueObliquityRadians) * Math.Sin(apparentSunLongitudeRadians));

            return MathUtilities.ConvertRadiansToDegrees(geocentricDeclination);
        }
        #endregion

        #region Observer's local hour angle
        /// <summary>
        /// Calculates the observer's local hour angle for an object with the given right ascension
        /// </summary>
        /// <param name="apparentGreenwichSiderealTime">The apparent sidereal time at Greenwich</param>
        /// <param name="longitude">The observer's longitude</param>
        /// <param name="rightAscension">The right ascension</param>
        /// <returns>Returns the observer's hour angle for the object in degrees</returns>
        private static double CalculateObserverHourAngle(double apparentGreenwichSiderealTime, double longitude, double rightAscension)
        {
            return MathUtilities.LimitDegrees0To360(apparentGreenwichSiderealTime + longitude - rightAscension);
        }
        #endregion

        #region Topocentric Sun right ascension
        /// <summary>
        /// Calculates the equatorial horizontal parallax of the Sun, given the Earth's radius vector
        /// </summary>
        /// <param name="earthRadiusVector">The Earth's radius vector</param>
        /// <returns>Returns the equatorial horizontal parallax of the Sun in degrees</returns>
        private static double CalculateTheSunEquatorialHorizontalParallax(double earthRadiusVector)
        {
            return 8.794 / (3600.0 * earthRadiusVector);
        }

        /// <summary>
        /// Calculates the parallax in right ascension and the topocentric declination of an object.
        /// Returns the results in the variables rightAscensionParallax and topocentricDeclination
        /// </summary>
        /// <param name="latitude">The observer's latitude in degrees</param>
        /// <param name="elevation">The observer's elevation in meters</param>
        /// <param name="equatorialHorizontalParallax">The equatorial horizontal parallax of the object in degrees</param>
        /// <param name="observerLocalHourAngle">The observer's hour angle in degrees</param>
        /// <param name="geocentricDeclination">The geocentric declination of the object in degrees</param>
        /// <param name="rightAscensionParallax">Contains the parallax in right ascension in degrees after the method has finished</param>
        /// <param name="topocentricDeclination">Contains the topocentric declination in degrees after the method has finished</param>
        private static void CalculateRightAscensionParallaxAndTopocentricDeclination(
            double latitude,
            double elevation,
            double equatorialHorizontalParallax,
            double observerLocalHourAngle,
            double geocentricDeclination,
            out double rightAscensionParallax,
            out double topocentricDeclination)
        {
            double latitudeRadians = MathUtilities.ConvertDegreesToRadians(latitude);
            double equatorialHorizontalParallaxRadians = MathUtilities.ConvertDegreesToRadians(equatorialHorizontalParallax);
            double observerLocalHourAngleRadians = MathUtilities.ConvertDegreesToRadians(observerLocalHourAngle);
            double geocentricDeclinationRadians = MathUtilities.ConvertDegreesToRadians(geocentricDeclination);

            double u = Math.Atan(0.99664719 * Math.Tan(latitudeRadians));
            double x = Math.Cos(u) + elevation * Math.Cos(latitudeRadians) / 6378140.0;
            double y = 0.99664719 * Math.Sin(u) + elevation * Math.Sin(latitudeRadians) / 6378140.0;

            double rightAscensionParallaxRadians = Math.Atan2(
                    -x * Math.Sin(equatorialHorizontalParallaxRadians) * Math.Sin(observerLocalHourAngleRadians),
                    Math.Cos(geocentricDeclinationRadians) -
                        x * Math.Sin(equatorialHorizontalParallaxRadians) * Math.Cos(observerLocalHourAngleRadians));

            double topocentricDeclinationRadians = Math.Atan2(
                    (Math.Sin(geocentricDeclinationRadians) - y * Math.Sin(equatorialHorizontalParallaxRadians))
                        * Math.Cos(rightAscensionParallaxRadians),
                    Math.Cos(geocentricDeclinationRadians) -
                        x * Math.Sin(equatorialHorizontalParallaxRadians) * Math.Cos(observerLocalHourAngleRadians));

            rightAscensionParallax = MathUtilities.ConvertRadiansToDegrees(rightAscensionParallaxRadians);
            topocentricDeclination = MathUtilities.ConvertRadiansToDegrees(topocentricDeclinationRadians);
        }

        /// <summary>
        /// Calculates the topocentric right ascension for an object
        /// </summary>
        /// <param name="geocentricRightAscension">The geocentric right ascension of the object</param>
        /// <param name="rightAscensionParallax">The parallax in the object's right ascension</param>
        /// <returns>Returns the topocentric right ascension of the object in degrees</returns>
        private static double CalculateTopocentricRightAscension(double geocentricRightAscension, double rightAscensionParallax)
        {
            return geocentricRightAscension + rightAscensionParallax;
        }
        #endregion

        #region Topocentric local hour angle
        /// <summary>
        /// Calculates the topocentric local hour angle of an object
        /// </summary>
        /// <param name="observerLocalHourAngle">The observer's local hour angle</param>
        /// <param name="rightAscensionParallax">The parallax in the right ascension of the object</param>
        /// <returns>Returns the topocentric local hour angle in degrees</returns>
        private static double CalculateTopocentricHourAngle(double observerLocalHourAngle, double rightAscensionParallax)
        {
            return observerLocalHourAngle - rightAscensionParallax;
        }
        #endregion

        #region Topocentric zenith angle
        /// <summary>
        /// Calculates the topocentric elevation angle of an object, without refraction correction
        /// </summary>
        /// <param name="latitude">The observer's latitude in degrees</param>
        /// <param name="topocentricDeclination">The topocentric declination of the object in degrees</param>
        /// <param name="topocentricHourAngle">The topocentric hour angle of the object in degrees</param>
        /// <returns>Returns the topocentric elevation angle of the object in degrees</returns>
        private static double CalculateTopocentricElevationAngle(
            double latitude,
            double topocentricDeclination,
            double topocentricHourAngle)
        {
            double latitudeRadians = MathUtilities.ConvertDegreesToRadians(latitude);
            double topocentricDeclinationRadians = MathUtilities.ConvertDegreesToRadians(topocentricDeclination);
            double topocentricHourAngleRadians = MathUtilities.ConvertDegreesToRadians(topocentricHourAngle);

            double topocentricElevationRadians = Math.Asin(Math.Sin(latitudeRadians) * Math.Sin(topocentricDeclinationRadians) +
                Math.Cos(latitudeRadians) * Math.Cos(topocentricDeclinationRadians) * Math.Cos(topocentricHourAngleRadians));

            return MathUtilities.ConvertRadiansToDegrees(topocentricElevationRadians);
        }

        /// <summary>
        /// Calculates the atmospheric refraction correction for an object observed at a place on the Earth.
        /// The refraction correction is zero if the object is below the horizon
        /// </summary>
        /// <param name="pressure">The annual average local pressure in millibars</param>
        /// <param name="temperature">The annual average local temperature in degrees Celsius</param>
        /// <param name="horizonRefraction">The refraction at the horizon (at sunrise / sunset)</param>
        /// <param name="topocentricElevation">The topocentric elevation of the object in degrees</param>
        /// <returns>The atmospheric refraction correction in degrees</returns>
        private static double CalculateAtmosphericRefraction(
            double pressure,
            double temperature,
            double horizonRefraction,
            double topocentricElevation)
        {
            double refraction = 0;
            double tanArgument = MathUtilities.ConvertDegreesToRadians(topocentricElevation + 10.3 / (topocentricElevation + 5.11));

            bool isObjectBelowHorizon = topocentricElevation >= -1 * (Constants.SunRadius + horizonRefraction);
            if (isObjectBelowHorizon)
            {
                refraction = (pressure / 1010.0) * (283.0 / (273.0 + temperature)) * 1.02 / (60.0 * Math.Tan(tanArgument));
            }

            return refraction;
        }

        /// <summary>
        /// Calculates the topocentric elevation angle of an object, with refraction correction
        /// </summary>
        /// <param name="topocentricElevationAngle">The topocentric declination of the object in degrees</param>
        /// <param name="refractionCorrection">The refraction correction in degrees</param>
        /// <returns>Returns the topocentric elevation angle</returns>
        private static double CalculateTopocentricElevationAngleWithRefractionCorrection(double topocentricElevationAngle, double refractionCorrection)
        {
            return topocentricElevationAngle + refractionCorrection;
        }

        /// <summary>
        /// Calculates the topocentric zenith angle of an object
        /// </summary>
        /// <param name="elevation">The topocentric elevation angle of the object in degrees</param>
        /// <returns>Returns the topocentric zenith angle in degrees</returns>
        private static double CalculateTopocentricZenithAngle(double elevation)
        {
            return 90.0 - elevation;
        }
        #endregion

        #region Topocentric azimuth angle
        /// <summary>
        /// Calculates the astronomical topocentric azimuth for an object observed at a place on the Earth
        /// </summary>
        /// <param name="topocentricLocalHourAngle">The topocentric hour angle of the object in degrees</param>
        /// <param name="latitude">The observer's latitude in degrees</param>
        /// <param name="topocentricDeclination">The topocentric declination of the object in degrees</param>
        /// <returns>Returns the astronomical topocentric azimuth of the object in degrees</returns>
        private static double CalculateAstronomicalTopocentricAzimuth(
            double topocentricLocalHourAngle,
            double latitude,
            double topocentricDeclination)
        {
            double topocentricLocalHourAngleRadians = MathUtilities.ConvertDegreesToRadians(topocentricLocalHourAngle);
            double latitudeRadians = MathUtilities.ConvertDegreesToRadians(latitude);
            double topocentricDeclinationRadians = MathUtilities.ConvertDegreesToRadians(topocentricDeclination);

            double astronomicalTopocentricAzimuth = Math.Atan2(
                    Math.Sin(topocentricLocalHourAngleRadians),
                    Math.Cos(topocentricLocalHourAngleRadians) * Math.Sin(latitudeRadians) -
                        Math.Tan(topocentricDeclinationRadians) * Math.Cos(latitudeRadians));

            return MathUtilities.LimitDegrees0To360(MathUtilities.ConvertRadiansToDegrees(astronomicalTopocentricAzimuth));
        }

        /// <summary>
        /// Calculates the topocentric azimuth (geodesic azimuth) from a given astronomical topocentric azimuth
        /// </summary>
        /// <param name="astronomicalTopocentricAzimuth">The astronomical topocentric azimuth in degrees</param>
        /// <returns>Returns the topocentric azimuth in degrees</returns>
        private static double CalculateTopocentricAzimuth(double astronomicalTopocentricAzimuth)
        {
            return MathUtilities.LimitDegrees0To360(astronomicalTopocentricAzimuth + 180.0);
        }
        #endregion
    }
}