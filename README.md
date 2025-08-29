### 🏍️ Motorcycle Rental API

API para gerenciamento de motos, entregadores e locações, construída em .NET 8 com Postgres e RabbitMQ.
Projeto desenvolvido como parte de um desafio backend.

### 📦 Tecnologias Utilizadas

- .NET 8 (C#)
- Entity Framework Core
- PostgreSQL
- RabbitMQ (mensageria)
- Docker & Docker Compose
- Serilog (logs estruturados)
- Swagger (documentação da API)

### 🚀 Como Rodar o Projeto
Pré-requisitos
1. Docker
2. Docker Compose

Passos
```bash
# Clonar o repositório
git clone https://github.com/seu-usuario/motorcycle-rental-api.git
cd motorcycle-rental-api

# Subir containers (API + DB + RabbitMQ)
docker-compose up --build
```

API estará disponível em:
👉 http://localhost:5000/swagger

RabbitMQ UI (para visualizar mensagens):
👉 http://localhost:15672 (usuário: guest / senha: guest)

### 🗂️ Estrutura do Projeto
```bash
src/
 ├── Api/               # Controllers e configuração da API
 ├── Application/       # DTOs, validações e serviços
 ├── Domain/            # Entidades e regras de negócio
 ├── Infrastructure/    # EF Core, repositórios, mensageria e storage
 └── Tests/             # Testes unitários e de integração
```
### 🔑 Principais Endpoints
🏍️ Motos

- POST /api/motorcycles → Cadastrar moto
- GET /api/motorcycles?plate=XXX → Listar motos (com filtro por placa)
- PUT /api/motorcycles/{id} → Alterar placa
- DELETE /api/motorcycles/{id} → Remover moto

👤 Entregadores

- POST /api/couriers → Cadastrar entregador
- POST /api/couriers/{id}/upload-cnh → Upload CNH (PNG/BMP)

📄 Locações

- POST /api/rentals → Criar locação
- PUT /api/rentals/{id}/return → Devolver moto
- GET /api/rentals/{id}/price → Consultar valor total da locação

### 📬 Mensageria

- Evento publicado ao cadastrar moto: MotorcycleCreated
- Consumidor grava no banco motos com ano 2024.

### 🧪 Testes

Rodar testes:
```bash
dotnet test
```

- Unit Tests → Validam regras de negócio.
- Integration Tests → Validam fluxo completo com DB e API.

### 📖 Convenções

- Código em inglês.
- Segue princípios de Clean Code e DDD light.
- Logs estruturados com Serilog.

### 📌 Próximos Passos (se houver tempo extra)

- Autenticação/Autorização (JWT).
- Pipeline CI/CD.
- Deploy em Cloud (Azure/AWS).
