using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ibaycli
{
    class Program
    {
        public static string baseurl = "http://localhost:80";
        static HttpClient client = new HttpClient();

        static void Main()
        {
            do
            {
                Console.WriteLine("Bienvenue dans l'application iBay CLI !");
                Console.WriteLine("Saisissez une commande (login,logout, addtocart, removefromcart, emptycart, pay, listcart, getallproducts, searchproducts, getproductbyid, newproduct, updateproduct, deleteproduct, getuserbyid, newuser, updateuser, deleteuser, exit) :");
                string userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "login":
                        RunLoginAsync().GetAwaiter().GetResult();
                        break;
                    case "logout":
                        RunLogoutAsync().GetAwaiter().GetResult();
                        break;

                    case "addtocart":
                        RunAddToCartAsync().GetAwaiter().GetResult();
                        break;

                    case "removefromcart":
                        RunRemoveFromCartAsync().GetAwaiter().GetResult();
                        break;

                    case "emptycart":
                        RunEmptyCartAsync().GetAwaiter().GetResult();
                        break;

                    case "pay":
                        RunPayAsync().GetAwaiter().GetResult();
                        break;

                    case "listcart":
                        RunListCartAsync().GetAwaiter().GetResult();
                        break;

                    case "getallproducts":
                        RunGetAllProductsAsync().GetAwaiter().GetResult();
                        break;

                    case "searchproducts":
                        RunSearchProductsAsync().GetAwaiter().GetResult();
                        break;

                    case "getproductbyid":
                        RunGetProductByIdAsync().GetAwaiter().GetResult();
                        break;

                    case "newproduct":
                        RunNewProductAsync().GetAwaiter().GetResult();
                        break;

                    case "updateproduct":
                        RunUpdateProductAsync().GetAwaiter().GetResult();
                        break;

                    case "deleteproduct":
                        RunDeleteProductAsync().GetAwaiter().GetResult();
                        break;

                    case "getuserbyid":
                        RunGetUserByIdAsync().GetAwaiter().GetResult();
                        break;

                    case "newuser":
                        RunNewUserAsync().GetAwaiter().GetResult();
                        break;

                    case "updateuser":
                        RunUpdateUserAsync().GetAwaiter().GetResult();
                        break;

                    case "deleteuser":
                        RunDeleteUserAsync().GetAwaiter().GetResult();
                        break;

                    case "exit":
                        Console.WriteLine("Merci d'avoir utilisé iBay CLI. Au revoir !");
                        return;

                    default:
                        Console.WriteLine("Commande invalide. Veuillez saisir une commande valide.");
                        break;
                }

                Console.WriteLine("Appuyez sur une touche pour continuer ou tapez 'exit' pour quitter.");
            } while (Console.ReadLine().ToLower() != "exit");
        }
        

        static async Task<bool> RunLoginAsync()
        {
            Console.WriteLine("Saisissez votre pseudo :");
            string username = Console.ReadLine();
            Console.WriteLine("Saisissez votre mot de passe :");
            string password = Console.ReadLine();

            var loginModel = new { username , password };

            var response = await client.PostAsJsonAsync($"{baseurl}/api/auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Connexion réussie !");
                
                string accessToken = await response.Content.ReadAsStringAsync();
                
                File.WriteAllText("jeton.txt", accessToken);

                Console.WriteLine("Le jeton d'authentification a été enregistré dans le fichier jeton.txt.");

                return true;
            }
            else
            {
                Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
                return false;
            }
        }
        
        static async Task RunLogoutAsync()
        {
            Console.WriteLine("Déconnexion réussie !");
            File.Delete("jeton.txt");
        }
        

        static async Task RunAddToCartAsync()
        {
            Console.WriteLine("Saisissez l'id produit à ajouter au panier :");
            string productId = Console.ReadLine();
            
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);
            
            var addToCartModel = new { productId };
            var response = await client.PostAsJsonAsync($"{baseurl}/api/cart/add", addToCartModel);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Produit ajouté au panier !");
            }
            else
            {
                Console.WriteLine($"Échec de l'ajout au panier. Code d'état : {response.StatusCode}");
            }
        }

        static async Task RunRemoveFromCartAsync()
        {
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);

            Console.WriteLine("Saisissez l'id du produit à retirer du panier :");
            Guid id;
            if (Guid.TryParse(Console.ReadLine(), out id))
            {
                var response = await client.DeleteAsync($"{baseurl}/api/cart/remove?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Produit retiré du panier !");
                }
                else
                {
                    Console.WriteLine($"Échec du retrait du panier. Code d'état : {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("Format d'ID invalide. Veuillez saisir un GUID valide.");
            }
        }




        static async Task RunEmptyCartAsync()
        {
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);
            
            var response = await client.DeleteAsync($"{baseurl}/api/cart/empty");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Panier vidé !");
            }
            else
            {
                Console.WriteLine($"Échec du vidage du panier. Code d'état : {response.StatusCode}");
            }
        }

        static async Task RunPayAsync()
        {
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);
            
            var response = await client.DeleteAsync($"{baseurl}/api/cart/empty");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Paiement accepté !");
            }
            else
            {
                Console.WriteLine($"Paiement rejeté. Code d'état : {response.StatusCode}");
            }
        }

        static async Task RunListCartAsync()
        {
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);
            
            var response = await client.GetAsync($"{baseurl}/api/cart/list");
            if (response.IsSuccessStatusCode)
            {
                var cartContent = await response.Content.ReadAsStringAsync();
                if (cartContent == "[]")
                {
                    Console.WriteLine("Le panier est vide.");
                }
                else
                {
                    Console.WriteLine(cartContent);
                }
                
            }
            else
            {
                Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
            }
        }

        static async Task RunGetAllProductsAsync()
        {
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);
            Console.WriteLine("Saisissez la limite de produits à afficher :");
            int limit = int.Parse(Console.ReadLine());
            Console.WriteLine("Saisissez le tri des produits (Date,Name,Price) :");
            string sort = Console.ReadLine();
            
            var response = await client.GetAsync($"{baseurl}/api/product/getall?limit={limit}&sortby={sort}");
            if (response.IsSuccessStatusCode)
            {
                var productsContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(productsContent);
            }
            else
            {
                Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
            }
        }

        static async Task RunSearchProductsAsync()
        {
           
            Console.WriteLine("Saisissez le nom du produit à rechercher :");
            string name = Console.ReadLine();
            var response = await client.GetAsync($"{baseurl}/api/product/search?name={name}");
            if (response.IsSuccessStatusCode)
            {
                var productsContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(productsContent);
            }
            else
            {
                Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
            }
        }

        static async Task RunGetProductByIdAsync()
        {
            Console.WriteLine("Saisissez l'ID du produit à afficher :");
            Guid id;
            if (Guid.TryParse(Console.ReadLine(), out id))
            {
                var response = await client.GetAsync($"{baseurl}/api/product/getby?Id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var productContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(productContent);
                }
                else
                {
                    Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("Format d'ID invalide. Veuillez saisir un GUID valide.");
            }
        }


        static async Task RunNewProductAsync()
        {
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);

            Console.WriteLine("Saisissez le nom du produit :");
            string name = Console.ReadLine();
            Console.WriteLine("Saisissez l'URL de l'image du produit :");
            string image = Console.ReadLine();
            Console.WriteLine("Saisissez le prix du produit :");
            int price = int.Parse(Console.ReadLine());
            Console.WriteLine("Le produit est-il disponible ? (true/false) :");
            bool available = bool.Parse(Console.ReadLine());
            
            var newProductModel = new { name, image, price, available };
            var response = await client.PostAsJsonAsync($"{baseurl}/api/product/new", newProductModel);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Création réussie !");
            }
            else
            {
                Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
            }
        }

        static async Task RunUpdateProductAsync()
        {
            
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);
            Console.WriteLine("Saisissez l'ID du produit à mettre à jour :");
            Guid id;
            if (Guid.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Saisissez le nouveau nom du produit :");
                string name = Console.ReadLine();
                Console.WriteLine("Saisissez la nouvelle URL de l'image du produit :");
                string image = Console.ReadLine();
                Console.WriteLine("Saisissez le nouveau prix du produit :");
                int price = int.Parse(Console.ReadLine());
                Console.WriteLine("Le produit est-il disponible ? (true/false) :");
                bool available = bool.Parse(Console.ReadLine());
                
                var updateProductModel = new { id, name, image, price, available };
                var response = await client.PutAsJsonAsync($"{baseurl}/api/product/update", updateProductModel);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Mise à jour réussie !");
                }
                else
                {
                    Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("Format d'ID invalide. Veuillez saisir un GUID valide.");
            }
        }

        static async Task RunDeleteProductAsync()
        {
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);
            Console.WriteLine("Saisissez l'ID du produit à supprimer :");
            Guid id;
            if (Guid.TryParse(Console.ReadLine(), out id))
            {
                var response = await client.DeleteAsync($"{baseurl}/api/product/delete?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Suppression réussie !");
                }
                else
                {
                    Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("Format d'ID invalide. Veuillez saisir un GUID valide.");
            }
        }

        static async Task RunGetUserByIdAsync()
        {
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);
            Console.WriteLine("Saisissez l'ID de l'utilisateur à afficher :");
            Guid id;
            if (Guid.TryParse(Console.ReadLine(), out id))
            {
                var response = await client.GetAsync($"{baseurl}/api/user/getby?Id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var userContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(userContent);
                }
                else
                {
                    Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("Format d'ID invalide. Veuillez saisir un GUID valide.");
            }
        }

        static async Task<bool> RunNewUserAsync()
        {
            Console.WriteLine("Saisissez votre pseudo :");
            string username = Console.ReadLine();
            Console.WriteLine("Saisissez votre email :");
            string email = Console.ReadLine();
            Console.WriteLine("Saisissez votre mot de passe :");
            string password = Console.ReadLine();
            
            var newUserModel = new { username,email, password };
            var response = await client.PostAsJsonAsync($"{baseurl}/api/user/new", newUserModel);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Création réussie !");
                return true;
            }
            else
            {
                Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
                return false;
            }
            
           
        }

        static async Task RunUpdateUserAsync()
        {
            
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);
            Console.WriteLine("Saisissez l'ID de l'utilisateur à mettre à jour :");
            Guid id;
            if (Guid.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Saisissez le nouveau pseudo de l'utilisateur :");
                string username = Console.ReadLine();
                Console.WriteLine("Saisissez le nouvel email de l'utilisateur :");
                string email = Console.ReadLine();
                Console.WriteLine("Saisissez le nouveau mot de passe de l'utilisateur :");
                string password = Console.ReadLine();
                
                var updateUserModel = new { id, username, email, password };
                var response = await client.PutAsJsonAsync($"{baseurl}/api/user/update", updateUserModel);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Mise à jour réussie !");
                }
                else
                {
                    Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("Format d'ID invalide. Veuillez saisir un GUID valide.");
            }
        }

        static async Task RunDeleteUserAsync()
        {
            
            string jwtContent = File.ReadAllText("jeton.txt");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtContent);
            Console.WriteLine("Saisissez l'ID de l'utilisateur à supprimer :");
            Guid id;
            if (Guid.TryParse(Console.ReadLine(), out id))
            {
                var response = await client.DeleteAsync($"{baseurl}/api/user/delete?Id={id}");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Suppression réussie !");
                }
                else
                {
                    Console.WriteLine($"Échec de la connexion. Code d'état : {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("Format d'ID invalide. Veuillez saisir un GUID valide.");
            }
        }
        
    }
}
