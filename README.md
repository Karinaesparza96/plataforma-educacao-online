# **[PlataformaEducação] - Plataforma de Educação Online**

## **1. Apresentação**

Bem-vindo ao repositório do projeto **[PlataformaEducação]**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao **Módulo 3: Arquitetura, Modelagem e Qualidade de Software**.
O objetivo principal desenvolver uma plataforma educacional online com múltiplos bounded contexts (BC), aplicando DDD, TDD, CQRS e padrões arquiteturais para gestão eficiente de conteúdos educacionais, alunos e processos financeiros.


### **Autor(es)**
- **Karina Esparza**

## **2. Proposta do Projeto**

O projeto consiste em: 

- **API RESTful:** Exposição dos recursos da plataforma de educação.
- **Autenticação e Autorização:** Implementação de controle de acesso, diferenciando administradores e usuários comuns.
- **Acesso a Dados:** Implementação de acesso ao banco de dados através de ORM.

**ps.: Ainda não implementado**

## **3. Tecnologias Utilizadas**

- **Linguagem de Programação:** C#
- **Frameworks:**
  - ASP.NET Core Web API
  - Entity Framework Core
  - Xunit
- **Banco de Dados:** Sqlite
- **Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **Documentação da API:** Swagger

## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:


- src/
  - PlataformaEducacao.Core/ - Contexto Compartilhado
  - PlataformaEducacao.GestaoConteudos.Aplication/ - Contexto GestaoAlunos - (Commands, Handlers, Queries)
  - PlataformaEducacao.GestaoConteudos.Domain/ - Contexto GestaoAlunos - Entidades, Interfaces
  - PlataformaEducacao.GestaoConteudos.Data/ - Contexto GestaoAlunos - Acesso a Dados
  - PlataformaEducacao.GestaoAlunos.Aplication/ - Contexto GestaoAlunos - (Commands, Handlers, Queries)
  - PlataformaEducacao.GestaoAlunos.Domain/ - Contexto GestaoAlunos - Entidades, Interfaces
  - PlataformaEducacao.GestaoAlunos.Data/ - Contexto GestaoAlunos - Acesso a Dados
  - PlataformaEducacao.GestaoAlunos.Pagamentos/ - Contexto Pagamentos
- tests/
  - PlataformaEducacao.GestaoConteudos.Aplication.Tests/ - Testes Contexto GestaoAlunos - (Commands, Handlers, Queries)
  - PlataformaEducacao.GestaoConteudos.Domain.Tests/ - Testes Contexto GestaoAlunos - Entidades, Interfaces
  - PlataformaEducacao.GestaoAlunos.Aplication.Tests/ - Testes Contexto GestaoAlunos - (Commands, Handlers, Queries)
  - PlataformaEducacao.GestaoAlunos.Domain.Tests/ - Testes Contexto GestaoAlunos - Entidades, Interfaces
- README.md - Arquivo de Documentação do Projeto
- FEEDBACK.md - Arquivo para Consolidação dos Feedbacks
- .gitignore - Arquivo de Ignoração do Git

## **5. Funcionalidades Implementadas**

- **Gestao da Platorma:** Permite a gestão de cursos/matriculas/alunos/pagamentos e provê meios para que os
alunos realizem os cursos.
- **Autenticação e Autorização:** Diferenciação entre alunos e administradores.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 8.0 ou superior
- SQL Server
- Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   - `git clone https://github.com/seu-usuario/nome-do-repositorio.git`
   - `cd nome-do-repositorio`

2. **Configuração do Banco de Dados:**
   - No arquivo `appsettings.json`, configure a string de conexão do SQL Server.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos

3. **Executar os Testes:**
   - `cd tests/`
   - `dotnet run`
   - ps: Executar no Visual Studio

4. **Executar a API:**
   - `cd src/PlataformaEducacao.Api/`
   - `dotnet run`
   - Acesse a documentação da API em: http://localhost:5001/swagger

## **7. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

http://localhost:5001/swagger

## **9. Avaliação**

- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.
