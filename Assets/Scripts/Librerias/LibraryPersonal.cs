using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace LibraryPersonal
{
    public struct UtilNames
    {
        public const string PLAYER_DATA = "playerdata.dat";
    }
    public static class Datos
    {
        public static string RandomId(int LengthNewId = 15, bool Mayusculas = true, bool minusculas = true, bool simbolos = true)
        {
            if (!Mayusculas && !minusculas && !simbolos || LengthNewId <= 0)
                return "Null";
            string newId = "";
            string abecedarioMayus = "A-B-C-D-E-F-G-H-I-J-K-L-M-N-Ñ-O-P-Q-R-S-T-U-V-W-X-Y-Z";
            string abecedarioMinus = "a-b-c-d-e-f-g-h-i-j-k-l-m-n-ñ-o-p-q-r-s-t-u-v-w-x-y-z";
            string caracteres = "-_-/--&-";
            Random r = new Random();

            int actualLength = 0;
            while(actualLength < LengthNewId)
            {
                int value = r.Next(0, 1);
                string letra = "";
                switch (value)
                {
                    case 0:
                        int proba_Char = r.Next(0, 5);
                        if (Mayusculas)
                        {
                            if (proba_Char == 2)
                            {
                                if (simbolos)
                                {
                                    letra = caracteres.Split('-')[r.Next(0, 4)] + abecedarioMayus.Split('-')[r.Next(0, 26)];
                                    actualLength++;
                                }
                            }
                            else
                            {
                                letra = abecedarioMayus.Split('-')[r.Next(0, 26)];
                                actualLength++;
                            }
                        }
                        break;
                    case 1:
                        int proba_Char2 = r.Next(0, 5);
                        if (minusculas)
                        {
                            if (proba_Char2 == 3)
                            {
                                if (simbolos)
                                {
                                    letra = caracteres.Split('-')[r.Next(0, 4)] + abecedarioMinus.Split('-')[r.Next(0, 26)];
                                    actualLength++;
                                }
                            }
                            else
                            {
                                letra = abecedarioMinus.Split('-')[r.Next(0, 26)];
                                actualLength++;
                            }
                        }
                        break;
                }
                newId = newId + letra;
            }
            return newId;
        }

        /// <summary>
        /// Extensión que devuelve un Objeto apartir de un string que contiene el JSON
        /// </summary>
        /// <typeparam name="T">Corresponde a la clase que devolverá</typeparam>
        /// <param name="s">Corresponde al string que contiene el JSON del objeto</param>
        /// <returns></returns>
        public static T Deserializar<T>(this string s, bool encrypt = true)
        {
            try
            {
                if (encrypt)
                    return JsonUtility.FromJson<T>(s.Desencrypt());
                return JsonUtility.FromJson<T>(s);
            }
            catch (Exception)
            {
                if (encrypt)
                    s = s.Desencrypt();
                object obj = JsonUtility.FromJson<object>(s);
                T data = default;
                foreach (var objProperties in obj.GetType().GetProperties())
                {
                    foreach (var dataProperties in data.GetType().GetProperties())
                    {
                        if (objProperties.Name == dataProperties.Name)
                            data.GetType().GetProperty(objProperties.Name).SetValue(objProperties.GetType(), objProperties.GetValue(objProperties.GetType()));
                    }
                }
                return data;
            }
        }
        /// <summary>
        /// Extensión que devuelve un JSON del objeto extensionado
        /// </summary>
        /// <param name="o">Corresponde al objeto a Serializar</param>
        /// <returns></returns>
        public static string Serializar(this object o, bool encrypt = true)
        {
            if (encrypt)
                return JsonUtility.ToJson(o).Encrypt();
            return JsonUtility.ToJson(o);
        }
        static T Instancia<T>()
        {
            T obj = default;
            if (typeof(T).IsValueType)
            {
                obj = default;
            }
            else if (typeof(T) == typeof(string))
            {
                obj = (T)Convert.ChangeType(string.Empty, typeof(T));
            }
            else
            {
                obj = Activator.CreateInstance<T>();
            }
            return obj;
        }

        static AndroidJavaClass environment;
        public static string GetPath {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    environment = new AndroidJavaClass("android.os.Environment");
                    using (AndroidJavaObject externalStorageDirectory = environment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
                    {
                        string root = externalStorageDirectory.Call<string>("getPath") + "/" + Application.companyName;

                        return root + "/Sergio Ribera/" + Application.productName + "/";
                    }
                }
                return "C://Sergio Ribera/" + Application.productName + "/";
            }
        }
        public static bool Exist(string nameFile)
        {
            string path = GetPath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += nameFile;
            return File.Exists(path);
        }
        public static T Save<T>(this object o, string nameFile)
        {
            string path = GetPath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += nameFile;
            File.WriteAllText(path, o.Serializar());
            return (T)o;
        }
        public static T Load<T>(this object o, string nameFile)
        {
            string path = GetPath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (o == null) return Instancia<T>();
            path += nameFile;
            if (!File.Exists(path)) return Instancia<T>();
            o = File.ReadAllText(path).Deserializar<T>();
            return (T)o;
        }
        public static T Load<T>(string nameFile)
        {
            string path = GetPath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path += nameFile;
            if (!File.Exists(path)) return Instancia<T>();
            return File.ReadAllText(path).Deserializar<T>();
        }
        public static void Foreach(this List<string> l, Action<string> a)
        {
            if (l.Count > 0)
            {
                foreach (var i in l)
                {
                    a(i);
                }
            }
            else
                a(null);
        }

        static readonly string key = "Key Where Cube Shooter";
        /// <summary>
        /// es =Devuelve un string encriptado
        /// </summary>
        /// <param name="text">Establece el string a Encriptar</param>
        /// <returns></returns>
        public static string Encrypt(this string text)
        {
            //arreglo de bytes donde guardaremos la llave
            byte[] keyArray;
            //arreglo de bytes donde guardaremos el texto
            //que vamos a encriptar
            byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(text);
            //se utilizan las clases de encriptación
            //provistas por el Framework
            //Algoritmo MD5
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            //se guarda la llave para que se le realice
            //hashing
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            //Algoritmo 3DAS
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            //se empieza con la transformación de la cadena
            ICryptoTransform cTransform = tdes.CreateEncryptor();

            //arreglo de bytes dond
            byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);

            tdes.Clear();

            //se regresa el resultado en forma de una cadena
            return Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
        }
        /// <summary>
        /// es = Devuelve un string Desencriptado
        /// </summary>
        /// <param name="text">Establece el string a Desencriptar</param>
        /// <returns></returns>
        public static string Desencrypt(this string text)
        {
            byte[] keyArray;
            //convierte el texto en una secuencia de bytes
            byte[] Array_a_Descifrar = Convert.FromBase64String(text);

            //se llama a las clases que tienen los algoritmos
            //de encriptación se le aplica hashing
            //algoritmo MD5
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tdes.CreateDecryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);

            tdes.Clear();

            //se regresa en forma de cadena
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static bool IsEmpty(string[] ss)
        {
            bool b = false;
            foreach (var s in ss)
            {
                b = string.IsNullOrEmpty(s) && string.IsNullOrWhiteSpace(s);
                if (b) throw new Exception(string.Format("El string  {0}  está vacio, es nulo o es solo un espacio e blanco", ss));
            }
            return b;
        }
        public static bool IsEmpty(List<string> ss)
        {
            bool b = false;
            foreach (var s in ss)
            {
                b = string.IsNullOrEmpty(s) && string.IsNullOrWhiteSpace(s);
                if (b) throw new Exception(string.Format("El string  {0}  está vacio, es nulo o es solo un espacio e blanco", ss));
            }
            return b;
        }
        public static bool IsEmpty(string ss)
        {
            bool b = string.IsNullOrEmpty(ss) && string.IsNullOrWhiteSpace(ss);
            if (b) throw new Exception(string.Format("El string  {0}  está vacio, es nulo o es solo un espacio e blanco", ss));
            return b;
        }

        public static bool IsSimilarString(this string s1, string s2)
        {
            bool a = false;

            if (s1.Contains(s2) || s1.ToLower().Contains(s2.ToLower()))
                a = true;

            return a;
        }
        public static string ToFirstUpper(this string s)
        {
            //Separamos el string por espacios para obtener las palabras
            string[] palabras = s.Trim().Split(' ');
            string sR = "";
            //hacemos un recorrido por el array de palabras
            for (int i = 0; i < palabras.Length; i++)
            {
                //Convertimos la palabra en array de caracteres
                char[] letters = palabras[i].ToCharArray();
                if (letters.Length > 1)
                {
                    //si es una palabra mayor a 1 letra y verificamos que son letras
                    if (char.IsLetter(letters[0]))
                        letters[0] = char.Parse(letters[0].ToString().ToUpper());//hacemos el primer caracter de la palabra mayuscula
                }
                //remplazamos la palabra por los nuevos caracteres editados
                palabras[i] = string.Join("", letters);
            }
            //recorremos nuevamente las palabras armando el texto original nuevamente
            foreach (var word in palabras)
            {
                sR += word + " ";
            }
            //recortamos espacios en blanco del principio y el final
            return sR.Trim();
        }

        public static string Format(this string format, params object[] args)
        {
            string newString = format;
            for (int i = 0; i < args.Length; i++)
            {
                string rep = "{" + i + "}";
                newString = newString.Replace(rep, args[i].ToString());
            }
            return newString;
        }
        public static string Compatibilizate(this string s)
        {
            return s.Replace(" ", "");
        }
        public static string Decompatibilizar(this string s)
        {
            //Separamos el string por espacios para obtener las palabras
            string sR = "";
            List<string> palabras = new List<string>();
            //Convertimos la palabra en array de caracteres
            char[] letters = s.ToCharArray();
            //si es una palabra mayor a 1 letra y verificamos que son letras
            for (int i = 0; i < letters.Length; i++)
            {
                if (char.IsLetter(letters[i]))
                    if (char.IsUpper(letters[i]))
                        sR += " ";
                sR += letters[i].ToString();
            }
            //recortamos espacios en blanco del principio y el final
            return sR.Trim();
        }
        public static string RemoveQuotes(this string Value)
        {
            return Value.Replace("\"", "");
        }
        public static float TwoDecimal(this float v)
        {
            return (float) Math.Round(v);
        }
    }    
    public static class ExtraToolColor
    {
        public static string ToHex(this Color color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }
        public static Color hexToColor(this string hex)
        {
            if (hex.Contains("0x"))
                hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
            if (hex.Contains("#"))
                hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
            byte a = 255;//assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }
        public static string GetRandomColor()
        {
            string[] c = { "#98FF2A", "#2AFFBA", "#AF2AFF", "#FC2AFF", "#FF2AA5", "#FF2A49", "#FF642A", "#FFBE2A", "#F0FF2A", "#2AFF2B", "#FF2A2E" };
            Random r = new Random();
            int i = r.Next(0, c.Length - 1);
            return c[i];
        }
    }
}