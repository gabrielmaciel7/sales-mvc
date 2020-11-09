using Microsoft.AspNetCore.Mvc;
using sales_mvc.Models;
using sales_mvc.Models.ViewModels;
using sales_mvc.Services;
using sales_mvc.Services.Exceptions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace sales_mvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var sellersList = await _sellerService.FindAll();

            return View(sellersList);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                return View(seller);
            }

            await _sellerService.Insert(seller);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });
            }

            var seller = await _sellerService.FindById(id.Value);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found." });
            }

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.Remove(id);

                return RedirectToAction(nameof(Index));
            }
            catch(IntegrityException err)
            {
                return RedirectToAction(nameof(Error), new { message = err.Message });
            }

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });
            }

            var seller = await _sellerService.FindById(id.Value);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found." });
            }

            return View(seller);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });
            }

            var seller = await _sellerService.FindById(id.Value);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found." });
            }

            var departments = await _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch." });
            }

            try
            {
                await _sellerService.Update(seller);

                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException err)
            {
                return RedirectToAction(nameof(Error), new { message = err.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var errorViewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(errorViewModel);
        }
    }
}
