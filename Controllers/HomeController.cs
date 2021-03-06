using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RocketElevatorsCustomerPortal.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace RocketElevatorsCustomerPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        

        

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> InterventionRequest()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var email = user.UserName;

            List<Battery> BatteryList = new List<Battery>();
            List<Building> BuildingList = new List<Building>();
            List<Column> ColumnList = new List<Column>();
            List<Elevator> ElevatorList = new List<Elevator>();
            
            Customer customer = new Customer();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/{email}/info"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    customer = JsonConvert.DeserializeObject<Customer>(apiResponse);
                }
            }



            
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/{email}/buildings"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    BuildingList = JsonConvert.DeserializeObject<List<Building>>(apiResponse);
                }
            }

            using (var httpClient = new HttpClient())
            {
            
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/{email}/batteries"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    BatteryList = JsonConvert.DeserializeObject<List<Battery>>(apiResponse);
                }
            }

            using (var httpClient = new HttpClient())
            {
            
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/{email}/columns"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ColumnList = JsonConvert.DeserializeObject<List<Column>>(apiResponse);
                }
            }

            using (var httpClient = new HttpClient())
            {
            
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/{email}/elevators"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ElevatorList = JsonConvert.DeserializeObject<List<Elevator>>(apiResponse);
                }
            }

           
            ViewBag.Buildings = BuildingList;
            ViewBag.Batteries = BatteryList;
            ViewBag.Columns = ColumnList;
            ViewBag.Elevators = ElevatorList;
            ViewBag.author_id = customer.id;

   
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InterventionRequest(Intervention intervention)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var email = user.UserName;
           
            using (HttpClient httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://domin-rest.azurewebsites.net/api/interventions/"),
                    Method = new HttpMethod("Post"),
                    Content = new StringContent("{\"author\": \"" + intervention.author + "\", \"building_id\": \"" + intervention.building_id + "\", \"battery_id\": \"" + intervention.battery_id + "\", \"column_id\": \"" + intervention.column_id + "\", \"elevator_id\": \"" + intervention.elevator_id + "\",\"customer_id\": \"" + intervention.author + "\", \"report\": \"" + intervention.report + "\" }", Encoding.UTF8, "application/json")
                };


 
                var response = await httpClient.SendAsync(request);

            

                
                ViewBag.Result = "Success";
            }

            return RedirectToAction("Index");
        }


       



        [Authorize]
        public async Task<IActionResult> Batteries()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            
            List<Battery> BatteryList = new List<Battery>();
            var email = user.UserName;
            

            using (var httpClient = new HttpClient())
            {
            
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/{email}/batteries"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    BatteryList = JsonConvert.DeserializeObject<List<Battery>>(apiResponse);
                }
            }
            return View(BatteryList);
        }

         


        [Authorize]
        public async Task<IActionResult> Columns()
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            
            List<Battery> BatteryList = new List<Battery>();
            var email = user.UserName;

            List<Column> ColumnList = new List<Column>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/{email}/columns"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ColumnList = JsonConvert.DeserializeObject<List<Column>>(apiResponse);
                }
            }
            return View(ColumnList);
        }

        [Authorize]
       public async Task<IActionResult> Elevators()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var email = user.UserName;


            List<Elevator> ElevatorList = new List<Elevator>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/{email}/elevators"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ElevatorList = JsonConvert.DeserializeObject<List<Elevator>>(apiResponse);
                }
            }
            return View(ElevatorList);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ElevatorIntervention(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var email = user.UserName;

            Intervention elevatorIntervention = new Intervention();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/email={email}&elevator_id={id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    elevatorIntervention = JsonConvert.DeserializeObject<Intervention>(apiResponse);
                }
            }
            return View(elevatorIntervention);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> BatteryIntervention(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var email = user.UserName;

            Intervention batteryIntervention = new Intervention();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/email={email}&battery_id={id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    batteryIntervention = JsonConvert.DeserializeObject<Intervention>(apiResponse);
                }
            }
            return View(batteryIntervention);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> BatteryIntervention(Intervention intervention)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var email = user.UserName;
           
            using (HttpClient httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://domin-rest.azurewebsites.net/api/interventions/"),
                    Method = new HttpMethod("Post"),
                    Content = new StringContent("{\"author\": \"" + intervention.author + "\", \"building_id\": \"" + intervention.building_id + "\", \"battery_id\": \"" + intervention.battery_id + "\", \"customer_id\": \"" + intervention.author + "\", \"report\": \"" + intervention.report + "\" }", Encoding.UTF8, "application/json")
                };
                
                var response = await httpClient.SendAsync(request);
               
 
                
               
                
                ViewBag.Result = "Success";
            }

            return View(intervention);
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ColumnIntervention(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var email = user.UserName;

            Intervention columnIntervention = new Intervention();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/email={email}&column_id={id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    columnIntervention = JsonConvert.DeserializeObject<Intervention>(apiResponse);
                }
            }
            return View(columnIntervention);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ColumnIntervention(Intervention intervention)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var email = user.UserName;
           
            using (HttpClient httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://domin-rest.azurewebsites.net/api/interventions/"),
                    Method = new HttpMethod("Post"),
                    Content = new StringContent("{\"author\": \"" + intervention.author + "\", \"building_id\": \"" + intervention.building_id + "\", \"battery_id\": \"" + intervention.battery_id + "\", \"column_id\": \"" + intervention.column_id + "\", \"customer_id\": \"" + intervention.author + "\", \"report\": \"" + intervention.report + "\"}", Encoding.UTF8, "application/json")
                };
 
                var response = await httpClient.SendAsync(request);
                
                ViewBag.Result = "Success";
            }

            return View(intervention);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ElevatorIntervention(Intervention intervention)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var email = user.UserName;
           
            using (HttpClient httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://domin-rest.azurewebsites.net/api/interventions/"),
                    Method = new HttpMethod("Post"),
                    Content = new StringContent("{\"author\": \"" + intervention.author + "\", \"building_id\": \"" + intervention.building_id + "\", \"battery_id\": \"" + intervention.battery_id + "\", \"column_id\": \"" + intervention.column_id + "\", \"elevator_id\": \"" + intervention.elevator_id + "\",\"customer_id\": \"" + intervention.author + "\", \"report\": \"" + intervention.report + "\" }", Encoding.UTF8, "application/json")
                };
 
                var response = await httpClient.SendAsync(request);
                
                ViewBag.Result = "Success";
            }

            return View(intervention);
        }
        [Authorize]
        public async Task<IActionResult> UpdateProfile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var email = user.UserName;
            Customer customer = new Customer();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://domin-rest.azurewebsites.net/api/customers/{email}/info/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    customer = JsonConvert.DeserializeObject<Customer>(apiResponse);
                }
            }
            return View(customer);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Customer customer)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var email = user.UserName;
            using (HttpClient httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {

                    RequestUri = new Uri("https://domin-rest.azurewebsites.net/api/customers/" + email),
                    Method = new HttpMethod("Patch"),
                    Content = new StringContent("[{ \"op\": \"replace\", \"path\": \"company_name\", \"value\": \"" + customer.company_name + "\"},{ \"op\": \"replace\", \"path\": \"company_headquarters_address\", \"value\": \"" + customer.company_headquarters_address + "\"},{ \"op\": \"replace\", \"path\": \"full_name_of_the_company_contact\", \"value\": \"" + customer.full_name_of_the_company_contact + "\"},{ \"op\": \"replace\", \"path\": \"company_contact_phone\", \"value\": \"" + customer.company_contact_phone + "\"},{ \"op\": \"replace\", \"path\": \"email_of_the_company_contact\", \"value\": \"" + customer.email_of_the_company_contact + "\"}]", Encoding.UTF8, "application/json")
                };
 
                var response = await httpClient.SendAsync(request);
                ViewBag.Result = "Success";
            }
            return View(customer);
        }
    
    


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

