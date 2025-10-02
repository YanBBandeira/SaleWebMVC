
Neste projeto vamos por em prática todos os tópicos vistos ao longo do curso de C#. O objetivo geral desse projeto é:

- Introduzir o aluno ao desenvolvimento de aplicações web com ASP.NET Core MVC 
- Permitir que o aluno conheça os fundamentos e a utilização do framework, de modo que ele possa depois prosseguir estudando as especificidades que desejar

**Visão geral do ASP.NET Core MVC**

- É um framework para criação de aplicações web
- Criado pela Microsoft e comunidade
- Open source
- Roda tanto no .NET Framewrok quanto no .NET Core
- O framework trabalha com uma estrutura bem definida, incluindo:
	- Controllers
	- Views
	- Models
		- View Models

**Checklist da criação do projeto**

![[Pasted image 20250922154409.png]]
## Nivelamento: MVC para aplicaçãos web com template engine

![[Pasted image 20250922164137.png]]
#### **Web Services** 
Nas aplicações web services, temos a separação entre **front-end** e **back-end**.  O **back-end** é a parte que roda no servidor, responsável pela lógica de negócio, tratamento de dados, autenticação, persistência em banco de dados e comunicação com APIs externas. Já o **front-end** é a camada que roda no navegador do usuário, sendo responsável pela interface gráfica e pela interação direta com quem está utilizando o sistema.

O front-end normalmente é construído com **HTML, CSS e JavaScript**, que juntos permitem estruturar, estilizar e dar dinamismo às páginas.

Um conceito bastante comum nesse contexto é o de **SPA (Single Page Application)**. Esse tipo de aplicação carrega uma única página HTML principal e atualiza o conteúdo de forma dinâmica, sem a necessidade de recarregar a página inteira a cada interação do usuário. Isso melhora a experiência, tornando-a mais fluida e rápida.

Para o desenvolvimento de SPAs e de aplicações mais robustas, é muito comum o uso de **frameworks e bibliotecas JavaScript**, tais como:

- **React** (biblioteca focada em interfaces e componentes reutilizáveis),
    
- **Angular** (framework completo com muitas funcionalidades embutidas),
    
- **Vue.js** (framework progressivo, simples de adotar e bastante flexível).
    

Além disso, o front-end geralmente se comunica com o back-end por meio de **APIs REST** ou **GraphQL**, enviando e recebendo dados no formato **JSON**. Essa troca é essencial para que o front-end exiba informações atualizadas vindas do servidor.

Do lado do back-end, é comum encontrarmos tecnologias como **Node.js**, **Java (Spring Boot)**, **.NET**, **Python (Django, Flask, FastAPI)**, entre outras. Essas ferramentas permitem criar serviços robustos, escaláveis e seguros.

Outro ponto importante é que, em arquiteturas modernas, muitas aplicações web seguem princípios de **arquitetura em microsserviços** e podem ser hospedadas em **nuvem** (AWS, Azure, GCP), utilizando contêineres (**Docker**) e orquestração (**Kubernetes**).

**Backend, Model e Controladores**

