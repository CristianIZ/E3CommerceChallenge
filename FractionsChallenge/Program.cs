using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FractionsChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Calculadora simplificadora de fracciones");

                    Console.WriteLine("Ingrese numerador");
                    var num = Console.ReadLine();
                    Validate(num, false);

                    Console.WriteLine("Ingrese denominador");
                    var div = Console.ReadLine();
                    Validate(div, true);

                    var simplifiedFraction = CalculateResult(int.Parse(num), int.Parse(div));
                    Console.WriteLine($"El resultado es: {simplifiedFraction}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Intentemos nuevamente");
                }

                Console.WriteLine("Presione cualquier tecla para realizar un nuevo calculo");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static List<int> GetCommonMultiples(List<int> listA, List<int> listB)
        {
            var result = new List<int>();

            foreach (var a in listA)
            {
                if (listB.Contains(a))
                    result.Add(a);
            }

            return result;
        }

        private static List<int> GetAllDividers(int number, int limiter = int.MaxValue - 1)
        {
            var dividers = new List<int>();

            for (int i = 2; i <= number; i++)
            {
                if (number % i == 0)
                    dividers.Add(i);

                if (i >= limiter)
                    break;
            }

            return dividers;
        }

        private static void Validate(string number, bool isDivider)
        {
            try
            {
                if (number.Contains(" "))
                    throw new Exception("No se admiten espacios");

                if (string.IsNullOrWhiteSpace(number))
                    throw new Exception("No se detecto ningun valor ingresado");

                if (number.Contains(".") || number.Contains(","))
                    throw new Exception("No se admiten separadores de miles ni numeros decimales");

                var regex = new Regex(@"^\d+$");
                if (!regex.Match(number).Success)
                    throw new Exception("Solo se admiten valores numericos");

                if (number.Length > 10)
                    throw new Exception($"El valor es demasiado grande, intente con un valor mas chico");

                long val;
                var longRes = long.TryParse(number, out val);
                if (val > int.MaxValue - 1)
                    throw new Exception($"El valor es demasiado grande, intente con un valor mas chico");

                int value;
                var result = int.TryParse(number, out value);
                if (!result)
                    throw new Exception($"No es posible convertir el valor: {number} en un entero");

                if (isDivider && value == 0)
                    throw new Exception("No es posible dividir por 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string CalculateResult(int numerator, int divider)
        {
            if (numerator == 0)
                return "0";

            if (numerator == divider)
                return "1";

            List<int> numDividers, divDividers;

            if (numerator > divider)
            {
                divDividers = GetAllDividers(divider);

                if (divDividers.Count > 0)
                    numDividers = GetAllDividers(numerator, divDividers.Max());
                else
                    numDividers = new List<int>();
            }
            else
            {
                numDividers = GetAllDividers(numerator);

                if (numDividers.Count > 0)
                    divDividers = GetAllDividers(divider, numDividers.Max());
                else
                    divDividers = new List<int>();
            }

            var commonMultiples = GetCommonMultiples(divDividers, numDividers).ToList();

            if (commonMultiples.Count > 0)
            {
                var simplifiedNumerator = numerator / commonMultiples.Max();
                var simplifiedDivider = divider / commonMultiples.Max();

                if (simplifiedDivider == 1)
                    return simplifiedNumerator.ToString();
                else
                    return $"{simplifiedNumerator}/{simplifiedDivider}";
            }
            else
            {
                if (divider == 1)
                    return numerator.ToString();
                else
                    return $"{numerator}/{divider}";
            }
        }
    }
}
