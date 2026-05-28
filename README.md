# TaskDb - Управление задачами с EF Core и SQLite

**ФИО:** Ткаченко ВМ
**Группа:** ИСП-231

## Краткое описание работы

В данной лабораторной работе было разработано REST API приложение для управления задачами с использованием ASP.NET Core, Entity Framework Core и SQLite. Реализован полный CRUD (Create, Read, Update, Delete) для задач с поддержкой фильтрации, поиска, пагинации и агрегации данных.

### Основные возможности

- Создание, просмотр, обновление и удаление задач
- Фильтрация задач по статусу выполнения и приоритету
- Поиск по заголовку и описанию
- Пагинация списка задач
- Статистика по задачам (количество выполненных, группировка по приоритету)
- Просроченные задачи (DueDate)
- Автоматическое логирование SQL-запросов

### Технологии

| Компонент | Технология |
| --- | --- |
| Бэкенд | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| База данных | SQLite |
| Документация API | Swagger / Swashbuckle |

## Полезные команды dotnet ef

| Команда | Описание |
| --- | --- |
| `dotnet ef --version` | Проверить версию установленного инструмента |
| `dotnet tool install --global dotnet-ef --version 10.0.3` | Установить dotnet-ef нужной версии |
| `dotnet ef migrations add ИмяМиграции` | Создать новую миграцию на основе изменений в моделях |
| `dotnet ef database update` | Применить все неприменённые миграции к базе данных |
| `dotnet ef migrations list` | Показать список всех миграций и их статус |
| `dotnet ef migrations remove` | Удалить последнюю миграцию (только если не применена) |
| `dotnet ef database update ИмяМиграции` | Откатить БД до указанной миграции |
| `dotnet ef migrations script` | Сгенерировать SQL-скрипт миграций |
## Структура проекта

TaskDb/
├── Controllers/
│ └── TasksController.cs # Контроллер задач (все CRUD операции)
├── Models/
│ ├── TaskItem.cs # Модель задачи (сущность БД)
│ └── TaskDtos.cs # DTO для создания и обновления
├── Data/
│ └── AppDbContext.cs # Контекст базы данных (DbSet, настройки)
├── Migrations/ # Автоматически сгенерированные миграции
│ ├── InitialCreate.cs
│ └── AddDueDateToTask.cs
├── Program.cs # Точка входа, настройка DI и CORS
├── appsettings.json # Строка подключения к БД
├── appsettings.Development.json # Настройки логирования SQL
└── taskdb.db # Файл базы данных SQLite


## Список всех реализованных маршрутов

| Метод | URL | Описание | Коды ответа |
| --- | --- | --- | --- |
| GET | /api/tasks | Получить все задачи (с фильтрацией по completed, priority) | 200 |
| GET | /api/tasks/{id} | Получить задачу по ID | 200, 404 |
| GET | /api/tasks/search | Поиск задач (query, priority, completed) | 200 |
| GET | /api/tasks/stats | Получить статистику по задачам | 200 |
| GET | /api/tasks/paged | Получить задачи с пагинацией (page, pageSize) | 200 |
| GET | /api/tasks/overdue | Получить просроченные задачи (DueDate < сегодня) | 200 |
| POST | /api/tasks | Создать новую задачу | 201, 400 |
| PUT | /api/tasks/{id} | Полностью обновить задачу | 200, 400, 404 |
| PATCH | /api/tasks/{id}/complete | Переключить статус выполнения задачи | 200, 404 |
| DELETE | /api/tasks/{id} | Удалить задачу | 204, 404 |

### Параметры запросов

| Маршрут | Параметры | Описание |
| --- | --- | --- |
| GET /api/tasks | completed (bool?), priority (string) | Фильтрация по статусу и приоритету |
| GET /api/tasks/search | query (string), priority (string), completed (bool?) | Поиск по тексту + фильтры |
| GET /api/tasks/paged | page (int, default=1), pageSize (int, default=10, max=50) | Номер страницы и размер |

## Таблица применённых миграций

| Миграция | Описание | Статус |
| --- | --- | --- |
| InitialCreate | Создание таблицы Tasks с колонками: Id, Title, Description, IsCompleted, CreatedAt, Priority | Применена |
| AddDueDateToTask | Добавление колонки DueDate (nullable DateTime) для срока выполнения задачи | Применена |

