
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
  - [Como executar as operações](#como-executar-as-operações)
- 8. Executando testes de carga
- 9. Desenvolvimento
  - 9.1. Configurando requisitos mínimos
  - 9.2. Requisitos para desenvolvimento
  - 9.3. Extensões úteis para o VS Code (#extensões-úteis-para-o-vs-code)
  - 9.4. Iniciando banco de dados das aplicações
  - 9.5. Como executar migrations programaticamente
  - 9.6. Executando as aplicações
  - 9.7. Fluxo de trabalho
- 10. Documentação dos endpoints (#documentação-dos-endpoints)
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

### Como executar as operações
[Veja a documentação dos endpoints](#documentação-dos-endpoints) para saber como realizar as requisições e quais são os retornos possíveis.

Lembrando que também é possível fazer requisições diretamente pelo _Visual Studio Code_ se estiver usando a extensão [REST Client](#extensões-úteis-para-o-vs-code), acessando o arquivo [Apis.http](./Apis.http).








https://marketplace.visualstudio.com/items?itemName=humao.rest-client
