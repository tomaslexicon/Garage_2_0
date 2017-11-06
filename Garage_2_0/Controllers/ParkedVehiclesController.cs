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
            bool searchIsEmpty = string.IsNullOrEmpty(search);
            var v = db.ParkedVehicles.Include(e => e.Member).Include(e => e.VehicleType).Where(p => searchIsEmpty ? true : p.RegNo.ToLower().Contains(search.ToLower())).Select(p => new OverviewVehicle
            {
                Id = p.Id,
                RegNo = p.RegNo,
                Type = p.VehicleType.Type,
                Brand = p.Brand,
                StartTime = p.StartTime,
                OwnerName = p.Member.LastName + ", " + p.Member.FirstName
            });

            switch (sortBy.ToLower())
            {
                case "type":
                    return isDescending ? v.OrderByDescending(i => i.Type.ToString()).ToList() : v.OrderBy(i => i.Type.ToString()).ToList();
                case "ownername":
                    return isDescending ? v.OrderByDescending(i => i.OwnerName).ToList() : v.OrderBy(i => i.OwnerName).ToList();
                case "starttime":
                    return isDescending ? v.OrderByDescending(i => i.StartTime).ToList() : v.OrderBy(i => i.StartTime).ToList();
                case "brand":
                    return isDescending ? v.OrderByDescending(i => i.Brand).ToList() : v.OrderBy(i => i.Brand).ToList();
                default:
                    return isDescending ? v.OrderByDescending(i => i.RegNo).ToList() : v.OrderBy(i => i.RegNo).ToList();
            }
        }

        // GET: ParkedVehicles
        public ActionResult OverviewDetails(string sortBy = "RegNo", string isDescending = "True", string search = "")
        {
            bool desc = isDescending.ToLower() == "true";
            var model = new OverviewDetailModel();
            model.IsDescending = desc;
            model.SortBy = sortBy;
            model.Search = search;
            model.Vehicles = GetOverviewDetailVehicleList(sortBy, desc, search);

            return View(model);
        }

        private List<OverviewDetailVehicle> GetOverviewDetailVehicleList(string sortBy, bool isDescending, string search)
        {
            bool searchIsEmpty = string.IsNullOrEmpty(search);
            var v = db.ParkedVehicles.Include(e => e.Member).Include(e => e.VehicleType).Where(p => searchIsEmpty ? true : p.RegNo.ToLower().Contains(search.ToLower())).Select(p => new OverviewDetailVehicle
            {
                Id = p.Id,
                RegNo = p.RegNo,
                Type = p.VehicleType.Type,
                Brand = p.Brand,
                Color = p.Color,
                NumberOfWheels = p.NumberOfWheels,
                StartTime = p.StartTime,
                FirstName = p.Member.FirstName,
                LastName = p.Member.LastName,
                MembershipId = p.Member.MembershipId
            });

            switch (sortBy.ToLower())
            {
                case "type":
                    return isDescending ? v.OrderByDescending(i => i.Type.ToString()).ToList() : v.OrderBy(i => i.Type.ToString()).ToList();
                case "ownername":
                    return isDescending ? v.OrderByDescending(i => i.LastName).ToList() : v.OrderBy(i => i.LastName).ToList();
                case "starttime":
                    return isDescending ? v.OrderByDescending(i => i.StartTime).ToList() : v.OrderBy(i => i.StartTime).ToList();
                case "brand":
                    return isDescending ? v.OrderByDescending(i => i.Brand).ToList() : v.OrderBy(i => i.Brand).ToList();
                case "color":
                    return isDescending ? v.OrderByDescending(i => i.Color).ToList() : v.OrderBy(i => i.Color).ToList();
                case "numberofwheels":
                    return isDescending ? v.OrderByDescending(i => i.NumberOfWheels).ToList() : v.OrderBy(i => i.NumberOfWheels).ToList();
                case "membershipid":
                    return isDescending ? v.OrderByDescending(i => i.MembershipId ).ToList() : v.OrderBy(i => i.MembershipId).ToList();
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
            ParkedVehicle parkedVehicle = db.ParkedVehicles.Include(p => p.Member).Include(p => p.VehicleType).Where(p => p.Id == id).First();
            if (parkedVehicle == null)
            {
                return HttpNotFound();
            }

            var model = new DetailModel();

            model.Id = parkedVehicle.Id;
            model.Type = parkedVehicle.VehicleType.Type;
            model.RegNo = parkedVehicle.RegNo;
            model.Color = parkedVehicle.Color;
            model.Brand = parkedVehicle.Brand;
            model.Model = parkedVehicle.Model;
            model.NumberOfWheels = parkedVehicle.NumberOfWheels;
            model.StartTime = parkedVehicle.StartTime.ToString("g");
            model.ParkingTime = formatTimeSpan(DateTime.Now.Subtract(parkedVehicle.StartTime).ToString(@"dd\:hh\:mm"));
            model.FirstName = parkedVehicle.Member.FirstName;
            model.LastName = parkedVehicle.Member.LastName;
            return View(model);
        }

        // GET: ParkedVehicles/CheckIn
        public ActionResult CheckIn(int? id)
        {
            var types = BuildVehicleTypeList();
            var members = BuildMemberList(id);

            CheckInModel model = new CheckInModel
            {
                RegNo = "",
                VehicleTypes = types,
                Members = members,
                Brand = "",
            };

            return View(model);
        }

        // POST: ParkedVehicles/CheckIn
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn([Bind(Include = "Id,MemberId,Type,RegNo,Color,Brand,Model,NumberOfWheels")] CheckInModel checkInVehicle)
        {
            if (!ModelState.IsValid)
            {
                checkInVehicle.Members = BuildMemberList(checkInVehicle.MemberId);
                checkInVehicle.VehicleTypes = BuildVehicleTypeList(checkInVehicle.Type);

                return View(checkInVehicle);
            }

            var v = db.ParkedVehicles.Where(p => p.RegNo == checkInVehicle.RegNo).ToList();
            if (v.Count != 0)
            {
                checkInVehicle.Members = BuildMemberList(checkInVehicle.MemberId);
                checkInVehicle.VehicleTypes = BuildVehicleTypeList(checkInVehicle.Type);

                return View(checkInVehicle);
            }

            var parkedVehicle = new ParkedVehicle()
            {
                Id = checkInVehicle.Id,
                RegNo = checkInVehicle.RegNo,
                VehicleTypeId = checkInVehicle.Type,
                Color = checkInVehicle.Color,
                Brand = checkInVehicle.Brand,
                Model = checkInVehicle.Model,
                NumberOfWheels = checkInVehicle.NumberOfWheels,
                StartTime = DateTime.Now,
                MemberId = checkInVehicle.MemberId        
            };

            db.ParkedVehicles.Add(parkedVehicle);
            db.SaveChanges();

            TempData["Feedback"] = "Your " + checkInVehicle.Type + " with registration number " + checkInVehicle.RegNo + " has been checked in";
            return RedirectToAction("Index");
        }

        private IEnumerable<SelectListItem> BuildMemberList(int? id)
        {
            return db.Members.OrderBy(p => p.LastName).ToList().Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.LastName + ", " + m.FirstName,
                Selected = id == null ? false : m.Id == id      
            });
        }

        private IEnumerable<SelectListItem> BuildVehicleTypeList(int? typeId = null)
        {
            return db.VehicleTypes.OrderBy(p => p.Type).ToList().Select(vt => new SelectListItem
            {
                Value = vt.Id.ToString(),
                Text = vt.Type,
                Selected = typeId == null ? false : vt.Id == typeId
            });
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
                NumberOfWheels = parkedVehicle.NumberOfWheels,
                StartTime = parkedVehicle.StartTime,
                OriginalRegNo = parkedVehicle.RegNo,
                Members = BuildMemberList(parkedVehicle.MemberId),
                VehicleTypes = BuildVehicleTypeList(parkedVehicle.VehicleTypeId),
                Type = parkedVehicle.VehicleTypeId,
                MemberId = parkedVehicle.MemberId
            };

            return View(model);
        }

        public ActionResult Statistics()
        {
            var vehicles = db.ParkedVehicles.ToList();

            //var result = db.ParkedVehicles.Sum(v => v.NumberOfWheels); // ta bort kör linq lambda istället 

            var model = new StatisticsModel()
            {
                NumberOfVehicles = db.ParkedVehicles.Count(),
                mostPopularBrand = MostPopularBrand(),
                TotalNumberOfWheels = db.ParkedVehicles.Sum(v => v.NumberOfWheels), 
                TotalCost = CalculateTotalCost(vehicles)
            };
                        return View(model);
        }

        private int CalculateTotalCost(List<ParkedVehicle> vehicles)
        {
            double totalParkingCost = 0;

            foreach (var vehicle in vehicles)
            {
                totalParkingCost += 0.2 * (Convert.ToDouble(DateTime.Now.Subtract(vehicle.StartTime).TotalMinutes));
            }

            return Convert.ToInt32(totalParkingCost);
        }

        

        private string MostPopularBrand()
        {
            var result = db.ParkedVehicles
                .GroupBy(x => x.Brand)
                .OrderByDescending(x => x.Count())                
                .First().Key;

 
            return result;
        }





        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MemberId,Type,RegNo,Color,Brand,Model,NumberOfWheels")] EditModel parkedVehicle)
        {
            if (!ModelState.IsValid)
            {
                parkedVehicle.Members = BuildMemberList(parkedVehicle.MemberId);
                parkedVehicle.VehicleTypes = BuildVehicleTypeList(parkedVehicle.Type);
                return View(parkedVehicle);
            }

            var vh = db.ParkedVehicles.Where(p => p.RegNo == parkedVehicle.RegNo && p.Id != parkedVehicle.Id).ToList();
            if (vh.Count != 0)
            {
                parkedVehicle.StartTime = db.ParkedVehicles.AsNoTracking().FirstOrDefault(p => p.Id == parkedVehicle.Id).StartTime;
                parkedVehicle.Members = BuildMemberList(parkedVehicle.MemberId);
                parkedVehicle.VehicleTypes = BuildVehicleTypeList(parkedVehicle.Type);
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
                VehicleTypeId = parkedVehicle.Type,
                MemberId = parkedVehicle.MemberId,
                NumberOfWheels = parkedVehicle.NumberOfWheels,
                StartTime = startTime,
            };

            db.Entry(v).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Feedback"] = "Your " + v.VehicleType + " with registration number " + v.RegNo + " has been successfully changed";
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
               
            };

            return View(CheckOutVehicle);
        }

        // generates receipt
        public ActionResult ConfirmCheckout(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // var ParkedVehicle = db.ParkedVehicles.Find(id);
            var ParkedVehicle = db.ParkedVehicles
                .Include(v => v.Member)
                .Where(v => v.Id == id)
                .FirstOrDefault();

            var ParkStopTime = DateTime.Now; 
            var ParkingMinutes = ParkStopTime.Subtract(ParkedVehicle.StartTime).TotalMinutes;
            const double COST_PER_MINUTE = 0.20;

            

            var CheckOutVehicle = new ReceiptModel()
            {
                Id = ParkedVehicle.Id,
                RegNo = ParkedVehicle.RegNo,
                StartTime = ParkedVehicle.StartTime.ToString("g"),
                StopTime = ParkStopTime.ToString("g"),
                ParkingTime = formatTimeSpan(ParkStopTime.Subtract(ParkedVehicle.StartTime).ToString(@"dd\:hh\:mm")),
                ParkingCost = Math.Floor(ParkingMinutes * COST_PER_MINUTE).ToString() + " kr",
                LastName = ParkedVehicle.Member.LastName,
                FirstName = ParkedVehicle.Member.FirstName
                
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
            
        }

        
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
                if (timeList[2] == "01") timeSpan += separator + "1 minute";
                else
                {
                    if ((timeList[2])[0] == '0') timeSpan += separator + (timeList[2])[1] + " minutes";
                    else timeSpan += separator + timeList[2] + " minutes";
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
