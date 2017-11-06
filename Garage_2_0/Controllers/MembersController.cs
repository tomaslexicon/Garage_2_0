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
    public class MembersController : Controller
    {
        private GarageContext db = new GarageContext();

        //// GET: Members
        //public ActionResult Index()
        //{
        //    return View(db.Members.ToList());
        //}

        // GET: 
        public ActionResult Index(string sortBy = "LastName", string isDescending = "True", string search = "")
        {
            bool descending = isDescending.ToLower() == "true";
            var model = new MemberViewModel();
            model.IsDescending = descending;
            model.SortBy = sortBy;
            model.Search = search;
            model.Members = GetViewMemberList(sortBy, descending, search);

            return View(model);
        }

        private List<MemberView> GetViewMemberList(string sortBy, bool isDescending, string search)
        {
            bool searchIsEmpty = string.IsNullOrEmpty(search);
            var v = db.Members.Where(m => searchIsEmpty ? true : m.LastName.ToLower().Contains(search.ToLower())).Select(m => new MemberView
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName,
                MembershipId = m.MembershipId
            });

            switch (sortBy.ToLower())
            {
                case "firstname":
                    return isDescending ? v.OrderByDescending(i => i.FirstName).ToList() : v.OrderBy(i => i.FirstName).ToList();
                case "membershipid":
                    return isDescending ? v.OrderByDescending(i => i.MembershipId).ToList() : v.OrderBy(i => i.MembershipId).ToList();
                default:
                    return isDescending ? v.OrderByDescending(i => i.LastName).ToList() : v.OrderBy(i => i.LastName).ToList();
            }
        }


        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }

            // try class = Member, VehicleList
            var model = new MemberDetailsModel();
            model.Id = member.Id;
            model.FirstName = member.FirstName;
            model.LastName = member.LastName;
            model.MembershipId = member.MembershipId;
            model.MemberParkedVehicles = db.ParkedVehicles.Where(i => i.MemberId == id).Select(v => new MemberVehicle
            {
                Id = v.Id,
                RegNo = v.RegNo,
                Brand = v.Brand,
                Model = v.Model
            }).ToList();

            // return View(member);
            return View(model);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LastName,FirstName")] Member member)
        {
            if (ModelState.IsValid)
            {
                

                db.Members.Add(member);
                db.SaveChanges();
                member.MembershipId = 100000 + member.Id;
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LastName,FirstName,MembershipId")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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
