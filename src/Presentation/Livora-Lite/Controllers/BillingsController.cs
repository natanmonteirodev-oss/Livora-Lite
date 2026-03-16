using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;

namespace Livora_Lite.Presentation.Controllers
{
    [Authorize]
    public class BillingsController : Controller
    {
        private readonly IBillingService _billingService;
        private readonly IContractService _contractService;
        private readonly IBillingStatusService _billingStatusService;

        public BillingsController(
            IBillingService billingService,
            IContractService contractService,
            IBillingStatusService billingStatusService)
        {
            _billingService = billingService;
            _contractService = contractService;
            _billingStatusService = billingStatusService;
        }

        // GET: Billings
        public async Task<IActionResult> Index()
        {
            var billings = await _billingService.GetAllBillingsAsync();
            return View(billings);
        }

        // GET: Billings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _billingService.GetBillingByIdAsync(id.Value);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }

        // GET: Billings/Create
        public async Task<IActionResult> Create()
        {
            var contracts = await _contractService.GetAllContractsAsync();
            var billingStatuses = await _billingStatusService.GetAllBillingStatusesAsync();

            ViewBag.Contracts = contracts;
            ViewBag.BillingStatuses = billingStatuses;

            return View();
        }

        // POST: Billings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBillingRequestDTO request)
        {
            if (ModelState.IsValid)
            {
                await _billingService.CreateBillingAsync(request);
                return RedirectToAction(nameof(Index));
            }

            var contracts = await _contractService.GetAllContractsAsync();
            var billingStatuses = await _billingStatusService.GetAllBillingStatusesAsync();

            ViewBag.Contracts = contracts;
            ViewBag.BillingStatuses = billingStatuses;

            return View(request);
        }

        // GET: Billings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _billingService.GetBillingByIdAsync(id.Value);
            if (billing == null)
            {
                return NotFound();
            }

            var contracts = await _contractService.GetAllContractsAsync();
            var billingStatuses = await _billingStatusService.GetAllBillingStatusesAsync();

            ViewBag.Contracts = contracts;
            ViewBag.BillingStatuses = billingStatuses;

            var request = new UpdateBillingRequestDTO
            {
                Id = billing.Id,
                ContractId = billing.ContractId,
                Period = billing.Period,
                Amount = billing.Amount,
                DueDate = billing.DueDate,
                BillingStatusId = billing.BillingStatusId
            };

            return View(request);
        }

        // POST: Billings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateBillingRequestDTO request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _billingService.UpdateBillingAsync(request);
                return RedirectToAction(nameof(Index));
            }

            var contracts = await _contractService.GetAllContractsAsync();
            var billingStatuses = await _billingStatusService.GetAllBillingStatusesAsync();

            ViewBag.Contracts = contracts;
            ViewBag.BillingStatuses = billingStatuses;

            return View(request);
        }

        // GET: Billings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billing = await _billingService.GetBillingByIdAsync(id.Value);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }

        // POST: Billings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _billingService.DeleteBillingAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}