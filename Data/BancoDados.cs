using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using MySqlConnector;

namespace ProjetoLocadora1.Data;

public static class BancoDados
{
    private static readonly string Servidor = Environment.GetEnvironmentVariable("LOCADORA_MYSQL_SERVER") ?? "127.0.0.1";
    private static readonly string Porta = Environment.GetEnvironmentVariable("LOCADORA_MYSQL_PORT") ?? "3306";
    private static readonly string NomeBanco = Environment.GetEnvironmentVariable("LOCADORA_MYSQL_DATABASE") ?? "locadora_veiculos";
    private static readonly string Usuario = Environment.GetEnvironmentVariable("LOCADORA_MYSQL_USER") ?? "root";
    private static readonly string Senha = Environment.GetEnvironmentVariable("LOCADORA_MYSQL_PASSWORD") ?? "";

    private static string ConnectionString(bool withDatabase = true)
    {
        var builder = new MySqlConnectionStringBuilder
        {
            Server = Servidor,
            UserID = Usuario,
            Password = Senha,
            Port = uint.TryParse(Porta, out var port) ? port : 3306,
            SslMode = MySqlSslMode.Preferred
        };
        if (withDatabase) builder.Database = NomeBanco;
        return builder.ConnectionString;
    }

    public static void InicializarBanco()
    {
        if (!Regex.IsMatch(NomeBanco, @"^[A-Za-z0-9_]+$"))
            throw new InvalidOperationException("Nome do banco inválido. Utilize apenas letras, números e underscore.");

        using (var server = new MySqlConnection(ConnectionString(false)))
        {
            server.Open();
            using var cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS `{NomeBanco}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;", server);
            cmd.ExecuteNonQuery();
        }

        using var db = AbrirConexao();
        string[] definitions =
        {
            @"CREATE TABLE IF NOT EXISTS Clientes (
                Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Nome VARCHAR(150) NOT NULL,
                CPF VARCHAR(14) NULL,
                Telefone VARCHAR(30) NULL,
                Email VARCHAR(150) NULL
            ) ENGINE=InnoDB;",
            @"CREATE TABLE IF NOT EXISTS Veiculos (
                Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Marca VARCHAR(80) NOT NULL,
                Modelo VARCHAR(100) NOT NULL,
                Placa VARCHAR(10) NOT NULL UNIQUE,
                Ano INT NOT NULL,
                Diaria DECIMAL(12,2) NOT NULL
            ) ENGINE=InnoDB;",
            @"CREATE TABLE IF NOT EXISTS Locacoes (
                Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                ClienteId INT NOT NULL,
                VeiculoId INT NOT NULL,
                Inicio DATE NOT NULL,
                Fim DATE NOT NULL,
                ValorTotal DECIMAL(12,2) NOT NULL,
                Status VARCHAR(20) NOT NULL DEFAULT 'Ativa',
                CONSTRAINT FK_Locacoes_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
                CONSTRAINT FK_Locacoes_Veiculos FOREIGN KEY (VeiculoId) REFERENCES Veiculos(Id),
                INDEX IX_Locacoes_VeiculoDatas (VeiculoId, Inicio, Fim)
            ) ENGINE=InnoDB;"
        };
        foreach (var sql in definitions)
        {
            using var cmd = new MySqlCommand(sql, db);
            cmd.ExecuteNonQuery();
        }
    }

    public static MySqlConnection AbrirConexao()
    {
        var connection = new MySqlConnection(ConnectionString());
        connection.Open();
        return connection;
    }

    public static DataTable ListarClientes()
    {
        using var db = AbrirConexao();
        using var cmd = new MySqlCommand("SELECT Id, Nome, CPF, Telefone, Email FROM Clientes ORDER BY Nome;", db);
        using var reader = cmd.ExecuteReader();
        var result = new DataTable();
        result.Load(reader);
        return result;
    }

    public static DataTable ListarVeiculos()
    {
        using var db = AbrirConexao();
        using var cmd = new MySqlCommand("SELECT Id, Marca, Modelo, Placa, Ano, Diaria FROM Veiculos ORDER BY Marca, Modelo;", db);
        using var reader = cmd.ExecuteReader();
        var result = new DataTable();
        result.Load(reader);
        return result;
    }

    public static DataTable ListarLocacoes()
    {
        using var db = AbrirConexao();
        using var cmd = new MySqlCommand(
            @"SELECT l.Id, c.Nome AS Cliente, CONCAT(v.Marca,' ',v.Modelo,' (',v.Placa,')') AS Veiculo,
                     l.Inicio, l.Fim, l.ValorTotal, l.Status
              FROM Locacoes l
              INNER JOIN Clientes c ON c.Id = l.ClienteId
              INNER JOIN Veiculos v ON v.Id = l.VeiculoId
              ORDER BY l.Id DESC;", db);
        using var reader = cmd.ExecuteReader();
        var result = new DataTable();
        result.Load(reader);
        return result;
    }

    public static void CadastrarCliente(string nome, string cpf, string telefone, string email)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Informe o nome do cliente.");
        using var db = AbrirConexao();
        using var cmd = new MySqlCommand(
            "INSERT INTO Clientes (Nome, CPF, Telefone, Email) VALUES (@nome, @cpf, @telefone, @email);", db);
        cmd.Parameters.AddWithValue("@nome", nome.Trim());
        cmd.Parameters.AddWithValue("@cpf", string.IsNullOrWhiteSpace(cpf) ? DBNull.Value : cpf.Trim());
        cmd.Parameters.AddWithValue("@telefone", string.IsNullOrWhiteSpace(telefone) ? DBNull.Value : telefone.Trim());
        cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(email) ? DBNull.Value : email.Trim());
        cmd.ExecuteNonQuery();
    }

    public static void CadastrarVeiculo(string marca, string modelo, string placa, int ano, decimal diaria)
    {
        if (string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(placa))
            throw new ArgumentException("Informe marca, modelo e placa.");
        if (ano < 1900 || ano > DateTime.Today.Year + 1) throw new ArgumentException("Ano inválido.");
        if (diaria <= 0) throw new ArgumentException("A diária precisa ser maior que zero.");
        using var db = AbrirConexao();
        using var cmd = new MySqlCommand(
            "INSERT INTO Veiculos (Marca, Modelo, Placa, Ano, Diaria) VALUES (@marca, @modelo, @placa, @ano, @diaria);", db);
        cmd.Parameters.AddWithValue("@marca", marca.Trim());
        cmd.Parameters.AddWithValue("@modelo", modelo.Trim());
        cmd.Parameters.AddWithValue("@placa", placa.Trim().ToUpperInvariant());
        cmd.Parameters.AddWithValue("@ano", ano);
        cmd.Parameters.AddWithValue("@diaria", diaria);
        cmd.ExecuteNonQuery();
    }

    public static void CadastrarLocacao(int clienteId, int veiculoId, DateTime inicio, DateTime fim)
    {
        inicio = inicio.Date;
        fim = fim.Date;
        if (clienteId <= 0 || veiculoId <= 0) throw new ArgumentException("Selecione um cliente e um veículo.");
        if (fim < inicio) throw new ArgumentException("A data final não pode ser anterior à inicial.");

        using var db = AbrirConexao();
        using var transaction = db.BeginTransaction(System.Data.IsolationLevel.Serializable);

        // O bloqueio da linha do veículo serializa novas reservas para a mesma unidade.
        using var lockVehicle = new MySqlCommand("SELECT Diaria FROM Veiculos WHERE Id = @id FOR UPDATE;", db, transaction);
        lockVehicle.Parameters.AddWithValue("@id", veiculoId);
        var rawDiaria = lockVehicle.ExecuteScalar();
        if (rawDiaria is null) throw new InvalidOperationException("Veículo não encontrado.");
        decimal diaria = Convert.ToDecimal(rawDiaria, CultureInfo.InvariantCulture);

        using var conflict = new MySqlCommand(
            @"SELECT COUNT(*) FROM Locacoes
              WHERE VeiculoId = @veiculo AND Status = 'Ativa'
                AND Inicio <= @fim AND Fim >= @inicio;", db, transaction);
        conflict.Parameters.AddWithValue("@veiculo", veiculoId);
        conflict.Parameters.AddWithValue("@inicio", inicio);
        conflict.Parameters.AddWithValue("@fim", fim);
        if (Convert.ToInt64(conflict.ExecuteScalar()) > 0)
            throw new InvalidOperationException("Já existe uma locação ativa para esse veículo nas datas informadas.");

        var dias = (fim - inicio).Days + 1;
        using var insert = new MySqlCommand(
            @"INSERT INTO Locacoes (ClienteId, VeiculoId, Inicio, Fim, ValorTotal, Status)
              VALUES (@cliente, @veiculo, @inicio, @fim, @valor, 'Ativa');", db, transaction);
        insert.Parameters.AddWithValue("@cliente", clienteId);
        insert.Parameters.AddWithValue("@veiculo", veiculoId);
        insert.Parameters.AddWithValue("@inicio", inicio);
        insert.Parameters.AddWithValue("@fim", fim);
        insert.Parameters.AddWithValue("@valor", diaria * dias);
        insert.ExecuteNonQuery();
        transaction.Commit();
    }

    public static void CancelarLocacao(int id)
    {
        using var db = AbrirConexao();
        using var cmd = new MySqlCommand(
            "UPDATE Locacoes SET Status = 'Cancelada' WHERE Id = @id AND Status = 'Ativa';", db);
        cmd.Parameters.AddWithValue("@id", id);
        if (cmd.ExecuteNonQuery() == 0)
            throw new InvalidOperationException("Locação não encontrada ou já cancelada.");
    }
}
