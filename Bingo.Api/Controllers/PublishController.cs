﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Controllers
{
    public class PublishController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}