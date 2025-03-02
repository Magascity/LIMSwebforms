using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WaterCorp.Usermanagement
{
    public class PasswordGenerator
    {
        private static readonly Random random = new Random();
        private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string Digits = "0123456789";
        private const string SpecialCharacters = "!@#$%^&*()_-+=<>?";

        public static string GenerateStrongPassword(int length)
        {
            if (length < 8)
            {
                throw new ArgumentException("Password length must be at least 8 characters.");
            }

            StringBuilder password = new StringBuilder();
            StringBuilder characterPool = new StringBuilder();

            // Ensure the password contains at least one character from each category
            password.Append(GetRandomCharacter(UppercaseLetters));
            password.Append(GetRandomCharacter(LowercaseLetters));
            password.Append(GetRandomCharacter(Digits));
            password.Append(GetRandomCharacter(SpecialCharacters));

            // Add all characters to the character pool
            characterPool.Append(UppercaseLetters);
            characterPool.Append(LowercaseLetters);
            characterPool.Append(Digits);
            characterPool.Append(SpecialCharacters);

            // Fill the rest of the password length with random characters from the character pool
            for (int i = password.Length; i < length; i++)
            {
                password.Append(GetRandomCharacter(characterPool.ToString()));
            }

            // Shuffle the password to ensure randomness
            return ShufflePassword(password.ToString());
        }

        private static char GetRandomCharacter(string characters)
        {
            int index = random.Next(characters.Length);
            return characters[index];
        }

        private static string ShufflePassword(string password)
        {
            char[] array = password.ToCharArray();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                char temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
            return new string(array);
        }

    }
}