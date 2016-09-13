using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackOverflowClone.Models;
using Microsoft.AspNetCore.Mvc;

namespace StackOverflowClone.ViewComponents
{
    public class ResponseFormViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int questionId)
        {
            var response = new Response(questionId);
            return View(response);
        }
    }
}
