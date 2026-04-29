using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;
using Infraestrutura;
using EstruturaEntradas;
using EstrututraSaidas;
using ObservacoesSQL;
using LerSaida;
using LerEntrada;
using EntradaMET;
using SaidaMET;
using delGasto;
//using delEntrada;


public class Conexao
{
    //private string? pasta;
    private string? caminhoGastos;
    private string? caminhoEntradas;
    public (string localGastos, string localEntradas) Endereco() // visualização | (o que retorna) | nome do metodo
    {
        //pasta = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        caminhoGastos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Gastos.db");
        caminhoEntradas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Entradas.db");
        string localGastos = $"Data Source = {caminhoGastos}";
        string localEntradas = $"Data Source = {caminhoEntradas}";
        return (localGastos, localEntradas);
    }
}


namespace Infraestrutura
{
    class BancoEntradas
    {
        public void conexaoEntradas()
        {
            var ent = new Conexao();
            var (gastos, entradas) = ent.Endereco();
            try
            {
                using (SqliteConnection conexao = new SqliteConnection(entradas))
                {
                    conexao.Open();
                    string sql = @"CREATE TABLE IF NOT EXISTS Entradas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    Data TEXT NOT NULL,
                    Entrada REAL NOT NULL, 
                    Observacoes TEXT NOT NULL
                    )";
                    using (SqliteCommand cmd = new SqliteCommand(sql, conexao))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    //Console.WriteLine("Banco de entradas abertos.");
                }
            }
            catch (Exception erro)
            {
                Console.WriteLine($"Opsss, um erro foi idenficado: {erro}");
            }
        }
    }
}

namespace Infraestrutura
{
    class BancoGastos
    {
        public void conexaoGastos()
        {
            Conexao connect = new Conexao();
            var (gastos, entradas) = connect.Endereco();
            try
            {
                using (SqliteConnection conexao = new SqliteConnection(gastos)) // firma conexao com o endereçp
                {
                    conexao.Open();
                    string sql = @"CREATE TABLE IF NOT EXISTS Gastos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    Data TEXT NOT NULL,
                    Tipo TEXT NOT NULL,
                    Valor REAL NOT NULL,
                    Observacoes TEXT NOT NULL
                    )"; // cria as colunas e a tabela
                    using (SqliteCommand comando = new SqliteCommand(sql, conexao)) // cria um objeto comando ao endereço SQL
                    {
                        comando.ExecuteNonQuery(); // executa command
                    }
                    ;
                    //Console.WriteLine("Banco de gastos abertos.");
                }

            }
            catch (Exception erro)
            {
                Console.WriteLine("Opss, algo deu erro..." + erro.Message);
                //return false;
            }
        }
    }
}

namespace EstruturaEntradas
{
    class AddBancoEntradas
    {

        public void AddEntradas(string? nome, double? entrada, System.DateTime data, string? obs)
        {
            Conexao connect = new Conexao();
            var (gastos, entradas) = connect.Endereco();

            try
            {
                using (SqliteConnection conexao = new SqliteConnection(entradas))
                {
                    conexao.Open();
                    string sql = "INSERT INTO Entradas (Nome, Data, Entrada, Observacoes) VALUES (@nome, @data, @entrada, @obs)"; // insere dados na coluna tal, tal e define e os valores que serao inserios
                    using (SqliteCommand cmd = new SqliteCommand(sql, conexao)) //executa, mas diz que os valores SQL serao correspondentes aos parametros quwe recebe, valores dessa variavel
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@data", data);
                        cmd.Parameters.AddWithValue("@entrada", entrada);
                        cmd.Parameters.AddWithValue("@obs", obs);

                        cmd.ExecuteNonQuery();
                        //Console.WriteLine(strConexao);

                    }
                    Console.WriteLine("Dados adicionados com sucesso!");
                }
            }
            catch (Exception erro)
            {
                Console.WriteLine($"Opsss, foi indentificado um '{erro}'");
            }
        }
    }
}

