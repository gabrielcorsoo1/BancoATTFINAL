using AtlasAir.Interfaces;
using AtlasAir.Models;
using AtlasAir.Services;
using AtlasAir.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AtlasAir.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public AccountController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var all = await _customerRepository.GetAllAsync() ?? new List<Customer>();

            if (all.Any(c => !string.IsNullOrEmpty(c.Phone) && c.Phone == vm.Phone) ||
                (!string.IsNullOrEmpty(vm.Email) && all.Any(c => !string.IsNullOrEmpty(c.Email) && c.Email == vm.Email)))
            {
                ModelState.AddModelError(string.Empty, "Já existe usuário com esse telefone/email.");
                return View(vm);
            }

            var customer = new Customer
            {
                Name = vm.Name,
                Phone = vm.Phone,
                Email = vm.Email ?? string.Empty,
                PasswordHash = PasswordHasher.Hash(vm.Password),
                IsAdmin = vm.IsAdmin // agora usamos o valor do viewmodel
            };

            await _customerRepository.CreateAsync(customer);

            TempData["SuccessMessage"] = "Registro efetuado com sucesso. Faça login para continuar.";
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (TempData["SuccessMessage"] != null)
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(vm);

            var customers = await _customerRepository.GetAllAsync() ?? new List<Customer>();
            var user = customers.FirstOrDefault(c =>
                (!string.IsNullOrEmpty(c.Email) && c.Email.Equals(vm.UserIdentifier, StringComparison.OrdinalIgnoreCase))
                || (!string.IsNullOrEmpty(c.Phone) && c.Phone == vm.UserIdentifier));

            if (user == null || !PasswordHasher.Verify(vm.Password, user.PasswordHash ?? string.Empty))
            {
                ModelState.AddModelError(string.Empty, "Credenciais inválidas.");
                return View(vm);
            }

            HttpContext.Session.SetInt32("CustomerId", user.Id);
            HttpContext.Session.SetString("CustomerName", user.Name);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin ? "1" : "0");

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            if (user.IsAdmin)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("DashboardTest", "Client");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            // redirecionar para a tela de login separada
            return RedirectToAction("Login", "Account");
        }
    }
}