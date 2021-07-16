﻿using Buoi_4_Bai_BigSchool__.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Buoi_4_Bai_BigSchool__.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();
            Course objCourse = new Course();
            objCourse.ListCategory = context.Categories.ToList();
            return View(objCourse);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            BigSchoolContext context = new BigSchoolContext();
            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = context.Categories.ToList();
                return View("Create", objCourse);
            }
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;
            context.Courses.Add(objCourse);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var course = new List<Course>();
            foreach(Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                    .FindById(objCourse.LecturerId).Name;
                course.Add(objCourse);
            }
            return View(course);
        }
        public  ActionResult Mine()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.Courses.Where(c => c.LecturerId == currentUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach(Course i in courses)
            {
                i.LectureName = currentUser.Name;
            }
            return View(courses);   
        }
        public ActionResult Edit(int? id)
        {
            BigSchoolContext context = new BigSchoolContext();
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Course course = context.Courses.Find(id);
            //if (course == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name", course.CategoryId);
            //return View(course);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course x = context.Courses.FirstOrDefault(m => m.Id == id);
            x.ListCategory = context.Categories.ToList();
            if (x == null)
            {
                return HttpNotFound();
            }
            return View(x);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LecturerId,Place,DateTime,CategoryId")] Course course)
        {
            BigSchoolContext context = new BigSchoolContext();
            //if (ModelState.IsValid)
            //{
            //    context.Entry(course).State = EntityState.Modified;
            //    context.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.CategoryId = new SelectList(context.Categories, "Id", "Name", course.CategoryId);
            //return View(course);
            if (!ModelState.IsValid)
            {
                course.ListCategory = context.Categories.ToList();
                return View("Edit", course);

            }

            context.Courses.AddOrUpdate(course);
            context.SaveChanges();
            return RedirectToAction("Mine");
        }
        public ActionResult Delete(int? id)
        {
            BigSchoolContext context = new BigSchoolContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = context.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            Course course = context.Courses.Find(id);
            context.Courses.Remove(course);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            BigSchoolContext context = new BigSchoolContext();

            var listFollwee = context.Followings.Where(p => p.FollowerId == currentUser.Id).ToList();

            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();

            var courses = new List<Course>();

            foreach(var course in listAttendances)
            {
                foreach(var item in listFollwee)
                {
                    if(item.FolloweeId == course.Course.LecturerId)
                    {
                        Course objCourse = course.Course;
                        objCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                            .FindById(objCourse.LecturerId).Name;
                        courses.Add(objCourse);
                    }
                }
            }
            return View(courses);
        }
    }
}