namespace EstrututraSaidas
{
    class AddBancoSaida
    {
        public void AddSaida(string? nome, double? saida, string? tipo, System.DateTime data, string? obs)
        {
            var connect = new Conexao();
            var (gastos, entradas) = connect.Endereco();
            try
            {
                using (SqliteConnection conexao = new SqliteConnection(gastos))
                {
                    conexao.Open();
                    string sql = @"INSERT INTO Gastos (Nome, Data, Tipo, Valor, Observacoes) VALUES (@nome, @data, @tipo, @valor, @obs)";
                    using (SqliteCommand cmd = new SqliteCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@data", data);
                        cmd.Parameters.AddWithValue("@tipo", tipo);
                        cmd.Parameters.AddWithValue("@valor", saida);
                        cmd.Parameters.AddWithValue("@obs", obs);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Dados adicionados com sucesso!");
            }
            catch (Exception erro)
            {
                Console.WriteLine($"Opsss, foi identificado o erro: {erro}");
            }
        }
    }
}



namespace LerSaida
{
    class LerDadosGastos
    {
        public (int storageMax, int storageMin) lerGastos()
        {
            var conexao = new Conexao();
            var (gastos, entradas) = conexao.Endereco();
            using (SqliteConnection conn = new SqliteConnection(gastos))
            {
                conn.Open();
                string sql = "SELECT * FROM Gastos";
                using var cmd = new SqliteCommand(sql, conn);
                using var reading = cmd.ExecuteReader();
                int storageMax = 0;
                int storageMin = 0;
                int cont = 0;
                if (reading.Read())
                {
                    do
                    {
                        int id = reading.GetInt32(0);
                        string nome = reading.GetString(1);
                        string data = reading.GetString(2);
                        string tipo = reading.GetString(3);
                        string valor = reading.GetString(4);
                        string obs = reading.GetString(5);

                        string tamId = id.ToString();
                        string tamTodos = tamId + nome + data + tipo + valor + obs;
                        string linhas = new string('-', tamTodos.Length + 20);

                        Console.WriteLine(linhas);

                        Console.WriteLine($"| {id} - {nome} - {data} - {tipo} - {valor} - {obs} |");

                        Console.WriteLine(linhas);

                        if (cont == 0)
                        {
                            cont++;
                            storageMin = id;
                        }

                        storageMax = id;
                    }
                    while (reading.Read());
                }
                else
                {
                    Console.WriteLine("Não há dados");
                }
                return (storageMax, storageMin);
            }
        }
    }
}


namespace LerEntrada
{
    class LerDadosEntrada
    {
        public (int storageMax, int storageMin) lerDadosEntrada()
        {
            var conn = new Conexao();
            var (gastos, entradas) = conn.Endereco();
            using (SqliteConnection conexao = new SqliteConnection(entradas))
            {
                conexao.Open();
                string sql = "SELECT * FROM Entradas";
                using var cmd = new SqliteCommand(sql, conexao);
                using var reading = cmd.ExecuteReader();
                int storageMax = 0;
                int storageMin = 0;
                int cont = 0;
                if (reading.Read())
                {
                    do
                    {
                        int id = reading.GetInt32(0);
                        string nome = reading.GetString(1);
                        string data = reading.GetString(2);
                        string entrada = reading.GetString(3);
                        string obs = reading.GetString(4);

                        string tamId = id.ToString();
                        string tamTodos = tamId + nome + data + entrada + obs;
                        string linhas = new string('-', tamTodos.Length + 16);

                        Console.WriteLine(linhas);

                        Console.WriteLine($"| {id} - {nome} - {data} - {entrada} - {obs} |");

                        Console.WriteLine(linhas);

                        if (cont == 0)
                        {
                            cont++;
                            storageMin = id;
                        }

                        storageMax = id;

                    }
                    while (reading.Read());

                }
                else
                {
                    Console.WriteLine("Não há dados");
                }
                Console.WriteLine($"{storageMax}, {storageMin}");
                return (storageMax, storageMin);
            }
        }
    }
}

namespace ObservacoesSQL
{
    class ObsValidacao
    {
        private string? observa;
        public string? Obs(string? obs)
        {

            if (obs == "s")
            {
                string obsTxt = " |Digite sua observação: | ";
                string embTxt = new string('-', obsTxt.Length);
                Console.WriteLine(embTxt);
                Console.Write(obsTxt);
                observa = Console.ReadLine();
                Console.WriteLine(embTxt);


            }
            else if (obs == "n")
            {
                observa = "Sem observações.";

            }
            return observa;
        }
    }
}

namespace EntradaMET
{
    class Ent
    {

