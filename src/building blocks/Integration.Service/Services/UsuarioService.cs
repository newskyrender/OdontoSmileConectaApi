using AutoMapper;
using FluentValidator;
using Integration.Domain.Entities;
using Integration.Domain.Http.Response;
using Integration.Domain.Http.Request;
using Integration.Domain.Repositories;
using Integration.Domain.Common;
using Integration.Domain.Enums;
using Integration.Infrastructure.Transactions;
using System.Security.Cryptography;
using System.Text;

namespace Integration.Service.Services
{
    public class UsuarioService : Notifiable
    {
        private readonly IUsuarioRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUow _uow;

        public UsuarioService(IUsuarioRepository repository, IMapper mapper, IUow uow)
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ICommandResult> Handle(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Alert", "Usuário não encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<UsuarioResponse>(entity);
        }

        public async Task<IEnumerable<ICommandResult>> Listar()
        {
            var entities = await _repository.GetListDataAsync();

            if (!entities.Any()) AddNotification("Alert", "Nenhum usuário encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<List<UsuarioResponse>>(entities);
        }

        public async Task<ICommandResult> Handle(UsuarioRegisterRequest request)
        {
            // Verificar se email já existe
            if (await _repository.ExisteEmailAsync(request.Email))
                AddNotification("Warning", "E-mail já cadastrado");

            if (!IsValid()) return default;

            var senhaHash = HashPassword(request.Senha);
            var entity = new Usuario(default, request.Email, senhaHash, request.Nome, request.Ativo);

            AddNotifications(entity.Notifications);

            if (!IsValid()) return default;

            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<UsuarioResponse>(entity);
        }

        public async Task<ICommandResult> Handle(UsuarioUpdateRequest request)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == request.Id);

            if (entity is null) AddNotification("Warning", "Usuário não encontrado");

            // Verificar se email já existe para outro usuário
            var existeEmail = await _repository.GetByEmailAsync(request.Email);
            if (existeEmail != null && existeEmail.Id != request.Id)
                AddNotification("Warning", "E-mail já cadastrado para outro usuário");

            if (!IsValid()) return default;

            var senhaHash = !string.IsNullOrEmpty(request.Senha) ? HashPassword(request.Senha) : entity.SenhaHash;
            var novoUsuario = new Usuario(request.Id, request.Email, senhaHash, request.Nome, request.Ativo);

            AddNotifications(novoUsuario.Notifications);

            if (!IsValid()) return default;

            await _repository.UpdateAsync(novoUsuario);
            await _uow.CommitAsync();

            return _mapper.Map<UsuarioResponse>(novoUsuario);
        }

        public async Task<ICommandResult> Delete(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Usuário não encontrado");

            if (!IsValid()) return default;

            _repository.Delete(entity);
            await _uow.CommitAsync();

            return _mapper.Map<UsuarioResponse>(entity);
        }

        public async Task<ICommandResult> Handle(UsuarioLoginRequest request)
        {
            var usuario = await _repository.GetByEmailAsync(request.Email);

            if (usuario is null || !VerifyPassword(request.Senha, usuario.SenhaHash))
                AddNotification("Error", "E-mail ou senha inválidos");

            if (!usuario.Ativo)
                AddNotification("Warning", "Usuário desativado");

            if (!IsValid()) return default;

            // Gerar token JWT (implementar conforme necessário)
            var token = GenerateJwtToken(usuario);

            return new LoginResponse
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Nome = usuario.Nome,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            // Implementar geração de JWT token
            return $"token_for_{usuario.Id}_{DateTime.UtcNow.Ticks}";
        }
        
        public async Task<ICommandResult> Handle(AlterarSenhaRequest request)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == request.Id);

            if (entity is null) AddNotification("Warning", "Usuário não encontrado");

            if (!VerifyPassword(request.SenhaAtual, entity.SenhaHash))
                AddNotification("Warning", "Senha atual incorreta");

            if (!IsValid()) return default;

            var senhaHash = HashPassword(request.NovaSenha);
            entity.AtualizarSenha(senhaHash);

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<UsuarioResponse>(entity);
        }
    }
}
