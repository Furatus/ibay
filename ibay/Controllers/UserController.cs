using System.Security.Claims;
using ibay.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ibay.Filters;
using ibay.Services;
using Microsoft.AspNetCore.Authorization;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Swashbuckle.AspNetCore.Annotations;


namespace ibay.Controllers
{
    [Route("/api/user/")]
    [SwaggerResponse(400, "Mauvaise requête", null)]
    [SwaggerResponse(405, "Méthode non autorisée", null)]
    [SwaggerResponse(500, "Erreur Interne", null)]
    [ApiController]
    [ExecutedReqFilter]

    public class UserController : ControllerBase
    {
        [HttpGet]
        [SwaggerResponse(404, "Utilisateur non trouvé", null)]
        [SwaggerResponse(200, "ok", typeof(User))]
        [Route("getby")]
        public IActionResult Get(IIbay ibay, [FromQuery] ItemId itemId)
        {
            var id = itemId.Id;
            var User = ibay.GetUserById(id);
            if (User == null)
            {
                return NotFound($"User id : {id} not found");
            }

            return Ok(User);
        }

        [HttpPost]
        [SwaggerResponse(200, "ok", typeof(Guid))]
        [Route("new")]
        public IActionResult Post(IIbay ibay, [FromBody] User User)
        {
            try
            {
                var id = ibay.CreateUser(User);

                return Created($"/api/stud/{id}", id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        [SwaggerResponse(404, "Utilisateur non trouvé", null)]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(200, "ok", null)]
        [Authorize]
        public IActionResult Put(IIbay ibay, [FromBody] User user)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != user.Id.ToString()) return Unauthorized("You are not allowed to modify other users");
            ibay.UpdateUser(user.Id, user);
            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(404, "Utilisateur non trouvé", null)]
        [SwaggerResponse(200, "ok", null)]
        [Authorize]
        public IActionResult Delete(IIbay ibay, [FromBody] ItemId itemId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != itemId.Id.ToString()) return Unauthorized("You are not allowed to delete other users");
            var id = itemId.Id;
            ibay.DeleteUser(id);
            return Ok();
        }



    }

    [Route("/api/product/")]
    [SwaggerResponse(400, "Mauvaise requête", null)]
    [SwaggerResponse(405, "Méthode non autorisée", null)]
    [SwaggerResponse(500, "Erreur Interne", null)]
    [ApiController]
    [ExecutedReqFilter]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(400)]
        [SwaggerResponse(200, "ok", typeof(Product[]))]
        [Route("getall")]
        [EnableCors("MyCustomPolicy")]
        public IActionResult Get(IIbay ibay, [FromQuery] ProductSorting sorting)
        {
            return Ok(ibay.GetProducts(sorting));
        }

        [HttpGet]
        [SwaggerResponse(200, "ok", typeof(Product[]))]
        [Route("search")]
        public IActionResult Get(IIbay ibay, [FromQuery] ProductSearch search)
        {
            return Ok(ibay.SearchProducts(search));
        }

        [HttpGet]
        [SwaggerResponse(404, "Produit non trouvé", null)]
        [SwaggerResponse(200, "ok", typeof(Product))]
        [Route("getby")]
        public IActionResult Get(IIbay ibay, [FromQuery] ItemId itemId)
        {
            var id = itemId.Id;
            var Product = ibay.GetProductById(id);
            if (Product == null)
            {
                return NotFound($"Product Id : {id} not found");
            }

            return Ok(Product);
        }

        [HttpPost]
        [Route("new")]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(200, "ok", typeof(Guid))]
        [Authorize]
        public IActionResult Post(IIbay ibay, [FromBody] Product product)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "seller" && userRole != "admin") return Unauthorized("You are not allowed to Modify products");
            try
            {
                product.SellerId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var id = ibay.CreateProduct(product);

                return Created($"/api/stud/{id}", id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        [SwaggerResponse(404, "Produit non trouvé.", null)]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(200, "ok", null)]
        [Authorize]
        public IActionResult Put(IIbay ibay, [FromBody] Product product)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "seller" && userRole != "admin") return Unauthorized("You are not allowed to Modify products");
            
            var checkProduct = ibay.GetProductById(product.Id);
            if (checkProduct == null)
            {
                return NotFound($"Product Id : {product.Id} not found");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != checkProduct.SellerId.ToString())
                return Unauthorized("You are only allowed to Modify your products");
            
            ibay.UpdateProduct(product.Id, product);
            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        [SwaggerResponse(404, "Produit non trouvé.", null)]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(200, "ok", null)]
        [Authorize]
        public IActionResult Delete(IIbay ibay, [FromBody] ItemId itemId)
        {
            var id = itemId.Id;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "seller" && userRole != "admin") return Unauthorized("You are not allowed to Modify products");
            
            var checkProduct = ibay.GetProductById(id);
            if (checkProduct == null)
            {
                return NotFound($"Product Id : {id} not found");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != checkProduct.SellerId.ToString())
                return Unauthorized("You are only allowed to Modify your products");
            
            ibay.DeleteProduct(id);
            return Ok();
        }



    }

    [Route("/api/cart/")]
    [SwaggerResponse(400, "Mauvaise requête", null)]
    [SwaggerResponse(405, "Méthode non autorisée", null)]
    [SwaggerResponse(500, "Erreur Interne", null)]
    [ApiController]
    [ExecutedReqFilter]

    public class CartController : ControllerBase
    {
        [HttpPost]
        [SwaggerResponse(404, "Produit non trouvé.", null)]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(200, "ok", null)]
        [Authorize]
        [Route("add")]

        public IActionResult Add(IIbay ibay, [FromBody] Cart cart)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            cart.UserId = new Guid(userId);
            ibay.AddToCart(cart);
            return Ok();
        }
        
        [HttpDelete]
        [SwaggerResponse(404, "Produit non trouvé.", null)]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(200, "ok", null)]
        [Authorize]
        [Route("remove")]

        public IActionResult remove(IIbay ibay, [FromBody] ItemId cart)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ibay.RemoveFromCart(userId,cart.Id);
            return Ok();
        }
        
        [HttpDelete]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(200, "ok", null)]
        [Authorize]
        [Route("empty")]
        public IActionResult EmptyCart(IIbay ibay)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ibay.EmptyCart(userId);
            return Ok();
        }
        
        [HttpPost]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(200, "ok", null)]
        [Authorize]
        [Route("pay")]
        public IActionResult PayCart(IIbay ibay)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ibay.PayCart(userId);
            return Ok();
        }
        
        [HttpGet]
        [Authorize]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(200, "ok", null)]
        [Route("list")]
        public IActionResult GetCartItems(IIbay ibay)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cartItems = ibay.GetCartItems(userId);
            return Ok(cartItems);
        }
    }

    [Route("/api/auth/")]
    [ApiController]
    [ExecutedReqFilter]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(200, "ok", typeof(string))]
        [Route("login")]
        public IActionResult Login(IJwtAuthService jwtAuthService, [FromBody] LoginModel login)
        {
            var user = jwtAuthService.Authenticate(login.Username, login.Password);
            if (user == null) return Unauthorized("User not found, wrong username/password");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = jwtAuthService.GenerateToken(
                Env.JwtSecret, claims);

            return Ok(token);
        }
    }
    
    [Route("/api/upgrade/")]
    [ApiController]
    [ExecutedReqFilter]
    public class UpgradeController : ControllerBase
    {
        [HttpPost]
        [SwaggerResponse(401, "Non Autorisé.", null)]
        [SwaggerResponse(200, "ok", typeof(string))]
        [Authorize]
        [Route("seller")]
        public IActionResult UpgradeToSeller (IIbay ibay, [FromBody] ItemId id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != id.Id.ToString()) return Unauthorized("You are not allowed to modify other users");
            var user = ibay.GetUserById(id.Id);
            user.Role = "seller";
            ibay.UpdateUserToSeller(id.Id, user);

            return Ok(userId + " updated to seller");
        }
    }
}
    