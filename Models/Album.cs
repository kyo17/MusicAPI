using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Album
    {
        public Album()
        {
            idAlbum = Guid.NewGuid().ToString();
            songs = new List<Song>();
        }
        [Key]
        public string idAlbum { get; set; }
        public string title { get; set; }
        public string band { get; set; }
        public string poster { get; set; }
        public List<Song> songs { get; set; }
    }
}
