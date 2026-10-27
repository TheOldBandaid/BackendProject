# Учебная разработка серверной части интернет-ресурса

Веб-приложение пиццерии на ASP.NET Core с базой данных MySQL и Nginx.

## Технологии

- C# / ASP.NET Core
- MySQL 8.4
- Docker Compose
- Nginx

## Запуск

```bash
docker compose up -d --build
```

Сайт доступен по адресу: http://localhost:8080

## Страницы

- `/` — главная страница
- `/catalog` — каталог товаров
- `/products/{id}` — страница товара
- `/delivery.html` — информация о доставке
- `/contacts.html` — контакты

## Остановка

```bash
docker compose down
```
