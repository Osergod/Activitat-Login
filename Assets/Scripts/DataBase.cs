using UnityEngine;
using System.Data;                // IDbConnection, IDataReader, etc.
using Mono.Data.Sqlite;           // SqliteConnection, etc.

public class DataBase : MonoBehaviour
{
    // Contador que se mantiene entre clics y escenas (si no destruyes el objeto)
    private int hitCount = 0;

    // Ruta recomendada: persistentDataPath → funciona en Editor y en builds (Windows, Android, etc.)
    // En Editor puedes usar Application.dataPath si prefieres, pero persistent es más seguro
    private string DbPath => "URI=file:" + Application.persistentDataPath + "/MyDatabase.sqlite";

    private void Start()
    {
        InitializeDatabase();   // Crea tabla + fila inicial si no existen
        LoadHitCount();         // Carga el valor guardado
        Debug.Log($"Valor inicial de hits: {hitCount}");
    }

    private void InitializeDatabase()
    {
        using (var conn = new SqliteConnection(DbPath))
        {
            conn.Open();

            // Crear tabla si no existe
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS HitCountTableSimple (
                        id   INTEGER PRIMARY KEY,
                        hits INTEGER NOT NULL DEFAULT 0
                    )";
                cmd.ExecuteNonQuery();
            }

            // Aseguramos que exista la fila con id = 0
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT OR IGNORE INTO HitCountTableSimple (id, hits)
                    VALUES (0, 0)";
                cmd.ExecuteNonQuery();
            }
        }
    }

    private void LoadHitCount()
    {
        using (var conn = new SqliteConnection(DbPath))
        {
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT hits FROM HitCountTableSimple WHERE id = 0 LIMIT 1";

                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    hitCount = System.Convert.ToInt32(result);
                }
            }
        }
    }

    private void SaveHitCount()
    {
        using (var conn = new SqliteConnection(DbPath))
        {
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    UPDATE HitCountTableSimple
                    SET hits = @newHits
                    WHERE id = 0";

                var param = cmd.CreateParameter();
                param.ParameterName = "@newHits";
                param.Value = hitCount;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
            }
        }
    }

    private void OnMouseDown()
    {
        hitCount++;
        Debug.Log($"Nuevo conteo: {hitCount}");

        SaveHitCount();  // Guardamos inmediatamente
    }

    // Para debug: ver todo el contenido de la tabla (ejecútalo desde el menú contextual)
    [ContextMenu("Mostrar todos los datos de la DB")]
    private void DebugShowAll()
    {
        using (var conn = new SqliteConnection(DbPath))
        {
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM HitCountTableSimple";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log($"id: {reader.GetInt32(0)} | hits: {reader.GetInt32(1)}");
                    }
                }
            }
        }
    }
}