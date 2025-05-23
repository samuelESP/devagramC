﻿using devagramC.Models;

namespace devagramC.repository.Impl
{
    public class UsuarioRepositoryImpl : IUsuarioRepository
    {
        private readonly DevagramContext _context;

        public UsuarioRepositoryImpl(DevagramContext context)
        {
            _context = context;
        }

        public void AtualizarUsuario(Usuario usuario)
        {
            _context.Update(usuario);
            _context.SaveChanges();
        }

        public Usuario GetUsuarioPorId(int id)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public Usuario GetUsuarioPorLoginSenha(string email, string senha)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
        }

        public void Salvar(Usuario usuario)
        {
            _context.Add(usuario);
            _context.SaveChanges();
        }

        public bool VerificarEmail(string email)
        {
            return _context.Usuarios.Any(usuario => usuario.Email == email);
        }
    }
}
