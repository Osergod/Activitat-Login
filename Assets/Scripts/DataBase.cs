using System;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

public class DataBase : MonoBehaviour
{
    private string dbPath;

    private void Awake()
    {
        dbPath = "URI=file:" + Application.persistentDataPath + "/usuaris.db";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS Usuaris (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE NOT NULL,
                    Password TEXT NOT NULL
                );";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public string RegisterUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            return "El nom d'usuari no pot estar buit";

        if (password.Length < 8)
            return "La contrasenya ha de tenir mínim 8 caràcters";

        try
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();

                // Comprovar si existeix
                using (var checkCmd = conn.CreateCommand())
                {
                    checkCmd.CommandText =
                        "SELECT COUNT(*) FROM Usuaris WHERE Username = @user";
                    checkCmd.Parameters.AddWithValue("@user", username);

                    long count = (long)checkCmd.ExecuteScalar();
                    if (count > 0)
                        return "Aquest usuari ja existeix";
                }

                // Inserir usuari
                using (var insertCmd = conn.CreateCommand())
                {
                    insertCmd.CommandText =
                        "INSERT INTO Usuaris (Username, Password) VALUES (@user, @pass)";
                    insertCmd.Parameters.AddWithValue("@user", username);
                    insertCmd.Parameters.AddWithValue("@pass", password);
                    insertCmd.ExecuteNonQuery();
                }

                return "OK";
            }
        }
        catch (Exception ex)
        {
            return "Error de base de dades: " + ex.Message;
        }
    }

    public (bool success, string message, int userId) LoginUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return (false, "Usuari i contrasenya obligatoris", -1);

        try
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "SELECT UserID FROM Usuaris WHERE Username = @user AND Password = @pass";
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        return (true, "Login correcte", Convert.ToInt32(result));
                    else
                        return (false, "Usuari o contrasenya incorrectes", -1);
                }
            }
        }
        catch (Exception ex)
        {
            return (false, "Error de connexió: " + ex.Message, -1);
        }
    }
}
