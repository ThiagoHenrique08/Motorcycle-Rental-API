# 🏍️ Motorcycle Rental API

API para gerenciamento de motos, entregadores e locações, construída em .NET 9.0 com C#, PostgreSQL e RabbitMQ, seguindo arquitetura Clean Architecture / UseCase 
Projeto desenvolvido como parte de um desafio backend, implementando REST APIs

---

## 📦 Tecnologias Utilizadas

### Desenvolvimento

- .NET 9.0
- C#
- FluentResults (resultados funcionais)
- FluentValidation (validações de DTOs)
- OpenAPI / Swagger (documentação interativa)
- Entity Framework Core (ORM + Migrations)
- ILogger (logs estruturados)
- Identity (gestão de usuários e roles)
- JWT Bearer Token (autenticação via token)
- Tratamento de erros global

### Testes Unitário e Integração

- XUnit
- Bogus (mock de dados)
- Moq (mock de dependências)
- Microsoft.AspNetCore.Mvc.Testing (testes de integração)
- FluentAssertions (asserts legíveis)

### Infraestrutura e Deploy

- RabbitMQ (mensageria assíncrona)
- Docker & Docker Compose (containerização)
- GitHub Actions (execução de testes automáticos a cada push)
- PostgreSQL (banco relacional)

---

## 🏗️ Arquitetura e Estrutura do Projeto

Arquitetura Clean Architecture / UseCase / DDD Light, organizada em camadas:

```bash
src/
 ├── Api/               # Controllers, rotas, autenticação e configuração do Swagger
 ├── Application/       # UseCases, serviços, DTOs, validações e lógica de fluxo
 ├── Domain/            # Entidades, agregados e regras de negócio
 ├── Infrastructure/    # Repositórios, EF Core, mensageria, storage, Identity, JWT
 └── Tests/             # Testes unitários e de integração
```
## 🔑 Premissas do Projeto

Usuários e Roles

- Novo usuário criado recebe automaticamente a role ADMIN.
- É possível revogar a role admin: POST Auth/RevokeRoleToUser ou conceder outra role POST Auth/AddUserToRole.
- Ao atribuir nova role, é necessário logar novamente para atualizar o token.
- Criar role ENTREGADOR para testes em endpoints específicos.

Autenticação

- Login retorna JWT Bearer token.
- Para testar endpoints protegidos no Swagger: Bearer [seu_token_aqui] → clicar em Authorize.

Mensageria

- Ao cadastrar uma moto, evento MotorcycleCreated é publicado no RabbitMQ.
- Consumidor registra motos com ano 2024 no banco.
- Permite escalabilidade e processamento assíncrono de notificações.

Regras de locação

- Apenas entregadores habilitados na categoria A podem alugar motos.
- Planos de locação:

    - 7 dias → R$30/dia (multa 20% se antecipar)
    - 15 dias → R$28/dia (multa 40% se antecipar)
    - 30 dias → R$22/dia
    - 45 dias → R$20/dia
    - 0 dias → R$18/dia

- Data de início obrigatoriamente primeiro dia após a criação da locação.

- Devolução antecipada ou atrasada aplica multas conforme regras do plano.

##  🚀 Como Rodar o Projeto

Pré-requisitos

- Docker
- Docker Compose

```bash
### Clonar o repositório
git clone https://github.com/seu-usuario/motorcycle-rental-api.git
cd motorcycle-rental-api

### Subir containers (API + DB + RabbitMQ)

docker-compose up --build
```

URLs de Acesso

- API / Swagger UI: 👉 http://localhost:8080/index.html
- RabbitMQ UI: 👉 http://localhost:15672 (usuário: admin / senha: Criativos123@)

## 🏍️ Endpoints Principais

Motos

- POST /api/motorcycles → Cadastrar moto [ADMIN]
- GET /api/motorcycles/by-plate?plate=XXX → Listar motos (filtro por placa) [ADMIN]
- PUT /api/motorcycles/{id} → Alterar placa [ADMIN]
- DELETE /api/motorcycles/{id} → Remover moto (sem locações) [ADMIN]

Entregadores

- POST /api/DeliveryMan → Cadastrar entregador [ENTREGADORES]
- POST /api/DeliveryMan/{id}/cnh → Upload CNH (PNG/BMP) [ENTREGADORES]

Locações

- POST /api/Location → Criar locação [ENTREGADORES]
- PUT /api/Location/{id}/return → Devolver moto [ENTREGADORES]
- GET /api/Location/{id} → Consultar locação por id [ENTREGADORES]

Usuários / Roles

- POST /Auth/CreateRole → Cria uma nova role [ADMIN]
- POST /Auth/AddUserToRole → Adiciona uma role a um usuário [ADMIN]
- POST /Auth/Login → Realiza login [ADMIN]
- POST /Auth/register-user → Realiza o registro de um novo usuário [ADMIN]
- POST /Auth/Refresh-token → Cria um novo token temporario para postergar a sessao. [ADMIN]
- POST /Auth/revoke{username} → Revoga o token de acesso do usuário [ADMIN]
- POST /Auth/RevokeRoleToUser → Revoga o token de acesso do usuário [ADMIN]
- GET /Auth/getUsers → Retorna a lista de usuarios cadastrados [ADMIN]
- GET /Auth/getUserById → Retorna um usuário pelo seu Id [ADMIN]




## 🧪 Testes

```bash
dotnet test
```

- Unit Tests → Validação de regras de negócio com XUnit, Moq e FluentAssertions.

- Integration Tests → Fluxos completos envolvendo DB, API e RabbitMQ, usando Microsoft.AspNetCore.Mvc.Testing e Bogus para mock de dados.