# EventWebApp API

A API **EventWebApp** é uma aplicação desenvolvida para o gerenciamento de eventos, voltada para empresas de tecnologia que desejam organizar conferências, reuniões, meetups e calls de forma prática e eficiente. A API permite o cadastro e acompanhamento de eventos, além de promover a interação entre os usuários participantes.

A API foi construída com **ASP.NET Core**, é totalmente documentada usando **Swagger**, e permite o consumo de dados através de requisições **REST**.

## Bibliotecas utilizadas:

**SwaggerBuckle** e **ASP.NET Annotations** são utilizados para melhorar as validações e documentações dos endpoints.
**Entity Framework** é utilizado para manipulação de dados.

## Funcionalidades

- **Cadastro de Eventos**: Permite a criação de eventos, especificando detalhes como nome, data e categoria.
- **Gerenciamento de Categorias**: A API oferece endpoints para criação e edição de categorias para organizar os eventos.
- **Interação com Comentários**: Cada evento pode receber comentários dos usuários, promovendo a interação.
- **Controle de Participação**: Permite que os usuários confirmem presença em eventos.
- **Autenticação de Usuário**: Suporte a autenticação de usuários com login social (Google e Microsoft).
- **Controle de Acesso**: Algumas rotas são privadas e exigem que o usuário esteja logado.

## Documentação

A documentação da API está disponível diretamente via **Swagger**. Para acessá-la, basta executar o projeto e abrir o seguinte link no seu navegador:

```
http://localhost:{porta}/swagger
```

O Swagger irá fornecer uma interface interativa para testar os endpoints da API, com exemplos de request/response e validações.

## Configuração e Instalação

Para rodar o **EventWebApp API**, siga os seguintes passos:

### 1. Clone o repositório:
```bash
git clone https://github.com/JoaoIto/EventWebApp
```

### 2. Execute o projeto:
Execute o projeto com o seguinte comando:

```bash
dotnet build
dotnet run
```

### 3. Verifique o banco de dados:
Após executar a aplicação, acesse seu localhost, o banco de dados está sendo rodado em memória, portando não é preciso a configuração do banco e verifique se as tabelas foram criadas corretamente.

---

## Estrutura do Projeto

A API segue a arquitetura **Model-View-Controller (MVC)**, organizada da seguinte forma:

- **Models**: Contêm as entidades e a lógica de dados.
- **Controllers**: Gerenciam as operações de cada entidade e definem as rotas da API.
- **Views**: Não aplicável, já que é uma API sem interface visual.

---

## Exemplos de Endpoints

### 1. **Cadastrar Evento**

**POST** `/api/eventos`

Request Body:
```json
{
  "nome": "Conferência de Tecnologia 2024",
  "data": "2024-12-12",
  "categoriaId": 1,
  "comentarios": [
    {
      "usuario": "user@example.com",
      "comentario": "Este evento será incrível!"
    }
  ]
}
```

Resposta:
```json
{
  "id": 1,
  "nome": "Conferência de Tecnologia 2024",
  "data": "2024-12-12",
  "categoriaId": 1,
  "comentarios": [
    {
      "usuario": "user@example.com",
      "comentario": "Este evento será incrível!"
    }
  ]
}
```

### 2. **Obter Todos os Eventos**

**GET** `/api/eventos`

Resposta:
```json
[
  {
    "id": 1,
    "nome": "Conferência de Tecnologia 2024",
    "data": "2024-12-12",
    "categoriaId": 1
  }
]
```

---

## Contribuição

Contribuições são bem-vindas! Para contribuir, basta seguir as etapas:

1. Faça um *fork* do repositório.
2. Crie uma nova branch.
3. Realize suas modificações e crie um *commit*.
4. Envie um *Pull Request*.

---

## Licença

Este projeto está licenciado sob a **MIT License**.


### Explicação:

- O README fornece uma visão geral da API, suas funcionalidades e como configurá-la.
- **Swagger** é mencionado para documentar a API de forma interativa.
- As instruções de configuração e instalação são detalhadas para facilitar o uso do repositório e a execução da API localmente.

---
