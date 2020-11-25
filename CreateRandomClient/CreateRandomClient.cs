using System;
using HotelLandon.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using RandomNameGeneratorLibrary;

namespace CreateRandomClient
{
    public static class CreateRandomClient
    {
        [FunctionName("CreateRandomClient")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            using (var context = new HotelLandonContext())
            {
                var personNameGenerator = new PersonNameGenerator();
                context.Customers.Add(new HotelLandon.Models.Customer
                {
                    FirstName = personNameGenerator.GenerateRandomFirstName(),
                    LastName = personNameGenerator.GenerateRandomLastName(),
                    BirthDate = DateTime.Now
                });
                context.SaveChanges();
            }
        }
    }
}