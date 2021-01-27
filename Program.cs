using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiClient
{
    class Program
    {
        class Job
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("url")]
            public string Url { get; set; }

            [JsonPropertyName("created_at")]
            public string CreatedAt { get; set; }

            [JsonPropertyName("company")]
            public string Company { get; set; }

            [JsonPropertyName("company_url")]
            public string CompanyUrl { get; set; }

            [JsonPropertyName("location")]
            public string Location { get; set; }

            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("how_to_apply")]
            public string HowToApply { get; set; }
        }
        static async Task Main(string[] args)
        {
            var toContinue = "";
            do
            {
                Console.Write("\nWhat's the description of the job that you're looking for? ");
                var jobDescription = Console.ReadLine();
                Console.Write("\nWhat's the desired location of the job type that you're looking for? ");
                var jobLocation = Console.ReadLine();

                var client = new HttpClient();

                var responseAsStream = await client.GetStreamAsync($"https://jobs.github.com/positions.json?description={jobDescription}&full_time=false&location={jobLocation}");

                var jobs = await JsonSerializer.DeserializeAsync<List<Job>>(responseAsStream);

                foreach (var job in jobs)
                {
                    Console.WriteLine($"\n{job.Type} {job.Title} needed at {job.Company}, {job.CompanyUrl} located in {job.Location}");
                    Console.WriteLine($"{job.HowToApply}. Posted on {job.CreatedAt}. Job Id: {job.Id}");
                }

                Console.WriteLine("\nWould you like to run another job search?");
                Console.Write("'yes' to continue, any other key to end: ");
                toContinue = Console.ReadLine().ToLower().Trim();
            }
            while (toContinue == "yes");
        }
    }
}
