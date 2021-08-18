using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebMvc.Services
{
    public interface IIdentityService<T> //it is generic type <T>.
                                         //whatever my service will send back will be my type
                                         //in future if a call is made using another token service not that you created but somebody else,
                                         //it can be repurpose<T> to accept whatever type that they send as ot everybody will be sending back application user,
                                         //example somebody can send you back a token
    {
        T Get(IPrincipal principal); //Get the User (Principal:term used in token for user information)
    }
}
