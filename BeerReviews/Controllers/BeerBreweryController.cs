using BeerReviews.Data;
using BeerReviews.Models;
using BeerReviews.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeerReviews.Controllers
{
    public class BeerBreweryController : Controller
    {
        private BeerReviewsContext db = new BeerReviewsContext();
        // GET: BeerBrewery
        public ActionResult Index()
        {
            return View();
        }

        // GET: BeerBrewery/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BeerBrewery/Create
        public ActionResult Create()
        {
            PopulateStylesDropDownList();
            return View();
        }

        // POST: BeerBrewery/Create
        [HttpPost]
        public ActionResult Create([Bind] BeerBreweryViewModel beerBreweryVM, HttpPostedFileBase file)
        {
            try
            {
                Beer beer = new Beer();
                beer.Abv = beerBreweryVM.Abv;
                beer.Description = beerBreweryVM.Description;
                beer.Gravity = beerBreweryVM.Gravity;
                beer.IBU = beerBreweryVM.IBU;
              //  beer.ImageUrl = beerBreweryVM.ImageUrl;
                beer.Name = beerBreweryVM.Name;
                beer.StyleID = beerBreweryVM.StyleID;
                beer.ImageUrl=FileUpload(file);

                    db.Beers.Add(beer);
                    db.SaveChanges();



                foreach(var bb in beerBreweryVM.BreweriesNames)
                {
                    var bId = db.Breweries.Where(b => b.Name == bb);
                    if (bId.Any())
                    { 
                    var bb2 = new BeerBrewery();
                    bb2.BeerID = beer.BeerID;
                    bb2.BreweryID = bId.First().BreweryID;
                    db.BeerBreweries.Add(bb2);
                    db.SaveChanges();
                    }
                }
                
                foreach (var bb in beerBreweryVM.BreweriesPlacesNames)
                {
                    var bId = db.Breweries.Where(b => b.Name == bb);
                    if (bId.Any())
                    {
                        var bb2 = new BeerBrewery();
                        bb2.BeerID = beer.BeerID;
                        bb2.isPlace = true;
                        bb2.BreweryID = bId.First().BreweryID;
                        db.BeerBreweries.Add(bb2);
                        db.SaveChanges();
                    }
                }
                //     var id = beerBreweryVM.BreweriesNames.First();
                //     beerBrewery.BreweryID=db.Breweries.Where(b => b.Name == id).First().BreweryID;

                //     db.BeerBreweries.Add(beerBrewery);


                PopulateStylesDropDownList(beerBreweryVM.StyleID);
                return RedirectToAction("Index", "Beer");
            }
            catch
            {
                PopulateStylesDropDownList(beerBreweryVM.StyleID);
                return View();
            }
        }

        [HttpPost]
        public JsonResult Create2(string Prefix)
        {
            //Note : you can bind same list from database  
            List<Brewery> ObjList = db.Breweries.ToList();

            //Searching records from list using LINQ query  
            var BreweryName = (from N in ObjList
                               where N.Name.ToLower().Contains(Prefix.ToLower())
                               select new { N.Name });
            //     var BreweryName = db.Beers.Where(b => b.Name.StartsWith(Prefix)).ToList();
            //                        where N.Name.StartsWith(Prefix)
            //                         select new { N.Name });
            return Json(BreweryName, JsonRequestBehavior.AllowGet);
        }

        // GET: BeerBrewery/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BeerBrewery/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index","Beer");
            }
            catch
            {
                return View();
            }
        }

        // GET: BeerBrewery/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BeerBrewery/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public string FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string p = Path.Combine("/Content/Images/Beers", pic);
                string path = Server.MapPath(p);

                file.SaveAs(path);

                return p;
            }
            else return "/Content/Images/no_image.png";
        }

        private void PopulateStylesDropDownList(object selectedStyle = null)
        {
            var stylesQuery = from s in db.Styles
                              orderby s.Name
                              select s;
            ViewBag.StyleID = new SelectList(stylesQuery, "StyleID", "Name", selectedStyle);
        }
    }


}
