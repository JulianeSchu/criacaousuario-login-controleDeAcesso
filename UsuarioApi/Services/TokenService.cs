
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UsuarioApi.Models;

namespace UsuarioApi.Services;

public class TokenService
{
    private IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(Usuario usuario)
    {
        Claim[] claim = new Claim[]
        {
            //Não colocar informações sensíveis como senhas, pois o token pode ser "desencodado".
            new Claim("Username", usuario.UserName),
            new Claim("id", usuario.Id),
            new Claim(ClaimTypes.DateOfBirth, usuario.DataNascimento.ToString())
        };

        var chave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["SymmetricSecurityKey"])); //Gerado aleatoriaamente pelo teclado

        var signingCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
            (
            expires: DateTime.Now.AddMinutes(10),
            claims: claim,
            signingCredentials: signingCredentials
            );

        return  new JwtSecurityTokenHandler().WriteToken(token);
    }
}