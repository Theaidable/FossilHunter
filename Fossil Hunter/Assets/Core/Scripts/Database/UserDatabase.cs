using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// Database klasser
///<author> David Gudmund Danielsen </author>
namespace Database
{
    public enum UserRole
    {
        Student,
        Teacher
    }

    [Serializable]
    public class UserRecord
    {
        public string Username;
        public string PasswordHash;
        public string Salt;
        public UserRole Role;
    }

    [Serializable]
    public class UserDatabaseModel
    {
        public List<UserRecord> Users = new List<UserRecord>();
    }

    public static class UserDatabase
    {
        private static readonly string FilePath = Path.Combine(Application.persistentDataPath, "users.json");
        private static UserDatabaseModel _cache;
       
        private static UserDatabaseModel LoadDatabase()
        {
            if(_cache != null)
            {
                return _cache;
            }

            if(File.Exists(FilePath) == false)
            {
                _cache = new UserDatabaseModel();
                return _cache;
            }

            try
            {
                string json = File.ReadAllText(FilePath);
                _cache = JsonUtility.FromJson<UserDatabaseModel>(json);

                if(_cache == null)
                {
                    _cache = new UserDatabaseModel();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load userdatabase: {ex}");
                _cache = new UserDatabaseModel();
            }

            return _cache;
        }

        private static void SaveDatabase()
        {
            try
            {
                string json = JsonUtility.ToJson(_cache, true);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save userdatabase: {ex}");
            }
        }

        public static bool TryCreateUser(string username, string password, UserRole role, out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(username))
            {
                error = "Username må ikke være tomt.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                error = "Password må ikke være tomt.";
                return false;
            }

            var database = LoadDatabase();

            if (database.Users.Exists(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)))
            {
                error = "Brugernavnet er allerede taget.";
                return false;
            }

            string salt = GenerateSalt();
            string hash = ComputeHash(password, salt);

            var record = new UserRecord
            {
                Username = username,
                PasswordHash = hash,
                Salt = salt,
                Role = role
            };

            database.Users.Add(record);
            SaveDatabase();
            return true;
        }

        public static bool TryValidateUser(string username, string password, out UserRecord user, out string error)
        {
            error = null;
            user = null;

            var database = LoadDatabase();

            user = database.Users.Find(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

            if(user == null)
            {
                ComputeHash(password,GenerateSalt());
                error = "Forkert brugernavn eller password.";
                return false;
            }

            string hash = ComputeHash(password, user.Salt);

            if(string.Equals(hash,user.PasswordHash,StringComparison.Ordinal) == false)
            {
                error = "Forkert brugernavn eller password.";
                user = null;
                return false;
            }

            return true;
        }

        private static string GenerateSalt(int size = 16)
        {
            byte[] bytes = new byte[size];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            return Convert.ToBase64String(bytes);
        }

        private static string ComputeHash(string password, string salt)
        {
            using (var sha = SHA256.Create())
            {
                byte[] input = Encoding.UTF8.GetBytes(password + salt);
                byte[] hash = sha.ComputeHash(input);
                return Convert.ToBase64String(hash);
            }
        }

        public static void EnsureTestUsers()
        {
            string error;

            TryCreateUser("Teacher", "1234", UserRole.Teacher, out error);
            TryCreateUser("Student1", "1234", UserRole.Student, out error);
            TryCreateUser("Student2", "1234", UserRole.Student, out error);

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogWarning($"Seeding test users: {error}");
            }
        }
    }
}
