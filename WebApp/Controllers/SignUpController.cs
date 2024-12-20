﻿using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using WebApp.ViewModels;
namespace WebApp.Controllers;


public class SignUpController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ProductService productService) : Controller
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IConfiguration _configuration = configuration;
    private readonly ProductService _productService = productService;

    public bool IsValidEmail(string email)
    {
        var emailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]{2,}$");
        return emailRegex.IsMatch(email);
    }

    [HttpGet]
    [Route("/signup")]
    public async Task<IActionResult> SignUp()
    {
        var categories = await _productService.GetAllCategoriesAsync();

        // Pass the list of categories to ViewData
        ViewData["Categories"] = categories;

        return View();
    }


    [HttpPost]
    [Route("/signup")]
    public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
    {
        if ((string.IsNullOrWhiteSpace(viewModel.FirstName) || viewModel.FirstName.Length < 2) ||
        (string.IsNullOrWhiteSpace(viewModel.LastName) || viewModel.LastName.Length < 2))
        {
            TempData["MessageType"] = "error";
            TempData["Message"] = "First and last name must be at least 2 characters long and cannot be empty.";
            return View(viewModel);
        }

        if (string.IsNullOrWhiteSpace(viewModel.Email))
        {
            TempData["MessageType"] = "error";
            TempData["Message"] = "Email address cannot be empty.";
            return View(viewModel);
        }

        if (!IsValidEmail(viewModel.Email))
        {
            TempData["MessageType"] = "error";
            TempData["Message"] = "Invalid email address format.";
            return View(viewModel);
        }

        if (!viewModel.TermsAndConditions)
        {
            TempData["MessageType"] = "error";
            TempData["Message"] = "You must accept the terms and conditions to proceed.";
            return View(viewModel);
        }

        var securityKey = _configuration!.GetValue<string>("WebAppKey");
        var apiKey = _configuration!.GetValue<string>("ApiKey");
        var dto = new SignUpDto
        {
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            Email = viewModel.Email,
            Password = viewModel.Password,
            SecurityKey = securityKey
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var client = _httpClientFactory.CreateClient();

        try
        {
            var response = await client.PostAsync($"https://userprovider-rika-win23.azurewebsites.net/api/SignUp/?key={apiKey}", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                TempData["MessageType"] = "error";

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        TempData["Message"] = "An account with this email address already exists.";
                        break;

                    case HttpStatusCode.BadRequest:
                        TempData["Message"] = "Invalid data provided. Please check your information and try again.";
                        break;

                    case HttpStatusCode.InternalServerError:
                        TempData["Message"] = "A server error occurred. Please try again later.";
                        break;

                    default:
                        TempData["Message"] = "An error occurred while creating your account. Please try again.";
                        break;
                }

                return View(viewModel);
            }

            TempData["Message"] = "Account created successfully. Please sign in.";
            TempData["MessageType"] = "success";
            return RedirectToAction("SignIn", "SignIn");
        }

        catch (HttpRequestException)
        {
            TempData["MessageType"] = "error";
            TempData["Message"] = "There was a problem connecting to the server. Please try again later.";
            return View(viewModel);
        }
    }
}


