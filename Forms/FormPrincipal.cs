using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ProjetoLocadora1.Data;

namespace ProjetoLocadora1.Forms;

public sealed class FormPrincipal : Form
{
    private readonly Color fundo = Color.FromArgb(15, 23, 42);
    private readonly Color painel = Color.FromArgb(30, 41, 59);
    private readonly Color destaque = Color.FromArgb(34, 211, 238);
    private readonly Color texto = Color.FromArgb(241, 245, 249);

    private readonly DataGridView gridClientes = NovoGrid();
    private readonly DataGridView gridVeiculos = NovoGrid();
    private readonly DataGridView gridLocacoes = NovoGrid();

    private readonly TextBox txtNome = NovoCampo();
    private readonly TextBox txtCpf = NovoCampo();
    private readonly TextBox txtTelefone = NovoCampo();
    private readonly TextBox txtEmail = NovoCampo();
    private readonly TextBox txtMarca = NovoCampo();
    private readonly TextBox txtModelo = NovoCampo();
    private readonly TextBox txtPlaca = NovoCampo();
    private readonly NumericUpDown numAno = new() { Minimum = 1900, Maximum = DateTime.Today.Year + 1, Value = DateTime.Today.Year, Width = 110 };
    private readonly NumericUpDown numDiaria = new() { Minimum = 1, Maximum = 1000000, DecimalPlaces = 2, Width = 130 };
    private readonly ComboBox cbCliente = NovoCombo();
    private readonly ComboBox cbVeiculo = NovoCombo();
    private readonly DateTimePicker dtInicio = new() { Format = DateTimePickerFormat.Short, Width = 130 };
    private readonly DateTimePicker dtFim = new() { Format = DateTimePickerFormat.Short, Width = 130 };

