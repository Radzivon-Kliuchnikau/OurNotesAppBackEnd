using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}