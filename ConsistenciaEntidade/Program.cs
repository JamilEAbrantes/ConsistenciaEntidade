using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsistenciaDeEntidade
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-> Iniciando validação de entidade: \n");

            var acesso = new AcessoVO("DAVI", "123");
            var cliente = new Cliente("Davi Martins", 28, "davi123", acesso);

            if (cliente.Invalido)
                Console.WriteLine(cliente.ObterMensagens());
            else
                Console.WriteLine("Cliente validado com sucesso!");

            Console.WriteLine("\n-> Processo de validação de entidade finalizado.");

            Console.ReadKey();
        }
    }

    #region --> Entidade e Value Object

    public class Cliente : ValidadorBase
    {
        public Cliente(
            string nome,
            int idade,
            string email,
            AcessoVO acesso)
        {
            Nome = nome;
            Idade = idade;
            Email = email;
            Acesso = acesso;

            AdicionarMensagems(new Validacoes()
                .StringNulaOuVazia(nome, "Preencha um nome válido.")
                .StringNulaOuVazia(email, "Preencha um email válido.")
                .StringNulaOuVazia(acesso.Login, "Preencha um login válido.")
                .StringNulaOuVazia(acesso.Senha, "Preencha uma senha.")
                .IntZerado(idade, "Somente maiores de 18 anos.")
                .IntRangeMinimo(idade, 18, "Idade permitida somente para maiores de 18 anos.")
                .IntRangeMaximo(idade, 60, "Idade permitida somente até os 60 anos."));
        }

        public string Nome { get; private set; }
        public int Idade { get; private set; }
        public string Email { get; private set; }
        public AcessoVO Acesso { get; private set; }

        public override string ToString()
            => $"[ { GetType().Name } - Nome: { Nome }, Idade: { Idade }, E-mail: { Email } ]";
    }

    public class AcessoVO
    {
        public AcessoVO(
            string login,
            string senha)
        {
            Login = login;
            Senha = senha;
        }

        public string Login { get; private set; }
        public string Senha { get; private set; }

        public override string ToString()
            => $"[ { GetType().Name } - Login: { Login }, Senha: { Senha } ]";
    }

    #endregion

    #region --> Componente para validação

    public abstract class ValidadorBase
    {
        protected ValidadorBase()
        {
            Mensagens = new List<string>();
        }

        public readonly List<string> Mensagens;

        public bool Invalido => Mensagens.Any();

        public void AdicionarMensagems(params string[] mensagens)
            => Mensagens.AddRange(mensagens);

        public void AdicionarMensagems(ValidadorBase validadorBase)
            => Mensagens.AddRange(validadorBase.Mensagens);

        public string ObterMensagens()
            => string.Join(", \n", Mensagens.ToList().Select(item => "" + item + "")).Replace(".", "");
    }

    public partial class Validacoes : ValidadorBase
    {

    }

    #region --> Validações de strings

    public partial class Validacoes
    {
        public Validacoes StringNulaOuVazia(
            string valor,
            string mensagem)
        {
            if (string.IsNullOrEmpty(valor))
                AdicionarMensagems(mensagem);

            return this;
        }

        public Validacoes StringTamanhoMinimo(
            string valor,
            int tamanho,
            string mensagem)
        {
            if (valor.Count() <= tamanho)
                AdicionarMensagems(mensagem);

            return this;
        }

        public Validacoes StringTamanhoMaximo(
            string valor,
            int tamanho,
            string mensagem)
        {
            if (valor.Count() <= tamanho)
                AdicionarMensagems(mensagem);

            return this;
        }
    }

    #endregion

    #region --> Validações de inteiros

    public partial class Validacoes
    {
        public Validacoes IntZerado(
            int valor,
            string mensagem)
        {
            if (valor == 0)
                AdicionarMensagems(mensagem);

            return this;
        }

        public Validacoes IntRangeMinimo(
            int valor,
            int tamanho,
            string mensagem)
        {
            if (valor <= tamanho)
                AdicionarMensagems(mensagem);

            return this;
        }

        public Validacoes IntRangeMaximo(
            int valor,
            int tamanho,
            string mensagem)
        {
            if (valor >= tamanho)
                AdicionarMensagems(mensagem);

            return this;
        }
    }

    #endregion

    #endregion
}
