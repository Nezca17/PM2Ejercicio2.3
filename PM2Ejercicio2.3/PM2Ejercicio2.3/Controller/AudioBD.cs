using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace PM2Ejercicio2.3.Controller
{
     public class AudiosDB
{
    readonly SQLiteAsyncConnection db;

    public AudiosDB(string dbpath)
    {
        db = new SQLiteAsyncConnection(dbpath);
        db.CreateTableAsync<Audio>();
    }

    public Task<int> guardarAudio(Audio audio)
    {
        if (audio.id != 0)
        {
            return db.UpdateAsync(audio);
        }
        else
        {
            return db.InsertAsync(audio);
        }
    }

        public Task<List<Audio>> obtenerAudios()
        {
            return db.Table<Audio>().ToListAsync();
        }

        public Task<Audio> obtenerAudio(int aId)
        {
            return db.Table<Audio>()
                .Where(i => i.id == aId)
                .FirstOrDefaultAsync();
        }

        public Task<int> borrarAudio(Audio audio)
        {
            return db.DeleteAsync(audio);
        }
    }
}
