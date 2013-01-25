using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KinHelper.Model.Db;
using KinHelper.Model.Entities;
using KinHelper.Model.Parsers;
using KinHelper.Web.Filters;
using KinHelper.Web.Models;

namespace KinHelper.Web.Controllers
{
    public class KinshipController : Controller
    {
        private readonly KinHelperContext _context;
        //
        // GET: /Kinships/
        public KinshipController(KinHelperContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var kinships = _context.Kinships.ToList();
            return View(kinships);
        }

        [Timed]
        public ActionResult Kinship(int id)
        {
            var kinship = _context.Kinships.First(x => x.Id == id);
            return View(kinship);
        }

        [HttpGet]
        public ActionResult Import()
        {
            return View(new ImportViewModel()
                            {
                                Server = "Crickhollow"
                            });
        }

        [HttpPost]
        [Timed]
        public ActionResult Import(ImportViewModel viewModel)
        {
            var parser = new KinshipParser(_context);

            var kinshipId = viewModel.KinshipId;
            if (viewModel.KinshipId == null)
            {
                if (viewModel.Name != null && viewModel.Server != null)
                {
                    kinshipId = parser.GetKinshipId(viewModel.Server, viewModel.Name);
                }
            }
            if (kinshipId == null)
                return View(viewModel);

            var kinship = _context.Kinships.FirstOrDefault(x => x.LotroId == kinshipId);
            if (kinship == null)
            {
                kinship = new Kinship() { LotroId = kinshipId };
                _context.Kinships.Add(kinship);
            }

            parser.Update(kinship);
            _context.SaveChanges();

            return RedirectToAction("Kinship","Kinship", new { id = kinship.Id });
        }

        [HttpGet]
        [Timed]
        public ActionResult RefreshRoster(int id)
        {
            var parser = new KinshipParser(_context);
            var kinship = _context.Kinships.FirstOrDefault(x => x.Id == id);
            parser.UpdateRoster(kinship);
            _context.SaveChanges();
            return RedirectToAction("Kinship", new { id});
        }

        [HttpGet]
        [Timed]
        public ActionResult LoadPlayers(int id)
        {
            var parser = new CharacterParser(_context);
            var kinship = _context.Kinships.FirstOrDefault(x => x.Id == id);
            var characters = kinship.Roster.Select(x => x.Character).Where(x => x.User == null).OrderBy(x => x.Name);
            foreach (var character in characters)
            {
                parser.Update(character);
                _context.SaveChanges();
            }
            
            return RedirectToAction("Kinship", new { id });
        }

        public ActionResult CrossKinUsers()
        {
            //_context.Users.Where(x => _context.Characters.Any(y => y.User == x) && _context.Characters)
            return null;
        }
    }
}
