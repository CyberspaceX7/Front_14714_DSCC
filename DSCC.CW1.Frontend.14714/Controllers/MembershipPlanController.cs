using DSCC.CW1.Frontend._14714.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DSCC.CW1.Frontend._14714.Controllers
{
    public class MembershipPlanController : Controller
    {
        private readonly string Baseurl = "https://localhost:7107/";
        private readonly HttpClient client;

        public MembershipPlanController()
        {
            // Configure HttpClient with custom handler to bypass SSL validation in development
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            client = new HttpClient(handler)
            {
                BaseAddress = new Uri(Baseurl)
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: MembershipPlan
        public async Task<ActionResult> Index()
        {
            List<MembershipPlan> MembershipPlans = new List<MembershipPlan>();
            HttpResponseMessage response = await client.GetAsync("api/MembershipPlan");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                MembershipPlans = JsonConvert.DeserializeObject<List<MembershipPlan>>(jsonResponse);
            }

            return View(MembershipPlans);
        }

        // GET: MembershipPlan/Details/5
        public async Task<ActionResult> Details(int id)
        {
            MembershipPlan membershipPlan = null;
            HttpResponseMessage response = await client.GetAsync($"api/MembershipPlan/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                membershipPlan = JsonConvert.DeserializeObject<MembershipPlan>(jsonResponse);
            }

            if (membershipPlan == null)
            {
                return HttpNotFound();
            }
            return View(membershipPlan);
        }

        // GET: MembershipPlan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MembershipPlan/Create
        [HttpPost]
        public async Task<ActionResult> Create(MembershipPlan membershipPlan)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(membershipPlan);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/MembershipPlan", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Log error if needed
                ModelState.AddModelError("", "Error creating membership plan: " + ex.Message);
            }
            return View(membershipPlan);
        }

        // GET: MembershipPlan/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            MembershipPlan membershipPlan = null;
            HttpResponseMessage response = await client.GetAsync($"api/MembershipPlan/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                membershipPlan = JsonConvert.DeserializeObject<MembershipPlan>(jsonResponse);
            }

            if (membershipPlan == null)
            {
                return HttpNotFound();
            }
            return View(membershipPlan);
        }

        // POST: MembershipPlan/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(MembershipPlan membershipPlan)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(membershipPlan);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"api/MembershipPlan/{membershipPlan.PlanId}", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating membership plan: " + ex.Message);
            }
            return View(membershipPlan);
        }

        // GET: MembershipPlan/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            MembershipPlan membershipPlan = null;
            HttpResponseMessage response = await client.GetAsync($"api/MembershipPlan/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                membershipPlan = JsonConvert.DeserializeObject<MembershipPlan>(jsonResponse);
            }

            if (membershipPlan == null)
            {
                return HttpNotFound();
            }
            return View(membershipPlan);
        }

        // POST: MembershipPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"api/MembershipPlan/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting membership plan: " + ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
