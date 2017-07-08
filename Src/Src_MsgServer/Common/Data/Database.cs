using GrandChase.IO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Manager.Factories;

namespace GrandChase.Data
{
    public class TemporaryConnection : IDisposable
    {
        private string oldHost;
        private string oldSchema;
        private string oldUsername;
        private string oldPassword;

        internal TemporaryConnection(string host, string schema, string username, string password)
        {
            this.oldHost = Database.Host;
            this.oldSchema = Database.Schema;
            this.oldUsername = Database.Username;
            this.oldPassword = Database.Password;

            Database.Host = host;
            Database.Schema = schema;
            Database.Username = username;
            Database.Password = password;
        }

        public void Dispose()
        {
            Database.Host = this.oldHost;
            Database.Schema = this.oldSchema;
            Database.Username = this.oldUsername;
            Database.Password = this.oldPassword;
        }
    }

    public class TemporarySchema : IDisposable
    {
        private string oldSchema;

        internal TemporarySchema(string schema)
        {
            this.oldSchema = Database.Schema;
            Database.Schema = schema;
        }

        public void Dispose()
        {
            Database.Schema = this.oldSchema;
        }
    }

    public static class Database
    {
        public static string Host { get; set; }
        public static string Schema { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }

        internal static string CorrectFields(string fields)
        {
            string final = string.Empty;
            string[] tokens = fields.Replace(",", " ").Replace(";", " ").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int processed = 0;

            foreach (string field in tokens)
            {
                final += field;
                processed++;

                if (processed < tokens.Length)
                {
                    final += ", ";
                }
            }

            return final;
        }

        internal static string ConnectionString
        {
            get
            {
                return string.Format("server={0}; database={1}; uid={2}; password={3}; convertzerodatetime=yes;",
                    Database.Host,
                    Database.Schema,
                    Database.Username,
                    Database.Password);
            }
        }

        internal static void Execute(string nonQuery, params object[] args)
        {
            MySqlHelper.ExecuteNonQuery(Database.ConnectionString, string.Format(nonQuery, args));
        }

        public static string DefaultSchema
        {
            get
            {
                return Settings.GetString("Database/Schema");
            }
        }

        public static void Test()
        {
            using (MySqlConnection connection = new MySqlConnection(Database.ConnectionString))
            {
                connection.Open();
                LogFactory.GetLog("DB").LogInfo("conexão com banco de dados '{0}'feita.", connection.Database);
                connection.Close();
            }
        }

        public static void Analyze()
        {
            using (Database.TemporarySchema("information_schema"))
            {
                Meta.Initialize();
            }
        }

        public static void Delete(string table, string constraints, params object[] args)
        {
            Database.Execute("DELETE FROM {0} WHERE {1}", table, string.Format(constraints, args));
        }

        public static bool Exists(string table, string constraints, params object[] args)
        {
            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(Database.ConnectionString, string.Format(string.Format("SELECT * FROM {0} WHERE {1}", table, constraints), args)))
            {
                return reader.HasRows;
            }
        }

        public static dynamic Fetch(string table, string field, string constraints, params object[] args)
        {
            object value = new Datum(table).PopulateWith(field, constraints, args).Dictionary[field];

            if (value is DBNull)
            {
                return null;
            }
            else if (value is byte && Meta.IsBool(table, field))
            {
                return (byte)value > 0;
            }
            else
            {
                return value;
            }
        }

        public static void ExecuteScript(string query, params object[] args)
        {
            using (MySqlConnection connection = new MySqlConnection(string.Format("SERVER={0}; UID={1}; PASSWORD={2}; DATABASE={3};", Host, Username, Password, Schema)))
            {
                connection.Open();
                new MySqlScript(connection, string.Format(query, args)).Execute();
                connection.Close();
            }
        }

        public static void Query(ref DataSet ds, string query, params object[] args)
        {
            using (MySqlConnection connection = new MySqlConnection(string.Format("SERVER={0}; UID={1}; PASSWORD={2}; DATABASE={3};", Host, Username, Password, Schema)))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = new MySqlCommand(string.Format(query, args), connection);
                adapter.Fill(ds);
                connection.Close();
            }
        }

        public static void ExecuteFile(string host, string username, string password, string path)
        {
            using (MySqlConnection connection = new MySqlConnection(string.Format("SERVER={0}; UID={1}; PASSWORD={2};", host, username, password)))
            {
                connection.Open();

                using (TextReader reader = new StreamReader(path))
                {
                    new MySqlScript(connection, reader.ReadToEnd()).Execute();
                }

                connection.Close();
            }
        }

        public static TemporaryConnection TemporaryConnection(string host, string schema, string username, string password)
        {
            return new TemporaryConnection(host, schema, username, password);
        }

        public static TemporarySchema TemporarySchema(string schema)
        {
            return new TemporarySchema(schema);
        }

        public static string Escape(object argument)
        {
            if (argument is string)
                return "'" + MySqlHelper.EscapeString((string)argument) + "'";
            else if (argument is byte[])
            {
                string text = "";
                foreach (var _byte in (byte[])argument)
                    text += _byte.ToString("X2");

                return "UNHEX('" + text + "')";
            }
            else if (argument is DateTime)
                return "'" + ((DateTime)argument).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            else if (argument is bool)
                return (bool)argument ? "1" : "0";
            else if (argument == null)
                return "NULL";
            else
                return argument.ToString();
        }
    }
}