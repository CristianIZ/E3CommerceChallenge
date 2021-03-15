using System;
using System.Text.RegularExpressions;

namespace NameValidatorChallenge
{
    class Program
    {
        // Condicion: Primer letra mayuscula, el resto minuscula y no tener caracteres extra
        static Regex wordValidator = new Regex("^[A-Z]{1}[a-z]+$");
        static Regex lowerCaseValidator = new Regex("[a-z]+$");
        static Regex upperCaseValidator = new Regex("[A-Z]+$");

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Calculadora simplificadora de fracciones");

                    Console.WriteLine("Ingrese nombre");
                    var name = Console.ReadLine();
                    Validate(name);

                    Console.WriteLine($"El nombre es valido");
                }
                catch (InvalidInputException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Intentemos nuevamente");
                }
                catch (Exception ex)
                {
                    //Log
                    Console.WriteLine("Algo salio mal");
                    Console.WriteLine("Intentemos nuevamente");
                }

                Console.WriteLine("Presione cualquier tecla para realizar un nuevo calculo");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static bool Validate(string completeName)
        {
            var split = completeName.Split(" ");

            if (split.Length == 2)
                return TwoNamesValidation(split[0], split[1]);

            if (split.Length == 3)
                return ThreeNamesValidation(split[0], split[1], split[2]);

            throw new InvalidInputException("No posee el formato 2 nombres y 1 apellido o 1 apellido y 1 nombre (regla c)");
        }

        public static bool TwoNamesValidation(string name, string lastName)
        {
            if (IsValidInitial(name) && IsValidName(lastName))
                return true;

            if (IsValidName(name) && IsValidName(lastName))
                return true;

            throw new InvalidInputException("Solo se admite inicial como nombre y apellido completo para 2 terminos");
        }

        public static bool ThreeNamesValidation(string firstName, string secondName, string lasName)
        {
            // 2 iniciales y un apellido
            if (IsValidInitial(firstName) && IsValidInitial(secondName) && IsValidName(lasName))
                return true;

            // nombre, inicial, apellido
            if (IsValidName(firstName) && IsValidInitial(secondName) && IsValidName(lasName))
                return true;

            // Nombre nombre nombre
            if (IsValidName(firstName) && IsValidName(secondName) && IsValidName(lasName))
                return true;

            throw new InvalidInputException("Solo se admite iniciales y apellido, o");
        }

        /// <summary>
        /// Determina si es una incial valida (si no es una inicial devuelve false)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidInitial(string name)
        {
            if (name.Length == 1)
                throw new InvalidInputException($"La letra debe ser mayusculas y seguida de un punto para ser considerada una inicial");

            // Si tiene un punto debe tener una sola letra mayuscula antecesora
            if (name.Contains('.'))
            {
                if (name.Length != 2)
                    throw new InvalidInputException($"No valido (contiene mas de una letra para la inicial: {name})");

                if (!upperCaseValidator.Match(name.Substring(0, 1)).Success)
                    throw new InvalidInputException($"Solo se permite 1 letra mayuscula antes de un punto para la inicial");

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determina si es un nombre valido
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidName(string name)
        {
            // Verifico si es un nombre con formato "Nombre" (primer letra mayuscula resto en minuscula)
            if (!wordValidator.Match(name).Success)
                throw new InvalidInputException($"Los nombres o apellidos deben comenzar con 1 letra mayuscula y el resto en minusculas");

            return true;
        }
    }

    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message)
        {
        }
    }
}