## Сравнительная таблица LINQ vs SQL

| LINQ | SQL |
| --- | --- |
| `.Where(t => t.IsCompleted == false)` | `WHERE is_completed = 0` |
| `.OrderBy(t => t.CreatedAt)` | `ORDER BY created_at ASC` |
| `.OrderByDescending(t => t.CreatedAt)` | `ORDER BY created_at DESC` |
| `.Take(10)` | `LIMIT 10` |
| `.Skip(20).Take(10)` | `OFFSET 20 LIMIT 10` |
| `.Count()` | `SELECT COUNT(*)` |
| `.CountAsync(t => t.IsCompleted)` | `SELECT COUNT(*) WHERE is_completed = 1` |
| `.Any(t => t.Priority == "High")` | `SELECT EXISTS(... WHERE priority = 'High')` |
| `.Max(t => t.Id)` | `SELECT MAX(id)` |
| `.GroupBy(t => t.Priority).Select(g => new { g.Key, Count = g.Count() })` | `SELECT priority, COUNT(*) FROM Tasks GROUP BY priority` |
| `.Select(t => t.Title)` | `SELECT title` |
| `t.Title.Contains("SQL")` | `WHERE title LIKE '%SQL%'` |
| `t.DueDate < now` | `WHERE due_date < '2026-01-01'` |

## Главные выводы

1. **EF Core — это переводчик между C# и SQL** — вы пишете LINQ-запросы, а EF Core автоматически преобразует их в оптимальные SQL-запросы. Это позволяет работать с базой данных, не зная глубоко SQL, но знание SQL помогает понимать, что происходит под капотом.

2. **Миграции — система контроля версий для структуры БД** — аналогично Git для кода, миграции позволяют отслеживать изменения схемы базы данных, откатывать их и синхронизировать между разработчиками.

3. **Code First удобнее ручного SQL** — изменяешь класс C#, создаёшь миграцию (`dotnet ef migrations add`), применяешь (`dotnet ef database update`) — и база данных обновлена. Не нужно писать ALTER TABLE вручную.

4. **SaveChangesAsync() — ключевой момент** — до вызова этого метода все изменения (Add, Remove, Update) живут только в памяти приложения. Только после `SaveChangesAsync()` EF Core отправляет реальные INSERT/UPDATE/DELETE в базу данных.

5. **async/await при работе с БД — это стандарт, а не опция** — блокировать поток на время ожидания ответа от базы данных — плохая практика. `async/await` позволяет серверу обрабатывать другие запросы, пока один ждёт выполнения SQL-запроса.

6. **Пагинация обязательна для реальных приложений** — методы `Skip()` и `Take()` преобразуются в `OFFSET` и `LIMIT` в SQL, что позволяет загружать данные порциями, снижая нагрузку на сервер и ускоряя ответы API.

7. **Логирование SQL помогает отладке** — настройка логирования EF Core показывает реальные SQL-запросы в консоли, что помогает понять, какие запросы уходят в БД и оптимизировать их при необходимости.

## Сравнение хранения в памяти vs EF Core + SQLite

| Концепция | Хранение в памяти (static List<T>) | EF Core + SQLite |
| --- | --- | --- |
| Хранение данных | RAM (оперативная память) | Файл .db на диске |
| После перезапуска сервера | Данные пропадают | Данные сохраняются |
| Поиск по условию | LINQ to Objects | LINQ to Entities → SQL |
| Создание структуры | Не нужно | Миграции (dotnet ef) |
| Начальные данные | Хардкод в коде | HasData() в миграции |
| Получение по ID | `list.FirstOrDefault(x => x.Id == id)` | `await db.Tasks.FindAsync(id)` |
| Добавление | `list.Add(item)` | `db.Tasks.Add(item)` + `SaveChangesAsync()` |
| Удаление | `list.Remove(item)` | `db.Tasks.Remove(item)` + `SaveChangesAsync()` |
| Масштабируемость | Ограничена RAM | Гигабайты данных |
| Транзакции | Нет | Встроены в EF Core |

## Структура проекта
