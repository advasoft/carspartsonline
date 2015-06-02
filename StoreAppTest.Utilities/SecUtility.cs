﻿

namespace StoreAppTest.Web.Utilities
{
    using System;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.Text;

    public static class SecUtility
    {

        /// <summary>
        /// Decrypts the specified encryption key.
        /// </summary>
        /// <param name="encryptionKey">The encryption key.</param>
        /// <param name="cipherString">The cipher string.</param>
        /// <param name="useHashing">if set to <c>true</c> [use hashing].</param>
        /// <returns>
        ///  The decrypted string based on the key
        /// </returns>
        public static string Decrypt(string encryptionKey, string cipherString)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);


            //if (useHashing)
            //{
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(encryptionKey));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            //}
            //else
            //{
            //    //if hashing was not implemented get the byte code of the key
            //    keyArray = UTF8Encoding.UTF8.GetBytes(encryptionKey);
            //}

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Encrypts the specified to encrypt.
        /// </summary>
        /// <param name="toEncrypt">To encrypt.</param>
        /// <param name="useHashing">if set to <c>true</c> [use hashing].</param>
        /// <returns>
        /// The encrypted string to be stored in the Database
        /// </returns>
        public static string Encrypt(string encryptionKey, string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();

            ////If hashing use get hashcode regards to your key
            //if (useHashing)
            //{
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(encryptionKey));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            //}
            //else
            //    keyArray = UTF8Encoding.UTF8.GetBytes(encryptionKey);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}