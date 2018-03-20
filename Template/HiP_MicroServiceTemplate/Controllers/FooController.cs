using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PaderbornUniversity.SILab.Hip.EventSourcing;
using PaderbornUniversity.SILab.Hip.EventSourcing.EventStoreLlp;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Core;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Entity;
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate.Model.Rest;
using PaderbornUniversity.SILab.Hip.Webservice;
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

            var foos = _db.Database.GetCollection<Foo>(ResourceTypes.Foo)
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

            var foo = _db.Database.GetCollection<Foo>(ResourceTypes.Foo)
                .AsQueryable()
                .FirstOrDefault(x => x.Id == id);

            if (foo == null)
                return NotFound();

            var result = new FooResult
            {
                Id = id,
                DisplayName = foo.DisplayName,
                IsBar = foo.IsBar,
                Value = foo.Value,
                UserId = foo.UserId,
                Timestamp = foo.Timestamp
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

            var id = _entityIndex.NextId(ResourceTypes.Foo);
            await EntityManager.CreateEntityAsync(_eventStore, args, ResourceTypes.Foo, id, User.Identity.GetUserIdentity());
            return Created($"{Request.Scheme}://{Request.Host}/api/Foo/{id}", id);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]

        public async Task<IActionResult> PutAsync(int id, [FromBody]FooArgs args)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_entityIndex.Exists(ResourceTypes.Foo, id))
                return NotFound();

            var oldFoo = await _eventStore.EventStream.GetCurrentEntityAsync<FooArgs>(ResourceTypes.Foo, id);
            await EntityManager.UpdateEntityAsync(_eventStore, oldFoo, args, ResourceTypes.Foo, id, User.Identity.GetUserIdentity());
            return NoContent();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_entityIndex.Exists(ResourceTypes.Foo, id))
                return NotFound();

            await EntityManager.DeleteEntityAsync(_eventStore, ResourceTypes.Foo, id, User.Identity.GetUserIdentity());
            return NoContent();
        }
    }
}
