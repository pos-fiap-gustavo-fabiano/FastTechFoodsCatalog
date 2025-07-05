# Menu Service – Fast Food Delivery (PostgreSQL Example)

Este documento apresenta exemplos de endpoints REST para o serviço de Menu de uma aplicação Fast Food Delivery, utilizando PostgreSQL como banco de dados.

---

## Endpoints

### 1. Listar todos produtos
**GET /api/menu**

**Resposta:**
```json
[
  {
    "id": 1,
    "nome": "Cheeseburger",
    "descricao": "Hambúrguer com queijo, alface, tomate e molho especial.",
    "preco": 18.50,
    "disponibilidade": true,
    "tipo": "Lanche",
    "data_criacao": "2025-07-05T12:00:00Z"
  },
  {
    "id": 2,
    "nome": "Batata Frita Média",
    "descricao": "Porção média de batata frita crocante.",
    "preco": 8.00,
    "disponibilidade": true,
    "tipo": "Acompanhamento",
    "data_criacao": "2025-07-05T12:02:00Z"
  }
]
```

---

### 2. Detalhes de um produto
**GET /api/menu/{id}**

**Resposta (200 OK):**
```json
{
  "id": 1,
  "nome": "Cheeseburger",
  "descricao": "Hambúrguer com queijo, alface, tomate e molho especial.",
  "preco": 18.50,
  "disponibilidade": true,
  "tipo": "Lanche",
  "data_criacao": "2025-07-05T12:00:00Z"
}
```
**Resposta se não encontrado:**
```json
{
  "error": "Produto não encontrado"
}
```

---

### 3. Cadastrar produto
**POST /api/menu**

**Request Body:**
```json
{
  "nome": "Refrigerante 350ml",
  "descricao": "Coca-cola, lata 350ml.",
  "preco": 6.00,
  "disponibilidade": true,
  "tipo": "Bebida"
}
```

**Resposta (201 Created):**
```json
{
  "id": 3,
  "nome": "Refrigerante 350ml",
  "descricao": "Coca-cola, lata 350ml.",
  "preco": 6.00,
  "disponibilidade": true,
  "tipo": "Bebida",
  "data_criacao": "2025-07-05T14:00:00Z"
}
```

---

### 4. Atualizar produto
**PUT /api/menu/{id}**

**Request Body:**
```json
{
  "nome": "Batata Frita Grande",
  "descricao": "Porção grande de batata frita crocante.",
  "preco": 12.00,
  "disponibilidade": true,
  "tipo": "Acompanhamento"
}
```

**Resposta (200 OK):**
```json
{
  "id": 2,
  "nome": "Batata Frita Grande",
  "descricao": "Porção grande de batata frita crocante.",
  "preco": 12.00,
  "disponibilidade": true,
  "tipo": "Acompanhamento",
  "data_criacao": "2025-07-05T12:02:00Z"
}
```

---

### 5. Remover produto
**DELETE /api/menu/{id}**

**Resposta (204 No Content):**
_Sem conteúdo._

**Resposta se não encontrado:**
```json
{
  "error": "Produto não encontrado"
}
```

---

## Observação

- Todos os exemplos consideram integração com PostgreSQL e cenário de fast food.
- Os campos podem ser ajustados conforme a modelagem da sua aplicação.
- Para dúvidas sobre integração, autenticação e exemplos de código C#, consulte a documentação principal do projeto ou abra uma issue.