    public FormPrincipal()
    {
        Text = "AutoLoc | Gestão de Locadora";
        Width = 1150;
        Height = 760;
        MinimumSize = new Size(930, 650);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = fundo;
        ForeColor = texto;
        Font = new Font("Segoe UI", 10f);

        var topo = new Panel { Dock = DockStyle.Top, Height = 92, BackColor = fundo, Padding = new Padding(24, 12, 24, 10) };
        var titulo = new Label
        {
            Text = "AUTOLOC  /  CONTROLE DE LOCAÇÕES",
            ForeColor = destaque,
            Font = new Font("Segoe UI Semibold", 20f),
            Dock = DockStyle.Top,
            Height = 42
        };
        var subtitulo = new Label
        {
            Text = "Clientes  •  Frota  •  Reservas",
            ForeColor = Color.FromArgb(148, 163, 184),
            Dock = DockStyle.Bottom,
            Height = 24
        };
        topo.Controls.Add(titulo);
        topo.Controls.Add(subtitulo);

        var abas = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10f), Padding = new Point(18, 8) };
        abas.TabPages.Add(CriarAbaClientes());
        abas.TabPages.Add(CriarAbaVeiculos());
        abas.TabPages.Add(CriarAbaLocacoes());

        Controls.Add(abas);
        Controls.Add(topo);

        try { AtualizarTudo(); }
        catch (Exception ex) { MostrarErro(ex); }
    }

    private TabPage CriarAbaClientes()
    {
        var aba = NovaAba("01  Clientes");
        var campos = NovoFormulario();
        AdicionarCampo(campos, "Nome completo", txtNome);
        AdicionarCampo(campos, "CPF (opcional)", txtCpf);
        AdicionarCampo(campos, "Telefone", txtTelefone);
        AdicionarCampo(campos, "E-mail", txtEmail);
        var botao = NovoBotao("Cadastrar cliente");
        botao.Click += (_, _) => Executar(() =>
        {
            BancoDados.CadastrarCliente(txtNome.Text, txtCpf.Text, txtTelefone.Text, txtEmail.Text);
            txtNome.Clear(); txtCpf.Clear(); txtTelefone.Clear(); txtEmail.Clear();
            AtualizarTudo();
        });
        campos.Controls.Add(botao);
        MontarAba(aba, campos, gridClientes);
        return aba;
    }

    private TabPage CriarAbaVeiculos()
    {
        var aba = NovaAba("02  Veículos");
        var campos = NovoFormulario();
        AdicionarCampo(campos, "Marca", txtMarca);
        AdicionarCampo(campos, "Modelo", txtModelo);
        AdicionarCampo(campos, "Placa", txtPlaca);
        AdicionarCampo(campos, "Ano", numAno);
        AdicionarCampo(campos, "Diária (R$)", numDiaria);
        var botao = NovoBotao("Cadastrar veículo");
        botao.Click += (_, _) => Executar(() =>
        {
            BancoDados.CadastrarVeiculo(txtMarca.Text, txtModelo.Text, txtPlaca.Text, (int)numAno.Value, numDiaria.Value);
            txtMarca.Clear(); txtModelo.Clear(); txtPlaca.Clear();
            AtualizarTudo();
        });
        campos.Controls.Add(botao);
        MontarAba(aba, campos, gridVeiculos);
        return aba;
    }

    private TabPage CriarAbaLocacoes()
    {
        var aba = NovaAba("03  Locações");
        var campos = NovoFormulario();
        AdicionarCampo(campos, "Cliente", cbCliente);
        AdicionarCampo(campos, "Veículo", cbVeiculo);
        AdicionarCampo(campos, "Data inicial", dtInicio);
        AdicionarCampo(campos, "Data final", dtFim);
        var cadastrar = NovoBotao("Registrar locação");
        cadastrar.Click += (_, _) => Executar(() =>
        {
            if (cbCliente.SelectedValue is not int cliente || cbVeiculo.SelectedValue is not int veiculo)
                throw new ArgumentException("Cadastre e selecione um cliente e um veículo.");
            BancoDados.CadastrarLocacao(cliente, veiculo, dtInicio.Value, dtFim.Value);
            AtualizarTudo();
        });
        campos.Controls.Add(cadastrar);

        var cancelar = NovoBotao("Cancelar locação selecionada", true);
        cancelar.Click += (_, _) => Executar(() =>
        {
            if (gridLocacoes.CurrentRow?.Cells["Id"].Value is not int id)
                throw new ArgumentException("Selecione uma locação na tabela.");
            if (MessageBox.Show(this, "Confirma o cancelamento desta locação?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            BancoDados.CancelarLocacao(id);
            AtualizarTudo();
        });
        campos.Controls.Add(cancelar);
        MontarAba(aba, campos, gridLocacoes);
        return aba;
    }

    private static TabPage NovaAba(string titulo) =>
        new(titulo) { BackColor = Color.FromArgb(15, 23, 42), ForeColor = Color.White, Padding = new Padding(16) };

    private static FlowLayoutPanel NovoFormulario() => new()
    {
        Dock = DockStyle.Top,
        Height = 150,
        Padding = new Padding(10),
        AutoScroll = true,
        WrapContents = true,
        BackColor = Color.FromArgb(30, 41, 59)
    };

    private static void AdicionarCampo(FlowLayoutPanel panel, string titulo, Control campo)
    {
        var caixa = new Panel { Width = Math.Max(campo.Width + 20, 150), Height = 82, Margin = new Padding(4) };
        caixa.Controls.Add(new Label
        {
            Text = titulo, Dock = DockStyle.Top, ForeColor = Color.White,
            Height = 28, Font = new Font("Segoe UI", 9)
        });
        campo.Top = 31;
        campo.Left = 0;
        caixa.Controls.Add(campo);
        panel.Controls.Add(caixa);
    }

    private static void MontarAba(TabPage aba, FlowLayoutPanel campos, DataGridView tabela)
    {
        var container = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };
        tabela.Dock = DockStyle.Fill;
        container.Controls.Add(tabela);
        container.Controls.Add(campos);
        aba.Controls.Add(container);
    }

    private static TextBox NovoCampo() => new() { Width = 170, BorderStyle = BorderStyle.FixedSingle };
    private static ComboBox NovoCombo() => new() { Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
    private Button NovoBotao(string titulo, bool alternativo = false)
    {
        var botao = new Button
        {
            Text = titulo, AutoSize = true, MinimumSize = new Size(155, 42),
            FlatStyle = FlatStyle.Flat, Margin = new Padding(8, 28, 4, 4),
            BackColor = alternativo ? Color.FromArgb(148, 163, 184) : destaque,
            ForeColor = fundo, Font = new Font("Segoe UI Semibold", 9f)
        };
        botao.FlatAppearance.BorderSize = 0;
        return botao;
    }

    private static DataGridView NovoGrid() => new()
    {
        ReadOnly = true,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        MultiSelect = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        RowHeadersVisible = false,
        BackgroundColor = Color.FromArgb(30, 41, 59),
        BorderStyle = BorderStyle.None,
        AutoGenerateColumns = true
    };

    private void AtualizarTudo()
    {
        DataTable clientes = BancoDados.ListarClientes();
        DataTable veiculos = BancoDados.ListarVeiculos();
        gridClientes.DataSource = clientes;
        gridVeiculos.DataSource = veiculos;
        gridLocacoes.DataSource = BancoDados.ListarLocacoes();

        cbCliente.DataSource = clientes.Copy();
        cbCliente.DisplayMember = "Nome";
        cbCliente.ValueMember = "Id";

        var opcoes = veiculos.Copy();
        opcoes.Columns.Add("Descricao", typeof(string), "Marca + ' ' + Modelo + ' / ' + Placa");
        cbVeiculo.DataSource = opcoes;
        cbVeiculo.DisplayMember = "Descricao";
        cbVeiculo.ValueMember = "Id";
    }

    private void Executar(Action acao)
    {
        try { acao(); }
        catch (Exception ex) { MostrarErro(ex); }
    }

    private void MostrarErro(Exception ex) =>
        MessageBox.Show(this, ex.Message, "Locadora — aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
