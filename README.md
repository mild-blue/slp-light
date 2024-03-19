This is stub repository for interviewing purposes within Mild Blue s.r.o.

It is related to slp.blue application, developed for quality management in medical laboratories.

### Current Assignment

Our application is deployed on tens of servers on workplaces around the country. Each deployment is isolated from others - there is no multitenany.

We are using third party application called Authentik to handle login and user management. From this service we are synchronizing users to the main application, every five minutes. As stated earlier, this holds for each deployment separately.

For some internal reasons, Authentik creates users prefixed with `ak-outpost`. Unfortunately, these got synced into the application and looks bad for users (e.g. if you are selecting someone to revise a document, you want just real people).

The task here is to:
- modify synchronization code to not sync such users into main application
- somehow handle already synced-in users

There is one controller used to return all users from db. The application uses Swagger, for nice overview of endpoints (currently just one).

### Goals

Test candidate's abilities to:
- get project up and running
- solve real scenario problem
- pose questions
- create PR
- write tests, ideally
