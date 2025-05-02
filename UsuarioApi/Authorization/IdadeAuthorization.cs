using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace UsuarioApi.Authorization;

public class IdadeAuthorization : AuthorizationHandler<IdadeMinima>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IdadeMinima requirement)
    {
        var dataNascimentoClaim = context
            .User.FindFirst(claim =>
            claim.Type == ClaimTypes.DateOfBirth);

        if (dataNascimentoClaim is null)
        return Task.CompletedTask;

        var dataNascimento = Convert.ToDateTime(
            dataNascimentoClaim.Value);

        var idadeUsuario = DateTime.Today.Year - dataNascimento.Year;

        if (dataNascimento > DateTime.Today.AddYears(-idadeUsuario)) idadeUsuario--; 
        //Se a data de aniversário do usuário for maio que a data atual, diminui em 1 ano, ou seja fará aiversário no decorrer dos proximos dias ou meses.

        if (idadeUsuario>= requirement.Idade)context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
