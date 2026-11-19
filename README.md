# Sistema de Locadora de Veículos — Windows Forms + MySQL

> **Categoria:** Projeto de aplicação · **Tecnologias:** C#, .NET 8, Windows Forms e MySQL

Aplicação de demonstração para cadastro de clientes e veículos e gerenciamento de locações, com persistência em MySQL.

## Funcionalidades documentadas

- Cadastro e listagem de clientes.
- Cadastro e listagem de veículos.
- Registro de locações e cálculo do valor total.
- Verificação básica de conflito de datas.
- Cancelamento de locações e persistência no banco.

## Executar

Requisitos: **Windows**, **Visual Studio 2022** com desenvolvimento desktop .NET, **.NET 8 SDK** e **MySQL Server**.

1. Leia **[`LEIA-ME.txt`](LEIA-ME.txt)**, que contém as instruções completas.
2. Abra `ProjetoLocadora1.csproj` no Visual Studio.
3. Configure a conexão MySQL por variáveis de ambiente (especialmente `LOCADORA_MYSQL_PASSWORD`).
4. Restaure os pacotes e execute com **F5**.

> **Segurança:** nunca publique a senha do banco. O uso de `root` é apenas uma configuração didática; prefira um usuário restrito em ambientes reais.

## Status

Projeto demonstrativo acadêmico. Ainda requer validações e melhorias antes de qualquer uso em produção.

## Autor

[Philipp Magalhães](https://github.com/philipp211196)
