using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Livora_Lite.Application.DTO;
using Livora_Lite.Application.Interface;

namespace Livora_Lite.Presentation.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IBillingService _billingService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IAuditService _auditService;

        public PaymentsController(
            IPaymentService paymentService,
            IBillingService billingService,
            IPaymentMethodService paymentMethodService,
            IAuditService auditService)
        {
            _paymentService = paymentService;
            _billingService = billingService;
            _paymentMethodService = paymentMethodService;
            _auditService = auditService;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return View(payments);
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _paymentService.GetPaymentByIdAsync(id.Value);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public async Task<IActionResult> Create()
        {
            var billings = await _billingService.GetAllBillingsAsync();
            var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();

            ViewBag.Billings = billings;
            ViewBag.PaymentMethods = paymentMethods;

            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePaymentRequestDTO request)
        {
            if (ModelState.IsValid)
            {
                var paymentId = await _paymentService.CreatePaymentAsync(request);
                
                // Log audit
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Create",
                    "Payment",
                    paymentId.ToString() ?? "Unknown",
                    $"Criado pagamento de R$ {request.Amount} para cobrança ID: {request.BillingId}"
                );
                
                return RedirectToAction(nameof(Index));
            }

            var billings = await _billingService.GetAllBillingsAsync();
            var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();

            ViewBag.Billings = billings;
            ViewBag.PaymentMethods = paymentMethods;

            return View(request);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _paymentService.GetPaymentByIdAsync(id.Value);
            if (payment == null)
            {
                return NotFound();
            }

            var billings = await _billingService.GetAllBillingsAsync();
            var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();

            ViewBag.Billings = billings;
            ViewBag.PaymentMethods = paymentMethods;

            var request = new UpdatePaymentRequestDTO
            {
                Id = payment.Id,
                BillingId = payment.BillingId,
                PaymentDate = payment.PaymentDate,
                AmountPaid = payment.AmountPaid,
                PaymentMethodId = payment.PaymentMethodId
            };

            return View(request);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdatePaymentRequestDTO request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _paymentService.UpdatePaymentAsync(request);
                
                // Log audit
                var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
                var userName = User.Identity?.Name ?? "Unknown";
                await _auditService.LogActionAsync(
                    userId,
                    userName,
                    "Update",
                    "Payment",
                    request.Id.ToString(),
                    $"Atualizado pagamento ID: {request.Id}, valor: R$ {request.AmountPaid}"
                );
                
                return RedirectToAction(nameof(Index));
            }

            var billings = await _billingService.GetAllBillingsAsync();
            var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();

            ViewBag.Billings = billings;
            ViewBag.PaymentMethods = paymentMethods;

            return View(request);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _paymentService.GetPaymentByIdAsync(id.Value);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            var paymentInfo = payment != null ? $"Pagamento ID: {payment.Id}, valor: R$ {payment.AmountPaid}" : $"Pagamento ID: {id}";
            
            await _paymentService.DeletePaymentAsync(id);
            
            // Log audit
            var userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? "Unknown";
            var userName = User.Identity?.Name ?? "Unknown";
            await _auditService.LogActionAsync(
                userId,
                userName,
                "Delete",
                "Payment",
                id.ToString(),
                $"Deletado {paymentInfo}"
            );
            
            return RedirectToAction(nameof(Index));
        }
    }
}