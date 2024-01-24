using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace loanApi.Helper
{
    public class Claims
    {
        public static int GetCurrentUser(string jwtToken) {

            var th = new JwtSecurityTokenHandler();

            var rdth = th.ReadToken(jwtToken.Replace("Bearer ", "")) as JwtSecurityToken;

            IEnumerable<Claim> claims = rdth.Claims;


            var c = JsonConvert.SerializeObject(claims, Formatting.Indented);

            var myclaims = claims.Where(x => x.Type == ClaimTypes.Role || x.Type == ClaimTypes.NameIdentifier);

            var userId = int.Parse(claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault());


            return userId;
        }
    }
}
