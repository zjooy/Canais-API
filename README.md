# 📡 Canais.API

API REST desenvolvida para gerenciamento e classificação de reclamações recebidas por diferentes canais de atendimento. Este projeto utiliza arquitetura em camadas com DDD (Domain-Driven Design), autenticação JWT e integração com serviços AWS.

---

## 🚀 Tecnologias Utilizadas

- **.NET 8.0** – Backend com ASP.NET Core
- **Entity Framework Core** – ORM para persistência
- **JWT (JSON Web Token)** – Autenticação segura
- **AWS Lambda (Python)** – Processamento de reclamações fisicas e classificação de reclamações
- **Amazon SQS & SNS** – Comunicação assíncrona
- **Amazon S3** – Armazenamento de anexos e recebimento de reclamações fisicas
- **xUnit + Moq** – Testes unitários e de integração
- **Swagger / OpenAPI** – Documentação interativa da API
- **Angular 20** – Frontend

---

## 🧱 Arquitetura

O projeto está dividido em 4 camadas principais:
Canais.API → Camada de apresentação (Controllers) 
Canais.Application → Casos de uso e interfaces de serviço 
Canais.Domain → Entidades e regras de negócio 
Canais.Infrastructure→ Repositórios


---

## 🧰 Tecnologias e Bibliotecas Utilizadas nos Lambdas

### Lambda de Digitalização

Responsável por extrair texto de arquivos físicos (PDF) usando Textract e enviar os dados para a fila SQS.

**Bibliotecas:**

- `boto3` – Cliente AWS para acesso ao S3, SQS e Textract
- `pg8000` – Conexão direta com banco de dados PostgreSQL
- `re` – Expressões regulares para extrair campos como nome, CPF e texto
- `os` – Manipulação de caminhos e variáveis de ambiente
- `json` – Manipulação de payloads e mensagens

---

### Lambda de Classificação

Responsável por receber mensagens da fila, aplicar lógica de classificação por palavras-chave e gravar categorias no banco.

**Bibliotecas:**

- `boto3` – Cliente AWS para Secrets Manager
- `pg8000` – Conexão com PostgreSQL
- `json` – Manipulação de mensagens e dados
- `unicodedata` – Normalização de texto (remoção de acentos)
- `re` – Expressões regulares para limpeza e contagem de palavras

---

### Observações Técnicas

- A fila principal possui uma **DLQ configurada** para capturar mensagens que falham após 3 tentativas.
- O banco de dados utiliza as tabelas `tb_reclamacoes` e `tb_reclamacaocategoria` para persistência.
- A classificação é feita com base em palavras-chave cadastradas por categoria, com normalização e contagem de ocorrências.


## 🔐 Autenticação

A API utiliza JWT para proteger os endpoints. Para acessar rotas seguras, é necessário incluir o token no cabeçalho:
Bearer <seu-token>

---

## 📦 Endpoints Principais

- `POST /api/reclamacoes` → Endpoint de entrada de reclamações recebidas via site
- `GET /api/reclamacoes` → Lista reclamações classificadas com filtros
- `POST /api/reclamacoes-fisicas/anexo` → Upload de reclamações físicas (PDF)  
- `GET /api/reclamacoes/{cpf}/detalhes` → Histórico de reclamações por CPF  
- `POST /api/reclamacoes/{id}/enviar-legado` → Envia reclamação para sistema legado


---

## 🧪 Testes

O projeto inclui testes:

- **Unitários** para regras de negócio e serviços
- **(Futuramente) integração** para endpoints protegidos e persistência
