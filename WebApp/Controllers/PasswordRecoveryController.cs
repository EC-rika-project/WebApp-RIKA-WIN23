using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Factories;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class PasswordRecoveryController(IResetPasswordService resetService, IEmailService emailService) : Controller
{
    private readonly IResetPasswordService _resetService = resetService;
    private readonly IEmailService _emailService = emailService;

    [Route("/forgotpassword")]
    public IActionResult Forgotpassword()
    {
        return View();
    }

    [HttpPost]
    [Route("/forgotpassword")]
    public async Task<IActionResult> Forgotpassword(ForgotPasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        try
        {
            var resetPasswordDto = ForgotPasswordFactory.CreateForgotPasswordDto(viewModel);
            var resetToken = await _resetService.InitiatePasswordResetAsync(resetPasswordDto);

            if (resetToken != null)
            {
                var resetLink = Url.Action("ResetPassword", "PasswordRecovery",
                                new { token = resetToken.JWT },
                                protocol: Request.Scheme);

                await _emailService.SendPasswordResetEmailAsync(viewModel.Email, resetLink!);
                TempData["Message"] = "Reset instructions have been sent to your email.";
                TempData["MessageType"] = "success";
                return RedirectToAction("SignIn", "SignIn");
            }

            TempData["Message"] = "Unable to process your request. Please try again.";
            TempData["MessageType"] = "error";
        }
        catch (EmailServiceException ex)
        {
            TempData["Message"] = "Failed to send reset email. Please try again.";
            TempData["MessageType"] = "error";
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            TempData["Message"] = "An error occurred while processing your request.";
            TempData["MessageType"] = "error";
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(viewModel);
    }

    [HttpGet]
    [Route("/resetpassword")]
    public IActionResult ResetPassword(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            TempData["Message"] = "Invalid password reset token.";
            TempData["MessageType"] = "error";
            return RedirectToAction("SignIn", "SignIn");
        }

        var viewModel = new ResetPasswordViewModel { Token = token };
        return View(viewModel);
    }

    [HttpPost]
    [Route("/resetpassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        try
        {
            var resetPasswordDto = ResetPasswordFactory.CreateResetPasswordDto(viewModel);
            var result = await _resetService.ValidateAndResetPasswordAsync(resetPasswordDto);

            if (result)
            {
                TempData["Message"] = "Your password has been reset successfully.";
                TempData["MessageType"] = "success";
                return RedirectToAction("SignIn", "SignIn");
            }

            TempData["Message"] = "Failed to reset password. Please try again.";
            TempData["MessageType"] = "error";
        }
        catch (Exception ex)
        {
            TempData["Message"] = "An error occurred while resetting your password.";
            TempData["MessageType"] = "error";
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(viewModel);
    }
}
