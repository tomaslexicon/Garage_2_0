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
        public ActionResult Index(string sortBy = "RegNo", string isDescending = "True", string search = "")
        {
            bool desc = isDescending.ToLower() == "true";
            string searchString = search; // TODO: fix whitespaces

            var model = new OverviewModel();
            model.IsDescending = desc;
            model.SortBy = sortBy;
            model.Search = searchString;
            model.Vehicles = GetOverviewVehicleList(sortBy, desc, search);

            return View(model);
        }

        private List<OverviewVehicle> GetOverviewVehicleList(string sortBy, bool isDescending, string search)
        {
            var vehicles = db.ParkedVehicles.ToList();
            var v = vehicles.Where(p => p.RegNo.ToLower().Contains(search.ToLower())).Select(p => new OverviewVehicle
            {
                Id = p.Id,
                RegNo = p.RegNo,
                Color = p.Color,
                StartTime = p.StartTime,
                Type = p.Type
            });

            switch (sortBy.ToLower())
            {
                case "type":
                    return isDescending ? v.OrderByDescending(i => i.Type.ToString()).ToList() : v.OrderBy(i => i.Type.ToString()).ToList();
                case "starttime":
                    return isDescending ? v.OrderByDescending(i => i.StartTime).ToList() : v.OrderBy(i => i.StartTime).ToList();
                case "color":
                    return isDescending ? v.OrderByDescending(i => i.Color).ToList() : v.OrderBy(i => i.Color).ToList();
                default:
                    return isDescending ? v.OrderByDescending(i => i.RegNo).ToList() : v.OrderBy(i => i.RegNo).ToList();
            }
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

            var model = new DetailModel();

            model.Id = parkedVehicle.Id;
            model.Type = parkedVehicle.Type;
            model.RegNo = parkedVehicle.RegNo;
            model.Color = parkedVehicle.Color;
            model.Brand = parkedVehicle.Brand;
            model.Model = parkedVehicle.Model;
            model.NumberOfWheels = parkedVehicle.NumberOfWheels;
            model.StartTime = parkedVehicle.StartTime.ToString("g");

            // model.ParkingTime = DateTime.Now.Subtract(parkedVehicle.StartTime).ToString(@"hh\:mm");

            model.ParkingTime = DateTime.Now.Subtract(parkedVehicle.StartTime).Hours + " h, " + DateTime.Now.Subtract(parkedVehicle.StartTime).Minutes + " min";


            return View(model);
        }

        // GET: ParkedVehicles/CheckIn
        public ActionResult CheckIn()
        {
            return View();
        }

        // POST: ParkedVehicles/CheckIn
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn([Bind(Include = "Id,Type,RegNo,Color,Brand,Model,NumberOfWheels")] CheckInModel checkInVehicle)
        {
            if (!ModelState.IsValid)
            {
                return View(checkInVehicle);
            }

            var v = db.ParkedVehicles.Where(p => p.RegNo == checkInVehicle.RegNo).ToList();
            if (v.Count != 0)
            {
                return View(checkInVehicle);
            }

            var parkedVehicle = new ParkedVehicle()
            {
                Id = checkInVehicle.Id,
                RegNo = checkInVehicle.RegNo,
                Type = checkInVehicle.Type,
                Color = checkInVehicle.Color,
                Brand = checkInVehicle.Brand,
                Model = checkInVehicle.Model,
                NumberOfWheels = checkInVehicle.NumberOfWheels,
                StartTime = DateTime.Now
            };

            db.ParkedVehicles.Add(parkedVehicle);
            db.SaveChanges();

            TempData["Feedback"] = "Your " +checkInVehicle.Type + " with registration number " +checkInVehicle.RegNo +  " has been checked in";
            return RedirectToAction("Index");
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
