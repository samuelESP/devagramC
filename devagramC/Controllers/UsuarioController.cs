using devagramC.Dtos;
using devagramC.Models;
using devagramC.repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace devagramC.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : BaseController
    {

        private readonly ILogger<UsuarioController> _logger;
        public readonly IUsuarioRepository _usuarioRepository;
        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }


        [HttpGet]
        public IActionResult ObeterUsuario()
        {

            try
            {
                Usuario usuario = new Usuario()
                {
                    Email = "samuel@devaria.com.br",
                    Id = 12,
                    Nome = "Samuel"
                };

                return Ok(usuario);
            }catch(Exception e)
            {
                _logger.LogError("Ocorreu um erro ao obeter usuário");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
            
        }
        [HttpPost]
        public IActionResult SalvarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (usuario != null)
                {
                    var erros = new List<string>();

                    if (string.IsNullOrEmpty(usuario.Email) || string.IsNullOrWhiteSpace(usuario.Email) || !usuario.Email.Contains("@"))
                    {
                        erros.Add("Email inválido");
                    }
                    if (string.IsNullOrEmpty(usuario.Senha) || string.IsNullOrWhiteSpace(usuario.Senha))
                    {
                        erros.Add("Senha inválida");
                    }
                    if (string.IsNullOrEmpty(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Nome))
                    {
                        erros.Add("Nome inválido");
                    }

                    if(erros.Count > 0)
                    {
                        return BadRequest(new ErrorRespostaDto()
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Erros = erros
                        });
                    }

                    usuario.Senha = Utils.MD5Utils.GerarHashMD5(usuario.Senha);
                    usuario.Email = usuario.Email.ToLower();


                    if (!_usuarioRepository.VerificarEmail(usuario.Email))
                    {
                        _usuarioRepository.Salvar(usuario);
                    }
                    else
                    {
                        return BadRequest(new ErrorRespostaDto()
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Descricao = "Usuário já cadastrado!"
                        });
                    }
                        

                }
                    return Ok(usuario);
                
            }
            catch(Exception e)
            {
                _logger.LogError("Ocorreu um erro ao salvar um usuário");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
       
    }
}
