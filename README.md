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

## 🔐 Autenticação

A API utiliza JWT para proteger os endpoints. Para acessar rotas seguras, é necessário incluir o token no cabeçalho:
Authorization: Bearer <seu-token>

---

## 📦 Endpoints Principais

- `POST /api/reclamacoes` → Endpoint de entrada de reclamações recebidas via site
- `GET /api/reclamacoes` → Lista reclamações classificadas com filtros

---

## 🧪 Testes

O projeto inclui testes:

- **Unitários** para regras de negócio e serviços
- **(Futuramente) integração** para endpoints protegidos e persistência
