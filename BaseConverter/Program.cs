using System;

namespace BaseConverter
{
    class Program
    {

        public static int MIN_BASE { get { return 2; } }
        public static int MAX_BASE { get { return 36; } }

       
        public static string ConvertFromBaseToBase(string inputNumberText, int inputBase, int outputBase)
        {
            long numberAsLong = ConvertFromBaseToLong(inputNumberText, inputBase);
            return ConvertFromLongToBase(numberAsLong, outputBase);
        }

    
        public static long ConvertFromBaseToLong(string inputNumberText, int inputBase)
        {
            long convertedNumber = 0;
            int baseIndex = 0;

            if (inputBase < MIN_BASE || inputBase > MAX_BASE)
            {
                throw new ArgumentException("Input Base Provided '" + inputBase + "' is not between " + MIN_BASE + " and " + MAX_BASE);
            }

            for (int i = inputNumberText.Length - 1; i >= 0; i--)
            {
                int numericalChar = ConvertNumericalCharToInt(inputNumberText[i]);
                long actualNumberValue = checked(numericalChar * (long)Math.Pow(inputBase, baseIndex));
                convertedNumber = checked(convertedNumber + actualNumberValue);
                baseIndex++;
            }
            return convertedNumber;
        }

        public static string ConvertFromLongToBase(long inputNumber, int outputBase)
        {
            string convertedNumber = "";
            int largestBaseIndex = 0;
            int numberAtBase = 0;
            long remainingNumber = inputNumber;

            if (inputNumber == 0)
            {
                return "0";
            }

            if (outputBase < MIN_BASE || outputBase > MAX_BASE)
            {
                throw new ArgumentException("Output Base Provided '" + outputBase + "' is not between " + MIN_BASE + " and " + MAX_BASE);
            }

            while (((long)(inputNumber / Math.Pow(outputBase, largestBaseIndex))) >= 1)
            {
                largestBaseIndex++;
            }
            largestBaseIndex--;

            for (int i = largestBaseIndex; i >= 0; i--)
            {
                long nextBaseValue = (long)Math.Pow(outputBase, i);
                numberAtBase = (int)(remainingNumber / nextBaseValue);

                if (numberAtBase > 0)
                {
                    char convertedChar = ConvertIntToNumericalChar(numberAtBase);

                    convertedNumber = convertedNumber + convertedChar;
                    remainingNumber -= numberAtBase * nextBaseValue;

                }
                else
                {
                    convertedNumber = convertedNumber + '0';
                }
            }

            return convertedNumber;
        }

       
        public static bool ValidateBaseString(string inputNumberText, int inputBase)
        {

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

       
        private static char ConvertIntToNumericalChar(int number)
        {
            if (number >= 0 && number <= 9)
            {
                return (char)(number + (int)'0');

            }
            else if (number >= 10 && number <= 35)
            {
                return (char)((number - 10) + (int)'A');

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
