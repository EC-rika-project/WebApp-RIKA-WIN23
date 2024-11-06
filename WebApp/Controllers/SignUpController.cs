﻿using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using WebApp.ViewModels;
namespace WebApp.Controllers;


public class SignUpController(IHttpClientFactory httpClientFactory, IConfiguration configuration) : Controller
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IConfiguration _configuration = configuration;

    [HttpGet]
    [Route("/signup")]
    public IActionResult SignUp()
    {
        return View();
    }


    [HttpPost]
    [Route("/signup")]
    public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var securityKey = _configuration!.GetSection("SecurityKeys")["WebAppKey"];
        var apiKey = _configuration!.GetSection("ApiKey")["Secret"];

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

