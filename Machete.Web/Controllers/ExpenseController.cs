using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Web.ViewModel;
using Machete.Helpers;
using Machete.Service;
using Microsoft.Web.Mvc;
namespace Machete.Web.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService expenseService;
        private readonly ICategoryService categoryService;
        public ExpenseController(IExpenseService expenseService, ICategoryService categoryService)
        {
            this.expenseService = expenseService;
            this.categoryService = categoryService;  
        }  
        public ActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            //If date is not passed, take current month's first and last dte 
            DateTime dtNow;
            dtNow = DateTime.Today;
            if (!startDate.HasValue)
            {
                startDate = new DateTime(dtNow.Year, dtNow.Month, 1);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
            }
            //take last date of start date's month, if end date is not passed 
            if (startDate.HasValue && !endDate.HasValue)
            {
                endDate = (new DateTime(startDate.Value.Year, startDate.Value.Month, 1)).AddMonths(1).AddDays(-1);
            }
            var expenses = expenseService.GetExpenses(startDate.Value ,endDate.Value);
            //if request is Ajax will return partial view
            if (Request.IsAjaxRequest())
            {
                return PartialView("ExpenseList", expenses);
            } 
            //set start date and end date to ViewBag dictionary
            ViewBag.StartDate = startDate.Value.ToShortDateString();
            ViewBag.EndDate = endDate.Value.ToShortDateString();
            //if request is not ajax
            return View(expenses);
        }
        public ActionResult Create()
        {
            var expenseModel = new ExpenseViewModel();
            var categories = categoryService.GetCategories();
            expenseModel.Category = categories.ToSelectListItems(-1);
            expenseModel.Date = DateTime.Today;
            return View(expenseModel);
        }
        [HttpPost]
        public ActionResult Create(ExpenseViewModel expenseViewModel)
        {
            
                if (!ModelState.IsValid)
                {
                    var categories = categoryService.GetCategories();
                    expenseViewModel.Category = categories.ToSelectListItems(expenseViewModel.CategoryId);
                    return View("Save", expenseViewModel);
                }
                Expense expense=new Expense();
                ModelCopier.CopyModel(expenseViewModel,expense);
                expenseService.CreateExpense(expense);
                return RedirectToAction("Index");
            
        }
        // GET: /Expense/Edit
        public ActionResult Edit(int id)
        {
            var expenseModel = new ExpenseViewModel();
            var expense = expenseService.GetExpense(id);
            ModelCopier.CopyModel(expense, expenseModel);
            var categories = categoryService.GetCategories();
            expenseModel.Category = categories.ToSelectListItems(expense.Category.CategoryId);               
            return View(expenseModel);
        }
        [HttpPost]
        public ActionResult Edit(ExpenseViewModel expenseViewModel)
        {
            
                if (!ModelState.IsValid)
                {
                    var categories = categoryService.GetCategories();
                    expenseViewModel.Category = categories.ToSelectListItems(expenseViewModel.CategoryId);
                    return View("Save", expenseViewModel);
                }
                var expenseToEdit = expenseService.GetExpense(expenseViewModel.ExpenseId);
                ModelCopier.CopyModel(expenseViewModel, expenseToEdit);               
                expenseService.SaveExpense();
                return RedirectToAction("Index");           
        }
        public ActionResult Delete(int id)
        {
            expenseService.DeleteExpense(id);
            DateTime startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            var expenses = expenseService.GetExpenses(startDate, endDate);
            return PartialView("ExpenseList", expenses);
        }   
    }
}
