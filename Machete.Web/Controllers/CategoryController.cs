using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Helpers;
using Machete.Service;
namespace Machete.Web.Controllers
{
  
public class CategoryController : Controller
{
    private readonly ICategoryService categoryService;   

    public CategoryController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
       
    }  
    public ActionResult Index()
    {
        var categories = categoryService.GetCategories();
        return View(categories);
    }
    [HttpGet]
    public ActionResult Edit(int id)
    {

        var category = categoryService.GetCategory(id);
        return View(category);
    }

    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {

        var category = categoryService.GetCategory(id);
        if (TryUpdateModel(category))
        {
            categoryService.SaveCategory();
            return RedirectToAction("Index");
        }
        else return View(category);            
    } 

    [HttpGet]
    public ActionResult Create()
    {
        var category = new Category();
        return View(category);
    }
     
    [HttpPost]
    public ActionResult Create(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }
        categoryService.CreateCategory(category);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult Delete(int  id)
    {
        categoryService.DeleteCategory(id);
        var categories = categoryService.GetCategories();
        return PartialView("CategoryList", categories);

    }       
}
}
