using DSCC.CW1.Frontend._14714.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DSCC.CW1.Frontend._14714.Controllers
{
    public class MemberController : Controller
    {
        private readonly string Baseurl = "https://localhost:7107/";
        private readonly HttpClient client;

        public MemberController()
        {
            // Use a single HttpClient instance with SSL bypass
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

        private async Task PopulatePlansDropdown()
        {
            HttpResponseMessage response = await client.GetAsync("api/MembershipPlan");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var plans = JsonConvert.DeserializeObject<List<MembershipPlan>>(jsonResponse);

                ViewBag.Plans = new SelectList(plans, "PlanId", "PlanName");
            }
            else
            {
                // Handle errors if the API call fails
                ViewBag.Plans = new SelectList(new List<MembershipPlan>(), "PlanId", "PlanName");
            }
        }

        // GET: Member
        public async Task<ActionResult> Index()
        {
            List<Member> MemberInfo = new List<Member>();
            Dictionary<int, string> planNames = new Dictionary<int, string>();

            // Fetch members
            HttpResponseMessage response = await client.GetAsync("api/Member");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                MemberInfo = JsonConvert.DeserializeObject<List<Member>>(jsonResponse);
            }

            // Fetch plans and create dictionary for PlanId to PlanName mapping
            HttpResponseMessage planResponse = await client.GetAsync("api/MembershipPlan");
            if (planResponse.IsSuccessStatusCode)
            {
                var planJsonResponse = await planResponse.Content.ReadAsStringAsync();
                var plans = JsonConvert.DeserializeObject<List<MembershipPlan>>(planJsonResponse);
                planNames = plans.ToDictionary(p => p.PlanId, p => p.PlanName);
            }

            // Pass data to view
            ViewBag.PlanNames = planNames;
            return View(MemberInfo);
        }


        // GET: Member/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Member member = null;
            HttpResponseMessage response = await client.GetAsync($"api/Member/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                member = JsonConvert.DeserializeObject<Member>(jsonResponse);
            }

            if (member == null)
            {
                return HttpNotFound("Member not found.");
            }
            return View(member);
        }

        // GET: Member/Create
        public async Task<ActionResult> Create()
        {
            await PopulatePlansDropdown();
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        public async Task<ActionResult> Create(Member member)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(member);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/Member", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Error creating member. Please check your input and try again.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating member: {ex.Message}");
            }
            await PopulatePlansDropdown();
            return View(member);
        }

        // GET: Member/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Member member = null;
            HttpResponseMessage response = await client.GetAsync($"api/Member/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                member = JsonConvert.DeserializeObject<Member>(jsonResponse);
            }

            if (member == null)
            {
                return HttpNotFound("Member not found.");
            }

            await PopulatePlansDropdown();
            return View(member);
        }

        // POST: Member/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Member member)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(member);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"api/Member/{member.MemberId}", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Error updating member. Please check your input and try again.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating member: {ex.Message}");
            }
            await PopulatePlansDropdown();
            return View(member);
        }

        // GET: Member/Delete/5
        // GET: Member/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Member member = null;
            HttpResponseMessage response = await client.GetAsync($"api/Member/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                member = JsonConvert.DeserializeObject<Member>(jsonResponse);
            }

            if (member == null)
            {
                return HttpNotFound("Member not found.");
            }
            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"api/Member/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Error deleting member.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error deleting member: {ex.Message}");
            }
            return RedirectToAction("Index");
        }
    }
}
