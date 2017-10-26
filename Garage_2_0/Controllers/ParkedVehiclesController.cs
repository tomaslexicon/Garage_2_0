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
            var model = new OverviewModel();
            model.IsDescending = desc;
            model.SortBy = sortBy;
            model.Search = search;
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
            model.ParkingTime = formatTimeSpan(DateTime.Now.Subtract(parkedVehicle.StartTime).ToString(@"dd\:hh\:mm"));

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

            TempData["Feedback"] = "Your " + checkInVehicle.Type + " with registration number " + checkInVehicle.RegNo + " has been checked in";
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

            var model = new EditModel()
            {
                Id = parkedVehicle.Id,
                RegNo = parkedVehicle.RegNo,
                Color = parkedVehicle.Color,
                Brand = parkedVehicle.Brand,
                Model = parkedVehicle.Model,
                Type = parkedVehicle.Type,
                NumberOfWheels = parkedVehicle.NumberOfWheels,
                StartTime = parkedVehicle.StartTime,
                OriginalRegNo = parkedVehicle.RegNo
            };

            return View(model);
        }

        //[HttpPost, ActionName("Edit")]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditPost(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var vehicleToUpdate = db.ParkedVehicles.Find(id);

        //    //var vh = db.ParkedVehicles.Where(p => p.RegNo == vehicleToUpdate.RegNo && p.Id != vehicleToUpdate.Id).ToList();
        //    //if (vh.Count != 0)
        //    //{
        //    //    vehicleToUpdate.StartTime = db.ParkedVehicles.AsNoTracking().FirstOrDefault(p => p.Id == vehicleToUpdate.Id).StartTime;
        //    //    return View(vehicleToUpdate);
        //    //}

        //    if (TryUpdateModel(vehicleToUpdate, "ParkedVehicle",
        //       new string[] { "Type,RegNo,Color,Brand,Model,NumberOfWheels" }))
        //    {
        //        try
        //        {
        //            db.SaveChanges();

        //            TempData["Feedback"] = "Your " + vehicleToUpdate.Type + " with registration number " + vehicleToUpdate.RegNo + " has been successfully changed";
        //            return RedirectToAction("Index");
        //        }
        //        catch (DataException /* dex */)
        //        {
        //            //Log the error (uncomment dex variable name and add a line here to write a log.
        //            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
        //        }
        //    }

        //    return View(vehicleToUpdate);
        //}

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,RegNo,Color,Brand,Model,NumberOfWheels")] EditModel parkedVehicle)
        {
            if (!ModelState.IsValid)
            {
                return View(parkedVehicle);
            }

            var vh = db.ParkedVehicles.Where(p => p.RegNo == parkedVehicle.RegNo && p.Id != parkedVehicle.Id).ToList();
            if (vh.Count != 0)
            {
                parkedVehicle.StartTime = db.ParkedVehicles.AsNoTracking().FirstOrDefault(p => p.Id == parkedVehicle.Id).StartTime;
                return View(parkedVehicle);
            }

            var startTime = db.ParkedVehicles.AsNoTracking().FirstOrDefault(p => p.Id == parkedVehicle.Id).StartTime;

            var v = new ParkedVehicle()
            {
                Id = parkedVehicle.Id,
                RegNo = parkedVehicle.RegNo,
                Color = parkedVehicle.Color,
                Brand = parkedVehicle.Brand,
                Model = parkedVehicle.Model,
                Type = parkedVehicle.Type,
                NumberOfWheels = parkedVehicle.NumberOfWheels,
                StartTime = startTime
            };

            db.Entry(v).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Feedback"] = "Your " + v.Type + " with registration number " + v.RegNo + " has been successfully changed";
            return RedirectToAction("Index");
        }

        // GET: ParkedVehicles/CheckOut/5
        public ActionResult CheckOut(int? id)
        {
            var ParkedVehicle = db.ParkedVehicles.Find(id);
            var ParkStopTime = DateTime.Now;  // move to Confirmed
            //var ParkingMinutes = ParkStopTime.Subtract(ParkedVehicle.StartTime).TotalMinutes;
            //const double COST_PER_MINUTE = 0.20;

            // var temp = formatTimeSpan(ParkStopTime.Subtract(ParkedVehicle.StartTime).ToString(@"dd\:hh\:mm"));

            var CheckOutVehicle = new CheckOutModel()
            {
                Id = ParkedVehicle.Id,
                RegNo = ParkedVehicle.RegNo,
                StartTime = ParkedVehicle.StartTime.ToString("g"),
                //StopTime = "not set yet",
                //StopTime = ParkStopTime.ToString("g"),
                //ParkingTime = "not set yet",
                // ParkingTime = formatTimeSpan(ParkStopTime.Subtract(ParkedVehicle.StartTime).ToString(@"dd\:hh\:mm")),
                //ParkingCost = "not set yet"
                //ParkingCost = Math.Floor(ParkingMinutes * COST_PER_MINUTE).ToString() + " kr."
            };

            return View(CheckOutVehicle);
        }

        // generates receipt
        public ActionResult ConfirmCheckout(int? id)
        {
            var ParkedVehicle = db.ParkedVehicles.Find(id);
            var ParkStopTime = DateTime.Now;  // move to Confirmed
            var ParkingMinutes = ParkStopTime.Subtract(ParkedVehicle.StartTime).TotalMinutes;
            const double COST_PER_MINUTE = 0.20;

            //string temp1 = formatTimeSpan("01:02:00");
            //string temp2 = formatTimeSpan("01:02:30");
            //string temp3 = formatTimeSpan("00:22:10");
            //string temp4 = formatTimeSpan("01:12:00");
            //string temp5 = formatTimeSpan("01:00:09");
            //string temp6 = formatTimeSpan("01:00:40");

            var CheckOutVehicle = new ReceiptModel()
            {
                Id = ParkedVehicle.Id,
                RegNo = ParkedVehicle.RegNo,
                StartTime = ParkedVehicle.StartTime.ToString("g"),
                StopTime = ParkStopTime.ToString("g"),
                ParkingTime = formatTimeSpan(ParkStopTime.Subtract(ParkedVehicle.StartTime).ToString(@"dd\:hh\:mm")),
                ParkingCost = Math.Floor(ParkingMinutes * COST_PER_MINUTE).ToString() + " kr."
            };
            return View(CheckOutVehicle);
        }


        // POST: ParkedVehicles/Delete/5
        [HttpPost, ActionName("ConfirmCheckout")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOutConfirmed(int id)
        {
            ParkedVehicle parkedVehicle = db.ParkedVehicles.Find(id);
            db.ParkedVehicles.Remove(parkedVehicle);
            db.SaveChanges();
            return RedirectToAction("Index");
            // return RedirectToAction("Home");
        }

        //  // POST: ParkedVehicles/Delete/5
        //  [HttpPost, ActionName("CheckOut")]
        //  [ValidateAntiForgeryToken]
        //  public ActionResult CheckOutConfirmed(int id)
        //  {
        //    ParkedVehicle parkedVehicle = db.ParkedVehicles.Find(id);
        //    db.ParkedVehicles.Remove(parkedVehicle);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //  }

        // 01:02:00
        private string formatTimeSpan(string timeExpression)
        {
            var timeSpan = "";
            var timeList = timeExpression.Split(':');
            string separator = "";

            // Very quick chechout, customer regrets checkin within one minute
            if (timeList[0] == "00" && timeList[1] == "00" && timeList[2] == "00") return "No time registred";

            // Add days to output string
            if (timeList[0] != "00")
            {
                if (timeList[2] == "00") separator = " and ";
                else separator = ", ";

                if (timeList[0] == "01") timeSpan += "1 day ";
                else
                {
                    if ((timeList[0])[0] == '0') timeSpan += ((timeList[0])[1] + " days ");
                    else timeSpan += ((timeList[0]) + " days ");
                }
            }

            // Add hours to string
            if (timeList[1] != "00")
            {
                if (timeList[1] == "01") timeSpan += (separator + "1 hour");
                else
                {
                    if ((timeList[1])[0] == '0') timeSpan += separator + (timeList[1])[1] + " hours";
                    else timeSpan += separator + (timeList[1] + " hours");
                }
                separator = " and ";
            }

            // Add minutes to string
            if (timeList[2] != "00")
            {
                if (separator == ", ") separator = " and ";
                if (timeList[2] == "01") timeSpan += separator + "1 minute.";
                else
                {
                    if ((timeList[2])[0] == '0') timeSpan += separator + (timeList[2])[1] + " minutes.";
                    else timeSpan += separator + timeList[2] + " minutes.";
                }
            }

            return timeSpan;
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
