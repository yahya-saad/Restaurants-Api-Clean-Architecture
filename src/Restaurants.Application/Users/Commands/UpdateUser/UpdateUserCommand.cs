using Restaurants.Application.Users.DTOs;

namespace Restaurants.Application.Users.Commands.UpdateUser;
public record class UpdateUserCommand(UpdateUserDto Dto) : IRequest;

