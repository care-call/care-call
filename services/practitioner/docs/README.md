# Документация

### Создание и применение миграций

```ps
dotnet ef migrations add MIGRATION_NAME -s src\CC.PractitionerService -p src\CC.PractitionerService --output-dir Infrastructure\Persistence\Migrations
```

```ps
dotnet ef database update -s src\CC.PractitionerService -p src\CC.PractitionerService
```