using devagramC.Dtos;
using devagramC.Models;
using devagramC.repository;
using devagramC.Services;
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

        public UsuarioController(ILogger<UsuarioController> logger,
             IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _logger = logger;
        }


        [HttpGet]
        public IActionResult ObeterUsuario()
        {

            try
            {
                Usuario usuario = LerToken();

                return Ok(new UsuarioRepostaDto
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email
                });
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
        [AllowAnonymous]
        public IActionResult SalvarUsuario([FromForm] UsuarioRequisicaoDto usuariodto)
        {
            try
            {


                if (usuariodto != null)
                {
                    var erros = new List<string>();

                    if (string.IsNullOrEmpty(usuariodto.Email) || string.IsNullOrWhiteSpace(usuariodto.Email) || !usuariodto.Email.Contains("@"))
                    {
                        erros.Add("Email inválido");
                    }
                    if (string.IsNullOrEmpty(usuariodto.Senha) || string.IsNullOrWhiteSpace(usuariodto.Senha))
                    {
                        erros.Add("Senha inválida");
                    }
                    if (string.IsNullOrEmpty(usuariodto.Nome) || string.IsNullOrWhiteSpace(usuariodto.Nome))
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

                    CosmicService cosmicService = new CosmicService();

                    Usuario usuario = new Usuario()
                    {
                        Email = usuariodto.Email,
                        Senha = usuariodto.Senha,
                        Nome = usuariodto.Nome,
                        FotoPerfil = cosmicService.EnviarImagem(new ImagemDto { Imagem = usuariodto.FotoPerfil, Nome = usuariodto.Nome.Replace(" ", "") }),
                    };


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
                    return Ok("usuário foi salvo com sucesso");
                
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
