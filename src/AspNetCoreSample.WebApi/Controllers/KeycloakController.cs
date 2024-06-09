using AspNetCoreSample.WebApi.Models;
using AspNetCoreSample.WebApi.Models.Keycloak;
using AspNetCoreSample.WebApi.Services.Keycloak.Admin;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSample.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KeycloakController : ControllerBase
{
    private readonly ILogger<KeycloakController> _logger;
    private readonly IKeycloakService _keyclaokService;
    private readonly IValidator<CreateUserInput> _createUserInputValidator;
    private readonly IValidator<UpdateUserInput> _updateUserInputValidator;
    private readonly IValidator<ChangePasswordInput> _changePasswordInputValidator;
    private readonly IValidator<ResetPasswordByEmailInput> _resetPasswordByEmailInputValidator;
    private readonly IValidator<DeleteUserInput> _deleteUserInputValidator;

    public KeycloakController(ILogger<KeycloakController> logger,
        IKeycloakService keycloakService,
        IValidator<CreateUserInput> createUserInputValidator,
        IValidator<UpdateUserInput> updateUserInputValidator,
        IValidator<ChangePasswordInput> changePasswordInputValidator,
        IValidator<ResetPasswordByEmailInput> resetPasswordByEmailInputValidator,
        IValidator<DeleteUserInput> deleteUserInputValidator)
    {
        _logger = logger;
        _keyclaokService = keycloakService;
        _createUserInputValidator = createUserInputValidator;
        _updateUserInputValidator = updateUserInputValidator;
        _changePasswordInputValidator = changePasswordInputValidator;
        _resetPasswordByEmailInputValidator = resetPasswordByEmailInputValidator;
        _deleteUserInputValidator = deleteUserInputValidator;
    }

    private async ValueTask<IActionResult> CommonValidationResponse<T>(T input, IValidator<T> validator, Func<ValueTask<IActionResult>> func)
    {
        try
        {
            var result = await validator.ValidateAsync(input);
            if (!result.IsValid)
            {
                var errors = new WebApiFailResponse(result);
                return BadRequest(errors);
            }

            return await func();
        }
        catch (Exception ex)
        {
            return BadRequest(new WebApiFailResponse(ex));
        }
    }

    /// <summary>
    /// ユーザーを作成する
    /// </summary>
    /// <param name="input">ユーザー情報</param>
    [HttpPost("CreateUser")]
    public async ValueTask<IActionResult> CreateUser(CreateUserInput input)
    {
        return await CommonValidationResponse(input, _createUserInputValidator, async () =>
        {
            var request = new CreateUserRequest()
            {
                Username = input.Username,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                Enabled = true,
                Credentials = new List<Credential> { new(input.Password) }
            };
            var response = await _keyclaokService.CreateUserAsync(request);
            return Ok(response);
        });
    }

    /// <summary>
    /// ユーザー情報を更新する
    /// </summary>
    /// <param name="input">ユーザー情報</param>
    [HttpPut("UpdateUser")]
    public async ValueTask<IActionResult> UpdateUser(UpdateUserInput input)
    {
        return await CommonValidationResponse(input, _updateUserInputValidator, async () =>
        {
            var request = new UpdateUserRequest()
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email
            };
            if (!string.IsNullOrWhiteSpace(input.Password))
            {
                request.Credentials = new List<Credential>() { new Credential(input.Password) };
            }
            await _keyclaokService.UpdateUserAsync(input.UserId, request);
            return Ok();
        });
    }

    /// <summary>
    /// ユーザーのパスワードを変更する
    /// </summary>
    /// <param name="input">パスワード情報</param>
    [HttpPut("ChangePassword")]
    public async ValueTask<IActionResult> ChangePassword(ChangePasswordInput input)
    {
        return await CommonValidationResponse(input, _changePasswordInputValidator, async () =>
        {
            var request = new ChangePasswordRequest()
            {
                UserId = input.UserId,
                Credential = new Credential(input.Password)
            };
            await _keyclaokService.ChangePasswordAsync(request);
            return Ok();
        });
    }

    /// <summary>
    /// ユーザーのパスワードをリセットするためのメールを送信
    /// </summary>
    [HttpPut("ResetPasswordByEmail")]
    public async ValueTask<IActionResult> ResetPasswordByEmail(ResetPasswordByEmailInput input)
    {
        return await CommonValidationResponse(input, _resetPasswordByEmailInputValidator, async () =>
        {
            var request = new ResetPasswordByEmailRequest()
            {
                UserId = input.UserId,
            };
            await _keyclaokService.ResetPasswordByEmailAsync(request);
            return Ok();
        });
    }

    /// <summary>
    /// ユーザーを削除する
    /// </summary>
    /// <param name="input">ユーザー情報</param>
    [HttpDelete("DeleteUser")]
    public async ValueTask<IActionResult> DeleteUser(DeleteUserInput input)
    {
        return await CommonValidationResponse(input, _deleteUserInputValidator, async () =>
        {
            var request = new DeleteUserRequest()
            {
                UserId = input.UserId,
            };
            await _keyclaokService.DeleteUserAsync(request);
            return Ok();
        });
    }
}
