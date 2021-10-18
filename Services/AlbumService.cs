using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DML;
using Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class AlbumService : ICRUD<Album>
    {
        private readonly DataContext db;
        private readonly IAzure azure;
        private readonly string posters = "posters";
        private readonly string files = "songs";

        public AlbumService(DataContext context, IAzure storage)
        {
            db = context;
            azure = storage;
        }

        public async Task delete(string id)
        {
            using(var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var remove = await db.Albums.FindAsync(id);
                    db.Albums.Remove(remove);
                    await db.SaveChangesAsync();
                    await ts.CommitAsync();
                }
                catch (SqlException ex)
                {
                    await ts.RollbackAsync();
                    Console.WriteLine(ex.Message);

                }
            }
        }

        public async Task<IEnumerable<Album>> getAll()
        {
            try
            {
                return await db.Albums.
                    Include(x => x.songs).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        /*
        public async Task<Album> getById(string id)
        {
            try
            {
                return await db.Albums.Include(x => x.songs)
                  .FirstOrDefaultAsync(z => z.idAlbum == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        */
        public async Task<Album> getById(Expression<Func<Album, bool>> exp)
        {
            try
            {
                return await db.Set<Album>().Where(exp).
                    Include(x => x.songs)
                    .FirstOrDefaultAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task save(Album entity)
        {
            using(var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!string.IsNullOrEmpty(entity.poster))
                    {
                        var pic = Convert.FromBase64String(entity.poster);
                        entity.poster = await azure.save(pic, "png", posters);
                    }
                    foreach (var sing in entity.songs)
                    {
                        if (!string.IsNullOrEmpty(sing.song))
                        {
                            var music = Convert.FromBase64String(sing.song);
                            sing.song = await azure.save(music, "mp3", files);
                        }
                    }
                    await db.AddAsync(entity);
                    await db.SaveChangesAsync();
                    await ts.CommitAsync();
                }
                catch (SqlException ex)
                {
                    await ts.RollbackAsync();
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task update(Album entity)
        {
            using(var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(entity.poster))
                    {
                        var pic = Convert.FromBase64String(entity.poster);
                        entity.poster = await azure.edit(pic, "png", posters, entity.poster);
                    }
                    foreach (var sing in entity.songs)
                    {
                        if (!string.IsNullOrWhiteSpace(sing.song))
                        {
                            var music = Convert.FromBase64String(sing.song);
                            sing.song = await azure.edit(music, "mp3", files, sing.song);
                        }
                    }
                    db.Albums.Update(entity);
                    await db.SaveChangesAsync();
                    await ts.CommitAsync();
                }
                catch (SqlException ex)
                {
                    await ts.RollbackAsync();
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
