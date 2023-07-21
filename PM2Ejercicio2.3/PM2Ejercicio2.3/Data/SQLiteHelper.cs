using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace PM2Ejercicio2_3Data
{
    public class SQLiteHelper
    {
        SQLiteAsyncConnection db;

        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<Imagen>().Wait(); // Crear la tabla Imagen en la base de datos (si no existe)
        }

        // Método para insertar una imagen en la tabla Imagen
        public async Task<int> InsertImagenAsync(Imagen imagen)
        {
            return await db.InsertAsync(imagen);
        }

        // Método para obtener todas las imágenes de la tabla Imagen
        public async Task<List<Imagen>> GetImagenesAsync()
        {
            return await db.Table<Imagen>().ToListAsync();
        }
    }

    // Clase para representar la tabla Imagen en la base de datos
    public class Imagen
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string ImageName { get; set; }

        public byte[] ImageData { get; set; }

        public string Description { get; set; }
    }
}
