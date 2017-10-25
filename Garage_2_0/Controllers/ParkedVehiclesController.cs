using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage_2_0.DataAccessLayer;
using Garage_2_0.Models;
using Garage_2_0.ViewModels;

namespace Garage_2_0.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private GarageContext db = new GarageContext();

        // GET: ParkedVehicles
        public ActionResult Index(string sortBy = "RegNo", bool isDescending = true)
        {
            var model = new OverviewModel();
            model.IsDescending = isDescending;
            model.SortBy = sortBy;
            model.Vehicles = GetOverviewVehicleList(sortBy, isDescending);

            return View(model);
        }

        private List<OverviewVehicle> GetOverviewVehicleList(string sortBy, bool isDescending)
        { 
            var vehicles = db.ParkedVehicles.ToList();

            return vehicles.Select(p => new OverviewVehicle
            {
                Id = p.Id,
                RegNo = p.RegNo,
                Color = p.Color,
                StartTime = p.StartTime,
                Type = p.Type
            }).ToList();
        }

        // GET: ParkedVehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParkedVehicle parkedVehicle = db.ParkedVehicles.Find(id);
            if (parkedVehicle == null)
            {
                return HttpNotFound();
            }
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Create
        public ActionResult CheckIn()
        {
            return View();
        }

        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn([Bind(Include = "Id,Type,RegNo,Color,Brand,Model,NumberOfWheels,StartTime")] ParkedVehicle parkedVehicle)
        {
            if (ModelState.IsValid)
            {
                db.ParkedVehicles.Add(parkedVehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParkedVehicle parkedVehicle = db.ParkedVehicles.Find(id);
            if (parkedVehicle == null)
            {
                return HttpNotFound();
            }
            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,RegNo,Color,Brand,Model,NumberOfWheels,StartTime")] ParkedVehicle parkedVehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parkedVehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parkedVehicle);
        }

        //// GET: ParkedVehicles/Delete/5
        //public ActionResult CheckOut(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ParkedVehicle parkedVehicle = db.ParkedVehicles.Find(id);
        //    if (parkedVehicle == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(parkedVehicle);
        //}

        public ActionResult CheckOut(int? id)
        {
            var ParkedVehicle = db.ParkedVehicles.Find(id);
            var ParkStopTime = DateTime.Now;  // move to Confirmed
            var ParkingMinutes = ParkStopTime.Subtract(ParkedVehicle.StartTime).TotalMinutes;
            const double COST_PER_MINUTE = 0.20;

            // var temp = formatTimeSpan(ParkStopTime.Subtract(ParkedVehicle.StartTime).ToString(@"dd\:hh\:mm"));

            var CheckOutVehicle = new CheckOutModel()
            {
                Id = ParkedVehicle.Id,
                RegNo = ParkedVehicle.RegNo,
                StartTime = ParkedVehicle.StartTime.ToString("g"),
                StopTime = ParkStopTime.ToString("g"),
                ParkingTime = formatTimeSpan(ParkStopTime.Subtract(ParkedVehicle.StartTime).ToString(@"dd\:hh\:mm")),
                // ParkingCost = (Convert.ToInt32(ParkingMinutes) * COST_PER_MINUTE).ToString() + " kr."
                ParkingCost = Math.Ceiling(ParkingMinutes * COST_PER_MINUTE).ToString() + " kr."
            };

            // return Content(ParkingMinutes.ToString());
            // return Content(temp);
            return View(CheckOutVehicle);
        }

        // "00:22:28"   , 22 hours, 45 minutes
        private string formatTimeSpan(string timeExpression)
        {
            var timeSpan = "";
            var timeList = timeExpression.Split(':');

            // Very quick chechout, customer regrets checkin
            if (timeList[0] == "00" && timeList[1] == "00" && timeList[2] == "00") return "No time registred";

            // Days
            if (timeList[0] != "00")
            {
                if (timeList[0] == "01") timeSpan += "1 day ";
                else timeSpan += (timeList[0] + " days ");
            }

            // Hours
            if (timeList[1] != "00")
            {
                if (timeList[1] == "01") timeSpan += "1 hour";
                else
                {
                    if ((timeList[1])[0] == '0')    timeSpan += ((timeList[1])[1] + " hours");
                    else timeSpan += ((timeList[1]) + " hours");
                }
            }

            // Minutes
            if (timeList[2] != "00")
            {
                if (timeList[2] == "01") timeSpan += " and 1 minute.";
                else
                {
                    if ((timeList[2])[0] == '0') timeSpan += " and " + ((timeList[2])[1] + " minutes.");
                    else timeSpan += " and " + ((timeList[2]) + " minutes.");
                }
            }

            return timeSpan;
        }


        // POST: ParkedVehicles/Delete/5
        [HttpPost, ActionName("CheckOut")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOutConfirmed(int id)
        {
            ParkedVehicle parkedVehicle = db.ParkedVehicles.Find(id);
            db.ParkedVehicles.Remove(parkedVehicle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
