
using System.Text;

namespace SistemaGestao.Test
{
    public class Tests
    {
        private HttpClient _client;
        private string responseLastBody;

        [OneTimeSetUp]
        public async Task SetupAsync()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7089")
            };     
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
            
        }


        [Test]
        public async Task PostTarefa_DeveRetornarCreated()
        {
            await publicasTarefasAsync();
            var json = @"{
            ""titulo"": ""Teste via HttpClient"",
            ""descricao"": ""Criar tarefa pelo teste"",
            ""dataVencimento"": ""2026-05-20T00:00:00"",
            ""status"": 0
        }";

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("https://localhost:7089/api/Tarefas", content);

            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That((int)response.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task GetTarefas_DeveRetornarOk()
        {
            await publicasTarefasAsync();
            var response = await _client.GetAsync("https://localhost:7089/api/Tarefas");
            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetTarefaPorId_DeveRetornarOk()
        {
            await publicasTarefasAsync();
            var response = await _client.GetAsync("https://localhost:7089/api/Tarefas/" + responseLastBody.Trim('"'));
            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
        }
        [Test]
        public async Task PutTarefa_DeveRetornarOk()
        {
            await publicasTarefasAsync();
            var json = $@"{{
                ""titulo"": ""Tarefa Atualizada"",
                ""descricao"": ""Atualizar tarefa pelo teste"",
                ""dataVencimento"": ""2026-05-25T00:00:00"",
                ""status"": 1
            }}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("https://localhost:7089/api/Tarefas/"+ responseLastBody.Trim('"'), content);
            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
        }
        [Test]
        public async Task DeleteTarefa_DeveRetornarOK()
        {
            await publicasTarefasAsync();
            var response = await _client.DeleteAsync("https://localhost:7089/api/Tarefas/" + responseLastBody.Trim('"'));
            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
        }
        [Test]
        public async Task GetTarefaPorId_AposDelete_DeveRetornarNotFound()
        {
            await DeleteTarefa_DeveRetornarOK();
            var response = await _client.GetAsync("https://localhost:7089/api/Tarefas/" + responseLastBody.Trim('"'));
            Assert.That(response.IsSuccessStatusCode, Is.False);
            Assert.That((int)response.StatusCode, Is.EqualTo(404));
        }
        [Test]
        public async Task PostTarefa_ComDadosInvalidos_DeveRetornarBadRequest()
        {
            await publicasTarefasAsync();
            var json = @"{
            ""titulo"": """",
            ""descricao"": ""Criar tarefa com dados inválidos"",
            ""dataVencimento"": ""2026-05-20T00:00:00"",
            ""status"": 0
        }";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("https://localhost:7089/api/Tarefas", content);
            Assert.That(response.IsSuccessStatusCode, Is.False);
            Assert.That((int)response.StatusCode, Is.EqualTo(400));
        }
        [Test]
        public async Task PutTarefa_ComDadosInvalidos_DeveRetornarBadRequest()
        {
            await publicasTarefasAsync();
            var json = @"{
            ""id"": 1,
            ""titulo"": """",
            ""descricao"": ""Atualizar tarefa com dados inválidos"",
            ""dataVencimento"": ""2026-05-25T00:00:00"",
            ""status"": 1
        }";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("https://localhost:7089/api/Tarefas/674fac07-b66c-457b-bee1-81222c6b0a90", content);
            Assert.That(response.IsSuccessStatusCode, Is.False);
            Assert.That((int)response.StatusCode, Is.EqualTo(400));
        }
        [Test]
        public async Task GetTarefaPorId_ComIdInexistente_DeveRetornarNotFound()
        {
            await publicasTarefasAsync();
            var response = await _client.GetAsync("https://localhost:7089/api/Tarefas/674fac07-b66c-457b-bee1-81222c6b0a90");
            Assert.That(response.IsSuccessStatusCode, Is.False);
            Assert.That((int)response.StatusCode, Is.EqualTo(404));
        }
        [Test]
        public async Task DeleteTarefa_ComIdInexistente_DeveRetornarNotFound()
        {
            await publicasTarefasAsync();
            var response = await _client.DeleteAsync("https://localhost:7089/api/Tarefas/674fac07-b66c-457b-bee1-81222c6b0a90");
            Assert.That(response.IsSuccessStatusCode, Is.False);
            Assert.That((int)response.StatusCode, Is.EqualTo(404));
        }
        [Test]
        public async Task GetTarefas_AposCriarVarios_DeveRetornarListaCompleta()
        {
            await publicasTarefasAsync();
            // Obter a lista de tarefas
            var response = await _client.GetAsync("https://localhost:7089/api/Tarefas");
            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That((int)response.StatusCode, Is.EqualTo(200)); // OK
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.That(responseData, Does.Contain("Tarefa 0"));
            Assert.That(responseData, Does.Contain("Tarefa 1"));
            Assert.That(responseData, Does.Contain("Tarefa 2"));
            Assert.That(responseData, Does.Contain("Tarefa 3"));
            Assert.That(responseData, Does.Contain("Tarefa 4"));
        }
        [Test]
        public async Task GetTarefas_FiltrarPorStatus_DeveRetornarApenasTarefasComStatusEspecificado()
        {
            await publicasTarefasAsync();

            // Criar tarefas com diferentes status
            for (int i = 0; i < 5; i++)
            {
                var json = $@"{{
                    ""titulo"": ""Tarefa Status {i}"",
                    ""descricao"": ""Descrição da tarefa status {i}"",
                    ""dataVencimento"": ""2026-05-20T00:00:00"",
                    ""status"": {i % 2} // Alterna entre 0 e 1
                }}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _client.PostAsync("https://localhost:7089/api/Tarefas", content);
            }
            var response = await _client.GetAsync("https://localhost:7089/api/Tarefas?status=1");
            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.That(responseData, Does.Contain("Tarefa 1"));
            Assert.That(responseData, Does.Contain("Tarefa 3"));
            Assert.That(responseData, Does.Not.Contain("Tarefa 8"));
            Assert.That(responseData, Does.Not.Contain("Tarefa 9"));
            Assert.That(responseData, Does.Not.Contain("Tarefa 10"));
        }

        public async Task publicasTarefasAsync()
        {
            // Criar várias tarefas
            for (int i = 0; i < 5; i++)
            {
                var json = $@"{{
                    ""titulo"": ""Tarefa {i}"",
                    ""descricao"": ""Descrição da tarefa {i}"",
                    ""dataVencimento"": ""2026-05-20T00:00:00"",
                    ""status"": 0
                }}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("https://localhost:7089/api/Tarefas", content);
                responseLastBody = await response.Content.ReadAsStringAsync();

            }
        }
    }
}