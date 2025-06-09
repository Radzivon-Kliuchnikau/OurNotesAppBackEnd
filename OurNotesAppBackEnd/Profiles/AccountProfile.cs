using AutoMapper;
using OurNotesAppBackEnd.Dtos.Account;
using OurNotesAppBackEnd.Models;

namespace OurNotesAppBackEnd.Profiles;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        // Source -> Target
        CreateMap<RegistrationModelDto, AppUser>();
    }
}