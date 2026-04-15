var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env")
    .WithSshDeploySupport();

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5555)
    .WithPgAdmin();

var practitionerDb = postgres.AddDatabase("practitioner-db", "care-call");
var appointmentDb = postgres.AddDatabase("appointment-db", "care-call-appointment");
var notificationDb = postgres.AddDatabase("notification-db", "care-call-notification");
var handbookDb = postgres.AddDatabase("handbook-db", "handbook");

builder.AddProject<Projects.CC_PractitionerService>("practitioner")
    .WithReference(practitionerDb, "DefaultConnection")
    .WaitFor(practitionerDb)
    .WithEndpoint(4555)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.CC_AppointmentService>("appointment")
    .WithReference(appointmentDb, "DefaultConnection")
    .WaitFor(appointmentDb)
    .WithEndpoint(4556)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.CC_NotificationService>("notification")
    .WithReference(notificationDb, "DefaultConnection")
    .WaitFor(notificationDb)
    .WithEndpoint(4557)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.CC_HandbookService>("handbook")
    .WithReference(handbookDb, "DefaultConnection")
    .WaitFor(handbookDb)
    .WithEndpoint(4558)
    .WithExternalHttpEndpoints();


builder.Build().Run();