        public void entrada()
        {
            var connect1 = new BancoEntradas();
            connect1.conexaoEntradas();
            var connectEntrada = new AddBancoEntradas();
            string msgName = "| Digite um nome para essa entrada: | ";
            string embName = new string('-', msgName.Length);
            Console.WriteLine(embName);
            Console.Write(msgName);
            string? nomeEntrada = Console.ReadLine();
            Console.WriteLine(embName);


            string msgData = "| Digite a data 'dd/MM/yyyy' | ";
            string embData = new string('-', msgData.Length);
            Console.WriteLine(embData);
            Console.Write(msgData);
            string? dataEntrada = Console.ReadLine();
            Console.WriteLine(embData);

            string msgValor = "| Digite o valor da entrada em número | ";
            string embValor = new string('-', msgValor.Length);
            Console.WriteLine(embValor);
            Console.Write(msgValor);
            string? entrada = Console.ReadLine();
            Console.WriteLine(embValor);

            if (!string.IsNullOrWhiteSpace(entrada) &&
                !string.IsNullOrWhiteSpace(dataEntrada) &&
                double.TryParse(entrada, out double valorEnt) &&
                DateTime.TryParse(dataEntrada, out DateTime data))
            {
                string msgObs = "| Deseja fazer alguma observação? (s/n) | ";
                string embObs = new string('-', msgObs.Length);
                Console.WriteLine(embObs);
                Console.Write(msgObs);
                string? obs = Console.ReadLine();
                Console.WriteLine(embObs);

                var obsql = new ObsValidacao();
                var obsDado = obsql.Obs(obs);
                connectEntrada.AddEntradas(nomeEntrada, valorEnt, data, obsDado);
            }
            else
            {
                Console.WriteLine("Dados inválidos foram digitados, tente novamente.");
            }
        }
    }
}

namespace SaidaMET
{
    class Sai
    {

        public void saida()
        {
            var connect2 = new BancoGastos();
            connect2.conexaoGastos();
            var connectSaida = new AddBancoSaida();

            string msgTipo = "| Deseja guardar como uma saída fixa ou variável? | ";
            string embTipo = new string('-', msgTipo.Length);
            Console.WriteLine(embTipo);
            Console.Write(msgTipo);
            string? tipo = Console.ReadLine();
            Console.WriteLine(embTipo);

            string msgNome = $"| Digite um nome para essa saída {tipo}: | ";
            string embNome = new string('-', msgNome.Length);
            Console.WriteLine(embNome);
            Console.Write(msgNome);
            string? nomeSaida = Console.ReadLine();
            Console.WriteLine(embNome);

            string msgValor = "| Digite o valor da saida em número | ";
            string embValor = new string('-', msgValor.Length);
            Console.WriteLine(embValor);
            Console.Write(msgValor);
            string? saida = Console.ReadLine();
            Console.WriteLine(embValor);

            string msgData = "| Digite a data 'dd/MM/yyyy' | ";
            string embData = new string('-', msgData.Length);
            Console.WriteLine(embData);
            Console.Write(msgData);
            string? dataSaida = Console.ReadLine();
            Console.WriteLine(embData);

            if (!string.IsNullOrWhiteSpace(saida) &&
            !string.IsNullOrWhiteSpace(dataSaida) &&
            double.TryParse(saida, out double valorSai) &&
            DateTime.TryParse(dataSaida, out DateTime data))
            {
                string msgObs = "| Deseja fazer alguma observação? (s/n) | ";
                string embObs = new string('-', msgObs.Length);
                Console.WriteLine(embObs);
                Console.Write(msgObs);
                string? obs = Console.ReadLine();
                Console.WriteLine(embObs);

                var obsql = new ObsValidacao();
                var obsDado = obsql.Obs(obs);
                connectSaida.AddSaida(nomeSaida, valorSai, tipo, data, obsDado);
            }
            else
            {
                string msgError = "| Dados inválidos foram digitados, tente novamente. | ";
                string embError = new string('-', msgError.Length);
                Console.WriteLine(embError);
                Console.WriteLine(msgError);
                Console.WriteLine(embError);

            }
        }
    }
}

namespace delGasto
{
    class DeletarGasto
    {
        public void deletarGasto(int id)
        {
            var conn = new Conexao();
            var (gastos, entradas) = conn.Endereco();
            using (SqliteConnection conexao = new SqliteConnection(gastos))
            {
                conexao.Open();
                string sql = "DELETE FROM Gastos WHERE Id = @id";
                using (var cmd = new SqliteCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    {
                        while (true)
                        {
                            Console.Write($"Você tem certeza que deseja excluir a linha que corresponde ao id {id}? ");
                            string? decision = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(decision) && decision.ToLower() == "sim")
                            {
                                int linhas = cmd.ExecuteNonQuery();
                                if (linhas != 0)
                                {
                                    Console.WriteLine($"Apagando dados do id {id}");
                                    break;
                                } else
                                {
                                    Console.WriteLine("Essa id náo existe, verifique e tente novamente.");
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(decision) && decision.ToLower() == "não")
                            {
                                Console.WriteLine($"Canecelando processo...");
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"Resposta inválida!");
                            }

                        }
                    
                    
                }
            }
        }
    }
}

namespace delEntrada
{
    class DeletarEntrada
    {
        public void deletarEntrada(int id)
        {
            var conn = new Conexao();
            var (gastos, entradas) = conn.Endereco();
            using (SqliteConnection conexao = new SqliteConnection(entradas))
            {
                conexao.Open();
                string sql = "DELETE FROM Entradas WHERE Id = @id";
                using (var cmd = new SqliteCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    

                    while (true)
                    {
                        Console.Write($"Você tem certeza que deseja excluir a linha que corresponde ao id {id}? ");
                        string? decision = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(decision) && decision.ToLower() == "sim")
                        {
                                int linhas = cmd.ExecuteNonQuery();
                                if (linhas != 0)
                                {
                                    Console.WriteLine($"Apagando dados do id {id}");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Essa id náo existe, verifique e tente novamente.");
                                    continue;
                                }
                            }
                        else if (!string.IsNullOrWhiteSpace(decision) && decision.ToLower() == "não")
                        {
                            Console.WriteLine($"Canecelando processo...");
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Resposta inválida!");
                        }

                    }
                }
            }
        }
    }
}


namespace One
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Controle suas finanças!");

