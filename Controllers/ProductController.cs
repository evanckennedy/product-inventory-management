using Microsoft.AspNetCore.Mvc;
// This using directive imports the namespace containing the Product model.
// It is crucial for enabling the ProductController to recognize and use the Product class,
// facilitating operations like creating, reading, updating, and deleting product entries.
using product_inventory_management.Models;


namespace product_inventory_management.Controllers
{
    public class ProductController : Controller
    {
        private static List<Product> _products = new List<Product>();
        private static List<Inventory> _inventories = new List<Inventory>();

        // When you navigate to the index page associated with this controller, the return View() statement is executed.
        public IActionResult Index()
        {
            // This line of code returns a view called "Index" and passes the _products list as the model.
            // The view will render the list of products in the HTML markup and display it to the user.
            return View(_products);
        }

        // the [HttpGet] attribute is used to explicitly specify that the Create action method should be invoked when an HTTP GET request is made to the corresponding URL. This means that when a user navigates to the URL associated with the Create action method, the server will execute the Create method and return the corresponding view.
        [HttpGet]
        public IActionResult Create()
        {
            // Displays the form for creating a new product.
            // Invoked with an HTTP GET request when the user navigates to the form URL.
            return View();
        }

        [HttpPost]
        // Processes the submitted form data for creating a new product.
        // Invoked with an HTTP POST request when the form is submitted.
        // Adds the new product to the list and redirects to the product list view.
        public IActionResult Create(Product product)
        {
            // Check if the submitted model passes all validation rules
            if (!ModelState.IsValid)
            {
                // If not, return the same view with the current product model to display validation errors
                return View(product);
            }

            // Makes each subsequent product created have an ID that is 1 greater than previous
            int maxId = _products.Count > 0 ? _products.Max(p => p.Id) : 0;
            product.Id = maxId + 1;
            _products.Add(product);

            // Create an initial inventory record for the new product
            int maxInventoryId = _inventories.Count > 0 ? _inventories.Max(i => i.Id) : 0;
            var inventory = new Inventory
            {
                Id = maxInventoryId + 1,
                ProductId = product.Id,
                StockQuantity = 0 
            };
            _inventories.Add(inventory);


            // After product is created, redirect to the display of products
            return RedirectToAction("Index");
        }

        // By default, if an action method does not have an HTTP verb attribute specified, it is treated as an HTTP GET method.
        public IActionResult Delete(int id)
        {
            // FirstOrDefault returns the first product matching the ID, otherwise returns null
            Product product = _products.FirstOrDefault(p => p.Id == id);
            // If the product doesn't exist, return a NotFound result
            if (product == null) return NotFound();
            // If the product exists, pass it to the view for display
            // This passes the found product to the view, which will display the confirmation page.
            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            Product product = _products.FirstOrDefault(p => p.Id == id);
            // If the product exists, remove it from the list
            if (product != null) 
            {
                _products.Remove(product);
                // Remove the corresponding inventory record
                var inventory = _inventories.FirstOrDefault(i => i.ProductId == id);
                if (inventory != null)
                {
                    _inventories.Remove(inventory);
                }
            }
            // Redirect to the Index action to show the updated list of products
            return RedirectToAction("Index");
        }

        public IActionResult ProductInventoryDetails(int id)
        {
            // Find the first product in the _products list that matches the given id
            var product = _products.FirstOrDefault(p => p.Id == id);
            // Find the first inventory in the _inventories list that matches the given product id
            var inventory = _inventories.FirstOrDefault(i => i.ProductId == id);
            // If either the product or inventory is null, return a NotFound result
            if (product == null || inventory == null) return NotFound();

            // Create a new instance of the ProductInventoryViewModel class
            var viewModel = new ProductInventoryViewModel
            {
                Product = product, // Set the Product property of the viewModel to the found product
                Inventory = inventory // Set the Inventory property of the viewModel to the found inventory
            };

            return View(viewModel); // Return the viewModel to the view for display
        }

    }
}
