using devagramC.Models;

namespace devagramC.repository
{
    public interface IUsuarioRepository
    {
        public Usuario GetUsuarioPorLoginSenha(string email, string senha);
        public void Salvar(Usuario usuario);

        public bool VerificarEmail(string email);

        public Usuario GetUsuarioPorId(int id);

        public void AtualizarUsuario(Usuario usuario);
    }
}
