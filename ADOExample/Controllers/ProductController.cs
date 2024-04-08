using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  ADOExample.DAL;
using ADOExample.Models;


namespace ADOExample.Controllers
{
    public class ProductController : Controller
    {
        ProductDAL _productDAl= new ProductDAL();
        // GET: Product
        public ActionResult Index()
        {
            var productList=_productDAl.GetAllProducts();
            if(productList.Count==0)
            {
                TempData["InfoMessage"] = "Currently Products not available int the database......";
            }
            return View(productList);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var product = _productDAl.GetProductsByID(id).FirstOrDefault();

            try
            {
                if (product == null)
                {
                    TempData["InfoMessage"] = "Product not avaliable with id" + id.ToString();
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            bool IsInserted = false;

            try
            {
                if (ModelState.IsValid)
                {
                    IsInserted = _productDAl.InsertProduct(product);
                    if (IsInserted)
                    {
                        TempData["SuccessMessage"] = "Product details Saved Successfully..!";

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to save the product details";
                    }
                    
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }  
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var products= _productDAl.GetProductsByID(id).FirstOrDefault();
            if(products==null)
            {
                TempData["InfoMessage"] = "Product not avaliable with ID" + id.ToString();

                return RedirectToAction("Index");

            }
            return View(products);
        }

        // POST: Product/Edit/5
        [HttpPost,ActionName("Edit")]
        public ActionResult UpdateProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool IsUpdated = _productDAl.UpdateProduct(product);
                    if (IsUpdated)
                    {
                        TempData["SuccessMessage"] = "Product details Updated Successfully ...!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Product is alerady available/ anable to update the product details.";
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
            
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var product = _productDAl.GetProductsByID(id).FirstOrDefault();
                if (product == null)
                {
                    TempData["InfoMessage"] = "Product not avaliable with ID" + id.ToString();

                    return RedirectToAction("Index");

                }

                return View(product);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // POST: Product/Delete/5
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {
                string result = _productDAl.DeleteProduct(id);
                if (result.Contains("deleted"))
                {
                    TempData["SuccessMessage"] = result;

                }
                else
                {
                    TempData["ErrorMessage"] = result;
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
          
               
            
        }
    }
}
