# 🎓 PlataformaEducação - Plataforma de Educação Online

## 📘 Visão Geral
Este repositório contém a implementação da **Plataforma de Educação Online**, projeto desenvolvido como entrega do **Módulo 3 - Arquitetura, Modelagem e Qualidade de Software** do MBA DevXpert Full Stack .NET. A plataforma simula um ambiente educacional com gestão de conteúdos, alunos e pagamentos, utilizando boas práticas como **DDD**, **TDD**, **CQRS** e separação por **Bounded Contexts (BC)**.

## 👩‍💻 Autora
- **Karina Esparza**

## 🚀 Funcionalidades
- 📚 Gestão de cursos, alunos, matrículas, pagamentos, certificados e aulas
- 🔐 Autenticação e autorização com ASP.NET Core Identity e JWT
- 🌐 API REST com documentação via Swagger
- ✅ Testes unitários e de integração com xUnit

## 🧪 Tecnologias
- **Linguagem:** C#
- **Frameworks:** ASP.NET Core Web API, Entity Framework Core, xUnit
- **Banco de Dados:** SQL Server e SQLite
- **Segurança:** ASP.NET Core Identity + JWT
- **Documentação:** Swagger

## 📂 Estrutura do Projeto
```
src/
├── PlataformaEducacao.Core/
├── PlataformaEducacao.GestaoConteudos.Application/
├── PlataformaEducacao.GestaoConteudos.Domain/
├── PlataformaEducacao.GestaoConteudos.Data/
├── PlataformaEducacao.GestaoAlunos.Application/
├── PlataformaEducacao.GestaoAlunos.Domain/
├── PlataformaEducacao.GestaoAlunos.Data/
├── PlataformaEducacao.Pagamentos.AntiCorruption/
├── PlataformaEducacao.Pagamentos.Business/
├── PlataformaEducacao.Pagamentos.Data/
└── PlataformaEducacao.Api/

tests/
├── PlataformaEducacao.GestaoConteudos.Application.Tests/
├── PlataformaEducacao.GestaoConteudos.Domain.Tests/
├── PlataformaEducacao.GestaoAlunos.Application.Tests/
├── PlataformaEducacao.GestaoAlunos.Domain.Tests/
├── PlataformaEducacao.Pagamentos.Business.Tests/
└── PlataformaEducacao.Api.Tests/

Outros:
├── README.md
├── FEEDBACK.md
└── .gitignore
```

## ▶️ Como Executar o Projeto

### ✅ Pré-requisitos
- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- Visual Studio 2022+ ou outra IDE compatível
- SQL Server
- Git

### 🚧 Configuração Inicial
1. **Clone o repositório**
```bash
git clone https://github.com/Karinaesparza96/plataforma-educacao-online.git
cd plataforma-educacao-online
```

2. **Execute a API**
```bash
cd PlataformaEducacao/src/PlataformaEducacao.Api
dotnet run
```
- Acesse: [http://localhost:5224/swagger](http://localhost:5224/swagger)

3. **O banco será criado automaticamente** na primeira execução via *Seed de dados*. Não é necessário aplicar migrações manualmente.
   
- **Caso deseje usar SqlServer**
  - No arquivo `src/PlataformaEducacao.Api/appsettings.json`, ajuste a `ConnectionStrings:DefaultConnection` com sua instância do SQL Server.

### 🧪 Executar os Testes
```bash
cd PlataformaEducacao
dotnet test PlataformaEducacao.sln
```
> 💡 Você também pode executar os testes diretamente pelo Visual Studio (clicando com o botão direito na solution e escolhendo "Run Tests").

## ⚙️ Configurações Extras
- 🔑 **JWT:** As configurações estão em `appsettings.json` da API.
- 🗃️ **Seed de Dados:** Dados iniciais são criados automaticamente ao rodar a aplicação.
- 👩‍💻 **Logins / Senhas Cadastrados para Testes**:
   - Aluno:
       `aluno@teste.com` /
       `Teste@123`
    
   - Admin:
       `admin@teste.com` /
       `Teste@123`

## 📄 Documentação da API
Acesse a documentação gerada pelo Swagger após subir a API:
🔗 [http://localhost:5001/swagger](http://localhost:5001/swagger)

## 📌 Observações
- Este projeto é de uso acadêmico e **não aceita contribuições externas**.
- Para dúvidas ou sugestões, utilize a aba **Issues** do GitHub.
- O arquivo `FEEDBACK.md` é reservado para avaliações do instrutor.
