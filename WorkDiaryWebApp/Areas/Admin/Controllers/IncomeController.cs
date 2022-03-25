﻿using Microsoft.AspNetCore.Mvc;
using WorkDiaryWebApp.Core.Interfaces;

namespace WorkDiaryWebApp.Areas.Admin.Controllers
{
    public class IncomeController : BaseController
    {
        private readonly IIncomeService incomeService;
        public IncomeController(IIncomeService _incomeService)
        {
            incomeService = _incomeService;
        }

        public async Task <IActionResult> TotalIncomes()
        {
            var model = await incomeService.GetAllUsersIncomesHistory();
            return View(model);
        }
    }
}
