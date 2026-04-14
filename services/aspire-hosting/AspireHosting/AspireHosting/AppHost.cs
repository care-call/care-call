var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var practitionerDb = postgres.AddDatabase("practitioner-db", "care-call");
var appointmentDb = postgres.AddDatabase("appointment-db", "care-call-appointment");
var notificationDb = postgres.AddDatabase("notification-db", "care-call-notification");
var handbookDb = postgres.AddDatabase("handbook-db", "handbook");

builder.AddProject<Projects.CC_PractitionerService>("practitioner")
    .WithReference(practitionerDb, "DefaultConnection")
    .WaitFor(practitionerDb);

builder.AddProject<Projects.CC_AppointmentService>("appointment")
    .WithReference(appointmentDb, "DefaultConnection")
    .WaitFor(appointmentDb);

builder.AddProject<Projects.CC_NotificationService>("notification")
    .WithReference(notificationDb, "DefaultConnection")
    .WaitFor(notificationDb);

builder.AddProject<Projects.CC_HandbookService>("handbook")
    .WithReference(handbookDb, "DefaultConnection")
    .WaitFor(handbookDb);

builder.Build().Run();
