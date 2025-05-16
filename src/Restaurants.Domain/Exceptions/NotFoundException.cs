namespace Restaurants.Domain.Exceptions;
public class NotFoundException(string resourseType, string resourseIdentifier) :
    Exception($"{resourseType} with id: {resourseIdentifier} doesn't exist")
{
}
