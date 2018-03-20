using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PaderbornUniversity.SILab.Hip.EventSourcing;
using PaderbornUniversity.SILab.Hip.EventSourcing.FakeStore;
using PaderbornUniversity.SILab.Hip.EventSourcing.Mongo;
using PaderbornUniversity.SILab.Hip.EventSourcing.Mongo.Test;
using System.Threading.Tasks;
using Xunit;
#if (MakeSdk)
using PaderbornUniversity.SILab.Hip.HiP_MicroServiceTemplate;
#endif

namespace PaderbornUniversity.SILab.Hip.HiP._MicroServiceTemplate.Tests
{
    public class ExampleTests
    {
        private TestServer _server;
        private FakeEventStore _eventStore;
        private FakeMongoDbContext _mongoDb;

        public ExampleTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            _eventStore = (FakeEventStore)_server.Host.Services.GetService<IEventStore>();
            _mongoDb = (FakeMongoDbContext)_server.Host.Services.GetService<IMongoDbContext>();
        }

        [Fact]
        public async Task Test1()
        {
#if (!MakeSdk)
            //Write test code here
            await Task.CompletedTask;
#else
            //This is an example for testing genereated clients
            FooClient client = new FooClient("")
            {
                CreateHttpClient = _server.CreateClient,
                Authorization = "Admin-Administrator"
            };

            var args = new FooArgs()
            {
                DisplayName = "Foo",
                IsBar = true,
                Value = 25
            };

            var id = await client.PostAsync(args);
            var result = await client.GetByIdAsync(id);
            Assert.Equal(args.DisplayName, result.DisplayName);
            Assert.Equal(args.IsBar, result.IsBar);
            Assert.Equal(args.Value, result.Value);
#endif
        }
    }
}
