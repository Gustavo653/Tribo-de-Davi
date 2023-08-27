using Common.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain.Identity;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;

namespace TriboDavi.Service
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        public AccountService(SignInManager<User> signInManager,
                              IUserRepository userRepository,
                              ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        private async Task<SignInResult> CheckUserPassword(User user, UserLoginDTO userLoginDTO)
        {
            try
            {
                return await _signInManager.CheckPasswordSignInAsync(user, userLoginDTO.Password, false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar senha do usuário. Erro: {ex.Message}");
            }
        }

        private async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                return await _userRepository.GetEntities()
                                            .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                                            .FirstOrDefaultAsync(x => x.NormalizedEmail == email.ToUpper());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter o usuário. Erro: {ex.Message}");
            }
        }

        public async Task<ResponseDTO> Login(UserLoginDTO userDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var user = await GetUserByEmail(userDTO.Email);
                if (user == null)
                {
                    responseDTO.Code = 401;
                    responseDTO.Message = "Não autenticado! Email inexistente!";
                    return responseDTO;
                }
                var password = await CheckUserPassword(user, userDTO);
                if (!password.Succeeded)
                {
                    responseDTO.Code = 401;
                    responseDTO.Message = $"Não autenticado! {password}!";
                    return responseDTO;
                }

                responseDTO.Object = new
                {
                    userName = user.UserName,
                    name = user.Name,
                    email = user.Email,
                    token = await _tokenService.CreateToken(user)
                };
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetCurrent(string email)
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await GetUserByEmail(email);
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}