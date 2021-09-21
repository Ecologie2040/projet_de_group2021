using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PortailIEPSM.Controllers.Groupe_1
{
    [Area("Groupe_1")]
    public class AccueilController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
