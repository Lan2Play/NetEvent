﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using NetEvent.Server.Models;

namespace NetEvent.Server.Modules.Authorization.Endpoints.PostRegisterUser
{
    public class PostRegisterUserHandler : IRequestHandler<PostRegisterUserRequest, PostRegisterUserResponse>
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly ILogger<PostRegisterUserHandler> _Logger;

        public PostRegisterUserHandler(UserManager<ApplicationUser> userManager, ILogger<PostRegisterUserHandler> logger)
        {
            _UserManager = userManager;
            _Logger = logger;
        }

        public async Task<PostRegisterUserResponse> Handle(PostRegisterUserRequest request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser()
            {
                EmailConfirmed = false
            };

            user.UserName = request.RegisterRequest.Email;

            var result = await _UserManager.CreateAsync(user, request.RegisterRequest.Password);

            if (!result.Succeeded)
            {
                return new PostRegisterUserResponse(ReturnType.Error, result.Errors.FirstOrDefault()?.Description);
            }

            // TODO Schedule Task for sending E-Mail

            return new PostRegisterUserResponse();
        }
    }
}