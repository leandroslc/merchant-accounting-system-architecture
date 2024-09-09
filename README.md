
<div align="center">
  <h1>Sistema de lançamentos para comerciantes</h1>
  <p>Um modelo e exemplo de sistema de controle de lançamentos de comerciantes.</p>
</div>

## Tópicos
- [Sobre o sistema](#sobre-o-sistema)
  - [Funcionalidades](#funcionalidades)
  - [Considerações sobre o sistema](#considerações-sobre-o-sistema)
- [Arquitetura proposta](#arquitetura-proposta)
  - [Decisões arquiteturais](#decisões-arquiteturais)
  - [Considerações sobre a arquitetura](#considerações-sobre-a-arquitetura)
- 4. Como os sistemas funcionam
- [Arquitetura local](#arquitetura-local)
  - [Tecnologias](#tecnologias)
  - [Limitações](#limitações)
- [Requisitos mínimos](#requisitos-mínimos)
- [Executando as aplicações](#executando-as-aplicações)
  - [Como usar as funcionalidades](#como-usar-as-funcionalidades)
- [Testes de carga](#testes-de-carga)
  - [Executando testes de carga](#executando-testes-de-carga)
- [Desenvolvimento](#desenvolvimento)
  - [Requisitos para desenvolvimento](#requisitos-para-desenvolvimento)
  - [Extensões úteis para o VS Code](#extensões-úteis-para-o-vs-code)
  - [Iniciando banco de dados das aplicações](#iniciando-banco-de-dados-das-aplicações)
  - [Executar migrations programaticamente](#executar-migrations-programaticamente)
  - [Iniciando o broker de mensageria](#iniciando-o-broker-de-mensageria)
  - [Estrutura dos serviços](#estrutura-dos-serviços)
  - [Fluxo de trabalho](#fluxo-de-trabalho)
- [Documentação dos endpoints](#documentação-dos-endpoints)
  - 10.1. Registro de débito
  - 10.2. Registro de crédito
  - 10.3. Registro de saldo
- 11. Aprimoramentos futuros
  - 11.1. Exemplo de uma arquitetura em nuvem (Azure)

## Sobre o sistema
Registra operações de lançamentos (débitos e créditos) de comerciantes e gera um saldo diário consolidado com o balanço total de todas as operações realizadas em cada dia.

A ideia é que o próprio comerciante registre operações de caixa e realize a consulta do saldo diário, através de um site ou aplicativo.

### Funcionalidades
- Autenticação do comerciante
- Registro de lançamento de débito
- Registro de lançamento de crédito
- Consulta de saldo consolidado diário

### Considerações sobre o sistema
Alguns fatores influenciaram nas decisões durante a modelagem do sistema:

- Um "lançamento" foi considerado [como descrito na contabilidade](https://en.wikipedia.org/wiki/Debits_and_credits). Como é o próprio comerciante que está registrando a operação, foi considerado que ele está usando uma conta ativa. Dessa forma: débitos aumentam o valor do caixa e créditos diminuem o valor do caixa.

## Arquitetura proposta
> :bulb: Clique na imagem para aumentar

<a href="./docs/pt-br/architecture.drawio.svg" target="_blank">
  <img src="./docs/pt-br/architecture.drawio.svg" alt="Proposta de arquitetura" />
</a>

Olhando de uma visão macro, o sistema como um todo funciona da seguinte forma:
- O comerciante teria acesso a uma aplicação frontend como um site ou app.
- O aplicativo se autenticaria através de um provedor de autenticação utilizando OpenId Connect. O provedor de autenticação poderia ser o [Keycloak](https://www.keycloak.org/) ou o [Microsoft Entra ID](https://www.microsoft.com/pt-br/security/business/identity-access/microsoft-entra-id), por exemplo.
- Após se autenticar, a aplicação faria uma requisição para o API Gateway (nesse caso representado pelo [kong](https://konghq.com/products/kong-gateway)). O API Gateway valida o token de acesso através do provedor de autenticação e repassa a requisição para os sistemas internos.
- O sistema interno recebe a solicitação do API Gateway, processa e responde de volta para o API Gateway.
- Por sua vez, o API Gateway repassa a resposta, até chegar ao comerciante.

### Decisões arquiteturais
> :bulb: Clique na imagem para aumentar

<a href="./docs/pt-br/architecture-overview.drawio.svg" target="_blank">
  <img src="./docs/pt-br/architecture-overview.drawio.svg" alt="Proposta de arquitetura" />
</a>

- **Escalabilidade:**
  - Apesar de fazer parte de um sistema único, ele foi dividido em partes independetes para que possam ser facilmente escaláveis. Neste caso, as APIs serão escaladas pelo _throughput_ de requisições HTTP e o worker será escalado pelo _throughput_ de mensagem recebidas.
  - O banco de dados separado das duas aplicações também permite escalabilidade independente.
- **Resiliência:**
  - Os sistemas são independentes. Se uma API falhar, a outra não falha, e vice versa.
  - Se o worker falhar, as APIs não falham e, ao mesmo tempo, as mensagens também não são perdidas, ficam retidas na fila para que sejam consumidas logo quando o consumidor retomar o processamento.
- **Disponibilidade:**
  - A arquitetura foi projetada principalmente para manter uma alta disponibilidade.
  - A API de saldo suporta um alto _throughput_ pois recebe apenas uma requisição de consulta que é feita através de um banco de dados otimizado para leitura.
  - A API de lançamentos recebe requisições otimizadas para escrita, onde operações são apenas adicionadas.
  - O processamento do saldo é feito de forma assíncrona e não interrompe nenhum fluxo. É usada uma abordagem de concorrência otimista, portanto existe apenas uma operação de _lock_ no saldo de apenas um dia por um período de poucos milissegundos.
- **Segurança:**
  - O sistema como um todo possui apenas uma porta de entrada, que possui auntenticação e pode ser facilmente extensível para suportar _rate limit_.
  - Os sistemas da rede privada ficam inacessíveis externamente. Por conta disso, não há necessidade de revalidar os tokens de acesso já previamente validados e, além disso, não existe obrigatóriedade de usar HTTPS, que aumentaria latência.
- **Monitoramento:**
  - O API Gateway gera e repassa um _[Correlation Identifier](https://microservices.io/patterns/observability/distributed-tracing.html)_ para os sistemas. Com isso seria possível instrumentar ferramentas de observalidade para realizar o tracing distribuído.

### Considerações sobre a arquitetura
Algumas considerações importantes sobre a arquitetura:
- Apesar de os sistemas terem sido projetados para estarem separados preventivamente, esta arquitetura pode não ser a mais eficiente com relação a custo. Existem aplicações que suportam praticamente mais de 50 requisições por segundo usando apenas um banco de dados e realizando vários processos de consulta, escrita e processamento de mensagens ao mesmo tempo. Por isso é sempre interessante medir o quanto de processamento é suportado e os custos necessários.
- O banco de dados PostgreSQL foi selecionado porque ele é otimizado tanto para escrita e leitura. Além disso, ele foi escolhido principalmente pela alta disponibilidade.
- O banco de dados da API de saldo por ter inconsistência eventual, apesar de que por ser um consolidado diário, não cause impactos para comerciante.
- Nesse primeiro momento não houve necessidade de usar uma ferramenta de cache. Se fosse necessário, seria interessante experimentar um cache "na borda" como um [cache HTTP](https://developer.mozilla.org/en-US/docs/Web/HTTP/Caching) para consulta de saldo (saldos de dias passados não serão alterados com frequência).

## Arquitetura local
A arquitetura local (para fins de teste) é muito semelhante com a [arquitetura proposta](#arquitetura-proposta), mas com algumas pequenas limitações.

### Tecnologias
- API Gateway e autenticação: [Kong](https://konghq.com/products/kong-gateway).
- Banco de dados: [PostgreSQL](https://www.postgresql.org/).
- Aplicações: [.NET](https://dotnet.microsoft.com/).
- Broker de mensageria: [RabbitMQ](https://www.rabbitmq.com/).

### Limitações
- O API Gateway foi implementado com o Kong _Open Source_. Por conta disso, ele não suporta ferramentas do licença _Enterprise_, como _OpenId Connect_. Ou seja, ele não tem suporte para provedores como o Keycloak, por exemplo.
- Para autenticação é usado próprio Kong, com uma combinação de um sistema simples e utilitário para geração de tokens de acesso válidos.

## Requisitos mínimos
Requisitos mínimos para executar as aplicações:

- [Docker e Docker Compose](https://www.docker.com/get-started/).

## Executando as aplicações
Após garantir que você possui os [requisitos mínimos](#requisitos-mínimos), para iniciar as aplicações, apenas execute o comando abaixo:

```sh
docker compose up -d
```

> :bulb: As aplicações internamente são inicializadas na sequência apropriada para que inicializem corretamente.

> :warning: Observação: dependendo do sistema, é possível que as portas definidas no `docker-compose` já estejam em uso. Nesse caso, será necessário ajustá-las ou potencialmente parar as aplicações que utilizam estas portas.

### Como usar as funcionalidades
[Veja a documentação dos endpoints](#documentação-dos-endpoints) para saber como realizar as requisições e quais são os retornos possíveis.

Lembrando que também é possível fazer requisições diretamente pelo _Visual Studio Code_ se estiver usando a extensão [REST Client](#extensões-úteis-para-o-vs-code), acessando o arquivo [Apis.http](./Apis.http).

## Testes de carga
Existem testes de carga para validarem alguns cenários. Atualmente o seguinte cenário é suportado:

- 50 requisições simultâneas por segunda com taxa de falha menor que 5% para requisições de consulta de saldo.

> :construction: Os testes ainda não chegam perto de um cenário real. Faltam outros cenários de testes mais bem elaborados.

### Executando testes de carga
Para executar os testes de carga, utilize o comando abaixo:

```sh
docker compose -f docker-compose-test.yml up
```

Infelizmente ainda não há suporte para visualizar os testes de forma amigável a não ser pelos logs. Futuramente poderá ser incluído.

> :warning: Observação: O arquivo `docker-compose-test` estende o arquivo padrão, portanto podem ser aplicadas as mesmas observações da seção [executando as aplicações](#executando-as-aplicações).

Caso tenha executado os testes no modo _detached_ (`-d`), ainda é possível observar os logs com os resultados dos testes através do comando abaixo:

```sh
docker compose -f docker-compose-test.yml logs load-tests
```

## Desenvolvimento
Algumas orientações de como preparar o ambiente de desenvolvimento.

### Requisitos para desenvolvimento
- Será necessário configurar os [requisitos mínimos](#requisitos-mínimos).
- [.NET SDK](https://dotnet.microsoft.com/download) versão 8.
- [Node JS](https://nodejs.org/) (apenas obrigatório para o desenvolvimento de testes de carga).
- Uma IDE como _Visual Studio Code_ ou _Visual Studio_.

### Extensões úteis para o VS Code
Extensões recomendadas se estiver usando o _Visual Studio Code_:

- [EditorConfig](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig): Formata os arquivos de acordo com as regras. É muito importante que tenha instalada esta extensão.
- [Draw.io Integration](https://marketplace.visualstudio.com/items?itemName=hediet.vscode-drawio): Permite editar arquivos `.drawio.png` ou `.drawio.svg`.
- [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client): Permite executar requisições através de arquivos `.http`.
- [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp): No caso de estar desenvolvendo em C# no Visual Studio Code.

### Iniciando banco de dados das aplicações
Os bancos de dados de cada aplicação foram pré configurados no `docker compose`:

- Para o banco "Accounting Operations", execute `docker compose up -d accounting-operations-db`.
- Para o banco "Daily Balances", execute `docker compose up -d daily-balances-db`.

### Executar migrations programaticamente
Cada serviço possui um _CLI_ para executar migrations de banco de dados de forma independente. Para facilitar o uso, para cada banco de dados foi configurado um serviço no `docker compose`:

- Para o banco "Accounting Operations", execute `docker compose up accounting-operations-migrate`.
- Para o banco "Daily Balances", execute `docker compose up daily-balances-migrate`.

> :bulb: Não é necessário executar as migrations para executar os testes de integração.

### Iniciando o broker de mensageria
Para iniciar o broker de mensageria, basta executar o comando `docker compose up -d message-broker`.

### Estrutura dos serviços
As aplicações são separadas por contextos. No caso dos serviços, a separação é feita da seguinte forma:

```
- 📁 services
  - 📁 accounting-operations
  - 📁 daily-balances
  - 📁 simple-auth
```

Cada `service` possui sua própria solution. Se você estiver usando o _Visual Studio_ terá de abrí-las separadamente. Casa esteja usando o _Visual Studio Code_, use o comando `.NET Open Solution` para alternar entre uma solution e outra.

Estando dentro de cada serviço é possível iniciar cada aplicação e executar os testes.

### Fluxo de trabalho
Caso esteja desenvolvendo para este repositório é importante seguir as seguintes convenções:

- Crie uma branch com nome curto e descritivo do trabalho a ser feito. Inclua como prefixo o seu nome de usuário do github. Exemplo: `leandroslc/performance-improvement`.
- Após concluir o trabalho na branch, dê um _push_ e crie um _Pull Request_.
- O merge do _Pull Request_ deve sempre ser `Rebase`.

## Documentação dos endpoints
