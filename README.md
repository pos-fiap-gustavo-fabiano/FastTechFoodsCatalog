# Menu Service – Fast Food Delivery (PostgreSQL Example)

This document presents REST endpoint examples for the Menu service of a Fast Food Delivery application, using PostgreSQL as the database.

---

## Endpoints

### 1. List all products
**GET /api/menu**

**Response:**
```json
[
  {
    "id": 1,
    "nome": "Cheeseburger",
    "descricao": "Burger with cheese, lettuce, tomato and special sauce.",
    "preco": 18.50,
    "disponibilidade": true,
    "tipo": "Main Course",
    "data_criacao": "2025-07-05T12:00:00Z"
  },
  {
    "id": 2,
    "nome": "Medium French Fries",
    "descricao": "Medium portion of crispy french fries.",
    "preco": 8.00,
    "disponibilidade": true,
    "tipo": "Side Dish",
    "data_criacao": "2025-07-05T12:02:00Z"
  }
]
```

---

### 2. Product details
**GET /api/menu/{id}**

**Response (200 OK):**
```json
{
  "id": 1,
  "nome": "Cheeseburger",
  "descricao": "Burger with cheese, lettuce, tomato and special sauce.",
  "preco": 18.50,
  "disponibilidade": true,
  "tipo": "Main Course",
  "data_criacao": "2025-07-05T12:00:00Z"
}
```
**Response if not found:**
```json
{
  "error": "Product not found"
}
```

---

### 3. Create product
**POST /api/menu**

**Request Body:**
```json
{
  "nome": "Soda 350ml",
  "descricao": "Coca-cola, 350ml can.",
  "preco": 6.00,
  "disponibilidade": true,
  "tipo": "Beverage"
}
```

**Response (201 Created):**
```json
{
  "id": 3,
  "nome": "Soda 350ml",
  "descricao": "Coca-cola, 350ml can.",
  "preco": 6.00,
  "disponibilidade": true,
  "tipo": "Beverage",
  "data_criacao": "2025-07-05T14:00:00Z"
}
```

---

### 4. Update product
**PUT /api/menu/{id}**

**Request Body:**
```json
{
  "nome": "Large French Fries",
  "descricao": "Large portion of crispy french fries.",
  "preco": 12.00,
  "disponibilidade": true,
  "tipo": "Side Dish"
}
```

**Response (200 OK):**
```json
{
  "id": 2,
  "nome": "Large French Fries",
  "descricao": "Large portion of crispy french fries.",
  "preco": 12.00,
  "disponibilidade": true,
  "tipo": "Side Dish",
  "data_criacao": "2025-07-05T12:02:00Z"
}
```

---

### 5. Delete product
**DELETE /api/menu/{id}**

**Response (204 No Content):**
_No content._

**Response if not found:**
```json
{
  "error": "Product not found"
}
```

---

## Notes

- All examples consider integration with PostgreSQL and fast food scenario.
- Fields can be adjusted according to your application's data model.
- For questions about integration, authentication and C# code examples, consult the main project documentation or open an issue.
