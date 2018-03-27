using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Trek
{
    internal static class Program
    {
        private const string SurveyResultsUrl = "https://trekhiringassignments.blob.core.windows.net/interview/bikes.json";
        private const int NumberOfResults = 20;

        private static void Main()
        {
            // download the survey results
            var results = GetSurveyResultsAsync().GetAwaiter().GetResult();

            // group results and order descending by number of bikes in the group taking only the top x results
            var groups = results.GroupBy(x => x.GetHashCode())
                                 .OrderByDescending(x => x.Count())
                                 .Take(NumberOfResults)
                                 .ToList();

            // print results
            foreach(var group in groups)
            {
                var bikes = BuildBikeString(group.First().Bikes);

                Console.WriteLine($"[{group.Count():0000}] {bikes}");
            }

            Console.Read();
        }

        private static string BuildBikeString(ICollection<string> bikes)
        {
            var sb = new StringBuilder();

            foreach (var bike in bikes)
            {
                sb.Append(bike);
                sb.Append(',');
            }

            // remove last comma
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        private static async Task<ICollection<SurveyResult>> GetSurveyResultsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(SurveyResultsUrl).ConfigureAwait(false);

                return JsonConvert.DeserializeObject<Collection<SurveyResult>>(response);
            }
        }
    }
}
