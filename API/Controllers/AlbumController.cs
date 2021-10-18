using System;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : Controller
    {
        private ICRUD<Album> repo;

        public AlbumController(ICRUD<Album> collection)
        {
            repo = collection;
        }

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            return Ok(await repo.getAll());
        }

        [HttpPost]
        public async Task<IActionResult> save([FromBody] Album album)
        {
            await repo.save(album);
            return Created("Created", true);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getById(string id)
        {
            return Ok(await repo.getById(x => x.idAlbum == id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(string id)
        {
            await repo.delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update([FromBody] Album album)
        {
            await repo.update(album);
            return Created("Updated", true);
        }
    }
}
