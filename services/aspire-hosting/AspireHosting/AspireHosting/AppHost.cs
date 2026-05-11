var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env")
    .WithSshDeploySupport();

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5555)
    .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(5050));

var practitionerDb = postgres.AddDatabase("practitioner-db", "care-call");
var appointmentDb = postgres.AddDatabase("appointment-db", "care-call-appointment");
var notificationDb = postgres.AddDatabase("notification-db", "care-call-notification");
var handbookDb = postgres.AddDatabase("handbook-db", "handbook");
var storageDb = postgres.AddDatabase("storage-db", "care-call-storage");

var garage = builder.AddContainer("garage", "dxflrs/garage:v2.0.0")
    .WithVolume("garage-data", "/var/lib/garage/data")
    .WithVolume("garage-meta", "/var/lib/garage/meta")
    .WithHttpEndpoint(port: 3900, targetPort: 3900, name: "s3")
    .WithHttpEndpoint(port: 3901, targetPort: 3901, name: "rpc")
    .WithEntrypoint("/bin/sh")
    .WithArgs("-c", @"
        # Запускаем Garage сервер
        /usr/local/bin/garage server &
        GARAGE_PID=$!
        
        # Ждём готовности сервера
        sleep 5
        
        # Создаём бакет (игнорируем ошибку, если уже существует)
        /usr/local/bin/garage bucket create care-call-storage 2>/dev/null || true
        
        # Создаём ключ доступа с фиксированными значениями для разработки
        /usr/local/bin/garage key create --name dev-key --secret-key devsecretkey123 2>/dev/null || true
        
        # Даём права на бакет
        /usr/local/bin/garage bucket allow care-call-storage --read --write --key dev-key 2>/dev/null || true
        
        # Держим процесс
        wait $GARAGE_PID
    ");

var practitioner = builder.AddProject<Projects.CC_PractitionerService>("practitioner")
    .WithReference(practitionerDb, "DefaultConnection")
    .WaitFor(practitionerDb);

var appointment = builder.AddProject<Projects.CC_AppointmentService>("appointment")
    .WithReference(appointmentDb, "DefaultConnection")
    .WaitFor(appointmentDb);

var notification = builder.AddProject<Projects.CC_NotificationService>("notification")
    .WithReference(notificationDb, "DefaultConnection")
    .WaitFor(notificationDb);

var handbook = builder.AddProject<Projects.CC_HandbookService>("handbook")
    .WithReference(handbookDb, "DefaultConnection")
    .WaitFor(handbookDb);

var storage = builder.AddProject<Projects.CC_StorageService>("storage")
    .WithReference(storageDb, "StorageDb")
    .WithEnvironment("S3_ENDPOINT", garage.GetEndpoint("s3"))
    .WithEnvironment("S3_ACCESS_KEY", "dev-key")
    .WithEnvironment("S3_SECRET_KEY", "devsecretkey123")
    .WithEnvironment("S3_BUCKET", "care-call-storage")
    .WaitFor(storageDb)
    .WaitFor(garage);

builder.AddProject<Projects.CC_Gateway>("gateway")
    .WithReference(practitioner)
    .WithReference(appointment)
    .WithReference(notification)
    .WithReference(handbook)
    .WithReference(storage)
    .WaitFor(practitioner)
    .WaitFor(appointment)
    .WaitFor(notification)
    .WaitFor(handbook)
    .WithExternalHttpEndpoints();

builder.Build().Run();
