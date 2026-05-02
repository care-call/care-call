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

var practitioner = builder.AddProject<Projects.CC_PractitionerService>("practitioner")
    .WithReference(practitionerDb, "DefaultConnection")
    .WaitFor(practitionerDb)
    .WithExternalHttpEndpoints();

var appointment = builder.AddProject<Projects.CC_AppointmentService>("appointment")
    .WithReference(appointmentDb, "DefaultConnection")
    .WaitFor(appointmentDb)
    .WithExternalHttpEndpoints();

var notification = builder.AddProject<Projects.CC_NotificationService>("notification")
    .WithReference(notificationDb, "DefaultConnection")
    .WaitFor(notificationDb)
    .WithExternalHttpEndpoints();

var handbook = builder.AddProject<Projects.CC_HandbookService>("handbook")
    .WithReference(handbookDb, "DefaultConnection")
    .WaitFor(handbookDb)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.CC_Gateway>("gateway")
    .WithReference(practitioner)
    .WithReference(appointment)
    .WithReference(notification)
    .WithReference(handbook)
    .WaitFor(practitioner)
    .WaitFor(appointment)
    .WaitFor(notification)
    .WaitFor(handbook)
    .WithExternalHttpEndpoints();

builder.Build().Run();
