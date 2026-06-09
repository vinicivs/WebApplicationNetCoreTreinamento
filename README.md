# WebApplicationNetCoreTreinamento

Projeto Razor Pages em ASP.NET Core targeting .NET 10.

Descrição
- Repositório de exemplo/treinamento para aplicações web em ASP.NET Core usando Razor Pages.
- Projeto principal: WebApplicationAPINetCoreTreinamento (entrada em WebApplicationAPINetCoreTreinamento/Program.cs).

Requisitos
- .NET 10 SDK
- Visual Studio 2022/2026 (Community/Enterprise) ou Visual Studio Code
- pwsh (PowerShell) ou terminal de sua preferência

Execução (Visual Studio)
1. Abra a solução no Visual Studio.
2. Defina o projeto de inicialização: WebApplicationAPINetCoreTreinamento.
3. Execute com F5 (Debug) ou Ctrl+F5 (Sem depuração).

Execução (dotnet CLI)
1. Abra um terminal na raiz do repositório.
2. Restaurar pacotes: `dotnet restore`
3. Executar o projeto:
   `dotnet run --project ./WebApplicationAPINetCoreTreinamento`

Publicação
- Publicar em Release para pasta local:
  `dotnet publish -c Release -o ./publish --project ./WebApplicationAPINetCoreTreinamento`

Configurações
- Verifique `appsettings.json` e `Properties/launchSettings.json` no diretório do projeto para perfis e portas.
- Use variáveis de ambiente para segredos e configurações sensíveis.

Estrutura do repositório
- WebApplicationAPINetCoreTreinamento/   -> Código do projeto Razor Pages
- README.md                              -> Este arquivo

Testes
- Se existir um projeto de testes no repositório, execute:
  `dotnet test`

Contribuição
- Abra uma issue descrevendo o problema ou melhoria.
- Crie branches com nomes claros e envie Pull Requests apontando para a branch `master`.

Licença
- Adicione um arquivo LICENSE na raiz (ex.: MIT) conforme política do projeto.

Contato
- Para dúvidas ou ajustes, abra uma issue no repositório.
