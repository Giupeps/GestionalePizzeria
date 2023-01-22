﻿using GestionalePizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionalePizzeria.Controllers
{
    public class HomeController : Controller
    {
        ModelDBContext dBContext= new ModelDBContext();

        
        public ActionResult Index()
        {
            var Listapizza = dBContext.Pizze.ToList();
            return View(Listapizza);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}