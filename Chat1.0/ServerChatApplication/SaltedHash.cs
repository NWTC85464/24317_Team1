using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ServerChatApplication
{
    //This class will be used to generate Salts and Hash passwords
    public class SaltedHash
    {
        //Fields
        //CSPRNG object to generate secure random values
        private static RNGCryptoServiceProvider RNG = new RNGCryptoServiceProvider();

        //Methods
        //This method will create a salt value when a user signs up
        public static byte[] CreateSalt()
        {
            //Creates a byte array that holds 32 values
            //This will hold a randomly generated Salt value
            byte[] salt = new byte[32];

            //Using the CSPRNG method getbytes generates
            //and fills the array with random values
            RNG.GetBytes(salt);

            //return the new salt value as a byte array
            return salt;
        }

        //This method will be used when user signs up
        //A Salted Hash will be created and stored in the database instead of the password
        //the user created password is passed in as a parameter
        public static byte[] CreateSaltedHash(string password)
        {
            //Creates object for hashing slated password
            HashAlgorithm alg = new SHA256Managed();

            //convert user password to a byte array
            byte[] passwordByte = Encoding.UTF8.GetBytes(password);

            //Uses CreateSalt method to return salt array
            //TODO This salt value will need to be stored in the database user table and retrieved upon user password validation
            byte[] salt = CreateSalt();

            //Create new array to store salted hash
            byte[] saltedHash = new byte[salt.Length + passwordByte.Length];

            //Insert the salted array values into the hash array using a loop
            for (int i = 0; i<salt.Length; i++)
            {
                saltedHash[i] = salt[i];
            }
            
            //Insert the password byte array into the hash array using a loop
            //The salt will always come before the password
            for(int i = 0; i<passwordByte.Length; i++)
            {
                saltedHash[i + salt.Length] = passwordByte[i];
            }

            //use the HashAlgorithm ComputeHash method to hash the array that holds the salt and
            //password. Return the salted hash.
            //TODO this will need to be stored in database and validated upon user login
            return alg.ComputeHash(saltedHash);
        }

    }
}