            int cont = 0;
            while (true)
            {
                cont++;

                string msgDeci = "O que você deseja fazer? (Entrada/Saída/Consulta/Sair) ";
                Console.Write(msgDeci);
                string? decision = Console.ReadLine();

                if (decision?.ToLower() == "entrada")
                {
                    var metodo = new Ent();
                    metodo.entrada();

                }
                else if (decision?.ToLower() == "saida")
                {
                    var connSai = new Sai();
                    connSai.saida();
                }
                else if (decision!.ToLower() == "consulta") 
                {
                    Console.Write("Consultar que tipo de dado? (entradas/saidas) ");
                    string? decisao = Console.ReadLine();
                    if (decisao != null && decisao.ToLower() == "entradas")
                    {
                        var consulta = new LerDadosEntrada();
                        consulta.lerDadosEntrada();

                        string msgDec = "O que mais deseja fazer? (Excluir/Adicionar/Sair) ";
                        Console.Write(msgDec);
                        string? decisaoEnt = Console.ReadLine();


                        if (decisaoEnt != null && decisaoEnt == "excluir")
                        {
                            Console.Write("Qual linha você deseja remover? ");
                            string? idStr = Console.ReadLine();
                            int.TryParse(idStr, out int id);

                            var (idMax, idMin) = consulta.lerDadosEntrada();

                                var del = new delEntrada.DeletarEntrada();
                                del.deletarEntrada(id);
                            

                        }
                        else if (decisaoEnt != null && decisaoEnt == "adicionar")
                        {
                            var metodo = new Ent();
                            metodo.entrada();
                        }
                        else if (decisaoEnt != null && decisaoEnt == "sair")
                        {
                            string fch = "| Fechando... |";
                            string embFch = new string('-', fch.Length);
                            Console.WriteLine(embFch);
                            Console.WriteLine(fch);
                            Console.WriteLine(embFch);
                            break;
                        }
                    }
                    else if (decisao != null && decisao.ToLower() == "saidas")
                    {
                        var consulta = new LerDadosGastos();
                        consulta.lerGastos();

                        string msgDec = "O que mais deseja fazer? (Excluir/Adicionar/Sair) ";
                        Console.Write(msgDec);
                        string? decisaoSai = Console.ReadLine();

                        if (decisaoSai != null && decisaoSai == "excluir")
                        {
                            Console.Write("Qual linha você deseja remover? ");
                            string? idStr = Console.ReadLine();
                            int.TryParse(idStr, out int id);

                            var (idMax, idMin) = consulta.lerGastos();

                                var del = new DeletarGasto();
                                del.deletarGasto(id);
                            

                        }
                        else if (decisaoSai != null && decisaoSai == "adicionar")
                        {
                            var connSai = new Sai();
                            connSai.saida();
                        }
                        else if (decisaoSai != null && decisaoSai == "sair")
                        {
                            string msgFecha = "| Fechando... |";
                            string embFecha = new string('-', msgFecha.Length);
                            Console.WriteLine(embFecha);
                            Console.WriteLine(msgFecha);
                            Console.WriteLine(embFecha);
                            break;
                        }
                    }

                }
                else if (decision == "sair")
                {
                    Console.Write("Dejesa encerra o programa? ");
                    string pergunta = Console.ReadLine()!;

                    if (pergunta.ToLower() == "sim")
                    {
                        string fch = "| Fechando... |";
                        string embFch = new string('-', fch.Length);
                        Console.WriteLine(embFch);
                        Console.WriteLine(fch);
                        Console.WriteLine(embFch);
                        break;
                    }
                    else if (pergunta.ToLower() == "não")
                    {
                        string ctn = "| Continuando |";
                        string embCtn = new string('-', ctn.Length);
                        Console.WriteLine(embCtn);
                        Console.WriteLine(ctn);
                        Console.WriteLine(embCtn);
                        continue;
                    }
                }

                else
                {
                    Console.WriteLine("Opção inválida, tente novamente!");
                }
                }
            }
        }
    }
}


 
//formatar os textos
