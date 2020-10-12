using IntroToAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntroToAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient(); // new up HttpClient
            var response = httpClient.GetAsync("https://swapi.dev/api/people/1/").Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadKey();
                Person personResponse = response.Content.ReadAsAsync<Person>().Result;
                Console.WriteLine(personResponse.name);

                foreach(string vehicleURL in personResponse.vehicles)
                {
                    HttpResponseMessage vehicleResponse = httpClient.GetAsync(vehicleURL).Result;
                    Console.WriteLine(vehicleResponse.Content.ReadAsStringAsync().Result);
                    Vehicle vehicle = vehicleResponse.Content.ReadAsAsync<Vehicle>().Result;
                    Console.WriteLine(vehicle.Name);
                }
            }

            SWAPIService swapiService = new SWAPIService();
            for (int i=1; i <=10; i++)
            {
                Person personOne = swapiService.GetPersonAsync($"https://swapi.dev/api/people/{i}" +
                    $"").Result;
                if (personOne != null)
                {
                    Console.Clear();
                    Console.WriteLine($"The character entered is: {personOne.name}");
                    foreach (string vehicleURL in personOne.vehicles)
                    {
                        var vehicle = swapiService.GetVehicleAsync(vehicleURL).Result;
                        Console.WriteLine($"Has a {vehicle.Name} vehicle");
                    }
                    Console.ReadKey();
                }
            }
            var genericResponse = swapiService.GetAsynchGeneric<Vehicle>
                ("https://swapi.dev/api/vehicles/4/").Result;
            Console.WriteLine(genericResponse.Name);
            Console.WriteLine(genericResponse.CargoCapacity);
            Console.ReadKey();

            SearchResult<Person> skywalkers = swapiService.GetPersonSearchAsync("skywalker").Result;
            foreach(Person person in skywalkers.Results)
            {
                Console.WriteLine(person.name);
            }
        }
    }
}
