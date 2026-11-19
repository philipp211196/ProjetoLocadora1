<div align="center">

# 🚗 Sistema de Locadora de Veículos

### Gerenciamento de clientes, veículos e locações com C# e MySQL

![C#](https://img.shields.io/badge/C%23-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![.NET 8](https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows_Forms-0078D4?style=for-the-badge&logo=windows&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)

**Projeto acadêmico | Aplicação desktop | Em desenvolvimento**

</div>

---

## 📌 Sobre o projeto

Sistema desktop de estudos voltado ao gerenciamento de uma locadora de veículos. A proposta é centralizar operações de cadastro, consulta e controle de locações em uma interface Windows Forms, utilizando MySQL para persistência dos registros.

O projeto foi desenvolvido como exercício de programação em C#, integração com banco de dados e organização de aplicações desktop.

## 🧩 Funcionalidades previstas na documentação do projeto

| Módulo | Recursos descritos |
| --- | --- |
| **Clientes** | Cadastro e listagem de clientes |
| **Veículos** | Cadastro e listagem da frota |
| **Locações** | Registro de locações e cálculo do valor total |
| **Disponibilidade** | Verificação básica de conflito entre datas |
| **Cancelamentos** | Cancelamento da locação selecionada |
| **Persistência** | Armazenamento de dados no MySQL |

> Os recursos acima constam do arquivo [LEIA-ME.txt](LEIA-ME.txt). **A versão atualmente publicada no GitHub ainda não contém todos os arquivos necessários para verificar a implementação ou executar o sistema integralmente.**

## 🏗️ Arquitetura do projeto

```text
Aplicação desktop (C# / Windows Forms)
               |
               v
   Formulários e lógica de negócio
               |
               v
    Acesso a dados (MySqlConnector)
               |
               v
          Banco MySQL
```

**Principais tecnologias**

- **C#** — linguagem de programação.
- **.NET 8** — plataforma da aplicação.
- **Windows Forms** — interface gráfica desktop.
- **MySqlConnector 2.4.0** — biblioteca de acesso ao MySQL.
- **Visual Studio** — desenvolvimento e execução no Windows.

## 📁 Estrutura e situação do código publicado

```text
ProjetoLocadora1/
├── Program.cs
├── ProjetoLocadora1.csproj
├── ProjetoLocadora1.slnx
├── LEIA-ME.txt
├── README.md
└── parte2
```

O ponto de entrada `Program.cs` referencia `ProjetoLocadora1.Data`, `ProjetoLocadora1.Forms`, `BancoDados.InicializarBanco()` e `FormPrincipal`.

**Pendência identificada:** as pastas `Data/` e `Forms/`, além dos formulários e seus arquivos de Designer mencionados na configuração do projeto, não constam da árvore da branch `main` consultada. É necessário publicar os fontes faltantes antes de considerar a instalação reproduzível.

## ⚙️ Requisitos e configuração

Para executar a versão completa, depois de publicar os arquivos necessários:

1. Utilize Windows com Visual Studio 2022 (carga de trabalho **Desenvolvimento para desktop com .NET**).
2. Instale o **.NET 8 SDK** e configure um servidor **MySQL**.
3. Abra `ProjetoLocadora1.csproj` ou a solução no Visual Studio.
4. Configure as variáveis de ambiente de conexão, quando necessário:

| Variável | Finalidade |
| --- | --- |
| `LOCADORA_MYSQL_SERVER` | Servidor MySQL |
| `LOCADORA_MYSQL_PORT` | Porta de conexão |
| `LOCADORA_MYSQL_DATABASE` | Nome do banco |
| `LOCADORA_MYSQL_USER` | Usuário do banco |
| `LOCADORA_MYSQL_PASSWORD` | Senha do banco |

5. Consulte o [guia de configuração completo](LEIA-ME.txt) e execute o projeto com **F5** após confirmar que todas as dependências estão presentes.

> **Segurança:** nunca publique senhas reais no GitHub. Use credenciais locais ou variáveis de ambiente e, para aplicações reais, um usuário MySQL com permissões restritas.

## 📷 Demonstração

Capturas de tela reais serão incluídas após a publicação e validação dos formulários. Nenhuma imagem ilustrativa é apresentada como se fosse uma captura da aplicação.

## 🛣️ Próximas melhorias

- [ ] Publicar os arquivos de interface gráfica e acesso a dados que faltam.
- [ ] Compilar e testar a aplicação a partir de um clone limpo.
- [ ] Conferir as operações de cadastro, locação e cancelamento.
- [ ] Adicionar capturas de tela reais dos formulários.
- [ ] Melhorar as validações de entrada e o tratamento de erros.
- [ ] Preparar dados de exemplo e instruções de teste.

## 👨‍💻 Autor

**Philipp Magalhães** — Estudante de Análise e Desenvolvimento de Sistemas.

[![GitHub](https://img.shields.io/badge/GitHub-philipp211196-181717?style=flat-square&logo=github)](https://github.com/philipp211196)

---

<div align="center">

*Projeto de aprendizado com foco em aplicações desktop, C# e integração com banco de dados.*

</div>
