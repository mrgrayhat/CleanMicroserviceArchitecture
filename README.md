Asp.net core Clean Microservice Architecture Using Api Gateway and Angular 10
===========
(Under Development)
------
Microservices:
```
- Api Gateway: Main Service For client and server microservices communications.
Features:
Load Balancing, Qos and circuit breaker, Rate Limiting and centeral auth. (using ocelot and polly)
Microservices Health Checking And Health Check Ui.
```
![Api Gateway Console](Documentation/images/Gateway_RateLimit_001.png?raw=true "Gateway Api")
![Health Check Api UI](Documentation/images/HealthCheckUI_001.PNG?raw=true "Health Check Api UI")
```
- Identity Server (STS):
SSO and centeralized authentiction and authorization.
oauth and openid features.
User interface for Accounting. Multi Language and culture base contents.
Database Health Check.
```
![Identity Server Service Ui](Documentation/images/IdentityServer_Ui_001.png?raw=true "Identity Server Service Ui")
```
- Logger:
Centeralized Logging service. recieve/store all microservices logs and messages.
Serilog Inteegration, MongoDb database and file storage.
Query, Filter and Write services logs, using cqrs and MediatR
storage and database Health checks.
```
![Logger Service Console](Documentation/images/LoggerService_002.png?raw=true "Logger Api Output")
```
- Blog:
CMS and blogging abbilities.
Query, Filter and Write blog posts and contents, using cqrs and MediatR.
Permission and role base.
Multi Language Posts and culture base contents.
```
```
- Storage Management:
File system Storage and database index for Physical Storage Management.
Upload and download files for microservices and clints.
```
```
Client(Angular 10 UI):
Angular SPA application.
```
