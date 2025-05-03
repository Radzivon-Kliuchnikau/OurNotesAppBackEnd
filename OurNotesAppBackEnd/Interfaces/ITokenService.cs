using OurNotesAppBackEnd.Identity;

namespace OurNotesAppBackEnd.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}