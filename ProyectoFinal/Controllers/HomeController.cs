using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProyectoFinal.Models;
using ProyectoFinal.Rules;
using System.Diagnostics;

namespace ProyectoFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Post(int id)
        {
            var rule = new PublicacionRule(_configuration);
            var post = rule.GetPostById(id);
            if (post==null)
            {
                return NotFound();
            }
            return View(post);
        }

        public IActionResult Index()
        {
            var rule = new PublicacionRule(_configuration);
            var posts = rule.GetPostToHome();
            return View(posts);
        }

        public IActionResult Publicaciones(int cant = 5, int pagina = 0)
        {
            var rule = new PublicacionRule(_configuration);
            var posts = rule.GetPublicaciones(cant, pagina);

            return View(posts);
        }

        public IActionResult Suerte()
        {
            //le paso la config al constructor de PublicacionRule
            var rule = new PublicacionRule(_configuration);
            var post = rule.GetOnePostRandom();
            return View(post);
        }

        public IActionResult AcercaDe()
        {
            return View();
        }

        public IActionResult Contacto()
        {
            return View();
        }

        [Authorize]
        public IActionResult Nuevo()
        {
            return View();
        }

        public IActionResult Add(Publicacion data)
        {
            var rule= new PublicacionRule(_configuration);
            rule.InserPost(data);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Contacto(Contacto contacto)
        { 
            if (!ModelState.IsValid)
            {
                return View("Contacto", contacto);   
            }
            var rule = new ContactoRule(_configuration);
            var mensaje =@"<h1>Gracias por contactarnos</h1> 
                            <p>nos comunicaremos pronto</p>";
            //enviar respuesta
            rule.SendEmail(contacto.Email, mensaje, "Mensaje recibido", "polo mvc", "correo que se vera en la cabecera del correo recibido");
            //recibir copia del correo con datos del contacto que envio el correo
            rule.SendEmail("micorreo", contacto.Mensaje, "Nuevo contacto", contacto.Nombre, contacto.Email);
            return View("Contacto");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}