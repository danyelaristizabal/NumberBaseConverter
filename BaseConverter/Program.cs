using System;

namespace BaseConverter
{
    class Program
    {

        public static int MIN_BASE { get { return 2; } }
        public static int MAX_BASE { get { return 36; } }

        /*
        * Function: Convert From Base To Base
        *
        * Purpose: Converts a string containing a number at a specific base into a string representation at another
        *            base.
        *
        * Input: inputNumberText (string) - A String representation of the number we want to convert [ie: "204F"]
        *        inputBase (int) - The base of the original number.
        *        outputBase (int) - The base to convert the input number text into.
        *
        * Output: string - A numerical string corresponding to the input string at the specified base.
        *
        * Exceptions: ArgumentException- Encountered if an invalid input or output base is passed.
        *             OverflowException- Encounterd if the original input number or converted number
        *                                  exceeds the maximum size of a long.
        */
        public static string ConvertFromBaseToBase(string inputNumberText, int inputBase, int outputBase)
        {
            long numberAsLong = ConvertFromBaseToLong(inputNumberText, inputBase);
            return ConvertFromLongToBase(numberAsLong, outputBase);
        }

        /*
        * Function: Convert From Base To Long
        *
        * Purpose: Converts a string representation of a number at a specific base, and convert it to a long.
        *
        * Input: inputNumberText (string) - A String representation of the number we want to convert [ie: "204F"]
        *        inputBase (int) - The base of the original number.
        *
        * Output: long - A long number corresponding to the input string at the specified base.
        *
        * Exceptions: ArgumentException- Encountered if an invalid base is passed.
        *             OverflowException- Encounterd if the original input number or converted number
        *                                  exceeds the maximum size of a long.
        */
        public static long ConvertFromBaseToLong(string inputNumberText, int inputBase)
        {
            long convertedNumber = 0;
            int baseIndex = 0;

            //check if valid input base based
            if (inputBase < MIN_BASE || inputBase > MAX_BASE)
            {
                throw new ArgumentException("Input Base Provided '" + inputBase + "' is not between " + MIN_BASE + " and " + MAX_BASE);
            }

            //start at most insignificant bit, and move backwards towards the most significant bit
            //  converting the text number to an int and adding it to our converted number.
            for (int i = inputNumberText.Length - 1; i >= 0; i--)
            {
                int numericalChar = ConvertNumericalCharToInt(inputNumberText[i]);
                long actualNumberValue = checked(numericalChar * (long)Math.Pow(inputBase, baseIndex));
                convertedNumber = checked(convertedNumber + actualNumberValue);
                baseIndex++;
            }
            return convertedNumber;
        }

        /*
        * Function: Convert From Long to Base
        *
        * Purpose: Converts a Long number to its string represenation given a specific output base.  Returns
        *            a blank string if an invalid output base is provided
        *
        * Input: inputNumber (long) - The long number that you would like converted to a base string.
        *        outputBase (int) - The base that you want the inputNumber converted into.
        *
        * Output: string - A string representing the inputNumber at the passed output base.
        *
        * Exceptions: ArgumentException- Encountered if an invalid output base is passed.
        */
        public static string ConvertFromLongToBase(long inputNumber, int outputBase)
        {
            string convertedNumber = "";
            int largestBaseIndex = 0;
            int numberAtBase = 0;
            long remainingNumber = inputNumber;

            //check special case (0)
            if (inputNumber == 0)
            {
                return "0";
            }

            //check if valid output base provided
            if (outputBase < MIN_BASE || outputBase > MAX_BASE)
            {
                throw new ArgumentException("Output Base Provided '" + outputBase + "' is not between " + MIN_BASE + " and " + MAX_BASE);
            }

            //figure out largest base position
            while (((long)(inputNumber / Math.Pow(outputBase, largestBaseIndex))) >= 1)
            {
                largestBaseIndex++;
            }
            largestBaseIndex--;

            //create the output base string
            for (int i = largestBaseIndex; i >= 0; i--)
            {
                long nextBaseValue = (long)Math.Pow(outputBase, i);
                numberAtBase = (int)(remainingNumber / nextBaseValue);

                //check if we need to convert and add next character
                if (numberAtBase > 0)
                {
                    //convert found non-zero char
                    char convertedChar = ConvertIntToNumericalChar(numberAtBase);

                    //add converted character and subtract from remainer
                    convertedNumber = convertedNumber + convertedChar;
                    remainingNumber -= numberAtBase * nextBaseValue;

                    //else, next digit is a 0
                }
                else
                {
                    convertedNumber = convertedNumber + '0';
                }
            }

            return convertedNumber;
        }

        /*
        * Function: Validate Base String
        *
        * Purpose: Take an integer number and convert it to its base representation. 
        *
        * Input: inputNumberText (string) - The text number you would like validated.
        *        inputBase (int) - The base that the number string is in.
        *        
        * Output: bool - True: Valid number string for given base.
        *                False:  Invalid number string for given base.
        */
        public static bool ValidateBaseString(string inputNumberText, int inputBase)
        {

            //loop through each character of string and check if invalid
            foreach (char nextChar in inputNumberText)
            {
                try
                {
                    int intValue = ConvertNumericalCharToInt(nextChar);

                    if (intValue >= inputBase)
                    {
                        return false;
                    }
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }

            return true;

        }

        /*
        *
        * Function: Convert Numerical Char To Int
        *
        * Purpose: Take a char and convert it to its respective int.
        *
        * Input: numericalChar (char) - The char you would like converted.  Expects a character between '0'-'9' or 'A'-'Z'.
        *
        * Output: char - The character represenation of the int based (for base numbers).
        *
        * Exceptions: ArgumentException - If the provided character is outside the range of base characters 0-9 and A-Z
        */
        private static int ConvertNumericalCharToInt(char numericalChar)
        {
            if (numericalChar >= '0' && numericalChar <= '9')
            {
                return (int)numericalChar - (int)'0';
            }
            else if ((int)numericalChar >= (int)'A' && (int)numericalChar <= (int)'Z')
            {
                return (int)numericalChar - (int)'A' + 10;
            }
            else
            {
                throw new ArgumentException("The numerical character '" + numericalChar + "' is outside the range of base characters!");
            }

        }

        /*
        * Function: Convert Int To Numerical Char
        *
        * Purpose: Take an integer number and convert it to its base representation.
        *
        * Input: number (int) - The number you would like converted.  Expects a number between  0 and 35 (0-Z).
        *
        * Output: char - The character represenation of the int based (for base numbers).
        *
        * Exceptions: ArgumentException - If the provided number is outside the range of base characters 0-9 and A-Z
        */
        private static char ConvertIntToNumericalChar(int number)
        {
            //between numerical 0-9
            if (number >= 0 && number <= 9)
            {
                return (char)(number + (int)'0');

                //else between A-Z
            }
            else if (number >= 10 && number <= 35)
            {
                return (char)((number - 10) + (int)'A');

                //invlaid, return '-'
            }
            else
            {
                throw new ArgumentException("The number '" + number + "' is outisde the range of base characters!");
            }

        }

        static void Main(string[] args)
        {

            Console.WriteLine("Please write the number that you want to change to another Mathematical Base " );

            Console.WriteLine(ConvertFromBaseToBase("12", 10, 16)); 



        }
    }
}
