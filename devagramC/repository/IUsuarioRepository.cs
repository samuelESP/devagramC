using devagramC.Models;

namespace devagramC.repository
{
    public interface IUsuarioRepository
    {

        public void Salvar(Usuario usuario);

        public bool VerificarEmail(string email);
    }
}
