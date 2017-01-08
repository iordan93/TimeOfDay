# TimeOfDay
## Introduction
This project contains methods to calculate the Sun's position in the sky. It also contains a few helper mathematical functions.

It can be used in two ways: 
* Directly - to obtain astronomically significant data (such as the horizontal coordinates of the Sun, or some intermediate values)
* Indirectly - to check the part of the day given a location (latitude, longitude), and local date and time.

The algorithm to check the Sun's position is based on NREL's Solar Position Algorithm.

More information about the algorithm and its implementation can be found in the source code, comments and tests.

## Usage
* To get the part of the day, given the observer's coordinates and local time, use the PartOfDayChecker.GetPartOfDay() method.
There are two overloads of this method, depending on what information you want to pass and how you want the algorithm to behave.
The method returns a PartOfDay enumeration value (Night, Dawn, Day, Dusk, Unknown).

* To calculate the Sun's position in the sky, call SunPositionCalculator.CalculateSunPosition() and pass it a SunData object.
A SunData object contains several parameters: input, intermediate and output parameters. In order for the algorithm to work
correctly, you need to pass all input parameters, or at least substitute some default values.
	
The algorithm calculates all intermediate and output values, modifying the SunData argument. The Sun's position is in the three
variables - AstronomicalTopocentricZenithAngle / TopocentricZenithAngle (which can be used interchangeably), and TopocentricZenithAngle.
	
* To use some of the math helpers, just call the public static methods from the MathUtilities class.

## References
1. [Reda, I.; Andreas, A. (2003). Solar Position Algorithm for Solar Radiation Applications. 55 pp.; NREL Report No. TP-560-34302, Revised January 2008](http://www.nrel.gov/docs/fy08osti/34302.pdf)
2. https://www.nrel.gov/midc/spa/