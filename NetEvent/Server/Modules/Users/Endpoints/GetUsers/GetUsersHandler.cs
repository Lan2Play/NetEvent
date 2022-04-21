using MediatR;
using Microsoft.EntityFrameworkCore;
using NetEvent.Server.Data;
using NetEvent.Shared.Dto;

namespace NetEvent.Server.Modules.Users.Endpoints.GetUsers
{
    public class GetUsersHandler : IRequestHandler<GetUsersRequest, GetUsersResponse>
    {
        private readonly ApplicationDbContext _UserDbContext;
        private readonly ILogger<GetUsersHandler> _Logger;

        public GetUsersHandler(ApplicationDbContext userDbContext, ILogger<GetUsersHandler> logger)
        {
            _UserDbContext = userDbContext;
            _Logger = logger;
        }

        public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var allUsers = await _UserDbContext.Users.ToListAsync();

            var convertedUsers = allUsers.Select(a => new CurrentUser()
            {
                Id = a.Id,
                UserName = a.UserName,
                Email = a.Email,
                LastName = a.LastName,
                FirstName = a.FirstName,
                ProfileImage = a.ProfilePicture,
                EmailConfirmed = a.EmailConfirmed,           
            });


            return new GetUsersResponse(convertedUsers);
        }
    }
}
