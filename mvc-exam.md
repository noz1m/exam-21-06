# Экзаменационное задание по ASP.NET Core MVC

## 📚 Тема: Онлайн Библиотека

Ваша задача — создать мини-сайт библиотеки с использованием ASP.NET Core MVC. Используйте Entity Framework Core и PostgreSQL.

---

## 🧩 Требования к базе данных

Создайте три таблицы:

1. **Author** – Автор
   - Id (int)
   - Name (string, required)
   - BirthDate (DateTime, optional)
   - Country (string)

2. **Genre** – Жанр
   - Id (int)
   - Name (string, required)

3. **Book** – Книга
   - Id (int)
   - Title (string, required)
   - Description (string, optional)
   - PublishedYear (int)
   - AuthorId (foreign key)
   - GenreId (foreign key)

---

## 🛠️ CRUD Операции

Реализуйте полный набор CRUD-операций:

- **Author**: Create, Read (List c фильтрацией по автору), Update, Delete
- **Genre**: Create, Read, Update, Delete
- **Book**: Create, Read (List c фильтрацией по жанру и автору), Update, Delete

---

## 📦 Архитектура

- Используйте **MVC pattern**: Controllers + Views + Models
- Реализация на **ASP.NET Core MVC (.NET 7 или .NET 8)**
- Используйте **EF Core + Code First Migrations**
- Используйте **Bootstrap 5** для стилизации

---

## 🕓 Время на выполнение

- Дата сдачи: **до [22.06.2025 - 20 : 00]**
