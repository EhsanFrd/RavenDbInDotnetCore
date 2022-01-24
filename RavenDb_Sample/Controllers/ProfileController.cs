using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using RavenDb_Sample.Data;
using RavenDb_Sample.Models;

namespace Org.Quickstart.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly IDocumentSession session;
        public ProfileController()
        {
            this.session = RavenDbDocumentStore.OpenSession();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var profiles = new List<Profile>();
            for (int i = 1; i <= 10000; i++)
            {
                profiles.Add(new Profile
                {
                    Email = $"email-{i}",
                    FirstName = $"FirstName-{i}",
                    LastName = $"LastName-{i}",
                    Password = $"Password-{i}",
                    Id = $"{i}"
                });
            }
            var timer = new Stopwatch();
            timer.Start();
            profiles.ForEach(a => session.Store(a));
            session.SaveChanges();

            timer.Stop();
            var timeTaken = timer.Elapsed;
            var time = timeTaken.ToString(@"m\:ss\.fff");
            Console.WriteLine("---------------------------");
            Console.WriteLine("RavenDb Write Operation");
            Console.WriteLine("---------------------------");
            Console.WriteLine("DataCount        Time");
            Console.WriteLine("---------------------------");
            Console.WriteLine($"{profiles.Count}             {time}");
            await Task.CompletedTask;
            return Ok();

        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var items = session.Query<Profile>().ToList();
                items.ForEach(a => session.Delete<Profile>(a));
                session.SaveChanges();
                Console.WriteLine("data are deleted...");
                await Task.CompletedTask;
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message} {ex.StackTrace} {Request.GetDisplayUrl()}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var timer = new Stopwatch();
                timer.Start();
                var items = session.Query<Profile>().ToList();
                timer.Stop();
                var timeTaken = timer.Elapsed;
                var time = timeTaken.ToString(@"m\:ss\.fff");
                Console.WriteLine("---------------------------");
                Console.WriteLine("RavenDb Get Operation");
                Console.WriteLine("---------------------------");
                Console.WriteLine("DataCount        Time");
                Console.WriteLine("---------------------------");
                Console.WriteLine($"{items.Count}            {time}");
                await Task.CompletedTask;
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message} {ex.StackTrace} {Request.GetDisplayUrl()}");
            }
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            try
            {
                var timer = new Stopwatch();
                timer.Start();
                var items = session.Load<Profile>($"{id}");
                timer.Stop();
                var timeTaken = timer.Elapsed;
                var time = timeTaken.ToString(@"m\:ss\.fff");

                Console.WriteLine("---------------------------");
                Console.WriteLine("RavenDb Get Operation");
                Console.WriteLine("---------------------------");
                Console.WriteLine("DataCount        Time");
                Console.WriteLine("---------------------------");
                Console.WriteLine($"1            {time}");
                Console.WriteLine($"{items.FirstName}");
                await Task.CompletedTask;
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message} {ex.StackTrace} {Request.GetDisplayUrl()}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProfileUpdateRequestCommand request)
        {
            try
            {
                var timer = new Stopwatch();
                timer.Start();
                var item = session.Load<Profile>($"{request.Id}");
                item.FirstName = "test";
                session.SaveChanges();
                timer.Stop();
                var timeTaken = timer.Elapsed;
                var time = timeTaken.ToString(@"m\:ss\.fff");
                Console.WriteLine("---------------------------");
                Console.WriteLine("RavenDb Put Operation");
                Console.WriteLine("---------------------------");
                Console.WriteLine("DataCount        Time");
                Console.WriteLine("---------------------------");
                Console.WriteLine($"1            {time}");
                await Task.CompletedTask;
                return Ok(request);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message} {ex.StackTrace} {Request.GetDisplayUrl()}");
            }
        }
    }
}
