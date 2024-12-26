using OurNotesAppBackEnd.Identity;

namespace OurNotesAppBackEnd.Extensions;

public static class IdentityApiEndpointsExtensions
{
    public static IEndpointConventionBuilder CustomMapIdentityApi<TUser>(this IEndpointConventionBuilder endpoints) where TUser : AppUser, new()
    {
        return endpoints;
    }
}