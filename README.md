# Menu Service – Fast Food Delivery (PostgreSQL Example)

This document presents REST endpoint examples for the Menu service of a Fast Food Delivery application, using PostgreSQL as the database.

---

## Endpoints

### 1. List all products
**GET /api/menu**

Optional query parameters:

- `type` – filter by the product type (e.g., `Snack`, `Dessert`, `Drink`)
- `search` – free text search applied to the product name and description

**Response:**
```json
[
  {
    "id": 1,
    "name": "Cheeseburger",
    "description": "Burger with cheese, lettuce, tomato and special sauce.",
    "price": 18.50,
    "availability": true,
    "type": "Main",
    "created_date": "2025-07-05T12:00:00Z"
  },
  {
    "id": 2,
    "name": "Medium Fries",
    "description": "Medium portion of crispy french fries.",
    "price": 8.00,
    "availability": true,
    "type": "Side",
    "created_date": "2025-07-05T12:02:00Z"
  }
]
```

---

### 2. Product details
**GET /api/products/{id}**

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "Cheeseburger",
  "description": "Burger with cheese, lettuce, tomato and special sauce.",
  "price": 18.50,
  "availability": true,
  "type": "Main",
  "created_date": "2025-07-05T12:00:00Z"
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
**POST /api/products**

**Request Body:**
```json
{
  "name": "Soft Drink 350ml",
  "description": "Coca-cola, 350ml can.",
  "price": 6.00,
  "availability": true,
  "type": "Beverage"
}
```

**Response (201 Created):**
```json
{
  "id": 3,
  "name": "Soft Drink 350ml",
  "description": "Coca-cola, 350ml can.",
  "price": 6.00,
  "availability": true,
  "type": "Beverage",
  "created_date": "2025-07-05T14:00:00Z"
}
```

---

### 4. Update product
**PUT /api/products/{id}**

**Request Body:**
```json
{
  "name": "Large Fries",
  "description": "Large portion of crispy french fries.",
  "price": 12.00,
  "availability": true,
  "type": "Side"
}
```

**Response (200 OK):**
```json
{
  "id": 2,
  "name": "Large Fries",
  "description": "Large portion of crispy french fries.",
  "price": 12.00,
  "availability": true,
  "type": "Side",
  "created_date": "2025-07-05T12:02:00Z"
}
```

---

### 5. Update availability
**PATCH /api/products/{id}/availability**

**Request Body:**
```json
{
  "availability": false
}
```

**Response (200 OK):**
```json
{
  "id": 2,
  "name": "Large Fries",
  "description": "Large portion of crispy french fries.",
  "price": 12.00,
  "availability": false,
  "type": "Side",
  "created_date": "2025-07-05T12:02:00Z"
}
```

---

### 6. Delete product
**DELETE /api/products/{id}**

**Response (204 No Content):**
_No content._

**Response if not found:**
```json
{
  "error": "Product not found"
}
```

---

## Note

- All examples consider integration with PostgreSQL and fast food scenario.
- Fields can be adjusted according to your application's modeling.
- For questions about integration, authentication and C# code examples, please refer to the main project documentation or open an issue.
