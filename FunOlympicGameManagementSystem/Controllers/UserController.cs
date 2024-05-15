﻿using FunOlympicGameManagementSystem.Models;
using FunOlympicGameManagementSystem.Models.DAO;
using FunOlympicGameManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunOlympicGameManagementSystem.Controllers {
    [Authorize]
    public class UserController : Controller {
        private readonly ILogger<UserController> _logger;
        private readonly AppDbContext _appDbContext;

        public UserController(ILogger<UserController> logger, AppDbContext appDbContext) {
            _logger = logger;
            _appDbContext = appDbContext;
        }
        // GET: UserController
        public ActionResult List() {
            var users=_appDbContext.Users.Where(x=>x.Role.Equals(Roles.User.ToString())&&x.IsActive);
            return View(users);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id) {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create() {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            }
            catch {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id) {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            }
            catch {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(string id) {
            try
            {
                UserEntity user = _appDbContext.Users.Where(x => x.Id == id).SingleOrDefault();
                user.IsActive = false;
                _appDbContext.Update(user);
                _appDbContext.SaveChanges();
                TempData["Info"] = "Delete successfully";
            }
            catch (Exception)
            {
                TempData["Info"] = "Error when delete the recrod.";
            }
            return RedirectToAction("list");
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            }
            catch {
                return View();
            }
        }
    }
}
