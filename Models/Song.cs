using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Song
    {
        public Song()
        {
            id = Guid.NewGuid().ToString();
        }
        [Key]
        public string id { get; set; }
        public string name { get; set; }
        public string song { get; set; }
        public int stars { get; set; }
        public int track { get; set; }
        //[ForeignKey("idAlbum")]
        public string AlbumidAlbum { get; set; }
    }
}