O **backend** é geralmente estruturado em camadas. Uma dessas camadas é o **Model**, que representa a lógica de negócio e os dados da aplicação. Esse Model é implementado na mesma linguagem em que o backend foi construído (por exemplo, Java, Python, C#, Node.js etc.).

O **Model** conversa diretamente com os **controladores REST**, também escritos na mesma linguagem. Os controladores são responsáveis por receber requisições vindas do **front-end** e encaminhar as informações para o Model processar.

Esses controladores são preparados para responder a **requisições HTTP**, muitas vezes feitas de forma assíncrona pelo front-end através de **Ajax** (ou Fetch API, Axios, etc.), usando JavaScript.

As requisições podem trazer dados em diferentes formatos:

- **Parâmetros HTTP** (query string ou parâmetros de rota),
    
- **JSON** (o mais comum para troca de dados entre front-end e back-end),
    
- **FormData** (muito utilizado para upload de arquivos ou formulários complexos).
    

O fluxo funciona da seguinte forma:

1. O usuário realiza uma ação no front-end (por exemplo, enviar um formulário).
    
2. O JavaScript dispara uma requisição Ajax para o backend.
    
3. O **controlador** recebe a requisição, interpreta os dados (JSON, parâmetros ou form-data) e os converte em **objetos da linguagem** do backend.
    
4. Esses objetos são enviados ao **Model**, que processa as regras de negócio e pode acessar bancos de dados ou outros serviços.
    
5. Após o processamento, o Model retorna os resultados ao controlador.
    
6. O controlador, por sua vez, converte esse resultado em uma **resposta HTTP** (geralmente em formato **JSON** ou **texto**) e envia de volta ao front-end.
    
7. O front-end, por meio do JavaScript, interpreta a resposta e atualiza dinamicamente a interface do usuário.
    

==**O backend não é responsável por criar as páginas da aplicação**==

#### **Template Engine**

Além das aplicações que usam **APIs REST** + **front-end SPA**, existe outro modelo bastante utilizado: o de aplicações que geram páginas diretamente no servidor, utilizando um **Template Engine**.

Um **Template Engine** é um mecanismo que permite criar páginas HTML dinâmicas no servidor, combinando **HTML estático** com **dados vindos do backend**. Dessa forma, o servidor constrói a página pronta e a envia ao navegador do usuário.

 **Funcionamento**

1. O cliente (navegador) faz uma requisição HTTP (por exemplo, acessando uma rota `/produtos`).
    
2. O controlador no backend recebe essa requisição.
    
3. O controlador busca os dados necessários no **Model** (exemplo: lista de produtos do banco de dados).
    
4. Esses dados são passados para o **Template Engine**.
    
5. O Template Engine insere os dados dentro de um **template HTML**, preenchendo variáveis, laços de repetição, condicionais etc.
    
6. O resultado final é um **HTML renderizado** (pronto para ser exibido), que é devolvido ao navegador como resposta.
    

 **Exemplo de fluxo**

- O controlador recebe uma requisição GET em `/produtos`.
    
- O Model retorna uma lista de produtos.
    
- O controlador envia essa lista para o Template Engine.
    
- O Template Engine pega o HTML base e substitui algo como:
    

```html
<ul>
  {% for produto in produtos %}
     <li>{{ produto.nome }} - {{ produto.preco }}</li>
  {% endfor %}
</ul>
```

- O resultado final é um HTML já pronto com todos os produtos listados.
    

---

 **Vantagens do uso de Template Engines**

- **Renderização no servidor**: o cliente já recebe a página completa, sem depender de muita lógica no JavaScript.
    
- **SEO amigável**: como o HTML já vem montado, os mecanismos de busca conseguem indexar o conteúdo com mais facilidade.
    
- **Menor complexidade no front-end**: em muitas aplicações simples ou médias, evita a necessidade de criar uma SPA.
    
- **Integração simples com backends tradicionais**: frameworks como Django, Spring MVC ou Express facilitam bastante esse fluxo.
    

---

 **Exemplos de Template Engines populares**

- **EJS, Pug (antigo Jade), Handlebars** → muito usados com Node.js/Express.
    
- **Jinja2** → utilizado em **Python/Django** e **Flask**.
    
- **Thymeleaf, JSP, FreeMarker** → comuns em aplicações **Java/Spring**.
    
- **Razor** → utilizado em **.NET**.
    

---

 **Comparação com SPAs (Single Page Applications)**

- **SPA**: o servidor geralmente retorna **dados (JSON)**, e o front-end (React, Angular, Vue) monta a interface dinamicamente no navegador.
    
- **Template Engine**: o servidor já retorna **HTML pronto**, com os dados aplicados, e o navegador só exibe.
    

Em alguns casos, as duas abordagens podem ser **combinadas**:

- O servidor renderiza o HTML inicial com um Template Engine.
    
- O JavaScript depois faz requisições Ajax adicionais para atualizar dados dinamicamente.
    

---

👉 ==**Esse modelo com Template Engine é muito usado em sistemas tradicionais, painéis administrativos, portais de conteúdo, blogs e e-commerces, pois facilita a construção de páginas dinâmicas sem depender exclusivamente de SPAs.**==

![[Pasted image 20250922164147.png]]
## Nivelamento: Entity Framework

Por muitos anos, uma grande dificuldade de se criar sistemas orientados a objetos foi a comunicação com o banco de dados relacional. Segundo *Marti Fowler* cerca de 30% do esforço de se fazer um sistema era realizar esse contato. Abaixo, apresentamos um exemplo simples de como é realizado essa conexão sem um "framework", ou seja, na mão:

```CS
Clientclient= null; 
using(connection) 
{ 
	using(var command = new SqlCommand("SELECT * FROM Clients WHERE Id = @id;", connection)) 
	{ 
		command.Parameters.Add(new SqlParameter("@id", id)); 
		connection.Open(); 
		using(var reader= command.ExecuteReader()) 
		{ 
			if(reader.Read()) 
			{ 
				client= newClient(); 
				client.Id= reader.GetString(0); 
				client.Name= reader.GetString(1); 
				client.Email= reader.GetString(2); 
				client.Phone= reader.GetString(3); 
			} 
		} 
	} 
} 
return client;
```

Além disso, há outras questões que devem ser tratadas:
- O contexto de persistência (monitorara alterações nos objetos que estão atrelados a uma conexão em um dado momento)
	- Alterações
	- Transação
	- Concorrência
- Mapa de identidade (cache de objetos já carregados)
- Carregamento tardio (lazy loading)
- Etc.

### **Solução:** Mapeamento Objeto-Relacional

**ORM** (**O**bject-**R**elational **M**apping): permite programar em nível de objetos e comunicar de forma transparente com um banco de dados relacional. Como exemplo, veja a imagem abaixo:

![[Pasted image 20250923144623.png]]
Assim sendo, conforme programarmos em OOP será mapeado na base de dados relacional. Dentro da tecnologia .NET, temos o Entity Framework (EF) como um ORM (Mapeador Objeto-Relacional) de alta qualidade, que simplifica o acesso a bancos de dados ao permitir que desenvolvedores trabalhem com dados usando objetos C# em vez de SQL. O EF Core é a versão moderna, leve e multiplataforma do EF, ideal para novos projetos no ecossistema .NET. A conexão do framework com o banco de dados é realizada a partir dos *providers* os quais podem ser diversos como MySQL, SQLite, SQLServer e por aí vai.

#### Principais classes

- **DbContext**: um objeto DbContext encapsula uma sessão com o banco de dados para um determinado modelo de dados (representado por DbSet's)
	- É usado para consultar e salvar entidades no banco de dados
	- Define quais entidades farão parte do modelo de dados do sistema
	- Pode definir várias configurações
	- É uma combinação dos padrões *Unity of Work* e *Repository*
		- ***Unity of work***: "*mantém uma lista de objetos afetados por uma transação e coordena a escrita de mudanças e trata possíveis problemas de concorrência*" - Martin Fowler
		- **Repository***: define um objeto capaz de realizar operações de acesso a dados (consultar, salvar, atualizar, deletar) para uma entidade
- `DbSet<TEntity>`: representa a coleção de entidades de um dado tipo em um contexto. Tipicamente corresponde a uma tabela do banco de dados.

Em resumo, o processo geral para se executar operações:
					![[Pasted image 20250923150209.png]]

## Responsabilidade de cada parte do MVC

- **Model**: estutura os dados e suas transformações (domain model)
	- Também chamado de "*o sistema*"
	- Composto de:
		- Entities
		- Services (relacionado a *Entities*)
			- *Repositories* (objetos que acessam dados presistentes, acessam o banco de dados por exemplo)

- **Controllers**
	- recebe e trata as interações do usuário com o sistema

- **Views**
	- define a estrutura e o comportamento das telas.


**A arquitetura geral:**

![[Pasted image 20250922161402.png]]

**Estrutura do projeto**

- `wwwroot`: recursos da aplicação (css, imagens, etc.)
- `Controllers`: os controladores da aplicação conforme a arquitetura MVC
- `Models`: entidades e os "*view models*"
- `Views`: páginas (repare na convenção de denominação em relação aos *controllers*)
	- `Shared`: *views* usada para mais de uma *controller*
- `appsettings.json`: configuração de recursos externos (logging, connection strings, etc.)
- `Program.cs`: Ponto de entrada, ou começo
- `Startup.cs`: configuração da aplicação

### Primeiro *controller* e testando páginas *Razor*

**Checklist**

- Route pattern (rota padrão): Controller / Ação / Id
	- Cada método de controller é mapeado a uma ação
- Templates naturais (o endereço da url que ativa o controlador e o controlador leva para a página, porém a estrutura das views seguem a mesma lógica do caminho)
- Um bloco de código C# é inicial nas páginas Razor a partir de: `@{ }`
- `ViewData` dictionary
- Os Tag Helpers que em páginas razor, por exemplo são: `asp-controller` e `asp-action`
- `IActionResult` aceita diferentes `Method Builder` como `return`.

![[Pasted image 20250922163949.png]]

### Primeiro Model-Controller-View -> Department

**Checklist**
- Crie uma nova pasta `ViewModels` e mova`ErrorViewModel` (incluindo `namespace`)
	- Para solucionar referências `CTRL + SHIFT + B`
- Crie a classe `Models/Department`
- Crie o *Controller*: clique com o botão direito em *Controllers* -> `Add` -> `Controller` -> `MVC Controller Empty`
	- Name: `DepartmentsController` (Plural)
	- Instancie `List<Departmnet>` e retorne como parâmetro do método `View`
	  
- Crie uma nova pasta `Views/Departments` (Plural)
- Crie a `view`: clique com o botão direito em `Views/Department` -> `Add` -> `View`
	- View name: index
	- Template: List
	- Model class: Department
	- Troque o título para "Departments"
	- Note que:
		- Definição `@model`
		- intellisense para model
		- métodos de ajuda
		- Bloco `@foreach`


#### Deletando o Department - View e Controller

**Checklist**
- Controller deletar
- Deletar pasta `Views/Departments`

#### CRUD scaffolding

**Checklist**

- Clique com o botão direito em `Controllers` -> `Add` -> `New Scaffolded Item`
	- MVC controllers with views, using Entity Framework
	- Model Class: `Department`
	- Data context class: clique em `+` e aceite o nome
	- Views (opções): selecione todas as três
	- `Controller name`: `DepartmentsController`

#### Adaptação do `MySQL` e primeira migração

**Atenção:** estamos utilizando o fluxo de trabalho CODE-FIRST (criamos o código primeiro e após criamos o banco de dados)

**Checklist**

- Em `appsettings.json` preencha `ConnectionStrings` com:
	-  "server=localhost;userid=developer;password=1234567;database=saleswebmvcappdb"

- Instale o provider `MySQL`:
	- Abra o console de gerenciador de pacotes `NuGet`
	- `Install-Package Pomelo.EntityFrameworkCore.MySQL`
- Stop IIS
- `CTRL + SHIFT + B`

- Inicie o servidor `MySQL`
	- `Painel de controle` -> `ferramentas administrativas` -> `serviços`
- Inicie o `MySQL Workbench`


- No console de gerenciador de pacote -> crie a primeira migração:
	- `Add-Migration Initial`
	- `Update-Datase`

- Cheque a base de dados via `MySQL Workbench`
- Teste o app: `CTRL + F5`

#### Modificando o tema

**Checklist**

- Vá até: http://bootswatch.com/3 (cheque a versão do *Bootstrap*)
- Escolha o tema
- Realize o download `bootstrap.css`
	- Sugestão: renomeie para `bootstrap-name.css`
	- Save o arquivo em `wwwroot/lib/bootstrap/dist/css` (cole dentro do Visual Studio)
- Abra `_Layout.cshtml`
	- Atualize a referência para o `bootstrap`

#### Outras entidades e segunda migração

![[Pasted image 20250925101813.png]]

**Checklist**

- Implemente o modelo `domain`
	- Atributos básicos
	- Associação (usaremos `ICollection`, o qual *matches* `List`, `HashSet`, etc - **Instâncie**!)
	- Construtores (**padrão** e com **argumentos**)
	- Métodos customizados
- Adicione o DbSet em `DbContext`
- Adicione migração e outras entidades
	- Atualize a base de dados


#### Seeding Service

Povoando a base de dados. Muitos realizam esse procedimento durante a migration.

**Checklist**

- Pare o IIS
- Em `Data`, crie `SeedingService`
- Em `Starup.cs`, registre `SeedingService` para dependências do sistema de injeção
- Em `Starup.cs`, adicione `SeegingService` como parâmetro do método de configuração. Chame a `Seed` para o perfil de desenvolvimento

#### SellersController

**Checklist**
- Crie os links na barra de navegação para `Departments` e `Sellers`
- Em `Controller -> Add -> Controller -> MVC Cnotroller - Empty -> SellersController`
- Crie a pasta `Views/Sellers`
- Em `Views/Sellers -> Add -> View`
	- View nome: `Index`
	- Modifique o título


 
#### SellerService e básico FindAll

Lembrando que estamos trabalhando dentro da arquitetura MVC (ModelViewController), onde o Model pode conter outras três subcategorias Entities, Services e Repositories. Até então trabalhamos somente com Entities e os Repositories não utilizaremos pois o DbContext já realiza o contato com os dados. Os *Services* são classes relacionadas a entidades onde é responsável por realizar operações relacionadas a entidades e implementar as regras de negócio. Ao contrário do que realizamos no controlador de `Department` onde o contato com a base de dados foi direta, iremos utilizar os *services* como mais uma camada para podermos implementar regras a esta interação.

**Checklist**

- Criar uma pasta `Services`
- Criar `SellerService`
- Em `Startup.cs`, registrar a dependência de injeção de `SellerService`
- Em `SellerService`, implementar `FindAll` retornando `List<Seller>`
- Em `SellersController`, implementar o método `index`, o qual deve chamar `SellerServie.FindAll`
- Em `Views/Sellers/Index`, escreva o código template para mostrar `Sellers`

- **Sugestão:** usar as classes "table-striped table-hover" para a tabela
- **Nota:** iremos aplicar formatação nos próximos passos

#### Criando um simples formulário

**Checklist**

- Em `Views/Sellers/Index`, criar um link para `Create`
- Em `Controllers`, implemente `Create` com a ação GET
- Em `Views/Sellers`, crie a view `Create`
- Em `Services/SellersService` crie o método `Insert`
- Em `Controllers`, implemente `Create` com a ação POST

Para mais informações: [clique aqui](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery)


#### Foreign key not null (integridade referencial)

Do passo anterior, o cadastro ocorreu porém faltou no banco de dados a info sobre o Id do departamento acarretando em *Foreign key not null*. No que segue, iremos contornar isto.

**Checklist**

- Em `Seller`, adicione `DepartmentId`
- Drop database
- Crie uma nova migration, atualize a base de dados
- Atualize `SellerService.Insert` para agora: `obj.Department = _context.Department.First()`


#### SellerFormViewModel e Department componente de seleção

**Checklist**

- Crie `DepartmentService` com o método `FindAll`
- Em `Startup.cs`, registre `DepartmentService` para o sistema de injeção de dependência
- Crie  SellerFormViewModel
- Em `Controller`
	- Nova dependência: `DepartmentService`
	- Atualize "*create*" ação GET
- Em `Views/Sellers/Create`:
	- Atualize a model para `SellerFormViewModel`
	- Atualize os campos do formulário
	- Adicione o componente de select para `DepartmentId`

```html
<div class="form-group"> 
<label asp-for="Seller.DepartmentId" class="control-label"></label> 
<select asp-for="Seller.DepartmentId" asp-items="@(new SelectList(Model.Departments,"Id", 
"Name"))" class="form-control"></select> 
</div> 
```

- Em `SellerService.Insert`, delete a "*primeira*" chamada

#### Deletar seller

**Checklist**

- Em `SellerService`, crie as operações `FindById` e `Remove`
- Em `Controller`, crie a ação GET "delete"
- Em `View/Sellers/Index`, check o link para a ação "delete"
- Crie a view de confirmação de delção: `View/Sellers/Delete`
- Teste o WebApp
- Em `Controller`, crie a ação POST "delete"
- Teste o WebApp


#### Detalhes para Seller e eager loading

**Checklist**

- Em `View/Sellers/Index`, check o link para a ação "*Details*"
- Em `Controller`, crie a ação GET "*Details*"
- Crie a view: `View/Sellers/Details`
Ao testar, notará que não teremos acesso ao departamento, para termos acesso ao departamento, necessitamos realizar:
- Inclua em `FindAll`: `Include(obj => obj.Department)`
Assim realizamos o *eager loading* que é carregar outros objetos associados aquele objeto principal 

#### Atualize `seller` e exceção `custom service` 

**Checklist**

- Crie a pasta `Services/Exceptions`
- Crie `NotFoundException` e `DbConcurrencyException`
- Em `SellerService`, crie o método `Update`

Aqui, incluímos exceções na camada de serviço. Sendo uma forma de respeitar a estrutura MVC, com o Controlador (controller) lidando com o Model (Services, Repositories, Entities) tendo em vista que a exceção será definida em Services e será repassada ao controlador.

- Em `View/Sellers/Index`, cheque o link para a ação "*Edit*"
- Em `controller`, crie a ação GET "*Edit*"
- Crie a view: `Views/Sellers/Edit` (similar a view do `Creat`, porém com o adicional de `hidden id`)
- Teste o App
- Em `controller`, crie a ação POST "*Edit*"

-  **Nota:** ASP.NET Core seleciona as opções baseado em `DepartmendId`


#### Retornando páginas de erros customizadas

**Checklist**

- Atualize `ErrorViewModel`
- Atualize `Error.cshtml`
- Em `SellerController`:
	- Crie a ação de `Error` com o parâmetro de mensagem
	- Atualize a chamada dos métodos

```CS
catch (NotFoundException ex)
{
    return RedirectToAction(nameof(Error), new { message = ex.Message });
    ;
}
catch (DbConcurrencyException ex)
{
    return RedirectToAction(nameof(Error), new { message = ex.Message });
}
```

ou 

```CS
catch (ApplicationException ex)
{
    return RedirectToAction(nameof(Error), new { message = ex.Message });
    ;
}
```

pois `ApplicationException` é uma super classe, logo é válido por *upcasting*.

#### App locale, números e formatação de datas

App locale é para definir o local no qual o nosso sistema irá se basear. Além disso, devemos formatar os campos do create para o formato de numeros e datas.

**Checklist**

- Em `Program.cs`, defina as opções de local
- Em `Seller`:
	- Defina labels customizados `Display`
	- Defina a semantica para data `DataType`
	- Defina os formatos de display `DisplayFormat`

**Definição das opções de local:**
Em `Program.cs`, devemos inserir no cabeçalho `using Microsoft.AspNetCore.Localization;` e iremos inserir no corpo do documento: 

```CS
// Locale
var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("pt-BR")
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseAuthorization();
```

** labels customizados**
Para definir os labels customizados, iremos utilizar *Anotations* na model, por exemplo em `Models/Seller.cs`
```CS
[Display(Name = "Birth Date")]
public DateTime BirthDate { get; set; }
```
`[Display(Name = "Birth Date")]` representa como o atributo `BirthDate` irá ser apresentado a tela caso seja chamado no html pelo tag-helper.

Para outras customizações:
```CS
public string Name { get; set; }
[DataType(DataType.EmailAddress)]
public string Email { get; set; }
[Display(Name = "Birth Date")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] //0 representa o valor do atributo
public DateTime BirthDate { get; set; }
[Display(Name = "Base Salary")]
[DisplayFormat(DataFormatString = "{0:F2}")] //0 representa o valor do atributo
public double BaseSalary { get; set; }

public Department Department { get; set; }
[Display(Name = "Department")]
public int DepartmentId { get; set; }
```
#### Validação

Aqui utilizaremos as *annotations* para realizar as validações, que são parâmetros para restringir ou delimitar os dados a serem inseridos no nosso CRUD.

**Checklist**

- Em `Seller`, adicione anotações de validação:
```cs
[Required(ErrorMessage = "{0} required")] 

[EmailAddress(ErrorMessage = "Enter a valid email")] 

[Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
```

- Atualize o HTML para `Create` e `Edit`:

  ![[Pasted image 20251002120058.png]]
- Atualize `SellersController`


#### Operações assíncronas usando `Tasks` (`async`, `await`)


**Checklist**

- Atualize `DepartmentService`
- Atualize `SellerService`
- Atualize `SellersController`

