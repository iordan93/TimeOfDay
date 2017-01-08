namespace TimeOfDay
{
    using System;

    /// <summary>
    /// Contains mathematical utility methods
    /// </summary>
    public static class MathUtilities
    {
        /// <summary>
        /// Converts an angle in radians to degrees
        /// </summary>
        /// <param name="radians">The angle in radians</param>
        /// <returns>Returns the angle in degrees</returns>
        public static double ConvertRadiansToDegrees(double radians)
        {
            return 180.0 / Math.PI * radians;
        }

        /// <summary>
        /// Converts an angle in degrees to radians
        /// </summary>
        /// <param name="degrees">The angle in degrees</param>
        /// <returns>Returns the angle in radians</returns>
        public static double ConvertDegreesToRadians(double degrees)
        {
            return Math.PI / 180.0 * degrees;
        }

        /// <summary>
        /// Limits a given angle in degrees in the range 0 to 360 degrees inclusive
        /// </summary>
        /// <param name="degrees">The angle in degrees</param>
        /// <returns>Returns the limited angle</returns>
        public static double LimitDegrees0To360(double degrees)
        {
            double limitedDegrees = 0;
            degrees /= 360.0;
            limitedDegrees = 360.0 * (degrees - Math.Floor(degrees));
            if (limitedDegrees < 0)
            {
                limitedDegrees += 360.0;
            }

            return limitedDegrees;
        }

        /// <summary>
        /// Evaluates the third-degree polynomial a*x^3 + b*x^2 + c*x + d at point x
        /// </summary>
        /// <param name="a">The term in front of x^3</param>
        /// <param name="b">The term in front of x^2</param>
        /// <param name="c">The term in front of x</param>
        /// <param name="d">The constant term</param>
        /// <param name="x">The point to evaluate the polynomial at</param>
        /// <returns>Returns the value of the third-degree polynomial, evaluated at point x</returns>
        public static double CalculateThirdDegreePolynomialValue(double a, double b, double c, double d, double x)
        {
            return ((a * x + b) * x + c) * x + d;
        }
    }
}
