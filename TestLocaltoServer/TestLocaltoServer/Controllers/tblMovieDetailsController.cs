using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestLocaltoServer;
using Microsoft.AspNet.Identity;

namespace TestLocaltoServer.Controllers
{
    public class tblMovieDetailsController : Controller
    {
        private LocattoServerEntities db = new LocattoServerEntities();

        // GET: tblMovieDetails
        public ActionResult Index()
        {
                      
            if(CheckUserAuthorize() == true)    // checking user is authorized or not 
            {
                if(CheckIfAdmin() == true)     // checking user is admin or not
                {
                    var tblMovieDetails = db.tblMovieDetails.Include(t => t.AspNetUser);
                    return View(tblMovieDetails);
                }
                else
                {
                    var strUserId = User.Identity.GetUserId();
                    // var tblMovieDetails = db.tblMovieDetails.Include(t => t.AspNetUser);
                    var tblMovieDetails = db.tblMovieDetails.Where(t => t.UserId == strUserId);
                    return View(tblMovieDetails.ToList());
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // GET: tblMovieDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (CheckUserAuthorize() == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tblMovieDetail tblMovieDetail = db.tblMovieDetails.Find(id);
                if (tblMovieDetail == null)
                {
                    return HttpNotFound();
                }
                return View(tblMovieDetail);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: tblMovieDetails/Create
        public ActionResult Create()
        {
            if (CheckUserAuthorize() == true)
            {
                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: tblMovieDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,MovieName,Actor,Actress")] tblMovieDetail tblMovieDetail)
        {
            if (CheckUserAuthorize() == true)
            {
                tblMovieDetail.UserId = User.Identity.GetUserId();
                if (ModelState.IsValid)
                {
                    db.tblMovieDetails.Add(tblMovieDetail);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", tblMovieDetail.UserId);
                return View(tblMovieDetail);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: tblMovieDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMovieDetail tblMovieDetail = db.tblMovieDetails.Find(id);
            if (tblMovieDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", tblMovieDetail.UserId);
            return View(tblMovieDetail);
        }

        // POST: tblMovieDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,MovieName,Actor,Actress")] tblMovieDetail tblMovieDetail)
        {
            if (CheckUserAuthorize() == true)
            {
                tblMovieDetail.UserId = User.Identity.GetUserId();
                if (ModelState.IsValid)
                {
                    db.Entry(tblMovieDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", tblMovieDetail.UserId);
                return View(tblMovieDetail);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: tblMovieDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (CheckUserAuthorize() == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tblMovieDetail tblMovieDetail = db.tblMovieDetails.Find(id);
                if (tblMovieDetail == null)
                {
                    return HttpNotFound();
                }
                return View(tblMovieDetail);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: tblMovieDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblMovieDetail tblMovieDetail = db.tblMovieDetails.Find(id);
            db.tblMovieDetails.Remove(tblMovieDetail);
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

        public Boolean CheckUserAuthorize()
        {
            var strUserID = User.Identity.GetUserId();
            var strUserName = User.Identity.GetUserName();
            if(strUserID != null && strUserName != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean CheckIfAdmin()
        {
            if(User.Identity.GetUserName() == "admin@admin.com")
            {
                return true;
            }
            else
            {
                return false;
            }

        }


    }
}
