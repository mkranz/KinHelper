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
            return View(new ImportViewModel());
        }

        [HttpPost]
        [Timed]
        public ActionResult Import(ImportViewModel viewModel)
        {
            var parser = new KinshipParser(_context);
            var kinship = _context.Kinships.FirstOrDefault(x => x.LotroId == viewModel.KinshipId);
            if (kinship == null)
            {
                kinship = new Kinship() { LotroId = viewModel.KinshipId };
                _context.Kinships.Add(kinship);
            }

            parser.Update(kinship);
            _context.SaveChanges();

            return RedirectToAction("Kinship", new { id = kinship.Id });
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
            foreach (var character in kinship.Roster)
            {
                parser.Update(character.Character);
                _context.SaveChanges();
            }
            
            return RedirectToAction("Kinship", new { id });
        }
    }
}
