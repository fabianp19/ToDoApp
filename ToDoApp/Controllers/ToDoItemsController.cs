using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class ToDoItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ToDoItems
        public ActionResult Index()
        {
            return View();
        }

        private IEnumerable<ToDoItem> UserToDoes()
        {
            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);

            IEnumerable<ToDoItem> numbersOfToDoes = db.ToDoItems.ToList().Where(x => x.User == currentUser);

            int completedToDo = 0;
            foreach(var ToDo in numbersOfToDoes)
            {
                if(ToDo.IsDone)
                {
                    completedToDo++;
                }
            }

            ViewBag.Percent = Math.Round(100f * ((float)completedToDo / (float)numbersOfToDoes.Count()));

            return numbersOfToDoes;
        }

        public ActionResult BuildToDoTable()
        {
            return PartialView("_ToDoTable", UserToDoes());
        }

        // GET: ToDoItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoItem toDoItem = db.ToDoItems.Find(id);
            if (toDoItem == null)
            {
                return HttpNotFound();
            }
            return View(toDoItem);
        }

        // GET: ToDoItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,IsDone")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                toDoItem.User = currentUser;
                db.ToDoItems.Add(toDoItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toDoItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,Description")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                toDoItem.User = currentUser;
                toDoItem.IsDone = false;
                db.ToDoItems.Add(toDoItem);
                db.SaveChanges();
            }

            return PartialView("_ToDoTable", UserToDoes());
        }

        // GET: ToDoItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoItem toDoItem = db.ToDoItems.Find(id);

            if (toDoItem == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);

            if(toDoItem.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(toDoItem);
        }

        // POST: ToDoItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,IsDone")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toDoItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toDoItem);
        }

        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoItem toDoItem = db.ToDoItems.Find(id);
            if (toDoItem == null)
            {
                return HttpNotFound();
            }
            else
            {
                toDoItem.IsDone = value;
                db.Entry(toDoItem).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_ToDoTable", UserToDoes());
            }
        }

        // GET: ToDoItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ToDoItem toDoItem = db.ToDoItems.Find(id);

            if (toDoItem == null)
            {
                return HttpNotFound();
            }
            return View(toDoItem);
        }

        // POST: ToDoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToDoItem toDoItem = db.ToDoItems.Find(id);
            db.ToDoItems.Remove(toDoItem);
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
