SistemaGestao API

API REST desenvolvida em **ASP.NET Core** para gerenciamento de tarefas.  
Permite criar, atualizar, listar e excluir tarefas, com suporte a filtros por status e data de vencimento.

## Tecnologias utilizadas
- ASP.NET Core 7+
- Entity Framework Core
- UseInMemoryDatabase
- Swagger / OpenAPI

## 📂 Estrutura do projeto
- **Controllers** → Endpoints da API (`TarefasController`)
- **Models** → Entidades do domínio (`Tarefa`)
- **Dtos** → Objetos de transferência de dados (`TarefaCreateDto`, `TarefaReadDto`, `TarefaUpdateDto`)
- **Data** → Contexto do banco (`SistemaGestaoContext`)

## 🔗 Endpoints principais

### Tarefas
- `GET /api/Tarefas` → Lista todas as tarefas (com filtros opcionais).
- `GET /api/Tarefas/{id}` → Retorna uma tarefa específica.
- `POST /api/Tarefas` → Cria uma nova tarefa.
- `PUT /api/Tarefas/{id}` → Atualiza uma tarefa existente.
- `DELETE /api/Tarefas/{id}` → Exclui uma tarefa.

## 📖 Documentação Swagger
A documentação interativa está disponível em:
https://localhost:7089/swagger/index.html

## O arquivo JSON da especificação pode ser acessado em:
https://localhost:7089/swagger/v1/swagger.json

## 🛠️ Como executar
1. Clone o repositório:
2. git clone https://github.com/kassioburgadon/SistemaGestao.git
3. cd SistemaGestao\SistemaGestao
4. dotnet run

✅ Testes
1. cd SistemaGestao\SistemaGestao.Test
2. dotnet test
3. Os testes de integração utilizam HttpClient para validar os endpoints. Exemplos:
- PostTarefa_DeveRetornarCreated
- PutTarefa_DeveRetornarNoContent
- DeleteTarefa_DeveRetornarNoContent

👩‍💻 Autor
Projeto desenvolvido por Kassio.
