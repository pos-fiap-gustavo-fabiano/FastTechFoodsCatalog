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
    "name": "Cheeseburger",
    "description": "Burger with cheese, lettuce, tomato and special sauce.",
    "price": 18.50,
    "available": true,
    "type": "Snack",
    "created_at": "2025-07-05T12:00:00Z"
  },
  {
    "id": 2,
    "name": "Medium French Fries",
    "description": "Medium portion of crispy french fries.",
    "price": 8.00,
    "available": true,
    "type": "Side",
    "created_at": "2025-07-05T12:02:00Z"
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
  "name": "Cheeseburger",
  "description": "Burger with cheese, lettuce, tomato and special sauce.",
  "price": 18.50,
  "available": true,
  "type": "Snack",
  "created_at": "2025-07-05T12:00:00Z"
}
```
**Resposta se não encontrado:**
```json
{
  "error": "Product not found"
}
```

---

### 3. Cadastrar produto
**POST /api/menu**

**Request Body:**
```json
{
  "name": "Soda 350ml",
  "description": "Coca-cola, 350ml can.",
  "price": 6.00,
  "available": true,
  "type": "Drink"
}
```

**Resposta (201 Created):**
```json
{
  "id": 3,
  "name": "Soda 350ml",
  "description": "Coca-cola, 350ml can.",
  "price": 6.00,
  "available": true,
  "type": "Drink",
  "created_at": "2025-07-05T14:00:00Z"
}
```

---

### 4. Atualizar produto
**PUT /api/menu/{id}**

**Request Body:**
```json
{
  "name": "Large French Fries",
  "description": "Large portion of crispy french fries.",
  "price": 12.00,
  "available": true,
  "type": "Side"
}
```

**Resposta (200 OK):**
```json
{
  "id": 2,
  "name": "Large French Fries",
  "description": "Large portion of crispy french fries.",
  "price": 12.00,
  "available": true,
  "type": "Side",
  "created_at": "2025-07-05T12:02:00Z"
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
  "error": "Product not found"
}
```

---

## Observação

- Todos os exemplos consideram integração com PostgreSQL e cenário de fast food.
- Os campos podem ser ajustados conforme a modelagem da sua aplicação.
- Para dúvidas sobre integração, autenticação e exemplos de código C#, consulte a documentação principal do projeto ou abra uma issue.
