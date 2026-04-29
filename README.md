ORGANIZAÇÃO FINANCEIRA COM C# E SQLITE

Olá, darei uma breve explicação sobre esse projeto em C#, o qual me ensinou muito sobre banco
de dados relacional e a manipulação das entidades e campos na sua forma fisíca.

O SQLite foi escolhido pelo seu comportamento local em cada dispositivo, salvando 
informações pessoais, que nesse rumo seriam as entradas e saidas do dinheiro de um usúario.

Toda a informação é muito bem documentada, com entradas tendo dados de data, valores, observações quando
forem convenientes e o nome pertencente à ela, além de uma chave primária que numera aquela linha. As saídas 
também possuem o mesmo, com nome, data, tipo (fixa, variável), valor e as observações.

Foi preciso capacidade lógica, pesquisa quando houve erros, laços condicionais aninhados e laços de repetição para constrole de fluxo
nas decisões, limguagem SQLite, que nesse programa permite a criação e conexão ao banco (CREATE TABLE IF NOT EXISTS), inserir dados (INSERT), remover dados (DELETE), consultar os dados existentes (SELECT), se não existir é retornado a saída "não há dados" e também finalizar o programa, mas isso pertence a capacidade dos loops.
