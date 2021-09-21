using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PortailIEPSM.Controllers.Groupe_4
{
    [Area("Groupe_4")]
    public class AccueilController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
