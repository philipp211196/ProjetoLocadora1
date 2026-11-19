<div align="center">

# 🚗 AutoLoc | Sistema de Locadora de Veículos

**Gestão desktop de clientes, frota e locações em C#**

![C#](https://img.shields.io/badge/C%23-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![.NET 8](https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows_Forms-0078D4?style=for-the-badge&logo=windows&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)

**Projeto acadêmico reconstruído — testes locais pendentes**

</div>

## 💡 Sobre

Aplicação desktop desenvolvida para estudar programação orientada a eventos, formulários Windows e persistência de dados com SQL. A interface tem abas para clientes, veículos e locações, em uma paleta escura.

## ✅ Funcionalidades implementadas no código

| Área | Recurso |
| --- | --- |
| Clientes | Cadastro com nome, CPF, telefone e e-mail; listagem |
| Veículos | Cadastro de marca, modelo, placa, ano e valor da diária; listagem |
| Locações | Seleção de cliente e veículo, período e cálculo do valor total |
| Disponibilidade | Bloqueio de registro quando há outra locação ativa com datas sobrepostas |
| Cancelamento | Mudança de status da locação selecionada para cancelada |
| Banco | Criação do banco e tabelas ausentes na inicialização, sem apagar registros existentes |

**Regra didática de preço:** valor total = diária × quantidade de dias corridos, contando a data inicial e a final. Tarifas, devolução por horário e pagamentos não estão implementados.

## 🧰 Tecnologias

- **C# e .NET 8** com Windows Forms;
- **MySQL** com `MySqlConnector` 2.4.0;
- Comandos SQL parametrizados para inserir e consultar dados;
- Transação e bloqueio de linha do veículo para evitar conflitos em novos registros simultâneos.

## 🗂️ Estrutura

```text
ProjetoLocadora1/
├── Data/
│   └── BancoDados.cs         # conexão e operações SQL
├── Forms/
│   └── FormPrincipal.cs      # interface criada por código
├── Program.cs                # entrada da aplicação
├── ProjetoLocadora1.csproj   # projeto .NET 8 Windows
├── ProjetoLocadora1.slnx
├── LEIA-ME.txt               # instalação e configuração
└── README.md
```

**Nota:** a interface reconstruída usa controles criados diretamente em C#, sem arquivos `.Designer.cs`. As entradas legadas `Compile Update` do `.csproj` ainda poderão ser limpas futuramente, mas não são necessárias para os controles desta versão.

## ▶️ Executar no Windows

1. Instale o Visual Studio 2022 com suporte a **Desenvolvimento para desktop com .NET** e o **SDK .NET 8**.
2. Instale e inicie o **MySQL Server**.
3. Clone ou baixe o repositório e abra `ProjetoLocadora1.csproj`.
4. Configure as credenciais do banco por variáveis de ambiente (leia [LEIA-ME.txt](LEIA-ME.txt)).
5. Restaure os pacotes NuGet e execute usando **F5**.

Variáveis utilizadas:

| Variável | Valor padrão |
| --- | --- |
| `LOCADORA_MYSQL_SERVER` | `127.0.0.1` |
| `LOCADORA_MYSQL_PORT` | `3306` |
| `LOCADORA_MYSQL_DATABASE` | `locadora_veiculos` |
| `LOCADORA_MYSQL_USER` | `root` (apenas para ambiente local de estudo) |
| `LOCADORA_MYSQL_PASSWORD` | vazia; informe sua senha local |

> O usuário do MySQL precisa de permissão para criar o banco na primeira execução ou de um banco já existente. Evite usar `root` fora do ambiente de estudo. **Nunca publique credenciais reais.**

## 🖼️ Interface

A versão reconstruída utiliza três abas: **Clientes**, **Veículos** e **Locações**. Capturas reais serão inseridas após a execução e validação no Windows; não foram utilizadas imagens fictícias.

## 🔎 Status e limitações

- [x] Reconstruir arquivos `Data/BancoDados.cs` e `Forms/FormPrincipal.cs`.
- [x] Implementar cadastro e listagem de clientes e veículos.
- [x] Implementar locações com verificação de conflito e cancelamento.
- [ ] Compilar e executar em Windows com .NET 8 e MySQL.
- [ ] Validar fluxos de ponta a ponta com dados de teste.
- [ ] Adicionar imagens reais das telas.
- [ ] Implementar edição, exclusão controlada, pesquisa e validações mais completas.
- [ ] Criar testes automatizados.

**Atenção:** os arquivos foram publicados e revisados estaticamente, mas a compilação e a execução real não foram verificadas neste ambiente. Este projeto não foi preparado para produção.

---

Desenvolvido como projeto de aprendizado por [**Philipp Magalhães**](https://github.com/philipp211196).
