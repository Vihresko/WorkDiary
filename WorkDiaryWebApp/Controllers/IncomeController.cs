﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkDiaryWebApp.Core.Constants;
using WorkDiaryWebApp.Core.Interfaces;
using WorkDiaryWebApp.Core.Services;
using WorkDiaryWebApp.Models;
using WorkDiaryWebApp.Models.Income;
using WorkDiaryWebApp.WorkDiaryDB.Models;

namespace WorkDiaryWebApp.Controllers
{
    public class IncomeController : Controller
    {
        private readonly IClientService clientService;
        private readonly IProcedureService procedureService;
        private readonly UserManager<User> userManager;
        private readonly IIncomeService visitBagService;
        private readonly IIncomeService incomeService;
        public IncomeController(IClientService _clientService, IProcedureService _procedureService, UserManager<User> _userManager, IIncomeService _visitBagService, IIncomeService _incomeService)
        {
            clientService = _clientService;
            procedureService = _procedureService;
            visitBagService = _visitBagService;
            userManager = _userManager;
            incomeService = _incomeService;
        }
        public IActionResult UserIncomes()
        {
            return View();
        }

        public IActionResult TotalIncomes()
        {
            return View();
        }

        public IActionResult CreateIncome(string clientId)
        {
            var model = GetWorkModelForView(clientId);
            return View(model);
        }

        [HttpPost]
        public IActionResult AddIncomeToClientVisitBag(AddIncomePostModel addWorkmodel)
        {
            
            string userId = userManager.GetUserId(this.User);
            (bool isDone, string errors) = visitBagService.AddClientProcedureToVisitBag(addWorkmodel, userId);

            var model = GetWorkModelForView(addWorkmodel.ClientId);

            if (isDone)
            {
                ViewData[MessageConstant.SuccessMessage] = "success";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = errors;
            }

            return View("~/Views/Income/CreateIncome.cshtml",model);
        }

        public IActionResult ShowHistoryOfClient(string clientId)
        {
            TempData["Controller"] = "Income";
            TempData["Action"] = "ShowHistoryOfClient";
            TempData["neededId"] = $"?clientId={clientId}";
            var model = incomeService.ShowClientHistory(clientId);
            return View(model);
        }

        public IActionResult ShowClientVisitBag(string clientId)
        {
            TempData["Controller"] = "Income";
            TempData["Action"] = "CreateIncome";
            TempData["neededId"] = $"?clientId={clientId}";
            var model = incomeService.ShowClientVisitBag(clientId);
            return View(model);
        }

        private WorkModel GetWorkModelForView(string clientId)
        {
            //TempData is needed for Back button 
            TempData["Controller"] = "Income";
            TempData["Action"] = "CreateIncome";
            TempData["neededId"] = $"?clientId={clientId}";

            var clientModel = clientService.ClientInfo(clientId);
            var proceduresModel = procedureService.GetAllProcedures();
            var model = new WorkModel(clientModel, proceduresModel);
            return model;
        }
        
    }
}
