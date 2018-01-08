using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PaderbornUniversity.SILab.Hip.EventSourcing;
using PaderbornUniversity.SILab.Hip.EventSourcing.EventStoreLlp;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Core;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Entity;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Events;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Rest;
using PaderbornUniversity.SILab.Hip.Webservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Controllers
{
    //[Authorize] <-- TODO: Uncomment to force authentication
    [Route("api/[controller]")]
    public class FooController : Controller
    {
        private readonly EventStoreService _eventStore;
        private readonly CacheDatabaseManager _db;
        private readonly EntityIndex _entityIndex;

        public FooController(EventStoreService eventStore, CacheDatabaseManager db, InMemoryCache cache)
        {
            _eventStore = eventStore;
            _db = db;
            _entityIndex = cache.Index<EntityIndex>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FooResult>), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var foos = _db.Database.GetCollection<Foo>(ResourceType.Foo.Name)
                .AsQueryable()
                .ToList();

            return Ok(foos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FooResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var foo = _db.Database.GetCollection<Foo>(ResourceType.Foo.Name)
                .AsQueryable()
                .FirstOrDefault(x => x.Id == id);

            if (foo == null)
                return NotFound();

            var result = new FooResult
            {
                Id = id,
                DisplayName = foo.DisplayName,
                IsBar = foo.IsBar
            };

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromBody]FooArgs args)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ev = new FooCreated
            {
                Id = _entityIndex.NextId(ResourceType.Foo),
                UserId = User.Identity.GetUserIdentity(),
                Properties = args,
                Timestamp = DateTimeOffset.Now
            };

            await _eventStore.AppendEventAsync(ev);
            return Created($"{Request.Scheme}://{Request.Host}/api/Foo/{ev.Id}", ev.Id);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_entityIndex.Exists(ResourceType.Foo, id))
                return NotFound();

            var ev = new FooDeleted
            {
                Id = id,
                UserId = User.Identity.GetUserIdentity(),
                Timestamp = DateTimeOffset.Now
            };

            await _eventStore.AppendEventAsync(ev);
            return NoContent();
        }
    }
}
