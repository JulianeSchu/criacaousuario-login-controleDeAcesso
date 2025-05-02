using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UsuarioApi.Data.Dto;
using UsuarioApi.Models;

namespace UsuarioApi.Services
{
    public class UsuarioService
    {
        private IMapper _mapper;
        private UserManager<Usuario> _userManager;
        private SignInManager<Usuario> _signInManager;
        private TokenService _tokenService;

        public UsuarioService(UserManager<Usuario> userManager, IMapper mapper, SignInManager<Usuario> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task Cadastra(CreateUsuarioDto dto)
        {
            Usuario usuario = _mapper.Map<Usuario>(dto);
            IdentityResult resultado = await _userManager.CreateAsync(usuario, dto.Password);
            if (!resultado.Succeeded)
            {
                throw new ApplicationException("Falha ao cadastrar usuário!");
            }
        }

        public async Task<string> Login(LoginUsuarioDto dto)
        {
            var resultado = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);
            //Os dois elemento referem-se das seguintes ferramentas:
            //Se o usuário fechar o navegador  vai cair a sessão.
            //Se o usuário errar por algumas vezes vai bloquear o usuario, em nossa aplicação não queremos por enquanto, portanto false e false.
            {
                if (!resultado.Succeeded)
                {
                    throw new ApplicationException("Usuário não autenticado");
                }

                var usuario = _signInManager
                    .UserManager
                    .Users
                    .FirstOrDefault(user => user.NormalizedUserName == dto.Username.ToUpper());


                var token = _tokenService.GenerateToken(usuario);

                return token;
            }
        }
    }
}
