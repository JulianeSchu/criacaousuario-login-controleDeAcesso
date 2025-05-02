using Microsoft.AspNetCore.Identity;

namespace UsuarioApi.Models;

public class Usuario : IdentityUser
{
    public DateTime DataNascimento { get; set; } //No IdentityUser não tem Data de Nascimento
    public Usuario() : base() { } //AS demais propriedades estão sendo usadas da IdentityUser
    
}
