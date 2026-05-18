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

var minio = builder.AddContainer("minio", "minio/minio:latest")
    .WithHttpEndpoint(port: 3900, targetPort: 9000, name: "api")
    .WithHttpEndpoint(port: 3901, targetPort: 9001, name: "console")
    .WithEnvironment("MINIO_ROOT_USER", "minioadmin")
    .WithEnvironment("MINIO_ROOT_PASSWORD", "minioadmin")
    .WithEntrypoint("/bin/sh")
    .WithArgs("-c", "minio server /data --console-address ':9001'");

var storage = builder.AddProject<Projects.CC_StorageService>("storage")
    .WithReference(storageDb, "StorageDb")
    .WithEnvironment("S3_ENDPOINT", minio.GetEndpoint("api"))
    .WithEnvironment("S3_ACCESS_KEY", "minioadmin")
    .WithEnvironment("S3_SECRET_KEY", "minioadmin")
    .WithEnvironment("S3_BUCKET", "care-call-storage")
    .WaitFor(minio)
    .WaitFor(storageDb);

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
