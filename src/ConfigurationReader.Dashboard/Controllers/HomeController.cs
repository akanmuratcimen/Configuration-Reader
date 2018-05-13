using System;
using System.Globalization;
using System.Threading.Tasks;
using ConfigurationReader.Abstraction;
using ConfigurationReader.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ConfigurationReader.Dashboard.Controllers {
    public class HomeController : Controller {
        private readonly IStorageProvider<ObjectId> _storageProvider;

        public HomeController(IStorageProvider<ObjectId> storageProvider) {
            _storageProvider = storageProvider;
        }

        public async Task<IActionResult> Index() {
            var viewModel = new IndexViewModel();

            viewModel.Configurations = await _storageProvider.Configurations();

            return View(viewModel);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateViewModel viewModel) {
            if (!ModelState.IsValid) {
                return View("Create");
            }

            var configurationModel = new ConfigurationModel();

            configurationModel.ApplicationName = viewModel.ApplicationName;
            configurationModel.Name = viewModel.Name;
            configurationModel.Value = viewModel.Value;
            configurationModel.IsActive = viewModel.IsActive;

            await _storageProvider.Add(configurationModel);

            return Content("Configuration added.");
        }

        public async Task<IActionResult> Edit([FromRoute] string id) {
            if (!ObjectId.TryParse(id, out var objectId)) {
                return BadRequest("Invalid id.");
            }

            if (!ModelState.IsValid) {
                return View("Edit");
            }

            var configuration = await _storageProvider.Get(objectId);

            if (configuration == null) {
                return NotFound();
            }

            var viewModel = new EditViewModel();

            viewModel.ApplicationName = configuration.ApplicationName;
            viewModel.Name = configuration.Name;

            var value = Convert.ToString(configuration.Value, CultureInfo.InvariantCulture);

            viewModel.Value = value;
            viewModel.IsActive = configuration.IsActive;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EditViewModel viewModel) {
            if (!ModelState.IsValid) {
                return View("Edit");
            }

            var configurationModel = new ConfigurationModel();

            configurationModel.ApplicationName = viewModel.ApplicationName;
            configurationModel.Name = viewModel.Name;
            configurationModel.Value = viewModel.Value;
            configurationModel.IsActive = viewModel.IsActive;

            var updateResult = await _storageProvider
                .Update(ObjectId.Parse(viewModel.Id), configurationModel);

            if (!updateResult) {
                return Content("Configuration colud not updated!");
            }

            return Content("Configuration updated.");
        }
    }
}