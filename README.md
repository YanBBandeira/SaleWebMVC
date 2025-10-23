# SaleWebMVC

Um projeto para aprender e praticar ASP.NET MVC, Entity Framework, Razor, e a construção de dashboards interativos com Chart.js.

#### 🎯 Objetivo do projeto

O SaleWebMVC é uma aplicação web criada para entender como funciona o padrão MVC (Model–View–Controller) dentro do ASP.NET Core.
Durante o desenvolvimento, aprendemos como:

Criar modelos (Models) que representam entidades do banco de dados (como Vendedores, Departamentos e Vendas);

Controlar a navegação e lógica de negócio com Controllers;

Exibir informações dinâmicas com Views Razor;

Usar o Entity Framework Core para acessar e consultar o banco de dados;

Montar gráficos interativos com Chart.js para visualizar dados em tempo real;

Aplicar filtros e consultas dinâmicas (por data, departamento, vendedor, etc.).

Esse projeto é um ótimo ponto de partida para quem está começando no mundo do desenvolvimento web com C#.

#### Tecnologias utilizadas

O projeto foi desenvolvido com um conjunto de tecnologias que se integram muito bem dentro do ecossistema .NET:

Camada	Tecnologia	Função
Model (M)	C# + Entity Framework Core	Representa e manipula os dados, mapeando as tabelas do banco.
View (V)	Razor + HTML + CSS + Bootstrap	Cria as páginas e a interface visual.
Controller (C)	C# + ASP.NET MVC	Faz a ponte entre o usuário e a aplicação, controlando fluxos e regras.
Banco de Dados	SQL Server (ou SQLite)	Armazena vendedores, departamentos e registros de vendas.
Front-end Dinâmico	JavaScript + Chart.js + DataTables	Cria dashboards, gráficos e tabelas interativas.
#### 🧩 Estrutura do Projeto

A estrutura segue o padrão MVC clássico do ASP.NET Core:
```
SaleWebMVC/
├── Controllers/
│   ├── SellersController.cs
│   ├── DepartmentsController.cs
│   ├── SalesRecordsController.cs
│   └── StatsController.cs
│
├── Models/
│   ├── Seller.cs
│   ├── Department.cs
│   ├── SalesRecord.cs
│   ├── ViewModels/
│   │   └── StatsViewModel.cs
│
├── Services/
│   ├── StatsService.cs
│   ├── SalesRecordService.cs
│   └── SellerService.cs
│
├── Views/
│   ├── Sellers/
│   ├── Departments/
│   ├── SalesRecords/
│   └── Stats/
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
│
└── appsettings.json
```
#### O fluxo:

O usuário faz uma requisição (por exemplo: acessar “/Dashboard”).

O Controller recebe o pedido e consulta o Service, que vai buscar os dados no banco.

O Service usa o Entity Framework Core para consultar e transformar os dados.

O Controller envia essas informações para uma ViewModel.

A View mostra tudo para o usuário com HTML, Razor e Chart.js.

#### 📊 O Dashboard "*Stats*"

O módulo de dashboard é onde o projeto ganha vida.
Nele você encontra:

Gráfico de vendas por mês (linha)

Gráfico de vendas por vendedor (barras)

Gráfico de vendas por departamento (em desenvolvimento)

Filtros para selecionar período, vendedor e departamento

Tudo isso renderizado com Chart.js, que é simples, leve e totalmente personalizável.

Além disso, é possível integrar AJAX para atualizar os gráficos sem precisar recarregar a página — um passo importante para quem quer aprender interatividade no front-end com ASP.NET.
